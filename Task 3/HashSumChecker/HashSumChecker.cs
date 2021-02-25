using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace HashSumChecker
{
    public class HashSumChecker
    {
        string inputFile;
        string filesToCheck;

        List<FileContent> fileContents;

        enum Status
        {
            OK,
            FAIL,
            NOT_FOUND
        }

        struct FileContent
        {
            public string fileName;
            public string hashType;
            public string hashValue;
        }
        public HashSumChecker(string _inputFile, string _filesToCheck)
        {
            inputFile = _inputFile;
            filesToCheck = _filesToCheck;
        }

        public bool CheckHashSum()
        {
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Input file doesn't exist. Input file = {inputFile}");
                return false;
            }

            fileContents = new List<FileContent>();

            using (var fileStream = File.OpenRead(inputFile))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    string[] parts;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        parts = ParseLine(line);
                        if (parts.Length < 3)
                        {
                            Console.WriteLine("Incorrect line in input file");
                            return false;
                        }

                        FileContent fileContent;
                        fileContent.fileName = parts[0];
                        fileContent.hashType = parts[1];
                        fileContent.hashValue = parts[2];

                        fileContents.Add(fileContent);

                    }
                }
            }

            return CheckFiles();
        }

        private bool CheckFiles()
        {
            if (!Directory.Exists(filesToCheck))
            {
                Console.WriteLine($"Directory doesn't exist. Directory = {filesToCheck}");
                return false;
            }
            string path;
            foreach (var file in fileContents)
            {
                string[] paths = { filesToCheck, file.fileName };
                path = Path.Combine(paths);
                if (!File.Exists(path))
                {
                    WriteResult(file.fileName, Status.NOT_FOUND);
                    continue;
                }
                if (CheckHash(file, path)) WriteResult(file.fileName, Status.OK);
                else WriteResult(file.fileName, Status.FAIL);
            }

            return true;
        }

        private bool CheckHash(FileContent fileContent, string path)
        {
            string hash = "";
            string type = fileContent.hashType.ToLower();
            if (type == "md5")
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(path))
                    {
                       hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    }
                }
            }
            else if (type == "sha1")
            {
                using (var sha1 = SHA1.Create())
                {
                    using (var stream = File.OpenRead(path))
                    {
                        hash = BitConverter.ToString(sha1.ComputeHash(stream)).Replace("-", string.Empty);
                    }
                }
            }
            else if (type == "sha256")
            {
                using (var sha256 = SHA256.Create())
                {
                    using (var stream = File.OpenRead(path))
                    {
                        hash = BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", string.Empty);
                    }
                }
            }
            if (hash.ToLower() == fileContent.hashValue.ToLower()) return true;
            
            return false;
        }

        private void WriteResult(string fileName, Status status)
        {
            var defaultBackgroundColor = Console.BackgroundColor;
            Console.Write($"{fileName} ");
            switch (status)
            {
                case Status.OK:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write("OK");
                    break;
                case Status.FAIL:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write("FAIL");
                    break;
                case Status.NOT_FOUND:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.Write("NOT FOUND");
                    break;
                default:
                    break;
            }
            Console.BackgroundColor = defaultBackgroundColor;
            Console.WriteLine();
        }

        private string[] ParseLine(string line) => line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    }



}
