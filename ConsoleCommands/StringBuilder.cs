using System;
using System.Linq;

namespace ConsoleCommands
{
    class StringBuilder
    {
        public static string WithArray(string[] strArray, int endPos, int startPos = 0)
        {
            string sentence = "";
            for (int i = startPos; i < endPos; i++)
            {
                sentence += strArray[i] + " ";
            }
            return sentence;
        }
        public static string[] Split(string text, char separator=' ')
        {
            return text.Split(separator);
        }
        public static string[] Split(string text, char except, char separator=' ')
        {
            return text.Split(except)
                     .Select((element, index) => index % 2 == 0  // If even index
                                           ? element.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                           : new string[] { element })  // Keep the entire item
                     .SelectMany(element => element).ToArray(); ;
        }
    }
}
