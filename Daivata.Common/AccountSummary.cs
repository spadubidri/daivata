using Daivata.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Entities
{
    public class AccountSummary : DataObject
    {
        public Guid ProfileID;
        public string FirstName;
        public string LastName;
        public string Email;
        public string AliasType;
        public string AliasName;

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.ProfileID:
                    this.ProfileID = field.As<Guid>();
                    break;
                case Fields.FirstName:
                    this.FirstName = field.As<string>();
                    break;
                case Fields.LastName:
                    this.LastName = field.As<string>();
                    break;
                case Fields.Email:
                    this.Email = field.As<string>();
                    break;
                //case Fields.AliasType:
                //    this.AliasType = field.As<string>();
                //    break;
                case Fields.AliasName:
                    this.AliasName = field.As<string>();
                    break;
            }
        }

    }
}
