using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daivata.Database;


namespace Daivata.Entities
{
    public class Album : DataObject
    {
        public string AlbumTitle;
        public long ID = 0;
        public Guid CreatedBy;
        public string CreatedByName;
        public IList<Gallery> GalleryItems = new List<Gallery>();

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.ID:
                    this.ID = field.As<long>();
                    break;
                case Fields.CreatedBy:
                    this.CreatedBy = field.As<Guid>();
                    break;
                case Fields.AlbumTitle:
                    this.AlbumTitle = field.As<string>();
                    break;
                case Fields.CreatedByName:
                    this.CreatedByName = field.As<string>();
                    break;
            }
        }
        
        
    }
}
