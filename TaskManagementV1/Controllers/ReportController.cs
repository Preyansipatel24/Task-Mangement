using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TaskManagementV1.Models;
using TaskManagementV1.Models.ResponseModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagementV1.Controllers
{
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CommonController _commonController;

        public ReportController(IConfiguration configuration, CommonController commonController)
        {
            _configuration = configuration;
            _commonController = commonController;
        }
        public IActionResult Index()
        {
            var PermissionList = HttpContext.Session.GetObjectFromSession<List<ActionDetailsList>>("PermissionList");
            if (PermissionList != null && PermissionList.Any(x => x.ActionCode == CommonConstant.Report_View))
            {
                return View();
            }
            return RedirectToAction("Index", "Auth");
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

        public async Task<IActionResult> GetUserList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Project/GetProjectUserDetailList";
                
                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, null);

                //response.Status = apiResponse.status;
                response.Status = true;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                response.Data = new List<ProjectUserDetail>();
                if (response.Status == true)
                {
                    string dataString = JsonConvert.SerializeObject(apiResponse.data);
                    List<ProjectUserDetail> responseObject = JsonConvert.DeserializeObject<List<ProjectUserDetail>>(dataString);
                    response.Data = responseObject;
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return Json(response);
        }

        public async Task<IActionResult> DownloadReport(bool IsUserWiseReport, int UserId, int ProjectId, DateTime? FromDate, DateTime? ToDate)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/TaskManagement/GenerateReport";

                var body = new
                {
                    IsUserWiseReport = IsUserWiseReport,
                    FromDate = FromDate,
                    ToDate = ToDate,
                    UserId = UserId,
                    ProjectId = ProjectId
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
