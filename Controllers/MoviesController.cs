using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MovieZone.Controllers
{
    public class MoviesController : Controller
    {
        // private readonly ILogger<MoviesController> _logger;  // logging API của .Net core hỗ trợ sẵn

        // public MoviesController(ILogger<MoviesController> logger)
        // {
        //     _logger = logger;
        // }

        public IActionResult Index()    // Một action của controller
        {
            ViewData ["message"] = "This is the message"; //Gắn dữ liệu vào ViewData để truyền cho View
            return View(); // Return về cái View có cùng tên với action đó. <First, the runtime looks in the Views/[ControllerName] folder for the view. If it doesn't find a matching view there, it searches the Shared folder for the view.>
            //IActionResult : interface của ActionResult, ko cần định trước là sẽ return về View hay resource
        }
    }
}