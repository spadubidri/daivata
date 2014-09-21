using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daivata.Database;

namespace Daivata.Entities
{
    public class HomeSlider : PersistedDataObject
    {
        public string MainCaption = "";
        public string ShortDescription = "";
        public string Image = "";

        protected override void ReadField(Field field)
        {
            switch (field.Name)
            {
                case Fields.SliderMainCaption:
                    this.MainCaption = field.As<string>();
                    break;
                case Fields.SliderShortDescription:
                    this.ShortDescription = field.As<string>();
                    break;
                case Fields.SliderImage:
                    this.Image = field.As<string>();
                    break;
            }
        }
    }
}
