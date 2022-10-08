using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FPTBOOK_STORE.Controllers
{
    public class BookController : Controller
    {
        private readonly MvcContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public BookController(MvcContext context, IWebHostEnvironment environment)
        {
            _context = context;

            hostEnvironment = environment;
        }
        

        // GET: Book
        public async Task<IActionResult> Index()
        {
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
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Id");
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Id");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,UploadImage,AuthorID,CategoryID,PublisherID")] Book book, IFormFile myfile)
        {
            if (!ModelState.IsValid)
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
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Id", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Id", book.PublisherID);
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Id", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Id", book.PublisherID);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,UploadImage,AuthorID,CategoryID,PublisherID")] Book book)
        {
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
            ViewData["AuthorID"] = new SelectList(_context.Author, "Id", "Id", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "Id", "Id", book.PublisherID);
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
