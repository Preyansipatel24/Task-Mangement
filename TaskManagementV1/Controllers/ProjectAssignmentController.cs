using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TaskManagementV1.Models;
using TaskManagementV1.Models.RequestModels;
using TaskManagementV1.Models.ResponseModels;

namespace TaskManagementV1.Controllers
{
    public class ProjectAssignmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CommonController _commonController;

        public ProjectAssignmentController(IConfiguration configuration, CommonController commonController)
        {
            _configuration = configuration;
            _commonController = commonController;
        }

        public async Task<IActionResult> Index(int ProjectId)
        {
            var PermissionList = HttpContext.Session.GetObjectFromSession<List<ActionDetailsList>>("PermissionList");
            if (PermissionList != null && PermissionList.Any(x => x.ActionCode == CommonConstant.Project_View))
            {
                CommonResponse response = new CommonResponse();
                try
                {
                    var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                    string apiUrl = BEBaseURL + "api/Project/GetProjectById";
                    var body = new
                    {
                        id = ProjectId
                    };

                    var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                    response.Status = apiResponse.status;
                    response.StatusCode = apiResponse.statusCode;
                    response.Message = apiResponse.message;
                    if (response.Status == true)
                    {
                        string dataString = JsonConvert.SerializeObject(apiResponse.data);
                        ProjectAddEditResModel responseObject = JsonConvert.DeserializeObject<ProjectAddEditResModel>(dataString);
                        response.Data = responseObject;
                    }
                }
                catch (Exception ex) { response.Message = ex.Message; }
                return View(response);
            }
            return RedirectToAction("Index", "Auth");
        }

        public async Task<IActionResult> ProjectUserList(int ProjectId, int DepartmentId, int DesignationId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Project/GetProjectUserList";
                var body = new
                {
                    ProjectId = ProjectId
                };

                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                response.Status = apiResponse.status;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data);
                    ProjectUserListResModel responseObject = JsonConvert.DeserializeObject<ProjectUserListResModel>(dataString);
                    if (DepartmentId > 0)
                    {
                        responseObject.ProjectUserDetailList = responseObject.ProjectUserDetailList.Where(x => x.DepartmentId == DepartmentId).ToList();
                    }
                    if (DesignationId > 0)
                    {
                        responseObject.ProjectUserDetailList = responseObject.ProjectUserDetailList.Where(x => x.DesignationId == DesignationId).ToList();
                    }
                    response.Data = responseObject;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return View(response);
        }

        public async Task<IActionResult> SaveUpdateProjectAssignment([FromBody] SaveUpdateProjectAssignmentReqModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Project/SaveUpdateProjectAssignment";
                var body = new
                {
                    ProjectId = request.ProjectId,
                    UserIdList = request.UserIdList
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

        public async Task<IActionResult> GetDepartmentList(int ProjectId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Project/GetProjectUserList";
                var body = new
                {
                    ProjectId = ProjectId
                };

                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                response.Status = apiResponse.status;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data);
                    ProjectUserListResModel responseObject = JsonConvert.DeserializeObject<ProjectUserListResModel>(dataString);
                    List<KeyValueResModel> DepartmentList = responseObject.ProjectUserDetailList.Select(x => new KeyValueResModel { Key = x.DepartmentName, Value = x.DepartmentId }).DistinctBy(x => x.Value).ToList();

                    response.Data = DepartmentList;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }
        public async Task<IActionResult> GetDesignationList(int ProjectId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Project/GetProjectUserList";
                var body = new
                {
                    ProjectId = ProjectId
                };

                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                response.Status = apiResponse.status;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data);
                    ProjectUserListResModel responseObject = JsonConvert.DeserializeObject<ProjectUserListResModel>(dataString);
                    List<KeyValueResModel> DepartmentList = responseObject.ProjectUserDetailList.Select(x => new KeyValueResModel { Key = x.DesignationName, Value = x.DesignationId }).DistinctBy(x => x.Value).ToList();

                    response.Data = DepartmentList;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }
    }
}
