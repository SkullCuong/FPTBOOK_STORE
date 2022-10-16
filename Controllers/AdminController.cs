using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace FPTBOOK_STORE.Controllers
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