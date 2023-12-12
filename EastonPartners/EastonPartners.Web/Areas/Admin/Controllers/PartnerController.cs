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
public class PartnerController : BaseController<PartnerController>
{
	public class PartnerIndexViewModel 
	{
		[Key]            
	    public int PartnerId { get; set; }
	    public string Name { get; set; }
	    public string Description { get; set; }
	    public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
	    public string Website { get; set; }
	    public int PartnerTypeId { get; set; }
	}

	private const string createBindingFields = "PartnerId,Name,Description,PhoneNumber,Email,Address,City,State,PostalCode,Website,PartnerTypeId";
    private const string editBindingFields = "PartnerId,Name,Description,PhoneNumber,Email,Address,City,State,PostalCode,Website,PartnerTypeId";
    private const string areaTitle = "Admin";

    private readonly ApplicationDbContext _context;

    public PartnerController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Partner
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage Partner");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<PartnerIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Admin/Partner/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Partner", "Index", "Partner", new { Area = "Admin" })
            .Then("Partner Details");            

        if (id == null)
        {
            return NotFound();
        }

        var partner = await _context.Partner
                .Include(p => p.PartnerType)
            .FirstOrDefaultAsync(m => m.PartnerId == id);
        if (partner == null)
        {
            return NotFound();
        }

        return View(partner);
    }

    // GET: Admin/Partner/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Partner", "Index", "Partner", new { Area = "Admin" })
            .Then("Create Partner");     

        ViewData["PartnerTypeId"] = new SelectList(_context.PartnerType, "Id", "Name");

       return View();
	}

    // POST: Admin/Partner/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Partner partner)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Partner", "Index", "PartnerController", new { Area = "Admin" })
        .Then("Create Partner");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            _context.Add(partner);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        ViewData["PartnerTypeId"] = new SelectList(_context.PartnerType, "Id", "Name", partner.PartnerTypeId);
        return View(partner);
    }

    // GET: Admin/Partner/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Partner", "Index", "Partner", new { Area = "Admin" })
        .Then("Edit Partner");     

        if (id == null)
        {
            return NotFound();
        }

        var partner = await _context.Partner.FindAsync(id);
        if (partner == null)
        {
            return NotFound();
        }
        
        ViewData["PartnerTypeId"] = new SelectList(_context.PartnerType, "Id", "Name", partner.PartnerTypeId);

        return View(partner);
    }

    // POST: Admin/Partner/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(editBindingFields)] Partner partner)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Partner", "Index", "Partner", new { Area = "Admin" })
        .Then("Edit Partner");  
    
        if (id != partner.PartnerId)
        {
            return NotFound();
        }
        
        Partner model = await _context.Partner.FindAsync(id);

        model.Name = partner.Name;
        model.Description = partner.Description;
        model.PhoneNumber = partner.PhoneNumber;
        model.Email = partner.Email;
        model.Address = partner.Address;
        model.City = partner.City;
        model.State = partner.State;
        model.PostalCode = partner.PostalCode;
        model.Website = partner.Website;
        model.PartnerTypeId = partner.PartnerTypeId;
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
                if (!PartnerExists(partner.PartnerId))
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
        ViewData["PartnerTypeId"] = new SelectList(_context.PartnerType.OrderBy(x => x.Name), "Id", "Name", partner.PartnerTypeId);
        return View(partner);
    }

    // GET: Admin/Partner/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Partner", "Index", "Partner", new { Area = "Admin" })
        .Then("Delete Partner");  

        if (id == null)
        {
            return NotFound();
        }

        var partner = await _context.Partner
                .Include(p => p.PartnerType)
            .FirstOrDefaultAsync(m => m.PartnerId == id);
        if (partner == null)
        {
            return NotFound();
        }

        return View(partner);
    }

    // POST: Admin/Partner/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var partner = await _context.Partner.FindAsync(id);
        _context.Partner.Remove(partner);
        await _context.SaveChangesAsync();
        
        _toast.Success("Partner deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool PartnerExists(int id)
    {
        return _context.Partner.Any(e => e.PartnerId == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetPartner(DtRequest request)
    {
        try
		{
			var query = _context.Partner.Include(p => p.PartnerType);
			var jsonData = await query.GetDatatableResponseAsync<Partner, PartnerIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Partner index json");
        }
        
        return StatusCode(500);
    }

}

