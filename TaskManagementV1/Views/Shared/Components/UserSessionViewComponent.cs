using Microsoft.AspNetCore.Mvc;

namespace TaskManagementV1.Views.Shared.Component
{
    public class UserSessionViewComponent : ViewComponent
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public UserSessionViewComponent(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public IViewComponentResult Invoke()
        {
            // Retrieve session data (e.g., UserName)
            var token = HttpContext.Session.GetString("Token");
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrWhiteSpace(token))
            {
                // If session not found, redirect to login page
                HttpContext.Response.Redirect("/Auth/Index");
                return Content(""); // Return an empty response because redirect is triggered
            }

            // Pass the session value to the view (UserName) to render it
            return View("Default", userName);
        }
    }
}
