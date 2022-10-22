using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace FPTBOOK_STORE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        
        private string Layout ="AdminLayout"; 
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {   

            Content("Admin");
             ViewBag.Layout = Layout;
            return View();
        }

    }
}