using System;

namespace Daivata.Database
{
    public static class IDataReaderExtensions
    {
        public static T Get<T>(this System.Data.IDataRecord record, int fieldIndex, T defaultValue)
        {
            if (record == null)
                return defaultValue;

            try
            {
                if (record.IsDBNull(fieldIndex) == true)
                    return defaultValue;
                else
                    return (T)record.GetValue(fieldIndex);
            }
            catch (IndexOutOfRangeException)
            {
                return defaultValue;
            }
        }
        public static T Get<T>(this System.Data.IDataRecord record, string fieldName, T defaultValue)
        {
            if (string.IsNullOrEmpty(fieldName) == true)
                return defaultValue;

            int fieldIdx = record.GetOrdinal(fieldName);
            return Get<T>(record, fieldIdx, defaultValue);
        }
    }
}
