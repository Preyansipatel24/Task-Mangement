using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
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
            var UserDetail = HttpContext.Session.GetObjectFromSession<UserInfoDetail>("UserDetail");
            int LoggedInUserId = UserDetail != null ? UserDetail.Id : 0;
            CommonResponse response = new CommonResponse();
            try
            {
                DashboardResModel dashboardResModel = new DashboardResModel();
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;

                #region TaskList
                dashboardResModel.TaskList = new List<GetTaskListResModel>();
                string apiUrl = BEBaseURL + "api/TaskManagement/GetAllDailyTaskList";
                var body = new
                {
                    pageNumber = 0,
                    pageSize = 0,
                    orderBy = true,
                    searchByString = "",
                    searchByStatus = "",
                    ProjectId = 0,
                    UserId = -1
                };

                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);
                if (apiResponse != null)
                {
                    if (apiResponse.status == true)
                    {
                        string dataString = JsonConvert.SerializeObject(apiResponse.data.dailyTaskDetailList);
                        dashboardResModel.TaskList = JsonConvert.DeserializeObject<List<GetTaskListResModel>>(dataString);
                        foreach (var item in dashboardResModel.TaskList)
                        {
                            item.TaskDateStr = item.TaskDate.ToString("dd-MM-yyyy");
                        }
                    }
                    else
                    {
                        response.StatusCode = apiResponse.statusCode;
                        response.Message = apiResponse.message;
                    }
                }
                #endregion

                #region ProjectList
                dashboardResModel.ProjectList = new List<ProjectDetailResModel>();
                apiUrl = BEBaseURL + "api/Project/GetAllProjectDetailList";
                var GetAllProjectDetailListBody = new
                {
                    pageNumber = 0,
                    pageSize = 0,
                    orderBy = true,
                    searchByString = "",
                    searchByStatus = ""
                };

                apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, GetAllProjectDetailListBody);
                if (apiResponse != null)
                {
                    if (apiResponse.status == true)
                    {
                        string dataString = JsonConvert.SerializeObject(apiResponse.data.projectDetailList);
                        dashboardResModel.ProjectList = JsonConvert.DeserializeObject<List<ProjectDetailResModel>>(dataString);
                        //dashboardResModel.ProjectList = dashboardResModel.ProjectList.Where(x => x.ReportingPersonUserId == LoggedInUserId).ToList();
                        foreach (var item in dashboardResModel.ProjectList)
                        {
                            item.ProjectStartDateStr = item.ProjectStartDate != null ? item.ProjectStartDate.Value.ToString("dd-MM-yyyy") : "NA";
                            item.ProjectEndDateStr = item.ProjectEndDate != null ? item.ProjectEndDate.Value.ToString("dd-MM-yyyy") : "NA";
                        }
                    }
                    else
                    {
                        response.StatusCode = apiResponse.statusCode;
                        response.Message = apiResponse.message;
                    }
                }
                #endregion

                response.Status = true;
                response.Data = dashboardResModel;
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return View(response);
        }

    }
}
