using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    static class UserInput
    {
        //Get input from user and only return valid entries
        //Expected format array or list of string
        public static string Get(ICollection<string> expected)
        {
            while (true)
            {
                string input = Console.ReadLine().ToLower();
                //Check input against every string in expected
                foreach (var es in expected)
                {
                    if (input == es)
                    {
                        //Yes, this was an expected input
                        return input;
                    }
                }
                Console.WriteLine("Invalid input. Try again.");
            }
        }

        //Ask the user for input
        public static string Prompt(string text)
        {
            Console.Write(text);
            string value = Console.ReadLine();

            return value;
        }

        public static int PromptInt(string text)
        {
            while (true)
            {
                string s = Prompt(text);

                try
                {
                    return int.Parse(s);
                }
                catch
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }
    }
}
