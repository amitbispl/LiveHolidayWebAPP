using LiveHolidayapp.Models;
using Newtonsoft.Json;
using System.Text;

namespace LiveHolidayapp.Repository
{
    public class R_Login 
    {
        public async Task<Loginresponse> UserLogin(Loginreq loginreq)
        {
            Loginresponse obj = new Loginresponse();
            try
            {
                var detail = JsonConvert.SerializeObject(loginreq);
                var resp = await CallPostFunction(detail);
                var output = JsonConvert.DeserializeObject<CommonResponse<Loginresponse>>(resp);
                obj = output.Data;
            }
            catch(Exception ex)
            { 
            
            }
            return obj;
        }

        private async Task<string> CallPostFunction(string detail)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var httpContent = new StringContent(detail, Encoding.UTF8, "application/json");

                    // Do the actual request and await the response
                    var httpResponse = await httpClient.PostAsync("http://holidayapi1.bisplindia.in/api/login", httpContent);

                    // If the response contains content we want to read it!
                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();

                        //var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        return responseContent;
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            return null;
        }
    }
}
