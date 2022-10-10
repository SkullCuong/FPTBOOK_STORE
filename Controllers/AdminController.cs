using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class AdminController : Controller
    {

        private string Layout ="AdminLayout"; 
        public IActionResult Index()
        {   
             ViewBag.Layout = Layout;
            return View();
        }

    }
}