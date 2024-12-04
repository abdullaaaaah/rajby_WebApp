using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  [Authorize]
  public class SparesController : Controller
  {
    private readonly RajbyTextileContext context;

    public SparesController(RajbyTextileContext context)
    {
        this.context = context;
    }
    public IActionResult List()
    {
      return View();
    }
  }
}
