using Daivata.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Entities
{
    public enum AliasType {Facebook = 1, Twitter = 2};
    public class AccountAlias : DataObject
    {
        public long AccountId;
        public long AliasId;
        public AliasType Type;
        public string AliasName;

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.AccountId:
                    this.AccountId = field.As<long>();
                    break;
                case Fields.AliasId:
                    this.AliasId = field.As<long>();
                    break;
                case Fields.AliasType:
                    this.Type = field.As<AliasType>();
                    break;
                case Fields.AliasName:
                    this.AliasName = field.As<string>();
                    break;
            }
        }
    }
}
