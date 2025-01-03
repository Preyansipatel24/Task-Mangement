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
            return View();
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
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/TaskManagement/AddEditDailyTask";
                var body = new
                {
                    id = request.Id,
                    projectId = request.ProjectId,
                    taskDate = request.TaskDate,
                    taskDuration = request.TaskDuration,
                    taskDescription = request.TaskDescription,
                    taskStatus = request.TaskStatus
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
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }

    }
}
