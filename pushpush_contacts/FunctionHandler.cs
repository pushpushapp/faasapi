using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Function
{
    public class FunctionHandler
    {
        public void Handle(string input)
        {
            Console.WriteLine(File.ReadAllText("contacts.json"));
        }
    }
}
