using System.Collections.Generic;
using Daivata.Entities;
using System.Xml.Linq;
using Daivata.Database;

namespace Daivata.Repository
{
    public static class HomeSliderRepository
    {
        public static IList<HomeSlider> sliders = null;

         static HomeSliderRepository()
        {
            PopulateSliders();

        }

        private static void PopulateSliders(){

            //sliders = new List<HomeSlider>();
            ////read from XML file and populate sliders
            //XDocument sliderDoc = XDocument.Load("../Content/Slider/slider.xml");
            ////XElement sliderElement = sliderDoc.Elements("slide");
            //foreach (XElement sliderElement in sliderDoc.Elements("slide"))
            //{
            //    HomeSlider slider = new HomeSlider();
            //    slider.MainCaption = sliderElement.Element("maincaption").Value.ToString();
            //    slider.MainCaption = sliderElement.Element("shortdescription").Value.ToString();
            //    slider.Image = sliderElement.Element("image").Value.ToString();
            //    sliders.Add(slider);
            //}

            Query query = new StoredProcedure(Procedures.GetHomeSliders);

            sliders = Database.Database.GetItems<HomeSlider>(query);
            Database.Database.ExecuteQuery(query);

        }

        public static IList<HomeSlider> Refresh()
        {
            PopulateSliders();
            return sliders;
        }

    }
}
