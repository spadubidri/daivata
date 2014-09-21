using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Daivata.Database
{
		public class LoopControl
		{
				internal bool BreakRequested { get; set; }

				public void Break()
				{
						this.BreakRequested = true;
				}

				internal void Reset()
				{
						this.BreakRequested = false;
				}
		}

		/// <summary>
		/// Provides a wrapper around a DataReader result set. Provides a way to automatically
		/// enumerate over items, and load data
		/// </summary>
		public class Resultset
		{
				public Resultset(SqlDataReader reader, int index)
				{
						this.Reader = reader;
						this.Index = index;
				}

				public int Index { get; private set; }
				public SqlDataReader Reader { get; private set; }

				/// <summary>
				/// Provides a way for code to enumerate over each record in the result set
				/// </summary>
				/// <param name="newRecordHandler"></param>
				public void ForEach(Action<IDataRecord, LoopControl> newRecordHandler)
				{
						if (this.Reader == null)
								return;

						if (this.Reader.HasRows == true)
						{
								LoopControl cntl = new LoopControl();
								while (this.Reader.Read())
								{
										cntl.Reset();
										newRecordHandler(this.Reader, cntl);

										if (cntl.BreakRequested == true)
												break;
								}
						}
				}

				/// <summary>
				/// Provides a way to automatically create and populate a data entity using data found from the result set
				/// </summary>
				/// <typeparam name="T"></typeparam>
				/// <returns></returns>
				public T GetItem<T>() where T : Entity, new()
				{
						T item = new T();
						GetItem<T>(item);
						return item;
				}
				public void GetItem<T>(T item) where T : Entity
				{
						ForEach((rec, lc) =>
						{
								IDbStorage storage = item as IDbStorage;
								storage.Load(rec);

								//because we are only loading one item, break after processing the first record
								lc.Break();
						});
				}

				/// <summary>
				/// Provides a way to automatically create and populate a list of data entities using the data found in the result set
				/// </summary>
				/// <typeparam name="T"></typeparam>
				/// <param name="itemsContainer"></param>
				public void GetItems<T>(IList<T> itemsContainer) where T : Entity, new()
				{
						ForEach((rec, lc) =>
						{
								T item = new T();
								IDbStorage storage = item as IDbStorage;
								storage.Load(rec);
								itemsContainer.Add(item);
						});
				}

				/// <summary>
				/// Provides a way to automatically create and populate a list of data entities using the data found in the result set
				/// </summary>
				/// <typeparam name="T"></typeparam>
				/// <param name="itemsContainer"></param>
				public IList<T> GetItems<T>() where T : Entity, new()
				{
						List<T> items = new List<T>();
						GetItems<T>(items);
						return items;
				}
		}

		public class Database
		{
				#region Attributes
				private const string RETURN_VALUE_PARAMETER_NAME = "@retValue";
				private static string m_connString = string.Empty;
				private static readonly IDatabaseActions databaseActions = null;
				#endregion

				#region Constructor
				static Database()
				{
                    string typename = "";
                    if (!string.IsNullOrWhiteSpace(typename))
						{
                            Type typeToCreate = System.Type.GetType(typename, false, true);
								if (typeToCreate != null)
								{
										databaseActions = Activator.CreateInstance(typeToCreate) as IDatabaseActions;
								}
						}
				}
				#endregion

				#region Methods
				#region Public
				public static IList<T> GetItems<T>(Query query) where T : Entity, new()
				{
						if (query == null)
								throw new ArgumentNullException("query");

						IList<T> items = null;

						//enumerate the result sets
						ForEach(query, (rs, lc) =>
								{
										items = rs.GetItems<T>();

										//we anticipate only one result set, so break after processing it
										lc.Break();
								}, CommandBehavior.SingleResult | CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);

						return items;
				}

				public static void GetItem<T>(Query query, T item) where T : Entity
				{
						if (query == null)
								throw new ArgumentNullException("query");

						ForEach(query, (rs, lc) =>
						{
								rs.GetItem<T>(item);

								//break after the first item is loaded
								lc.Break();
						}, CommandBehavior.SingleResult | CommandBehavior.SingleRow | CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);
				}

				public static T GetItem<T>(Query query) where T : Entity, new()
				{
						T item = new T();
						GetItem<T>(query, item);
						return item;
				}

				public static int ExecuteQuery(Query query)
				{
						int rowsAffected = 0;
						Run((cmd) =>
								{
										rowsAffected = 0;
										string name = (query is StoredProcedure ? query.GetText() : "Batch");
										TimeQuery(() => rowsAffected = cmd.ExecuteNonQuery(), name, query.Parameters);

								}, query);

						//return the number of rows affected
						return rowsAffected;
				}

				public static DataSet ExecuteDataSet(Query query)
				{
						DataSet dataSet = new DataSet();
						Run((cmd) =>
						{
								SqlDataAdapter adapter = new SqlDataAdapter(cmd);

								int rowsAffected = 0;
								string name = (query is StoredProcedure ? query.GetText() : "Batch");
								TimeQuery(() => rowsAffected = adapter.Fill(dataSet), name, query.Parameters);

						}, query);

						//return the number of rows affected
						return dataSet;
				}

				public static void ForEach(Query query, Action<Resultset, LoopControl> newResultsetHandler)
				{
						ForEach(query, newResultsetHandler, CommandBehavior.CloseConnection | CommandBehavior.SequentialAccess);
				}

				public static void ForEach(Query query, Action<Resultset, LoopControl> newResultsetHandler, CommandBehavior behavior)
				{
						if (query == null)
								throw new ArgumentNullException("query");

						if ((behavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection)
								behavior = behavior | CommandBehavior.CloseConnection;

						Run((cmd) =>
								{
										//If we open the datareader with the SequentialAccess flag, this will tell SqlServer that we are going to
										//access the fields in the data reader sequentially (ie we will read the data from field #1 first, and then read
										//data from field #2 second, etc).  What this does is allow Sql to do some optimizations that increase the performance
										//of the call. Because we are accessing the field values sequentially, Sql knows that it doesn't have to wait for all
										//field values to become available before making the current row accessible to the caller. Instead, it only has to wait
										//for the current field to become available. By doing this, our application performance will be increased.  The only
										//caveat to this, is that we have to guarantee that the data reader fields will be accessed sequentially, or else
										//sql will throw an exception to us. The way we enforce that is by only loading the data into objects who inherit
										//from Entity. The Entity base class provides indirect way to populate an object while at the same time enforcing
										//the sequential nature of the datareader.

										SqlDataReader reader = null;

										try
										{
												//log the time taken to run the query.
												string name = (query is StoredProcedure ? query.GetText() : "Batch");
												TimeQuery(() => reader = cmd.ExecuteReader(behavior), name, query.Parameters);

												LoopControl cntl = new LoopControl();
												int resultIdx = 0;
												do
												{
														cntl.Reset();

														Resultset rs = new Resultset(reader, resultIdx);
														newResultsetHandler(rs, cntl);

														if (cntl.BreakRequested == true)
																break;

														resultIdx++;
												}
												while (reader.NextResult());
										}
										finally
										{
												//we are done with the reader, close it
												if (reader != null && reader.IsClosed == false)
														reader.Dispose();
										}
								}, query);
				}
				#endregion

				#region Private
				private static void TimeQuery(Action code, string queryName, IDictionary<string, object> parameters)
				{
						//this function will record how long it takes for a query to run, and update any performance counters
						bool counterRecorded = false;
						Stopwatch sw = new Stopwatch();
						try
						{
								//Log.Start(ScopeTypes.Query, queryName, parameters);
								code();
						}
						catch
						{
								sw.Stop();
								counterRecorded = true;
								//Log.Stop(ScopeTypes.Query, queryName, sw.ElapsedMilliseconds);
								//Dell.Security.Diagnostics.PerformanceCounters.Increment(Categories.Membership, SharedCounters.AvgQueryDurationBase);
								//Dell.Security.Diagnostics.PerformanceCounters.IncrementBy(Categories.Membership, SharedCounters.AvgQueryDuration, sw.ElapsedMilliseconds);
								throw;
						}
						finally
						{
								sw.Stop();

								//make sure we don't double record the counter, if it was already recorded during the catch clasue
								if (counterRecorded == false)
								{
										//Log.Stop(ScopeTypes.Query, queryName, sw.ElapsedMilliseconds);
										//Dell.Security.Diagnostics.PerformanceCounters.Increment(Categories.Membership, SharedCounters.AvgQueryDurationBase);
										//Dell.Security.Diagnostics.PerformanceCounters.IncrementBy(Categories.Membership, SharedCounters.AvgQueryDuration, sw.ElapsedMilliseconds);
								}
						}
				}

				[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
				private static void Run(Action<SqlCommand> code, Query query)
				{
						//this function runs a piece of database code and applies automatic retry
						//logic
                        //using (MethodScope scope = new MethodScope("Database", "Run"))
                        //{
								Stopwatch timer = Stopwatch.StartNew();
								try
								{
										bool executed = false;
										while (executed == false)
										{
												try
												{
														using (SqlConnection conn = new SqlConnection())
														{
																if (!string.IsNullOrEmpty(query.ConnectionString))
																{
																		conn.ConnectionString = query.ConnectionString;
																}
																else
																{
																		conn.ConnectionString = Database.GetConnectionString("Daivata");
																}

																using (SqlCommand cmd = conn.CreateCommand())
																{
																		cmd.CommandType = query.Type;
																		cmd.CommandText = query.GetText();
																		CopyParameters(cmd, query);
																		conn.Open();

																		//run the code after we have connected
																		code(cmd);

																		//update query parameters with any output values sent from the database
																		ReadOuputParameters(cmd, query);

																		//populate the return code on the query object
																		if (cmd.Parameters[RETURN_VALUE_PARAMETER_NAME] != null)
																				query.ReturnCode = (int)cmd.Parameters[RETURN_VALUE_PARAMETER_NAME].Value;

																		executed = true;
																}
														}

														if (databaseActions != null)
														{
																databaseActions.AftreQueryExecution(query);
														}
												}
												catch (SqlException ex)
												{

                                                        executed = true;
														if (ex.Class == 16)// Custom exception
														{
															//Log.Info(ex.Message);
														}
														else
														{
															//ExceptionHelper.MapEventIdToException(ex);
														}
														if (databaseActions != null)
														{
																databaseActions.OnException(ex);
														}

														//check to see if the error thrown by the database, is one to indicate a failover
                                                        //SqlErrorSettings err = Config.Settings.Data.Failover.OnErrors.FirstOrDefault((errSetting) => ex.Number == errSetting.Number || ex.ErrorCode == errSetting.Code || (!string.IsNullOrWhiteSpace(errSetting.Message) && ex.Message.Contains(errSetting.Message)));
                                                        //if (err != null)
                                                        //{
                                                        //        //Log.Warning("An error has occurred while trying to access the database. This error qualifies for automatic retry.");
                                                        //        int duration = Convert.ToInt32(timer.ElapsedMilliseconds);
                                                        //        if (duration < Config.Settings.Data.Failover.RetryDuration)
                                                        //        {
                                                        //                //Log.Warning("We are in the retry duration window. Pausing thread before we attempt to retry");

                                                        //                //read the default retry interval
                                                        //                int retryInterval = Config.Settings.Data.Failover.RetryInterval;

                                                        //                //each error setting can override the default wait interval. So check if a value was supplied, and if so, then use it
                                                        //                if (err.RetryInterval > 0)
                                                        //                        retryInterval = err.RetryInterval;

                                                        //                Thread.Sleep(retryInterval);
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //                timer.Stop();
                                                        //                //Log.Warning("We have exceeded the retry duration window, and database is still throwing the error. Retry aborted, error will be thrown to caller");
                                                        //                //we have been retrying longer than the configured time. In that case we need
                                                        //                //to throw an exception indicating the failover
                                                        //                throw new FailoverTimeoutException("Operation timed out due to a database failover ocurring");
                                                        //        }
                                                        //}
                                                        //else
                                                        //{
                                                        //        timer.Stop();
                                                        //        throw new DatabaseException(ex.Message, ex);
                                                        //}
                                                        throw new Exception(ex.Message, ex);

												}
												catch (Exception e)
												{
                                                        executed = true;
														timer.Stop();
														if (databaseActions != null)
														{
																databaseActions.OnException(e);
														}
														//Log.Info(e.Message);
														throw new Exception(e.Message, e);
												}
										}
								}
								finally
								{
										timer.Stop();
								}
						//}
				}

				private static void CopyParameters(SqlCommand cmd, Query query)
				{
						if (cmd.Parameters.Count > 0)
								cmd.Parameters.Clear();

						//always ensure that the command has a return value parameter
						if (cmd.Parameters.Contains(RETURN_VALUE_PARAMETER_NAME) == false)
						{
								SqlParameter returnValue = cmd.CreateParameter();
								returnValue.ParameterName = RETURN_VALUE_PARAMETER_NAME;
								returnValue.SqlDbType = SqlDbType.Int;
								returnValue.Direction = ParameterDirection.ReturnValue;
								cmd.Parameters.Add(returnValue);
						}

						if (query.Parameters != null && query.Parameters.Count > 0)
						{
								foreach (KeyValuePair<string, object> entry in query.Parameters)
								{
										SqlParameter parm = new SqlParameter();
										parm.ParameterName = entry.Key;
										parm.Direction = query.GetDirection(entry.Key);

										if (entry.Value is Null)
										{
												parm.Value = DBNull.Value;
										}
										else
												DeriveType(entry.Value, parm);

										cmd.Parameters.Add(parm);
								}
						}
				}

				private static void ReadOuputParameters(SqlCommand cmd, Query query)
				{
						if (cmd.Parameters.Count == 0)
								return;

						foreach (SqlParameter parm in cmd.Parameters)
						{
								if (parm.Direction == ParameterDirection.InputOutput || parm.Direction == ParameterDirection.Output)
								{
										query[parm.ParameterName] = parm.Value;
								}
						}
				}

				private static void DeriveType(object value, SqlParameter parm)
				{
						if (value == null)
								return;

						Type valueType = value.GetType();
						parm.Value = value;

						switch (valueType.FullName)
						{
								case "System.String":
										parm.SqlDbType = SqlDbType.NVarChar;
										if (parm.Value != DBNull.Value)
												parm.Size = ((string)parm.Value).Length;
										break;
								case "System.Byte":
								case "System.SByte":
										parm.SqlDbType = SqlDbType.TinyInt;
										break;
								case "System.Int16":
								case "System.UInt16":
										parm.SqlDbType = SqlDbType.SmallInt;
										break;
								case "System.Int32":
								case "System.UInt32":
										parm.SqlDbType = SqlDbType.Int;
										break;
								case "System.Int64":
								case "System.UInt64":
										parm.SqlDbType = SqlDbType.BigInt;
										break;
								case "System.Boolean":
										parm.SqlDbType = SqlDbType.Bit;
										break;
								case "System.Guid":
										parm.SqlDbType = SqlDbType.UniqueIdentifier;
										break;
								case "System.Decimal":
										parm.SqlDbType = SqlDbType.Decimal;
										break;
								case "System.Single":
										parm.SqlDbType = SqlDbType.Real;
										break;
								case "System.Double":
										parm.SqlDbType = SqlDbType.Float;
										break;
								case "System.Byte[]":
										parm.SqlDbType = SqlDbType.Binary;
										if (parm.Value != DBNull.Value)
												parm.Size = ((byte[])parm.Value).Length;
										break;
								case "System.DateTime":
										parm.SqlDbType = SqlDbType.DateTime;
										break;
								case "System.Data.DataTable":
										parm.SqlDbType = SqlDbType.Structured;
										//we'll assume that the name of Table type is specified as the data table name
										DataTable dt = (DataTable)value;
										parm.TypeName = dt.TableName;
										break;
								default:
										throw new InvalidOperationException("Unsupported parameter type");
						}
				}

				private static string GetConnectionString(string key)
				{
						//if the query supplies a connection string, then use it. Otherwise use our default connection string
                    if (string.IsNullOrEmpty(key) == false)
                        return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;

                    return m_connString;
				}
				#endregion
				#endregion
		}
}
