using System.Data;

namespace Daivata.Database
{
    public interface IDbStorage
    {
        void Load(IDataRecord record);
    }
}
