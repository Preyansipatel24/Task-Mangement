using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using TaskManagementV1.Models;

namespace TaskManagementV1.Controllers
{
    public class CommonController : Controller
    {
        private IConfiguration _configuration { get; }
        private static readonly HttpClient client = new HttpClient();
        private IHttpContextAccessor _httpContextAccessor;
        public CommonController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public string EncryptString(string plainText)
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

            //return Ok(Convert.ToBase64String(encrypted));
            // return encrypted;
            return Convert.ToBase64String(encrypted);
        }

        public IActionResult DecryptString(string cipherText)
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
            return Ok(plaintext);
        }

        public async Task<dynamic> CallApiAsync(string url, HttpMethod method, object body = null, bool? IsTokenRequired = true, string contentType = "application/json")
        {
            try
            {
                if (IsTokenRequired == true)
                {
                    string Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                    client.DefaultRequestHeaders.Add("Custom-Header", "HeaderValue");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                }

                // Prepare the request message
                HttpRequestMessage request = new HttpRequestMessage(method, url);

                // Add content if there is a body (for POST, PUT requests)
                if (body != null)
                {
                    var jsonBody = JsonConvert.SerializeObject(body);
                    request.Content = new StringContent(jsonBody, Encoding.UTF8, contentType);
                }

                // Send the request and get the response
                var response = await client.SendAsync(request);

                // Ensure successful status code
                response.EnsureSuccessStatusCode();

                // Read the response content as string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse response into dynamic object
                dynamic responseObject = JsonConvert.DeserializeObject(responseBody);

                return responseObject;
            }
            catch (Exception ex)
            {
                // Handle exceptions (network issues, API errors, etc.)
                Console.WriteLine($"Error calling API: {ex.Message}");
                return null;   // Return null or a custom error object
            }
        }

        // Generic method to convert a dynamic object to a specific model
        public T ConvertDynamicToModel<T>(dynamic dynamicObject)
        {
            try
            {
                // Serialize the dynamic object to a JSON string
                var json = JsonConvert.SerializeObject(dynamicObject);

                // Deserialize the JSON string to the specified model type
                T model = JsonConvert.DeserializeObject<T>(json);

                return model;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during serialization/deserialization
                Console.WriteLine($"Error converting dynamic object to model: {ex.Message}");
                return default; // Return the default value of T in case of an error
            }
        }

        public dynamic ConvertJsonToDynamicModel(string json)
        {
            // Parse the JSON into a JObject
            JObject jsonObject = JObject.Parse(json);

            // Create a dynamic object (instead of ExpandoObject, use dynamic directly)
            dynamic dynamicModel = new System.Dynamic.ExpandoObject();
            var dictionary = (IDictionary<string, object>)dynamicModel;

            foreach (var property in jsonObject)
            {
                if (property.Value is JObject nestedObject)
                {
                    // Recursively call to handle nested objects
                    dictionary[property.Key] = ConvertJsonToDynamicModel(nestedObject.ToString());
                }
                else if (property.Value is JArray array)
                {
                    // Handle arrays
                    var list = new List<object>();
                    foreach (var item in array)
                    {
                        list.Add(item);
                    }
                    dictionary[property.Key] = list;
                }
                else
                {
                    // Handle primitive types
                    dictionary[property.Key] = property.Value.ToString();
                }
            }

            return dynamicModel; // Return dynamic object
        }

        public Type CreateModelFromDynamicObject(dynamic dynamicObject)
        {
            // Create a dynamic assembly
            AssemblyName assemblyName = new AssemblyName("DynamicModelAssembly");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModelModule");

            // Define a new type (class)
            TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicModel", TypeAttributes.Public | TypeAttributes.Class);

            // Iterate over the properties of the dynamic object and create class properties
            var dictionary = (IDictionary<string, object>)dynamicObject;
            foreach (var kvp in dictionary)
            {
                string propertyName = kvp.Key;
                Type propertyType = kvp.Value.GetType();

                // Define the field
                FieldBuilder fieldBuilder = typeBuilder.DefineField(propertyName, propertyType, FieldAttributes.Private);

                // Define the property
                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

                // Define the getter method
                MethodBuilder getMethodBuilder = typeBuilder.DefineMethod($"get_{propertyName}",
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                    propertyType, Type.EmptyTypes);
                ILGenerator ilGenerator = getMethodBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                ilGenerator.Emit(OpCodes.Ret);
                propertyBuilder.SetGetMethod(getMethodBuilder);

                // Define the setter method
                MethodBuilder setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyName}",
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                    null, new[] { propertyType });
                ilGenerator = setMethodBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                ilGenerator.Emit(OpCodes.Ret);
                propertyBuilder.SetSetMethod(setMethodBuilder);
            }

            // Create the class at runtime
            Type dynamicModelType = typeBuilder.CreateType();
            return dynamicModelType;
        }

        // Method to populate the dynamically created class
        public object PopulateDynamicModel(Type dynamicModelType, dynamic dynamicObject)
        {
            // Create an instance of the dynamic model
            object dynamicModelInstance = Activator.CreateInstance(dynamicModelType);

            // Populate the model instance with values from the dynamic object
            var dictionary = (IDictionary<string, object>)dynamicObject;
            foreach (var kvp in dictionary)
            {
                PropertyInfo propertyInfo = dynamicModelType.GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(dynamicModelInstance, kvp.Value);
                }
            }

            return dynamicModelInstance;
        }

        // Method to generate a C# class code as a string from a dynamic object
        public string GenerateCSharpClassCode(dynamic dynamicObject)
        {
            var dictionary = (IDictionary<string, object>)dynamicObject;
            string classCode = "using System;\n\npublic class DynamicModel\n{\n";

            // Generate properties for each key in the dynamic object
            foreach (var kvp in dictionary)
            {
                string propertyName = kvp.Key;
                string propertyType = kvp.Value.GetType().Name;

                // Define a simple property in the C# class code
                classCode += $"    public {propertyType} {propertyName} {{ get; set; }}\n";
            }

            classCode += "}\n";

            return classCode;
        }


        #region NEW

        // Main method to call the class generation
        public string GenerateClassSaveAndReturnFilePath(dynamic dynamicObject)
        {
            // Step 1: Generate the C# class code from the dynamic object
            // Convert dynamicObject to a dictionary
            var dictionary = (IDictionary<string, object>)dynamicObject;

            // Extract class name from dynamic object, default to "DynamicModel" if not found
            string className = dictionary.ContainsKey("className") ? dictionary["className"].ToString() : "DynamicModel";

            // Start building the C# class code
            string classCode = $"using System;\nusing System.Collections.Generic;\n\npublic class {className}\n{{\n";

            // Generate properties for each key in the dynamic object
            foreach (var kvp in dictionary)
            {
                if (kvp.Key != "className")  // Skip className if present
                {
                    string propertyName = kvp.Key;
                    Type propertyType = kvp.Value.GetType();

                    string csharpType = GetCSharpType(propertyType);

                    // Define a property in the C# class code
                    classCode += $"    public {csharpType} {propertyName} {{ get; set; }}\n";
                }
            }

            classCode += "\n    public " + className + "() {\n";

            // Assign values to the properties from the dynamic object
            foreach (var kvp in dictionary)
            {
                if (kvp.Key != "className")  // Skip className if present
                {
                    string propertyName = kvp.Key;
                    string value = FormatValue(kvp.Value);

                    classCode += $"        {propertyName} = {value};\n";
                }
            }

            classCode += "    }\n}";

            // Step 2: Write the class code to a .cs file
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{className}.cs");
            System.IO.File.WriteAllText(filePath, classCode);

            return filePath;
        }

        // Method to get the corresponding C# type for complex objects (List, Dictionary, etc.)
        private string GetCSharpType(Type type)
        {
            if (type == typeof(int))
                return "int";
            else if (type == typeof(string))
                return "string";
            else if (type == typeof(bool))
                return "bool";
            else if (type == typeof(double))
                return "double";
            else if (type == typeof(List<object>))
                return "List<object>";
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return "List<" + GetCSharpType(type.GetGenericArguments()[0]) + ">";
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                return "Dictionary<" + GetCSharpType(type.GetGenericArguments()[0]) + ", " + GetCSharpType(type.GetGenericArguments()[1]) + ">";
            else if (type == typeof(JObject))
                return "object";  // Nested objects as a generic object
            else
                return "object";  // Fallback for unsupported types
        }

        // Method to format value assignments in the generated class constructor
        private string FormatValue(object value)
        {
            if (value is string)
                return $"\"{value}\"";
            else if (value is List<object> list)
                return $"new List<object> {{ {string.Join(", ", list)} }}";
            else if (value is JObject nestedObject)
                return $"new {nestedObject.GetType().Name}()";  // Handle nested objects here
            else
                return value.ToString();
        }

        #endregion

    }
}
