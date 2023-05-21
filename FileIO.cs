namespace Advance
{
    internal static class FileIO
    {
        internal static void ValidateArgs(string[] args)
        {
            if (args.Length == 0)
                throw new System.ArgumentException("No arguments provided");

            args[0] = args[0].ToLower();
            switch (args[0])
            {
                case "white":
                case "black":
                    if (args.Length != 3)
                        throw new System.ArgumentException("Invalid number of arguments");
                    break;
                case "name":
                    if (args.Length != 1)
                        throw new System.ArgumentException("Invalid number of arguments");
                    return;
                default:
                    throw new System.ArgumentException("Invalid player color");
            }

            // check if filepath exists
            if (!System.IO.File.Exists(args[1]))
                throw new System.ArgumentException("Source file does not exist");

            if (!System.IO.File.Exists(args[2]))
                throw new System.ArgumentException("Destination file does not exist");
        }

        internal static void ValidateFile(string boardStr)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(boardStr, @"^[ZBMJSDCGzbmjsdcg\.#]+$"))
                throw new System.ArgumentException("Invalid characters in file");

            if (boardStr.Length != 81)
                throw new System.ArgumentException("Invalid number of characters in file");
        }

        internal static string LoadFile(string srcFile)
        {
            string boardStr = System.IO.File.ReadAllText(srcFile);

            // remove escape sequences
            boardStr = boardStr.Replace("\r", "");
            boardStr = boardStr.Replace("\n", "");

            ValidateFile(boardStr);

            return boardStr;
        }

        internal static void SaveFile(string destFile, string boardStr)
        {
            System.IO.File.WriteAllText(destFile, boardStr);
        }
    }
}