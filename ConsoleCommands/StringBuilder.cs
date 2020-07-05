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
    }
}
