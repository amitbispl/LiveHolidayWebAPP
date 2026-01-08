using LiveHolidayapp.Models;
using Newtonsoft.Json;
using System.Text;

namespace LiveHolidayapp.Repository
{
    public class R_Login 
    {
        General general = new General();
        
        public async Task<CommonResponse<Loginresponse>> UserLogin(Loginreq loginreq)
        {
            CommonResponse<Loginresponse> obj=new CommonResponse<Loginresponse>();
            try
            {
                var detail = JsonConvert.SerializeObject(loginreq);
                var resp = await general.CallPostFunction(detail);
                obj = JsonConvert.DeserializeObject<CommonResponse<Loginresponse>>(resp);
                //obj = output.Data;
                //obj.Message = output.Message;
                //obj.Code = output.Code;
            }
            catch(Exception ex)
            { 
            
            }
            return obj;
        }

        
    }
}
