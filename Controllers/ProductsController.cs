using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers;
public class ProductsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
