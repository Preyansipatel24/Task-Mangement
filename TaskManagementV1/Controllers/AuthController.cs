using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using TaskManagementV1.Models;
using TaskManagementV1.Models.RequestModels;
using TaskManagementV1.Models.ResponseModels;

namespace TaskManagementV1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CommonController _commonController;
        private HttpClient _client;
        public AuthController(IConfiguration configuration, CommonController commonController)
        {
            _configuration = configuration;
            _commonController = commonController;
            _client = new HttpClient();
        }
        public IActionResult Index()
        {
            var FEBaseURL = _configuration.GetSection("SiteConfiguration:FEBaseURL").Value;
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                HttpContext.Session.Remove("UserDetail");
                HttpContext.Session.Remove("PermissionList");
                HttpContext.Session.Remove("Token");
            }
            ViewBag.FEBaseURL = FEBaseURL;
            return View();
        }

        [HttpPost]
        public async Task<CommonResponse> Login(LoginReqModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var BEBaseURL = _configuration.GetSection("SiteConfiguration:BEBaseURL").Value;
                string apiUrl = BEBaseURL + "api/Auth/Login";
                string EncryptedPassword = _commonController.EncryptString(request.Password);
                var body = new
                {
                    CompanyEmail = request.EmailId,
                    Password = EncryptedPassword
                };

                var apiResponse = await _commonController.CallApiAsync(apiUrl, HttpMethod.Post, body, false);

                response.Status = apiResponse.status;
                response.StatusCode = apiResponse.statusCode;
                response.Message = apiResponse.message;
                if (response.Status == true)
                {
                    // Parse response into dynamic object
                    string dataString = JsonConvert.SerializeObject(apiResponse.data);
                    LoginResModel responseObject = JsonConvert.DeserializeObject<LoginResModel>(dataString);

                    //string FirstName = responseObject.UserDetail.FirstName;
                    //string LastName = responseObject.UserDetail.LastName;
                    //int RoleId = responseObject.UserDetail.RoleId;
                    //string RoleName = responseObject.UserDetail.RoleName;
                    //string Token = apiResponse.data.token;

                    if (responseObject.UserDetail.RoleId > 1)
                    {
                        var SubModuleList = responseObject.roleAndPermissionList.FirstOrDefault(x => x.ModuleCode == CommonConstant.Task_Management).SubModuleDetailsLists;
                        List<ActionDetailsList> actionDetailList = new List<ActionDetailsList>();
                        foreach (var item in SubModuleList)
                        {
                            actionDetailList.AddRange(item.ActionLists.Where(x=>x.IsClick==true).ToList());
                        }

                        HttpContext.Session.SetObjectInSession("UserDetail", responseObject.UserDetail);
                        HttpContext.Session.SetObjectInSession("PermissionList", actionDetailList);
                        HttpContext.Session.SetString("Token", responseObject.Token);
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Message = "Super Admin User Is Not Allowed To Login!";
                    }
                }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }

        public IActionResult Logout()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                HttpContext.Session.Remove("UserDetail");
                HttpContext.Session.Remove("PermissionList");
                HttpContext.Session.Remove("Token");
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return View("Index");
        }
    }
}
