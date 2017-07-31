using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Iris.Requests
{
    class RequestCreateAccount : Request
    {
        /// <summary>
        /// Uri for account creation requests.
        /// </summary>
        private const string uriCreateAccountRequest = instagramUri + "accounts/web_create_ajax/";

        /// <summary>
        /// List of errors with the account creation request.
        /// </summary>
        public List<string> Errors = new List<string>();

        /// <summary>
        /// Attempt to create an instagram account with the details specified.
        /// </summary>
        /// <param name="firstName">First name for the account.</param>
        /// <param name="username">Username for the account.</param>
        /// <param name="password">Password for the account.</param>
        /// <param name="email">Email for the account.</param>
        public RequestCreateAccount(string firstName, string username, string password, string email) : base(uriCreateAccountRequest)
        {
            // add csrftoken to headers from current session
            Header("x-csrftoken", GetCsrfToken(session));
            Header("x-instagram-ajax", "1");
            Header("x-requested-with", "XMLHttpRequest");

            // post all of the acc details
            Post("email", email);
            Post("password", password);
            Post("username", username);
            Post("first_name", firstName);

            string response = SubmitRequest();
            // Console.WriteLine("Response: {0}\n\n", response); // DEBUG

            dynamic jsonResponse = JObject.Parse(response);
            // Console.WriteLine("jsonResponse: {0}", jsonResponse); // DEBUG

            if (jsonResponse.errors != null)
            {
                Success = false;

                // start with error_type to get a general idea of what happened
                if (jsonResponse.error_type != null)
                    Errors.Add("Error: " + jsonResponse.error_type);

                // annoying way of looping through errors, but blame dynamic/lazy parsing
                foreach(var error in jsonResponse.errors)
                {
                    foreach (var subError in error)
                    {
                        int subErrorIndex = 0;
                        foreach(var errorDetails in subError)
                        {
                            if(errorDetails.message != null)
                            {
                                string errorMessage = string.Format("{0}[{1}]: {2}", error.Path, subErrorIndex++, errorDetails.message);
                                Errors.Add(errorMessage);
                            }
                            else if(errorDetails.ip != null)
                            {
                                string errorMessage = string.Format("{0}[{1}]: {2}", error.Path, subErrorIndex++, errorDetails.ip);
                                Errors.Add(errorMessage);
                            }
                        }
                    }
                }
            }
            else if((bool) jsonResponse.account_created)
            {
                // this is really what we want
                Success = true;
            }
            else
            {
                Success = false;
                // not sure if this will happen... really weird if it did
                Errors.Add("Account not created, check response details: " + Response);
            }
        }
    }
}
