using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using TaskManagementV1.Models;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using TaskManagementV1.Models.ResponseModels;
using System.Net;
using TaskManagementV1.Models.RequestModels;

namespace TaskManagementV1.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CommonController _commonController;

        public ProjectController(IConfiguration configuration, CommonController commonController)
        {
            _configuration = configuration;
            _commonController = commonController;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProjectList(string? searchByString = "", string? searchByStatus = "")
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Project/GetAllProjectDetailList";
                var body = new
                {
                    pageNumber = 0,
                    pageSize = 0,
                    orderBy = true,
                    searchByString = searchByString,
                    searchByStatus = searchByStatus
                };

                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body);

                //response.Status = apiResponse.status;
                response.Status = true;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                response.Data = new List<ProjectDetailResModel>();
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data.projectDetailList);
                    List<ProjectDetailResModel> responseObject = JsonConvert.DeserializeObject<List<ProjectDetailResModel>>(dataString);
                    response.Data = responseObject;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }
        public async Task<IActionResult> GetProjectStatusList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Project/GetProjectStatusList";
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
        public async Task<IActionResult> AddEditProject(int ProjectId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (ProjectId > 0)
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
                else
                {
                    response.Status = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Add Mode";
                    response.Data = new ProjectAddEditResModel();
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUpdateProject([FromBody] ProjectSaveUpdateReqModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Project/AddEditProject";
                var body = new
                {
                    id = request.Id,
                    projectName = request.ProjectName,
                    projectStatus = request.ProjectStatus,
                    projectStartDate = request.ProjectStartDate,
                    projectEndDate = request.ProjectEndDate,
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

        public async Task<IActionResult> AssignUsers(int ProjectId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (ProjectId > 0)
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
                else
                {
                    response.Status = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Add Mode";
                    response.Data = new ProjectAddEditResModel();
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return View(response);
        }
    }
}
