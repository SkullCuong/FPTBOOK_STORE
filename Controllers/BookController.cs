using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;
using Microsoft.Extensions.Caching.Memory;
using FPTBOOK_STORE.Utils;

namespace FPTBOOK_STORE.Controllers
{
    public class BookController : Controller
    {
        private readonly MvcContext _context;
         private string Layout ="StoreownerLayout"; 
        private readonly IWebHostEnvironment hostEnvironment;

        public BookController(MvcContext context, IWebHostEnvironment environment)
        {
            _context = context;

            hostEnvironment = environment;
        }
        

        // GET: Book
        public async Task<IActionResult> Index()
        {
            ViewBag.Layout = Layout;
            var mvcContext = _context.Book.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher);
            return View(await mvcContext.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewBag.Layout = Layout;
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name");
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Name");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,UploadImage,AuthorID,CategoryID,PublisherID")] Book book, IFormFile myfile)
        {
            ViewBag.Layout = Layout;
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileName(myfile.FileName);
                var filePath = Path.Combine(hostEnvironment.WebRootPath, "uploads");
                string fullPath = filePath + "\\" + filename;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await myfile.CopyToAsync(stream);
                }
                book.UploadImage = filename;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Name", book.PublisherID);
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Layout = Layout;
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Name", book.PublisherID);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,UploadImage,AuthorID,CategoryID,PublisherID")] Book book)
        {
            ViewBag.Layout = Layout;
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Name", book.PublisherID);
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Layout = Layout;
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Layout = Layout;
            if (_context.Book == null)
            {
                return Problem("Entity set 'MvcContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          ViewBag.Layout = Layout;
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        public IActionResult AddToCart(int id,string name,double price,int quantity)
        {
           ShoppingCart myCart;

        if (HttpContext.Session.GetObject<ShoppingCart>("cart") == null) {
            myCart = new ShoppingCart();
            HttpContext.Session.SetObject("cart",myCart);
        } 
        myCart= (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
        var newItem=myCart.AddItem(id,name,price,quantity);
        HttpContext.Session.SetObject("cart",myCart);
        ViewData["newItem"]=newItem;
        return View();
        }
        public IActionResult CheckOut()
        {            
            ShoppingCart cart=(ShoppingCart) HttpContext.Session.GetObject<ShoppingCart>("cart");
            ViewData["myItems"]=cart.Items;
            return View();
        }
        public IActionResult PlaceOrder(decimal total)
        {            
            ShoppingCart cart=(ShoppingCart) HttpContext.Session.GetObject<ShoppingCart>("cart");
            Order myOrder=new Order();
            myOrder.OrderDate=DateTime.Now;
            myOrder.Status=0;
            // myOrder.User =;
            _context.Order.Add(myOrder);
            _context.SaveChanges();//this generates the Id for Order
           
            foreach(var item in cart.Items)
            {
                OrderDetail myOrderItem=new OrderDetail();
                myOrderItem.BookID=item.Id;
                myOrderItem.Quantity=item.Quantity;
                myOrderItem.OrderID=myOrder.Id;//id of saved order above
                
                _context.OrderDetail.Add(myOrderItem);
            }
            _context.SaveChanges();
            //empty shopping cart
            cart=new ShoppingCart();
            HttpContext.Session.SetObject("cart",cart);
            return View();
        }
        [HttpPost]
        public RedirectToActionResult EditOrder(int id,int quantity)
        {
            ShoppingCart cart=(ShoppingCart) HttpContext.Session.GetObject<ShoppingCart>("cart");
            cart.EditItem(id,quantity);
            HttpContext.Session.SetObject("cart",cart);
            
            return RedirectToAction("CheckOut", "Book");
        }
        [HttpPost]
        public RedirectToActionResult RemoveOrderItem(int id)
        {
            ShoppingCart cart=(ShoppingCart) HttpContext.Session.GetObject<ShoppingCart>("cart");
            cart.RemoveItem(id);
            HttpContext.Session.SetObject("cart",cart);
            
            return RedirectToAction("CheckOut", "Book");
        }
        public async Task<IActionResult> BookHome(){
            var mvcContext = _context.Book.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher);
            return View(await mvcContext.ToListAsync());
        }
        public async Task<IActionResult> ContactUs(){
            return View();
        }
    }
}
