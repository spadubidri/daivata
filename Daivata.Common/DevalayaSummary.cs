using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daivata.Database;

namespace Daivata.Entities
{
    public class DevalayaSummary : DataObject
    {
        public Guid Identifier;
        public string Title;
        public string ShortDescription;
        public string Location = "";
        public string ThumbNail = "/Img/NoImage.jpg";

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.Identifier:
                    this.Identifier = field.As<Guid>();
                    break;
                case Fields.Title:
                    this.Title = field.As<string>();
                    break;
                case Fields.ShortDescription:
                    this.ShortDescription = field.As<string>();
                    break;
                // TO DO
                case Fields.Location:
                    this.Location = field.As<string>();
                    break;
                case Fields.ThumbNailImage:
                    this.ThumbNail = field.As<string>();
                    if (string.IsNullOrEmpty(this.ThumbNail)) this.ThumbNail = "/Img/NoImage.jpg";
                    break;
            }
        }

    }
}
