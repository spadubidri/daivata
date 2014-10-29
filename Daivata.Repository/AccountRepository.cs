using Daivata.Database;
using Daivata.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Repository
{
    public class AccountRepository
    {


        public Account CreateProfile(AccountProfile profileData, string aliasName)
        {

            Query query = new StoredProcedure(Procedures.CreateAccountProfile);
            query["@firstname"] = profileData.FirstName;
            query["@lastname"] = profileData.LastName;
            query["@email"] = profileData.Email;
            query["@source"] = profileData.Source;
            query["@contactnumber"] = profileData.ContactNumber;
            query["@aliasName"] = aliasName;

            //Account albumDetails = Database.Database.GetItem<Account>(query);
            Database.Database.ExecuteQuery(query);
            Account acct = new Account();
            IList<AccountAlias> aliases = new List<AccountAlias>();
            AccountProfile profile = new AccountProfile();

            Database.Database.ForEach(query, (rs, lc) =>
            {
                //enumerate each of the resultset's returned by the query
                switch (rs.Index)
                {
                    case 0: //accounts
                        acct = rs.GetItem<Account>();
                        break;
                    case 1: //aliases
                            aliases = rs.GetItems<AccountAlias>();
                        break;
                    case 2: //aliases
                        profile = rs.GetItem<AccountProfile>();
                        break;
                }
            }, System.Data.CommandBehavior.CloseConnection | System.Data.CommandBehavior.SequentialAccess);

            if (acct != null)
            {
                acct.Aliases = aliases;
                acct.Profile = profile;
            }

            return acct; 

        }

        public Account GetAccountDetails(Guid profileId)
        {
            Query query = new StoredProcedure(Procedures.GetAccountDetailsByProfileId);
            query["@profileId"] = profileId;

            Database.Database.ExecuteQuery(query);
            Account acct = new Account();
            IList<AccountAlias> aliases = new List<AccountAlias>();
            AccountProfile profile = new AccountProfile();

            Database.Database.ForEach(query, (rs, lc) =>
            {
                //enumerate each of the resultset's returned by the query
                switch (rs.Index)
                {
                    case 0: //accounts
                        acct = rs.GetItem<Account>();
                        break;
                    case 1: //aliases
                        aliases = rs.GetItems<AccountAlias>();
                        break;
                    case 2: //aliases
                        profile = rs.GetItem<AccountProfile>();
                        break;
                }
            }, System.Data.CommandBehavior.CloseConnection | System.Data.CommandBehavior.SequentialAccess);

            if (acct != null)
            {
                acct.Aliases = aliases;
                acct.Profile = profile;
            }
            return acct;

        }

        public Account SearchAccount(AliasType type, string aliasName)
        {

            Query query = new StoredProcedure(Procedures.GetAccountByAliasId);
            query["@aliasId"] = (int)type;
            query["@aliasName"] = aliasName;

            Database.Database.ExecuteQuery(query);
            Account acct = new Account();
            IList<AccountAlias> aliases = new List<AccountAlias>();
            AccountProfile profile = new AccountProfile();

            Database.Database.ForEach(query, (rs, lc) =>
            {
                //enumerate each of the resultset's returned by the query
                switch (rs.Index)
                {
                    case 0: //accounts
                        acct = rs.GetItem<Account>();
                        break;
                    case 1: //aliases
                        aliases = rs.GetItems<AccountAlias>();
                        break;
                    case 2: //aliases
                        profile = rs.GetItem<AccountProfile>();
                        break;
                }
            }, System.Data.CommandBehavior.CloseConnection | System.Data.CommandBehavior.SequentialAccess);

            if (acct != null)
            {
                acct.Aliases = aliases;
                acct.Profile = profile;
            }

            return acct; 
        }

        public IList<Follower> GetAllAssociations(Guid profileId)
        {
            IList<Follower> following = new List<Follower>();
            Query query = new StoredProcedure(Procedures.GetAllFollowingAssociations);
            query["@profileId"] = profileId;
            following = Database.Database.GetItems<Follower>(query);
            return following;
        }


        public void UpdateProfile(AccountProfile profileData, Guid profileId)
        {
            Query query = new StoredProcedure(Procedures.UpdateAccountProfile);
            query["@firstname"] = profileData.FirstName;
            query["@lastname"] = profileData.LastName;
            query["@email"] = profileData.Email;
            query["@contactnumber"] = profileData.ContactNumber;
            query["@profileId"] = profileId;

            Database.Database.ExecuteQuery(query);
        }

        public IList<AccountSummary> GetAllAccounts()
        {
            IList<AccountSummary> summary = new List<AccountSummary>();
            Query query = new StoredProcedure(Procedures.GetAccountSummary);
            summary = Database.Database.GetItems<AccountSummary>(query);

            return summary;
        }
    }
}
