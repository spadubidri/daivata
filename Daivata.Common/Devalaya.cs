using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daivata.Database;

namespace Daivata.Entities
{
    public class Devalaya : DataObject
    {
        public long ID = 0;
        public Guid Identifier;
        public string Title;
        public string ShortDescription;
        public string Location = "";
        public string Details;
        public string MapLocation;
        public string Contact;
        public string FAQ;
        public string TravelDetails;
        public string TimingDetails;
        public string Status;
        public DateTime Created;
        public string CreatedBy;
        public string ThumbNail = "/Img/NoImage.jpg";
        public string Country = "";
        public string State = "";
        public string References = "";


        public IList<Album> Albums = new List<Album>();

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.ID:
                    this.ID = field.As<long>();
                    break;
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
                case Fields.Details:
                    this.Details = field.As<string>();
                    break;
                case Fields.MapLocation:
                    this.MapLocation = field.As<string>();
                    break;
                case Fields.Contact:
                    this.Contact = field.As<string>();
                    break;
                case Fields.FAQ:
                    this.FAQ = field.As<string>();
                    break;
                case Fields.CreatedDate:
                    this.Created = field.As<DateTime>();
                    break;
                case Fields.CreatedBy:
                    this.CreatedBy = field.As<string>();
                    break;
                case Fields.TimingDetails:
                    this.TimingDetails = field.As<string>();
                    break;
                case Fields.Status:
                    this.Status = field.As<string>();
                    break;
                case Fields.State:
                    this.State = field.As<string>();
                    break;
                case Fields.Country:
                    this.Country = field.As<string>();
                    break;
                case Fields.References:
                    this.References = field.As<string>();
                    break;
                case Fields.TravelDetails:
                    this.TravelDetails = field.As<string>();
                    break;
                case Fields.ThumbNailImage:
                    this.ThumbNail = field.As<string>();
                    if (string.IsNullOrEmpty(this.ThumbNail)) this.ThumbNail = "/Img/NoImage.jpg";
                    break;
            }
        }

    }
}
