using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;

namespace FPTBOOK_STORE.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly MvcContext _context;

        public OrderDetailController(MvcContext context)
        {
            _context = context;
        }

        // GET: OrderDetail
        public async Task<IActionResult> Index()
        {
            var mvcContext = _context.OrderDetail.Include(o => o.Book).Include(o => o.Order);
            return View(await mvcContext.ToListAsync());
        }

        // GET: OrderDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Book)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetail/Create
        public IActionResult Create()
        {
            ViewData["BookID"] = new SelectList(_context.Book, "Id", "Id");
            ViewData["OrderID"] = new SelectList(_context.Order, "Id", "Id");
            return View();
        }

        // POST: OrderDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,OrderID,BookID")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookID"] = new SelectList(_context.Book, "Id", "Id", orderDetail.BookID);
            ViewData["OrderID"] = new SelectList(_context.Order, "Id", "Id", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["BookID"] = new SelectList(_context.Book, "Id", "Id", orderDetail.BookID);
            ViewData["OrderID"] = new SelectList(_context.Order, "Id", "Id", orderDetail.OrderID);
            return View(orderDetail);
        }

        // POST: OrderDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,OrderID,BookID")] OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.Id))
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
            ViewData["BookID"] = new SelectList(_context.Book, "Id", "Id", orderDetail.BookID);
            ViewData["OrderID"] = new SelectList(_context.Order, "Id", "Id", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Book)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetail == null)
            {
                return Problem("Entity set 'MvcContext.OrderDetail'  is null.");
            }
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetail.Remove(orderDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
          return (_context.OrderDetail?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
