using System;

namespace HashSumChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Launch program with two arguments: <path to the input file> and <path to the directory containing the files to check>");
                return;
            }

            string inputFile = args[0];
            string filesToCheck = args[1];

            HashSumChecker hashSumChecker = new HashSumChecker(inputFile, filesToCheck);
            hashSumChecker.CheckHashSum();

        }


    }
}
