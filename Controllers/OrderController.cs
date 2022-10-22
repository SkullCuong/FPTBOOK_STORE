using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;
using FPTBOOK_STORE.Utils;
using FPTBOOK_STORE.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace FPTBOOK_STORE.Controllers
{
    public class OrderController : Controller
    {   
        private string Layout ="StoreownerLayout"; 
        private readonly UserManager<FPTBOOKUser> _userManager;
        private readonly FPTBOOK_STOREIdentityDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public OrderController(FPTBOOK_STOREIdentityDbContext context, UserManager<FPTBOOKUser> userManager,   IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            hostEnvironment = environment;
        }
        public IActionResult PlaceOrder(decimal total)
        {
            ShoppingCart cart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
            Order myOrder = new Order();
            myOrder.OrderDate = DateTime.Now;
            myOrder.Status = 0;
            var userID = _userManager.GetUserId(HttpContext.User);
            FPTBOOKUser user = _userManager.FindByIdAsync(userID).Result;
            // if(user == null){
            //     return View("Book");
            // }
            myOrder.FPTBOOKUserId = user.Id;
            
            _context.Order.Add(myOrder);
            _context.SaveChanges();//this generates the Id for Order

            foreach (var item in cart.Items)
            {
                OrderDetail myOrderItem = new OrderDetail();
                myOrderItem.BookID = item.Id;
                myOrderItem.Quantity = item.Quantity;
                myOrderItem.OrderID = myOrder.Id;//id of saved order above

                _context.OrderDetail.Add(myOrderItem);
            }
            _context.SaveChanges();
            //empty shopping cart
            cart = new ShoppingCart();
            HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("BookHome", "Book");
        }
    }
}
