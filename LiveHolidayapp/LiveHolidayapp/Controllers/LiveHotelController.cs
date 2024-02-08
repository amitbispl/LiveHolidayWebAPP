using Microsoft.AspNetCore.Mvc;

namespace LiveHolidayapp.Controllers
{
    public class LiveHotelController : Controller
    {
        
        public IActionResult SearchHotel() 
        {
            return View();
        } 
    }
}
