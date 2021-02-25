using System;

namespace TestCase
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCase1 testCase1 = new TestCase1(1, "GetFiles");
            testCase1.Execute();

            TestCase2 testCase2 = new TestCase2(1, "Write file");
            testCase2.Execute();

        }
    }
}
