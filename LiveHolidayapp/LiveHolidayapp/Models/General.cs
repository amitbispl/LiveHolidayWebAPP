using System.Net;
using System.Reflection;
using System.Text;

namespace LiveHolidayapp.Models
{
    public class General
    {
        public async Task<string> CallPostFunction(string detail)
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

        public string CallPostFunction(string detail, string jwtToken, string Action)
        {
            string result = string.Empty;
            string apiUrl = "http://holidayapi1.bisplindia.in/api/" + Action + "";
            try
            {
                HttpWebRequest request = WebRequest.Create(apiUrl) as HttpWebRequest;
                request.ContentType = @"application/json";
                request.Headers.Add("Authorization", "Bearer " + jwtToken);
                request.Method = @"POST";
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(detail);
                requestWriter.Close();
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                result = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return result;
        }

        public async Task<string> CallPostFunctionAsync(string detail,string jwtToken,string Action)  
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Add headers to the request
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+jwtToken);
                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var httpContent = new StringContent(detail, Encoding.UTF8, "application/json");
                    // Do the actual request and await the response
                    var httpResponse = await httpClient.PostAsync("http://holidayapi1.bisplindia.in/api/"+ Action + "", httpContent);
                    // If the response contains content we want to read it!
                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
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
