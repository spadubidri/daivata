using System.Security.Claims;
using System.Collections.Generic;
using System.Collections;

namespace Daivata.UI
{
    public class DaivataIdentity : ClaimsIdentity
    {
        private bool _IsAuthenticated;
        private string name = "";
        public DaivataIdentity(bool isAuthenticated)
        {
            this._IsAuthenticated = isAuthenticated;
        }

        public override bool IsAuthenticated
        {
            get {
                return _IsAuthenticated;
            }
        }

        public override void AddClaim(Claim claim)
        {
            if (claim.Type == "name")
                name = claim.Value;

            base.AddClaim(claim);
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }


    }
}