using Daivata.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Daivata.Models
{
    public class DevalayaListings
    {
        public IList<DevalayaSummary> Listings = new List<DevalayaSummary>();

        public int PageCount;

        public string Filter;
    }
}