using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Daivata.Database
{
    public abstract class Field
    {

        public abstract string Name
        {
            get;
        }


        public T As<T>()
        {
            return As<T>(default(T));
        }
        public abstract T As<T>(T defaultValue);
    }
}
