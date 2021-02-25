using System;

namespace TestCase
{
	public abstract class TestCaseAbstract
	{
		protected string name;
		protected int tcId;

		private string errorMessage = "";

		protected string ErrorMessage { 
			get { return errorMessage; } 
			set { errorMessage = value ;} 
		}
        enum TestStages
        {
			prep = 0,
			run,
			clean_up
        }

		delegate bool RunStage();

		protected TestCaseAbstract()
		{
			name = "Test Case";
			tcId = 0;
		}

		protected TestCaseAbstract(int _tc_id, string _name)
		{
			name = _name;
			tcId = _tc_id;
		}

		
		public bool Execute() {
			Console.WriteLine(new string('-', 20));
			Console.WriteLine($"Started Test Case {tcId} with name {name}");

			RunStage runStage = new RunStage(Prep);
			runStage += Run;
			runStage += CleanUp;
			if (!StageCallback(runStage)) return false;

			return true;
        }

		private bool StageCallback(RunStage callback)
        {
			int stageNumber = 0;
            foreach (RunStage stageDel in callback.GetInvocationList())
			{
				TestStarted(stageNumber);
				bool result = false;
                try
                {
					result = stageDel();
				}
                catch (Exception e)
                {
					errorMessage = e.Message;
                }
				TestResult(stageNumber, result);
				if (!result) return false;
				stageNumber++;
			}
			return true;
		}

		private void TestStarted(int stage)
        {
			var defaultBackgroundColor = Console.BackgroundColor;
			Console.Write($"{(TestStages)stage}: ");
			
			Console.BackgroundColor = ConsoleColor.DarkYellow;
			Console.Write("STARTED");
			Console.BackgroundColor = defaultBackgroundColor;
			Console.WriteLine();

		}

		private void TestResult(int stage, bool result)
        {
			Console.Write($"{(TestStages)stage}: ");
			var defaultBackgroundColor = Console.BackgroundColor;
			if (result)
            {
				Console.BackgroundColor = ConsoleColor.DarkGreen;
				Console.Write("SUCCESS");
            }
			else
            {
				Console.BackgroundColor = ConsoleColor.DarkRed;
				Console.Write($"FAILED {errorMessage}");
			}
			Console.BackgroundColor = defaultBackgroundColor;
			Console.WriteLine();
		}


		protected abstract bool Prep();
		protected abstract bool Run();
		protected abstract bool CleanUp();
	}
}


