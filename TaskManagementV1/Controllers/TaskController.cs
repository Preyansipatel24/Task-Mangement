using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using TaskManagementV1.Models;
using TaskManagementV1.Models.RequestModels;
using TaskManagementV1.Models.ResponseModels;

namespace TaskManagementV1.Controllers
{
    public class TaskController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CommonController _commonController;

        public TaskController(IConfiguration configuration, CommonController commonController)
        {
            _configuration = configuration;
            _commonController = commonController;
        }

        public IActionResult Index()
        {
            var PermissionList = HttpContext.Session.GetObjectFromSession<List<ActionDetailsList>>("PermissionList");
            if (PermissionList != null && PermissionList.Any(x => x.ActionCode == CommonConstant.Task_View))
            {
                return View();
            }
            return RedirectToAction("Index", "Auth");
        }
        public async Task<IActionResult> GetTaskList(string? searchByString = "", string? searchByStatus = "", int? ProjectId = null)
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
                    searchByString = searchByString,
                    searchByStatus = searchByStatus,
                    ProjectId = ProjectId
                };

                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                //response.Status = apiResponse.status;
                response.Status = true;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data.dailyTaskDetailList);
                    List<GetTaskListResModel> responseObject = JsonConvert.DeserializeObject<List<GetTaskListResModel>>(dataString);
                    foreach (var item in responseObject)
                    {
                        item.TaskDateStr = item.TaskDate.ToString("dd-MM-yyyy");
                        item.TaskStartDateTimeStr = item.TaskStartDateTime != null ? item.TaskStartDateTime.Value.ToString("dd-MM-yyyy HH:mm") : "-";
                        item.TaskEndDateTimeStr = item.TaskEndDateTime != null ? item.TaskEndDateTime.Value.ToString("dd-MM-yyyy HH:mm") : "-";
                    }
                    response.Data = responseObject;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }
        public async Task<IActionResult> GetTaskStatusList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/TaskManagement/GetDailyTaskStatusList";
                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post);

                response.Status = apiResponse.status;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data);
                    List<LabelValueResModel> responseObject = JsonConvert.DeserializeObject<List<LabelValueResModel>>(dataString);
                    response.Data = responseObject;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }
        public async Task<IActionResult> GetCurrentUserProjectList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/TaskManagement/GetProjectListByLoggedInUserId";
                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post);

                response.Status = apiResponse.status;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data);
                    List<GetCurrentUserProjectListResModel> responseObject = JsonConvert.DeserializeObject<List<GetCurrentUserProjectListResModel>>(dataString);
                    response.Data = responseObject;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }
        public async Task<IActionResult> AddEditTask(int TaskId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (TaskId > 0)
                {
                    var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                    string apiUrl = BEBaseURL + "api/TaskManagement/GetDailyTaskById";
                    var body = new
                    {
                        id = TaskId
                    };

                    var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                    response.Status = apiResponse.status;
                    response.StatusCode = apiResponse.statusCode;
                    response.Message = apiResponse.message;
                    if (response.Status == true)
                    {
                        string dataString = JsonConvert.SerializeObject(apiResponse.data);
                        AddEditTaskResModel responseObject = JsonConvert.DeserializeObject<AddEditTaskResModel>(dataString);
                        response.Data = responseObject;
                    }
                }
                else
                {
                    response.Status = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Add Mode";
                    response.Data = new AddEditTaskResModel();
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUpdateTask([FromBody] SaveUpdateTaskReqModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.TaskEndDateTime == null || (request.TaskEndDateTime != null && request.TaskStartDateTime != null && request.TaskEndDateTime.Value > request.TaskStartDateTime.Value && request.TaskEndDateTime.Value <= request.TaskStartDateTime.Value.Date.Add(new TimeSpan(23, 59, 59))))
                {
                    request.TaskDate = request.TaskStartDateTime.Value.Date;
                    request.TaskDuration = "00:00";
                    if (request.TaskEndDateTime != null && request.TaskStartDateTime != null)
                    {
                        TimeSpan timeSpan = request.TaskEndDateTime.Value - request.TaskStartDateTime.Value;
                        request.TaskDuration = $"{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}";
                        //string.Format("{ 0:D2}:{ 1:D2}", (int)timeSpan.TotalHours, timeSpan.Minutes);
                    }

                    var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                    string apiUrl = BEBaseURL + "api/TaskManagement/AddEditDailyTask";
                    var body = new
                    {
                        id = request.Id,
                        projectId = request.ProjectId,
                        //taskDate = request.TaskDate,
                        //taskDuration = request.TaskDuration,
                        taskDate = request.TaskDate,
                        taskDuration = request.TaskDuration,
                        taskDescription = request.TaskDescription,
                        taskStatus = request.TaskStatus,
                        taskStartDateTime = request.TaskStartDateTime,
                        taskEndDateTime = request.TaskEndDateTime
                    };

                    var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                    response.Status = apiResponse.status;
                    response.StatusCode = apiResponse.statusCode;
                    response.Message = apiResponse.message;
                    if (response.Status == true)
                    {
                        string dataString = JsonConvert.SerializeObject(apiResponse.data);
                        var responseObject = JsonConvert.DeserializeObject(dataString);
                        response.Data = responseObject;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task end date-time must be greater then start date-time and both date must be same!";
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }

        public async Task<IActionResult> GetTaskDetailList(int ProjectId)
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
                    ProjectId = ProjectId,
                    UserId = -1
                };

                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                //response.Status = apiResponse.status;
                response.Status = true;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data.dailyTaskDetailList);
                    List<GetTaskListResModel> responseObject = JsonConvert.DeserializeObject<List<GetTaskListResModel>>(dataString);
                    foreach (var item in responseObject)
                    {
                        item.TaskDateStr = item.TaskDate.ToString("dd-MM-yyyy");
                        item.TaskStartDateTimeStr = item.TaskStartDateTime != null ? item.TaskStartDateTime.Value.ToString("dd-MM-yyyy HH:mm") : "-";
                        item.TaskEndDateTimeStr = item.TaskEndDateTime != null ? item.TaskEndDateTime.Value.ToString("dd-MM-yyyy HH:mm") : "-";
                        item.ProjectStartDateStr = item.ProjectStartDate != null ? item.ProjectStartDate.Value.ToString("dd-MM-yyyy") : "-";
                        item.ProjectEndDateStr = item.ProjectEndDate != null ? item.ProjectEndDate.Value.ToString("dd-MM-yyyy") : "NA";
                    }
                    response.Data = responseObject;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return View(response);
        }

    }
}
