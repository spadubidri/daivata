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
