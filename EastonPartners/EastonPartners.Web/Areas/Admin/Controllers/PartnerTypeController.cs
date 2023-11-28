using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoverCore.BreadCrumbs.Services;
using RoverCore.Datatables.DTOs;
using RoverCore.Datatables.Extensions;
using EastonPartners.Web.Controllers;
using EastonPartners.Infrastructure.Common.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EastonPartners.Domain.Entities;
using EastonPartners.Infrastructure.Persistence.DbContexts;

namespace EastonPartners.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PartnerTypeController : BaseController<PartnerTypeController>
{
	public class PartnerTypeIndexViewModel 
	{
		[Key]            
	    public int Id { get; set; }
	    public string Name { get; set; }
	}

	private const string createBindingFields = "Id,Name";
    private const string editBindingFields = "Id,Name";
    private const string areaTitle = "Admin";

    private readonly ApplicationDbContext _context;

    public PartnerTypeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/PartnerType
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage PartnerType");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<PartnerTypeIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Admin/PartnerType/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage PartnerType", "Index", "PartnerType", new { Area = "Admin" })
            .Then("PartnerType Details");            

        if (id == null)
        {
            return NotFound();
        }

        var partnerType = await _context.PartnerType
            .FirstOrDefaultAsync(m => m.Id == id);
        if (partnerType == null)
        {
            return NotFound();
        }

        return View(partnerType);
    }

    // GET: Admin/PartnerType/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage PartnerType", "Index", "PartnerType", new { Area = "Admin" })
            .Then("Create PartnerType");     

       return View();
	}

    // POST: Admin/PartnerType/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] PartnerType partnerType)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage PartnerType", "Index", "PartnerTypeController", new { Area = "Admin" })
        .Then("Create PartnerType");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            _context.Add(partnerType);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        return View(partnerType);
    }

    // GET: Admin/PartnerType/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage PartnerType", "Index", "PartnerType", new { Area = "Admin" })
        .Then("Edit PartnerType");     

        if (id == null)
        {
            return NotFound();
        }

        var partnerType = await _context.PartnerType.FindAsync(id);
        if (partnerType == null)
        {
            return NotFound();
        }
        

        return View(partnerType);
    }

    // POST: Admin/PartnerType/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(editBindingFields)] PartnerType partnerType)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage PartnerType", "Index", "PartnerType", new { Area = "Admin" })
        .Then("Edit PartnerType");  
    
        if (id != partnerType.Id)
        {
            return NotFound();
        }
        
        PartnerType model = await _context.PartnerType.FindAsync(id);

        model.Name = partnerType.Name;
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(editBindingFields);           

        if (ModelState.IsValid)
        {
            try
            {
                await _context.SaveChangesAsync();
                _toast.Success("Updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerTypeExists(partnerType.Id))
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
        return View(partnerType);
    }

    // GET: Admin/PartnerType/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage PartnerType", "Index", "PartnerType", new { Area = "Admin" })
        .Then("Delete PartnerType");  

        if (id == null)
        {
            return NotFound();
        }

        var partnerType = await _context.PartnerType
            .FirstOrDefaultAsync(m => m.Id == id);
        if (partnerType == null)
        {
            return NotFound();
        }

        return View(partnerType);
    }

    // POST: Admin/PartnerType/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var partnerType = await _context.PartnerType.FindAsync(id);
        _context.PartnerType.Remove(partnerType);
        await _context.SaveChangesAsync();
        
        _toast.Success("PartnerType deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool PartnerTypeExists(int id)
    {
        return _context.PartnerType.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetPartnerType(DtRequest request)
    {
        try
		{
			var query = _context.PartnerType;
			var jsonData = await query.GetDatatableResponseAsync<PartnerType, PartnerTypeIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PartnerType index json");
        }
        
        return StatusCode(500);
    }

}

