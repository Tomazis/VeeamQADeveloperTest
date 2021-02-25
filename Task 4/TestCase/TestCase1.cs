using System;
using System.IO;
using System.Collections;

namespace TestCase
{

	sealed public class TestCase1:  TestCaseAbstract
	{
        public TestCase1() : base() { }
		public TestCase1(int tc_id, string name): base(tc_id, name) { }
		protected override bool Prep()
        {
			long now = DateTimeOffset.Now.ToUnixTimeSeconds();
			if (now % 2 != 0) {
				ErrorMessage = $"The current system time is not even. Time={now}";
				return false;
			}
			return true;
        }
		protected override bool Run()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			if (!Directory.Exists(path))
			{
				ErrorMessage = $"Home directory doesn't exist. Path={path}";
				return false;
            }
			
			var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
				Console.WriteLine(file);
            }
			return true;
		}
		protected override bool CleanUp()
		{
			return true;
		}
	}

}

