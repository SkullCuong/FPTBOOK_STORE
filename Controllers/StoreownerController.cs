using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;

namespace MvcMovie.Controllers
{
    public class StoreownerController : Controller
    {
        private readonly MvcContext _context;

        public StoreownerController(MvcContext context)
        {
            _context = context;
        }
        private string Layout = "StoreownerLayout";
        public IActionResult Index()
        {

            ViewBag.Layout = Layout;
            return View();
        }
        public IActionResult Chart()
        {
            var data = _context.OrderDetail.Include(s => s.Book)
            .GroupBy(s => s.Book.Name)
            .Select(g => new { Name = g.Key, Total = g.Sum(s => s.Quantity) })
            .ToList();
            string[] labels = new string[data.Count()];
            string[] totals = new string[data.Count()];
            string[] rgbs = new string[data.Count()];
            Random rnd = new Random();
            int red = rnd.Next(0, 255);
            int blue = rnd.Next(0, 255);
            int green = rnd.Next(0, 255);
            for (int i = 0; i < data.Count(); i++)
            {
                labels[i] = data[i].Name;
                totals[i] = data[i].Total.ToString();

                rgbs[i] = ("'rgb(" + red.ToString() + "," + green.ToString() + "," + blue.ToString() + ")'");
            }


            ViewData["rgbs"] =  String.Join(",", rgbs);
            ViewData["labels"] = String.Format("'{0}'", String.Join("','", labels));
            ViewData["totals"] = String.Join(",", totals);

            ViewBag.Layout = Layout;
            return View();
        }

    }
}