using Newtonsoft.Json;
using System.Text;

namespace TaskManagementV1
{
    public static class SessionExtensions
    {
        // Serialize and store an object in session
        public static void SetObjectInSession(this ISession session, string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);  // Convert object to JSON string
            session.SetString(key, json);  // Store the JSON string in session
        }

        // Retrieve an object from session and deserialize it
        public static T GetObjectFromSession<T>(this ISession session, string key)
        {
            var json = session.GetString(key);  // Get the JSON string from session
            if (json == null) return default(T);  // If session value is not found, return default value
            return JsonConvert.DeserializeObject<T>(json);  // Deserialize the JSON string to object
        }
    }
}
