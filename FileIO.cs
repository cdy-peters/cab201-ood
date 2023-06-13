namespace Chess
{
    /// <summary>
    /// Class containing the logic for loading and saving files.
    /// </summary>
    internal static class FileIO
    {
        /// <summary>
        /// Validates command line arguments. Throws ArgumentException if arguments are invalid otherwise returns.
        /// </summary>
        /// <param name="args">The command line arguments to validate</param>
        internal static void ValidateArgs(string[] args)
        {
            /// Throws an exception if no arguments are provided.
            if (args.Length == 0)
                throw new System.ArgumentException("No arguments provided");

            /// Check the player color argument.
            switch (args[0].ToLower())
            {
                case "white":
                case "black":
                    /// Check for correct amount of arguments.
                    if (args.Length != 3)
                        throw new System.ArgumentException("Invalid number of arguments");
                    break;
                case "name":
                    /// Check for correct amount of arguments.
                    if (args.Length != 1)
                        throw new System.ArgumentException("Invalid number of arguments");
                    return;
                default:
                    throw new System.ArgumentException("Invalid player color");
            }

            /// Check if source file exists.
            if (!System.IO.File.Exists(args[1]))
                throw new System.ArgumentException("Source file does not exist");

            /// Check if destination file exists.
            if (!System.IO.File.Exists(args[2]))
                throw new System.ArgumentException("Destination file does not exist");
        }

        /// <summary>
        /// Validates the file to make sure it is valid, otherwise throwing an exception.
        /// </summary>
        /// <param name="boardStr">The string that contains the</param>
        private static void ValidateFile(string boardStr)
        {
            /// Check if the board contains invalid characters
            if (!System.Text.RegularExpressions.Regex.IsMatch(boardStr, @"^[PBNRQKpbnrqk\.]+$"))
                throw new System.ArgumentException("Invalid characters in file");

            /// Check if the board contains the correct number of characters
            if (boardStr.Length != 64)
                throw new System.ArgumentException("Invalid number of characters in file");
        }

        /// <summary>
        /// Loads and validates a Board file.
        /// </summary>
        /// <param name="srcFile">The path to the file to load.</param>
        /// <returns>The contents of the file as a string with all escape sequences removed.</returns>
        internal static string LoadFile(string srcFile)
        {
            string boardStr = System.IO.File.ReadAllText(srcFile);

            // Remove escape sequences
            boardStr = boardStr.Replace("\r", "");
            boardStr = boardStr.Replace("\n", "");

            ValidateFile(boardStr);

            return boardStr;
        }

        /// <summary>
        /// Saves the Board to a text file.
        /// </summary>
        /// <param name="destFile">The path to the file you want to save</param>
        /// <param name="boardStr">The board you want to save</param>
        internal static void SaveFile(string destFile, string boardStr)
        {
            System.IO.File.WriteAllText(destFile, boardStr);
        }
    }
}