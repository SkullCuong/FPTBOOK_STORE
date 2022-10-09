using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class StoreownerController : Controller
    {

        private string Layout ="StoreownerLayout"; 
        public IActionResult Index()
        {   
             ViewBag.Layout = Layout;
            return View();
        }

    }
}