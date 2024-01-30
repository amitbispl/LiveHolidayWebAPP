using LiveHolidayapp.DataAccess;
using System.Collections;
using System.Data;

namespace LiveHolidayapp.Repository
{
    public class R_adminHandler :I_adminHandler
    {
        public DataSet GetCompanySetting(string companyId, string URL)
        {
            DataSet rds=new DataSet();
            try
            {
                BLLDBOperations bl = new BLLDBOperations();
                Hashtable hh = new Hashtable();
                hh.Add("companyId", companyId);
                hh.Add("URL", URL);
                DataSet ds = bl.GetDataSet("sp_GetCompanySetting", CommandType.StoredProcedure,hh);
                rds = ds;
            }
            catch(Exception ex)
            {

            }
            return rds;
        }
    }
}
