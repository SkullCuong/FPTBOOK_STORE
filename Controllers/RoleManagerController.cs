using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using FPTBOOK_STORE.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FPTBOOK_STORE.Controllers;

public class RoleManagerController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleManagerController(RoleManager<IdentityRole> roleManager)
    {
        
        _roleManager = roleManager;
    }
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }
}