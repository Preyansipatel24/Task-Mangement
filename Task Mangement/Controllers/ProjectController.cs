using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using Task_Mangement.Models;


namespace Task_Mangement.Controllers
{
    public class ProjectController : Controller
    {
        private IConfiguration _configuration { get; }

        public ProjectController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private static readonly HttpClient client = new HttpClient();

        private string baseUrl = "https://localhost:7046";
        //private string  baseUrl = "https://rserp-be-dev.archesoftronix.in";
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginViewModel request)
        {
            string apiUrl = baseUrl + "/api/Auth/Login"; // Replace with the actual API URL
            var requestData = new
            {
                CompanyEmail = request.CompanyEmail,
                Password = EncryptString(request.Password).Result
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                if (jsonResponse.status == true)
                {
                    string token = jsonResponse.data.token;
                    HttpContext.Session.SetString("SessionKey", token);
                    return RedirectToAction("ProjectIndex");
                }
                else
                { // Handle the case where status is false
                    ViewBag.message = jsonResponse.message.ToString();
                    return View(Login);
                }
            }
            else
            {
                //return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                ViewBag.message = response.StatusCode.ToString();
                return View(Login);
            }
        }

        public async Task<string> EncryptString(string plainText)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["EncryptionKeys:EncryptionSecurityKey"].ToString());
            var iv = Encoding.UTF8.GetBytes(_configuration["EncryptionKeys:EncryptionSecurityIV"].ToString());
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.

            return Convert.ToBase64String(encrypted);
            //return encrypted;
        }

        public async Task<string> DecryptString(string cipherText)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["EncryptionKeys:EncryptionSecurityKey"].ToString());
            var iv = Encoding.UTF8.GetBytes(_configuration["EncryptionKeys:EncryptionSecurityIV"].ToString());
            var encrypted = Convert.FromBase64String(cipherText);
            // Check arguments.
            if (encrypted == null || encrypted.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(encrypted))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }
        public async Task<IActionResult> ProjectIndex()
        {
            string apiUrl = baseUrl + "/api/Project/GetAllProjectDetailList";

            var requestData = new
            {
                pageNumber = 0,
                pageSize = 0,
                orderBy = true,
                searchByString = "",
                searchByStatus = ""
            };

            var sessionValue = HttpContext.Session.GetString("SessionKey");
            string bearerToken = sessionValue;
            client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                if (jsonResponse.status == true)
                {
                    var jsonString = jsonResponse.data;
                    JObject jsonResponseObject = JObject.Parse(Convert.ToString(jsonString));
                    JArray jsonResponseArray = (JArray)jsonResponseObject["projectDetailList"];
                    var projectList = jsonResponseArray.ToObject<List<GetProjectIndex>>();
                    return View(projectList);

                }
            }
            return View(new List<GetProjectIndex>());
        }
        public async Task<IActionResult> AddEditProject(int id)
        {
            ProjectViewModel projectViewModel = new ProjectViewModel();
            string apiUrl = baseUrl + "/api/Project/GetProjectStatusList";
            var sessionValue = HttpContext.Session.GetString("SessionKey");
            string bearerToken = sessionValue;
            client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            HttpResponseMessage response = await client.PostAsync(apiUrl, null);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                if (jsonResponse.status == true)
                {
                    var jsonString = jsonResponse.data;
                    JArray jsonResponseArray = JArray.Parse(Convert.ToString(jsonString));
                    var cont = jsonResponseArray.Select(x => new SelectListItem { Text = x["label"].ToString(), Value = x["value"].ToString() }).ToList();
                    ViewBag.ProjectStatusList = cont;
                    if (id > 0)
                    {
                        apiUrl = baseUrl + "/api/Project/GetProjectById";
                        var requestData = new
                        {
                            id = id
                        };

                        client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                        var content1 = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response1 = await client.PostAsync(apiUrl, content1);
                        if (response1.IsSuccessStatusCode)
                        {
                            string responseData1 = await response1.Content.ReadAsStringAsync();
                            dynamic jsonResponse1 = JsonConvert.DeserializeObject(responseData1);
                            if (jsonResponse1.status == true)
                            {
                                var jsonString1 = jsonResponse1.data;
                                JObject jsonResponseObject1 = JObject.Parse(Convert.ToString(jsonString1));

                                projectViewModel.Id = (int)jsonResponseObject1["id"];
                                projectViewModel.ProjectStatus = (string)jsonResponseObject1["projectStatus"];
                                projectViewModel.ProjectName = (string)jsonResponseObject1["projectName"];
                                projectViewModel.ProjectStartDate = (DateTime?)jsonResponseObject1["projectStartDate"] != null ? (DateTime?)jsonResponseObject1["projectStartDate"] : null;
                                projectViewModel.ProjectEndDate = (DateTime?)jsonResponseObject1["projectEndDate"] != null ? (DateTime?)jsonResponseObject1["projectEndDate"] : null;
                            }
                        }
                    }
                }
            }
            return View(projectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUpdateProject(ProjectViewModel request)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = baseUrl + "/api/Project/AddEditProject"; // Replace with the actual API URL
                var requestData = new
                {
                    id = request.Id,
                    projectName = request.ProjectName,
                    projectStatus = request.ProjectStatus,
                    projectStartDate = request.ProjectStartDate,
                    projectEndDate = request.ProjectEndDate,
                };
                var sessionValue = HttpContext.Session.GetString("SessionKey");
                string bearerToken = sessionValue;
                client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                    if (jsonResponse.status == true)
                    {
                        return RedirectToAction("ProjectIndex");
                    }
                    else
                    {
                        ViewBag.message = jsonResponse.message.ToString();
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> ProjectAssignUserList()
        {
            string apiUrl = baseUrl + "/api/Project/GetAllProjectAssignDetailList";

            var requestData = new
            {
                pageNumber = 0,
                pageSize = 0,
                orderBy = true,
                searchByString = ""
            };

            var sessionValue = HttpContext.Session.GetString("SessionKey");
            string bearerToken = sessionValue;
            client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                if (jsonResponse.status == true)
                {
                    var jsonString = jsonResponse.data;
                    JObject jsonResponseObject = JObject.Parse(Convert.ToString(jsonString));
                    JArray jsonResponseArray = (JArray)jsonResponseObject["projectAssignDetailList"];
                    var projectList = jsonResponseArray.ToObject<List<GetAllProjectAssignDetailList>>();
                    return View(projectList);

                }
            }
            return View(new List<GetAllProjectAssignDetailList>());
          
        }

        public async Task<IActionResult> AddEditProjectAssignUser(int id)
        {
            ProjectAssignUserViewModel projectAssignUserViewModel = new ProjectAssignUserViewModel();


            var sessionValue = HttpContext.Session.GetString("SessionKey");
            string bearerToken = sessionValue;
            client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            #region ProjectDropDown
            string apiUrl = baseUrl + "/api/Project/GetAllProjectDetailList";
            var projectrequest = new
            {
                pageNumber = 0,
                pageSize = 0,
                orderBy = true,
                searchByString = "",
                searchByStatus = ""
            };
            var projectrequestJson = Newtonsoft.Json.JsonConvert.SerializeObject(projectrequest);
            var projectContent1 = new StringContent(projectrequestJson, Encoding.UTF8, "application/json");
            HttpResponseMessage projectResponseDropDown = await client.PostAsync(apiUrl, projectContent1);



            if (projectResponseDropDown.IsSuccessStatusCode)
            {
                string projectDataResponse = await projectResponseDropDown.Content.ReadAsStringAsync();
                dynamic ProjectListDropDown = JsonConvert.DeserializeObject(projectDataResponse);

                if (ProjectListDropDown.status == true)
                {
                    var jsonString = ProjectListDropDown.data;
                    JObject jsonResponseObject = JObject.Parse(Convert.ToString(jsonString));
                    if (jsonResponseObject["projectDetailList"] != null)
                    {
                        JArray projectListArray = (JArray)jsonResponseObject["projectDetailList"];
                        var projectList = projectListArray.Select(x => new SelectListItem { Text = x["projectName"].ToString(), Value = x["id"].ToString() }).ToList(); ViewBag.ProjectNameList = projectList;
                    }
                }

            }
            #endregion

            #region UserDropDown
            apiUrl = baseUrl + "/api/UserV2/GetAllUserList";
            var userRequest = new
            {
                pageNumber = 0,
                pageSize = 0,
                orderBy = true,
                searchByName = "",
                unitId = 0,
                areaId = 0,
                departmentId = 0,
                designationId = 0,
                userStatus = "",
                roleId = 0
            };

            var userJson = Newtonsoft.Json.JsonConvert.SerializeObject(userRequest);
            var userContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            HttpResponseMessage userResponseDropDown = await client.PostAsync(apiUrl, userContent);
            if (userResponseDropDown.IsSuccessStatusCode)
            {
                string userDataResponse = await userResponseDropDown.Content.ReadAsStringAsync();
                dynamic UserListDropDown = JsonConvert.DeserializeObject(userDataResponse);
                if (UserListDropDown.status == true)
                {
                    var jsonString = UserListDropDown.data;
                    JObject jsonResponseObject = JObject.Parse(Convert.ToString(jsonString));
                    if (jsonResponseObject["getAllUserDetailList"] != null)
                    {
                        JArray userListArray = (JArray)jsonResponseObject["getAllUserDetailList"];
                        var userList = userListArray.Select(x => new SelectListItem { Text = x["fullName"].ToString(), Value = x["id"].ToString() }).ToList();
                        ViewBag.UserList = userList;
                    }
                }
            }
            #endregion

            #region Edit Time

            if (id > 0)
            {
                apiUrl = baseUrl + "/api/Project/GetProjectAssignDetailById";
                var requestData = new
                {
                    id = id
                };

                client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content1 = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = await client.PostAsync(apiUrl, content1);
                if (response1.IsSuccessStatusCode)
                {
                    string responseData1 = await response1.Content.ReadAsStringAsync();
                    dynamic jsonResponse1 = JsonConvert.DeserializeObject(responseData1);
                    if (jsonResponse1.status == true)
                    {
                        var jsonString1 = jsonResponse1.data;
                        JObject jsonResponseObject1 = JObject.Parse(Convert.ToString(jsonString1));

                        projectAssignUserViewModel.Id = (int)jsonResponseObject1["id"];
                        projectAssignUserViewModel.ProjectId = (int)jsonResponseObject1["projectId"];
                        //projectAssignUserViewModel.ProjectName = (string)jsonResponseObject1["projectName"];
                        projectAssignUserViewModel.UserId = ((JArray)jsonResponseObject1["userId"]).ToObject<List<int>>();

                    }
                }
            }

            #endregion

            return View(projectAssignUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUpdateProjectAssignUser(ProjectAssignUserViewModel request)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = baseUrl + "/api/Project/AddEditProjectAssignDetail"; // Replace with the actual API URL
                var requestData = new
                {
                    id = request.Id,
                    projectId = request.ProjectId,
                    userId = request.UserId
                };
                var sessionValue = HttpContext.Session.GetString("SessionKey");
                string bearerToken = sessionValue;
                client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                    if (jsonResponse.status == true)
                    {
                        return RedirectToAction("ProjectAssignUserList");
                    }
                    else
                    {
                        ViewBag.message = jsonResponse.message.ToString();
                        return RedirectToAction("AddEditProjectAssignUser");
                    }

                }
                else
                {
                    return RedirectToAction("AddEditProjectAssignUser");
                }

            }
            else
            {
                return RedirectToAction("AddEditProjectAssignUser");
            }
        }

        public async Task<IActionResult> DailyTask()
        {
            string apiUrl = baseUrl + "/api/TaskManagement/GetAllDailyTaskList";

            var requestData = new
            {
                pageNumber = 0,
                pageSize = 0,
                orderBy = true,
                searchByString = "",
                searchByStatus = "",
                projectId = 0
            };

            var sessionValue = HttpContext.Session.GetString("SessionKey");
            string bearerToken = sessionValue;
            client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                if (jsonResponse.status == true)
                {
                    var jsonString = jsonResponse.data;
                    JObject jsonResponseObject = JObject.Parse(Convert.ToString(jsonString));
                    JArray jsonResponseArray = (JArray)jsonResponseObject["dailyTaskDetailList"];
                    var projectList = jsonResponseArray.ToObject<List<GetDailyTaskViewModel>>();
                    return View(projectList);

                }
            }
            return View(new List<GetDailyTaskViewModel>());
        }

        public async Task<IActionResult> AddEditDailyTask(int id)
        {
            DailyTaskViewModel dailyTaskViewModel = new DailyTaskViewModel();

            string apiUrl = baseUrl + "/api/TaskManagement/GetProjectListByLoggedInUserId";
            var sessionValue = HttpContext.Session.GetString("SessionKey");
            string bearerToken = sessionValue;
            client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            HttpResponseMessage response = await client.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                string projectDataResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(projectDataResponse);

                if (jsonResponse.status == true)
                {
                    var jsonString = jsonResponse.data;
                    JArray jsonResponseArray = JArray.Parse(Convert.ToString(jsonString));
                    var cont = jsonResponseArray.Select(x => new SelectListItem { Text = x["projectName"].ToString(), Value = x["id"].ToString() }).ToList();
                    ViewBag.ProjectList = cont;

                }

            }


            #region UserDropDown
            apiUrl = baseUrl + "/api/TaskManagement/GetDailyTaskStatusList";

            HttpResponseMessage taskResponseDropDown = await client.PostAsync(apiUrl, null);
            if (taskResponseDropDown.IsSuccessStatusCode)
            {
                string taskDataResponse = await taskResponseDropDown.Content.ReadAsStringAsync();
                dynamic TaskDailyListDropDown = JsonConvert.DeserializeObject(taskDataResponse);
                if (TaskDailyListDropDown.status == true)
                {
                    var jsonString = TaskDailyListDropDown.data;
                    JArray jsonResponseArray = JArray.Parse(Convert.ToString(jsonString));
                    var cont = jsonResponseArray.Select(x => new SelectListItem { Text = x["label"].ToString(), Value = x["value"].ToString() }).ToList();
                    ViewBag.TaskStatusList = cont;
                }
            }
            #endregion

            #region Edit Time

            if (id > 0)
            {
                apiUrl = baseUrl + "/api/TaskManagement/GetDailyTaskById";
                var requestData = new
                {
                    id = id
                };

                client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content1 = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = await client.PostAsync(apiUrl, content1);
                if (response1.IsSuccessStatusCode)
                {
                    string responseData1 = await response1.Content.ReadAsStringAsync();
                    dynamic jsonResponse1 = JsonConvert.DeserializeObject(responseData1);
                    if (jsonResponse1.status == true)
                    {
                        var jsonString1 = jsonResponse1.data;
                        JObject jsonResponseObject1 = JObject.Parse(Convert.ToString(jsonString1));

                        dailyTaskViewModel.Id = (int)jsonResponseObject1["id"];
                        dailyTaskViewModel.ProjectId = (int)jsonResponseObject1["projectId"];
                        dailyTaskViewModel.TaskDate = (DateTime)jsonResponseObject1["taskDate"];
                        dailyTaskViewModel.TaskDuration = (string)jsonResponseObject1["taskDuration"];
                        dailyTaskViewModel.TaskDescription = (string)jsonResponseObject1["taskDescription"];
                        dailyTaskViewModel.TaskStatus = (string)jsonResponseObject1["taskStatus"];
                    }
                }
            }
            else
            {
                dailyTaskViewModel.TaskDate = DateTime.Now;
            }
            #endregion

            return View(dailyTaskViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUpdateDailyTask(DailyTaskViewModel request)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = baseUrl + "/api/TaskManagement/AddEditDailyTask"; // Replace with the actual API URL
                var requestData = new
                {
                    id = request.Id,
                    projectId = request.ProjectId,
                    taskDate = request.TaskDate,
                    taskStatus = request.TaskStatus,
                    taskDescription = request.TaskDescription,
                    taskDuration = request.TaskDuration,
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                    if (jsonResponse.status == true)
                    {
                        return RedirectToAction("DailyTask");
                    }
                    else
                    {
                        ViewBag.message = jsonResponse.message.ToString();
                        return RedirectToAction("DailyTask");
                    }
                  
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return View();
            }
        }

    }
}
