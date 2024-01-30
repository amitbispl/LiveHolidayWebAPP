using System.Data;

namespace LiveHolidayapp.DataAccess
{
    public interface I_adminHandler
    {
        DataSet GetCompanySetting(string companyId, string URL);
    }
}
