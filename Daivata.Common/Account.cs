using Daivata.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Entities
{
    public class Account : DataObject
    {
        public long AccountId = 0;
        public Guid ProfileID;
        public string Status;
        public IList<AccountAlias> Aliases;
        public AccountProfile Profile;

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.AccountId:
                    this.AccountId = field.As<long>();
                    break;
                case Fields.ProfileID:
                    this.ProfileID = field.As<Guid>();
                    break;
                case Fields.Status:
                    this.Status = field.As<string>();
                    break;
            }
        }

    }
}
