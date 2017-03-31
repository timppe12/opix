using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace kirjautuminen
{
    public static class ApiService
    {
        private const string SessionCookieName = "PHPSESSID";

        private const string ServerUrl = "http://192.168.0.14";
        private const int Port = 9080;
        private static Cookie _sessionCookie;
        private static readonly HttpClient Client;
        private static readonly CookieContainer Cookies;

        static ApiService()
        {
            Cookies = new CookieContainer();
            var handler = new HttpClientHandler {CookieContainer = Cookies};
            Client = new HttpClient(handler);
        }

        public static bool IsLoggedIn => _sessionCookie != null;

        public static bool Login(string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password}
            };
            var content = new FormUrlEncodedContent(values);
            var url = $"{ServerUrl}:{Port}/login.php";
            var response = Client.PostAsync(url, content).Result;
            UpdateSessionCookie();
            return _sessionCookie != null;
        }

        public static bool Logout()
        {
            string url = $"{ServerUrl}:{Port}/logout.php";

            var response = Client.GetAsync(url).Result;
            if (response.StatusCode != HttpStatusCode.Found) return false;
            UpdateSessionCookie();
            return true;
        }

        public static bool Signup(string email,string username, string password, string confirmPassword)
        {
            var values = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password},
                {"password2", confirmPassword},
                {"email", email}
            };
            var content = new FormUrlEncodedContent(values);
            var url = $"{ServerUrl}:{Port}/registration.php";
            var response = Client.PostAsync(url, content).Result;
            return response.StatusCode == HttpStatusCode.OK;
        }

        public static bool CreateModule(string moduleName, int moduleGoal)
        {
            var values = new Dictionary<string, string>
            {
                {"mod_name", moduleName},
                {"mod_goal", moduleGoal.ToString()}
            };
            var content = new FormUrlEncodedContent(values);
            var url = $"{ServerUrl}:{Port}/create-module.php";
            var response = Client.PostAsync(url, content).Result;
            return response.StatusCode == HttpStatusCode.Found;
        }

        public static IEnumerable<Module> GetModules(int userId)
        {
            throw new NotImplementedException();
        }

        private static void UpdateSessionCookie()
        {
            _sessionCookie =
                Cookies.GetCookies(new Uri(ServerUrl))
                    .Cast<Cookie>()
                    .SingleOrDefault(c => c.Name == SessionCookieName);
        }
    }

    public class Module
    {
        public string Name { get; set; }
        public int Goal { get; set; }
    }
}