using Daivata.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Entities
{
    public class AccountProfile : DataObject
    {
        public long ID = 0;
        public long AccountId;
        public string FirstName;
        public string LastName;
        public string Email;
        public string ContactNumber;
        public string Source;

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.ID:
                    this.ID = field.As<long>();
                    break;
                case Fields.AccountId:
                    this.AccountId = field.As<long>();
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
                case Fields.ContactNumber:
                    this.ContactNumber = field.As<string>();
                    break;
                case Fields.Source:
                    this.Source = field.As<string>();
                    break;
            }

        }
        

    }
}
