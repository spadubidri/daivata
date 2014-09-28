using Daivata.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Entities
{
    public class Follower : DataObject
    {
        public long ID = 0;
        public Guid AssociationId;
        public Guid ProfileId;
        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.ID:
                    this.ID = field.As<long>();
                    break;
                case Fields.Identifier:
                    this.AssociationId = field.As<Guid>();
                    break;
                case Fields.ProfileID:
                    this.ProfileId = field.As<Guid>();
                    break;
            }
        }

    }
}
