using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using TaskManagementV1.Models;
using TaskManagementV1.Models.ResponseModels;

namespace TaskManagementV1.Controllers
{
    public class HomeController : Controller
    {
        private readonly CommonController _commonController;
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration, CommonController commonController)
        {
            _configuration = configuration;
            _commonController = commonController;
        }
        public async Task<IActionResult> Dashboard()
        {
            var PermissionList = HttpContext.Session.GetObjectFromSession<List<ActionDetailsList>>("PermissionList");
            if (PermissionList != null && PermissionList.Any(x => x.ActionCode == CommonConstant.Project_View))
            {
                CommonResponse response = new CommonResponse();
                try
                {
                    var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                    string apiUrl = BEBaseURL + "api/TaskManagement/GetAllDailyTaskList";
                    var body = new
                    {
                        pageNumber = 0,
                        pageSize = 0,
                        orderBy = true,
                        searchByString = "",
                        searchByStatus = "",
                        ProjectId = 0
                    };

                    var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);
                    if (apiResponse != null)
                    {
                        response.Status = apiResponse.status;
                        response.StatusCode = apiResponse.statusCode;
                        response.Message = apiResponse.message;
                        if (response.Status == true)
                        {
                            string dataString = JsonConvert.SerializeObject(apiResponse.data.dailyTaskDetailList);
                            List<GetTaskListResModel> responseObject = JsonConvert.DeserializeObject<List<GetTaskListResModel>>(dataString);
                            foreach (var item in responseObject)
                            {
                                item.TaskDateStr = item.TaskDate.ToString("dd-MM-yyyy");
                            }
                            response.Data = responseObject;
                        }
                    }
                }
                catch (Exception ex) { response.Message = ex.Message; }
                return View(response);
            }
            return RedirectToAction("Index", "Auth");
        }

    }
}
