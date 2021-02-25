using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace TestCase
{

	sealed public class TestCase2 : TestCaseAbstract
	{
        private string fileName = "test";
		public TestCase2() : base() { }
		public TestCase2(int tc_id, string name) : base(tc_id, name) { }
		protected override bool Prep()
		{
            double freeMemory = IsUnix() ? GetUnixFreeRam() : GetWindowsFreeRam();
            if (freeMemory <= 1)
            {
                ErrorMessage = $"Free memory lesser than 1GB. Free Memory={freeMemory}GB";
                return false;
            }
            return true;

        }
		protected override bool Run()
		{
            byte[] data = new byte[1024*1024];
            Random random = new Random();
            using (FileStream w = File.OpenWrite(fileName))
            {
                random.NextBytes(data);
                w.Write(data, 0, data.Length);
            }
			return true;
		}
		protected override bool CleanUp()
		{
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            else
            {
                ErrorMessage = $"File doesn't exist. Filename={fileName}";
                return false;
            }
			return true;
		}

		private bool IsUnix()
        {
			var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
					 RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

			return isUnix;
		}

        private double GetWindowsFreeRam()
        {
            double free = 0;
            var output = "";

            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);

            free = double.Parse(freeMemoryParts[1]) / (1024*1024);

            return free;
        }
        private double GetUnixFreeRam()
        {
            double free = 0;
            var output = "";

            var info = new ProcessStartInfo("free -m");
            info.FileName = "/bin/bash";
            info.Arguments = "-c \"free -m\"";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            free = double.Parse(memory[2])/1024;

            return free;
        }
    }

}

