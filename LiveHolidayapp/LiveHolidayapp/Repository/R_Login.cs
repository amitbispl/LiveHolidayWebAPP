using LiveHolidayapp.Models;
using Newtonsoft.Json;
using System.Text;

namespace LiveHolidayapp.Repository
{
    public class R_Login 
    {
        General general = new General();
        
        public async Task<Loginresponse> UserLogin(Loginreq loginreq)
        {
            Loginresponse obj = new Loginresponse();
            try
            {
                var detail = JsonConvert.SerializeObject(loginreq);
                var resp = await general.CallPostFunction(detail);
                var output = JsonConvert.DeserializeObject<CommonResponse<Loginresponse>>(resp);
                obj = output.Data;
            }
            catch(Exception ex)
            { 
            
            }
            return obj;
        }

        
    }
}
