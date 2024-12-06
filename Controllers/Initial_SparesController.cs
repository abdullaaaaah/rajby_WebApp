using Microsoft.AspNetCore.Mvc;

namespace Rajby_web.Controllers
{
  public class Initial_SparesController : Controller
  {
    public IActionResult List()
    {
      return View();
    }
  }
}
