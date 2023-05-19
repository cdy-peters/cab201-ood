namespace Advance
{
    public class FileIO
    {
        public static string LoadFile(string srcFilePath)
        {
            string boardStr = System.IO.File.ReadAllText(srcFilePath);

            // remove escape sequences
            boardStr = boardStr.Replace("\r", "");
            boardStr = boardStr.Replace("\n", "");

            return boardStr;
        }

        public static void SaveFile(string destFilePath, string boardStr)
        {
            System.IO.File.WriteAllText(destFilePath, boardStr);
        }
    }
}