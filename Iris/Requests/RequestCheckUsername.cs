using Newtonsoft.Json.Linq;
using System;

namespace Iris.Requests
{
    class RequestCheckUsername : Request
    {
        /// <summary>
        /// Uri for the username attempt/validation.
        /// </summary>
        private const string uriCheckUsernameRequest = instagramUri + "accounts/web_create_ajax/attempt/";

        /// <summary>
        /// Check if the username is available.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        public RequestCheckUsername(string username) : base(uriCheckUsernameRequest)
        {
            Header("x-csrftoken", GetCsrfToken(session));
            Header("x-instagram-ajax", "1");
            Header("x-requested-with", "XMLHttpRequest");

            // leave everything blank except the username since we're just checking availability
            Post("email", "");
            Post("password", "");
            Post("username", username);
            Post("first_name", "");

            string response = SubmitRequest();
            // Console.WriteLine("Response: {0}", response); // DEBUG

            dynamic jsonResponse = JObject.Parse(response);

            if (jsonResponse.errors.username != null)
            {
                Success = false;
                Response = jsonResponse.errors.username[0].message;
            }
            else
            {
                Success = true;
            }
        }
    }
}
