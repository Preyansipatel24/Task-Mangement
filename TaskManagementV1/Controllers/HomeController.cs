using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagementV1.Models;

namespace TaskManagementV1.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Dashboard()
        {
            return View();
        }

    }
}
