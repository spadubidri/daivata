using Daivata.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Entities
{
    public class Gallery : DataObject
    {

        
        public long ID = 0;
        public long AlbumId;
        public string GalleryType;
        public string FileName;

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.ID:
                    this.ID = field.As<long>();
                    break;
                case Fields.AlbumID:
                    this.AlbumId = field.As<long>();
                    break;
                case Fields.GalleryType:
                    this.GalleryType = field.As<string>();
                    break;
                case Fields.GalleryFileName:
                    this.FileName = field.As<string>();
                    break;
            }
        }
        



    }
}
