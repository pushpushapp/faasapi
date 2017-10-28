using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Function
{
    public class FunctionHandler
    {
        public void Handle(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || !IsAuthKeyPresentAndValid(input))
            {
                Console.WriteLine("Unauthorized");
                return;
            }

            Console.WriteLine(File.ReadAllText("contacts.json"));
        }

        private bool IsAuthKeyPresentAndValid(string input)
        {
            dynamic parsedBody = JsonConvert.DeserializeObject(input);
            var authKey = Environment.GetEnvironmentVariable("auth_key");

            if (parsedBody.auth_key == null ||
                string.IsNullOrWhiteSpace((string)parsedBody.auth_key) ||
                (string)parsedBody.auth_key != authKey)
            {
                return false;
            }

            return true;
        }
    }
}
