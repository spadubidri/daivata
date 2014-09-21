using System;

namespace Daivata.Database
{
    public class Null
    {
        public static Null Value<T>()
        {
            return new Null(){ValueType = typeof(T)};
        }

        public Null()
        {
        }
        public Type ValueType { get; set; }
    }
}
