using Microsoft.AspNetCore.Mvc;

namespace ProblemAnalysis3.Controllers
{
    public class QuoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
