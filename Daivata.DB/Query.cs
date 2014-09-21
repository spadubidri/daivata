using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System;

namespace Daivata.Database
{
    /// <summary>
    /// Represents a query that sends a series of sql statements in a single call
    /// </summary>
    public class Batch : Query
    {
        public Batch():base()
        {
            this.Type = CommandType.Text;
            this.BatchText = new StringBuilder();
        }

        private StringBuilder BatchText { get; set; }

        public void AddStatement(string text)
        {
            AddStatement(text, null);
        }

        public void AddStatement(string text, params object[] parameters)
        {
            if (text.EndsWith("\r\n") == false)
                text += "\r\n";

            if (parameters != null && parameters.Length > 0)
                text = string.Format(text, parameters);


            this.BatchText.Append(text);
        }

        public override string GetText()
        {
            return this.BatchText.ToString();
        }
    }
    public class StoredProcedure : Query
    {
        public StoredProcedure(string schemaName, string procedureName): base()
        {
            this.Type = CommandType.StoredProcedure;
            this.ProcedureName = string.Join(".", schemaName, procedureName);
        }
        public StoredProcedure(string procedureName):base()
        {
            this.Type = CommandType.StoredProcedure;
            this.ProcedureName = procedureName;
        }
        public string ProcedureName { get; set; }

        public override string GetText()
        {
            return this.ProcedureName;
        }
    }
    public abstract class Query
    {
        public Query()
        {
            this.Parameters = new Dictionary<string, object>();
        }
        private IDictionary<string, ParameterDirection> Directions { get; set; }

        public abstract string GetText();
        public CommandType Type { get; set; }
        public IDictionary<string, object> Parameters { get; set; }
        public long ReturnCode { get; set; }

        /// <summary>
        /// This property allows the caller to override the connection string used by the Database
        /// class. By default all queries are executed using the connection string with the key
        /// specified by Database.DEFAULT_CONNECTION_STRING_KEY property.  If the calling code
        /// has it's data in a database other than the one specified in the default connection string,
        /// then it must have a way to instruct the Database class to use a different connection
        /// string. That is done by setting this key. If this property is emty, then the 
        /// Database class with use the default connection string. If this property contains
        /// a value, the Database class will use the connection string in the <connectionStrings> 
        /// configuration element with a key that matches this property
        /// </summary>
        public string ConnectionStringKey { get; set; }

        public string ConnectionString { get; set; }

        public object this[string name]
        {
            get
            {
                if (this.Parameters.ContainsKey(name) == true)
                    return this.Parameters[name];
                else
                    return null;
            }
            set
            {
                if (this.Parameters.ContainsKey(name) == true)
                    this.Parameters[name] = value;
                else
                    this.Parameters.Add(name, value);
            }
        }

        public void SetDirection(string name, ParameterDirection direction)
        {
            if (direction == ParameterDirection.ReturnValue)
                throw new NotSupportedException("Return value parameter directions are reserved for the Datbase framework. If you need to read the return value of a query, since read the Query.ReturnCode property");

            if (this.Directions == null)
                this.Directions = new Dictionary<string, ParameterDirection>();

            if (this.Directions.ContainsKey(name) == true)
            {
                if (direction == ParameterDirection.Input)
                    this.Directions.Remove(name);
                else
                    this.Directions[name] = direction;
            }
            else
            {
                if (direction != ParameterDirection.Input)
                    this.Directions.Add(name, direction);
            }
        }
        public ParameterDirection GetDirection(string name)
        {
            if (this.Directions == null || this.Directions.Count == 0)
                return ParameterDirection.Input;

            if (this.Directions.ContainsKey(name) == true)
                return this.Directions[name];
            else
                return ParameterDirection.Input;
        }
    }
}
