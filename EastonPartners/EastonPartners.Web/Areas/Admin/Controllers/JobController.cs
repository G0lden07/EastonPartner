using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EastonPartners.Web.Controllers;
using RoverCore.BreadCrumbs.Services;

namespace EastonPartners.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class JobController : BaseController<JobController>
    {
        public IActionResult Index()
        {
	        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
		        .Then("Admin")
		        .ThenAction("Hangfire Dashboard", "Index", "Job", new { Area = "Admin" });

            return View();
        }
    }
}
