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
            var authKey = JsonConvert.DeserializeObject<AuthKey>(input);
            return !(authKey == null || authKey.Key != Environment.GetEnvironmentVariable("auth_key"));
        }
    }
}
