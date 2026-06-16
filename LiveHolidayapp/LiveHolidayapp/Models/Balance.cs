using LiveHolidayapp.Repository;
using Newtonsoft.Json;
using System.Data;

namespace LiveHolidayapp.Models
{
    public class Balance
    {
        General gen = new General();
        R_Hotel _Hotel = new R_Hotel();

        public WalletBal NexaBalace(Loginreq balreq, string Authnekot)
        {
            WalletBal wbal = new WalletBal();
            try
            {
                var walletres = _Hotel.Getuserbalance(balreq, Authnekot);
                var walletoutput = JsonConvert.DeserializeObject<CommonResponse<string>>(walletres);
                if (walletoutput.Code == 200)
                {
                    DataSet dswallet = new DataSet();
                    dswallet = gen.convertJsonStringToDataSet(walletoutput.Data);
                    if (Convert.ToString(dswallet.Tables[0].Rows[0]["response"]) == "OK" && Convert.ToString(dswallet.Tables[0].Rows[0]["msg"]) == "success")
                    {
                        wbal.Balance = Convert.ToString(dswallet.Tables[0].Rows[0]["couponwallet"]);
                    }
                }
                else
                {
                    wbal.Balance = "0";
                }
            }
            catch
            {

            }
            return wbal;
        }
    }
}
