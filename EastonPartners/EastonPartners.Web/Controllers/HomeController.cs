using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EastonPartners.Web.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : BaseController<HomeController>
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Help()
    {
        return View();
    }
    public IActionResult Assistant()
    {
        return View();
    }
}