using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Daivata.Database
{
    public class DataReaderField : Field
    {
        public DataReaderField(SqlDataReader reader)
        {
            this.Reader = reader;
        }

        private SqlDataReader Reader { get; set; }
        internal int Index { get; set; }

        public override string Name
        {
            get
            {
                return this.Reader.GetName(this.Index);
            }
        }

        public override T As<T>(T defaultValue)
        {
            return Reader.Get<T>(this.Index, defaultValue);
        }
    }
}
