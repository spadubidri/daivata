using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace Daivata.Database
{
		[Serializable]
		public class Entity : IDbStorage
		{
				public Entity()
				{
				}

				void IDbStorage.Load(IDataRecord record)
				{
					Load(record);
				}

				public void Load(IDataRecord record)
				{
					IDbStorage item = this as IDbStorage;
					DataReaderField field = new DataReaderField(record as SqlDataReader);
					for (int i = 0; i < record.FieldCount; i++)
					{
						field.Index = i;
						ReadField(field);
					}
				}

				protected virtual void ReadField(Field field)
				{
				}
		}
}
