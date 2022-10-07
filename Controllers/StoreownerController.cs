using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class StoreownerController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }




    }
}