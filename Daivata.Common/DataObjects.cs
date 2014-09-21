using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using Daivata.Database;


namespace Daivata.Entities
{
    /// <summary>
    /// Adds support for WCF's extensibility mechanisms to all objects
    /// </summary>
    [DataContract(Name = "DataObject", Namespace = NS.V1.Schema)]
    public abstract class DataObject : Entity, System.Runtime.Serialization.IExtensibleDataObject
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            Type type = this.GetType();
            sb.Append(type.FullName);

            bool bracketWritten = false;
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in properties.Where((p) => p.Name != "ExtensionData"))
            {
                object value = pi.GetValue(this, null);
                if (value != null)
                {
                    if (bracketWritten == false)
                    {
                        sb.Append("[");
                        bracketWritten = true;
                    }

                    sb.AppendFormat("{0}={1},", pi.Name, value);
                }
            }

            if (bracketWritten == true)
            {
                if (sb[sb.Length - 1] == ',')
                    sb.Remove(sb.Length - 1, 1);

                sb.Append("]");
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Adds properties commonly used across all Entities, and support for automatically
    /// populating those properties when loaded from the database
    /// </summary>
    [DataContract(Name = "PersistedDataObject", Namespace = NS.V1.Schema)]
    public abstract class PersistedDataObject : DataObject
    {
        public PersistedDataObject()
            : base()
        {
            this.Created = DateTime.MinValue;
            this.Updated = DateTime.MinValue;
            this.UpdatedBy = 0;
            this.ID = 0;
        }

        [DataMember(Name = "Created")]
        public DateTime Created { get; set; }

        [DataMember(Name = "Updated")]
        public DateTime Updated { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int UpdatedBy { get; set; }

        [DataMember(Name = "ID")]
        public long ID { get; set; }

        protected override void ReadField(Field field)
        {
            //switch (field.Name)
            //{
            //    //case Fields.Updated:
            //    //    this.Updated = field.As<DateTime>();
            //    //    break;
            //    //case Fields.UpdatedBy:
            //    //    this.UpdatedBy = field.As<int>();
            //    //    break;
            //    //case Fields.Created:
            //    //    this.Created = field.As<DateTime>();
            //    //    break;
            //}
            base.ReadField(field);
        }
    }

    internal class NS
    {
        internal const string Root = "http://edell.dell.com/membership/";
        private const string Contract = Root + "contract";
        private const string Schema = Root + "schema";
        private const string Headers = Root + "headers";
        private const string Faults = Root + "faults";

        internal class V1
        {
            public const string Version = "/v1";
            public const string Account_Service = NS.Contract + "/account" + Version;
            public const string AccountPermission_Service = NS.Contract + "/accountpermission" + Version;
            public const string Autologin_Service = NS.Contract + "/autologin" + Version;
            public const string OAuth_Service = NS.Contract + "/oauth" + Version;
            public const string Authentication_Service = NS.Contract + "/authentication" + Version;
            public const string Config_Service = NS.Contract + "/config" + Version;

            public const string Authorization_Header = NS.Headers + "/authorization" + Version;
            public const string RequestContext_Header = NS.Headers + "/requestcontext" + Version;

            public const string Faults = NS.Faults + Version;
            public const string Schema = NS.Schema + Version;
        }
    }

}
