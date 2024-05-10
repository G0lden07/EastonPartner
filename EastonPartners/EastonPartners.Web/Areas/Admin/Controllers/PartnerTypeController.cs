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
using EastonPartners.Web.Seeder;
using Parlot.Fluent;

namespace EastonPartners.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PartnerTypeController : BaseController<PartnerTypeController>
{
	// ViewModel for the Partner Index page
	public class PartnerTypeIndexViewModel 
	{
		[Key]            
	    public int Id { get; set; }
	    public string Name { get; set; }
	}

	// Define binding fields for creating and editing a Partner
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
		// Define breadcrumbs for navigation
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

		// Fetch the Partner type details
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
        PartnerTypeToJSON jsonConvertor = new PartnerTypeToJSON(_context);
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage PartnerType", "Index", "PartnerTypeController", new { Area = "Admin" })
        .Then("Create PartnerType");

        // Checks if name is empty
        if (!string.IsNullOrEmpty(partnerType.Name))
        {
            // Checks for duplicates
            var exists = await _context.PartnerType.FirstOrDefaultAsync(x => x.Name == partnerType.Name && x.Id != partnerType.Id);

            if (exists != null)
            {
                _toast.Error("Unable to create new partner type. This partner type already exists.");
                return RedirectToAction(nameof(Edit), new { id = exists.Id });
            }
        }

        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
			// Add the new Partner type to the context and save changes
			_context.Add(partnerType);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");

            jsonConvertor.copyToJSON();
            
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

		// Fetch the Partner details for editing
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
        PartnerTypeToJSON jsonConvertor = new PartnerTypeToJSON(_context);
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage PartnerType", "Index", "PartnerType", new { Area = "Admin" })
        .Then("Edit PartnerType");

		// Check if the provided ID matches the Partner type ID
		if (id != partnerType.Id)
        {
            return NotFound();
        }

		// Fetch the existing Partner type from the context
		PartnerType model = await _context.PartnerType.FindAsync(id);

        // Checks if name is empty
        if (!string.IsNullOrEmpty(partnerType.Name))
        {
            // Checks for duplicates
            var exists = await _context.PartnerType.FirstOrDefaultAsync(x => x.Name == partnerType.Name && x.Id != partnerType.Id);

            if (exists != null)
            {
                _toast.Error("Another partner type exists with the same name.");
                return RedirectToAction(nameof(Edit), new { id = exists.Id });
            }
        }

        // Update the model properties with values from the bound partner type
        model.Name = partnerType.Name;
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(editBindingFields);           

        if (ModelState.IsValid)
        {
            try
            {
				// Save changes to the context
				await _context.SaveChangesAsync();
                _toast.Success("Updated successfully.");
                jsonConvertor.copyToJSON();
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

		// Fetch the Partner type details for deletion
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
        PartnerTypeToJSON jsonConvertor = new PartnerTypeToJSON(_context);

        // Fetch the Partner type and remove it from the context, then save changes
        var partnerType = await _context.PartnerType.FindAsync(id);
        _context.PartnerType.Remove(partnerType);
        await _context.SaveChangesAsync();
        
        _toast.Success("PartnerType deleted successfully");

        jsonConvertor.copyToJSON();

        return RedirectToAction(nameof(Index));
    }

	// Check if a Partner type with the given ID exists in the context
	private bool PartnerTypeExists(int id)
    {
        return _context.PartnerType.Any(e => e.Id == id);
    }

	// Endpoint to get Partner data for Datatables
	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetPartnerType(DtRequest request)
    {
        try
		{
			// Query to fetch Partner type data
			var query = _context.PartnerType;

			// Get Datatables response JSON data
			var jsonData = await query.GetDatatableResponseAsync<PartnerType, PartnerTypeIndexViewModel>(request);

			// Return the JSON data
			return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PartnerType index json");
        }
        
        return StatusCode(500);
    }

}

