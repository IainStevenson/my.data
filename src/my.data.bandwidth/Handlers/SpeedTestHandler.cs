using My.Data.Bandwidth.Models;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace My.Data.Bandwidth.Handlers
{

    public class SpeedTestHandler
    {
        private static readonly string exeFileName = @"speedtest";
        private static int PROCESS_TIMEOUT_MILLSECONDS = 240000;
        StringBuilder outputStringBuilder = new StringBuilder();
        StringBuilder errorStringBuilder = new StringBuilder();
        public Task<SpeedTestDataModel> Execute(DateTimeOffset dataPointTime)
        {

            var result = new SpeedTestDataModel() { TimeStamp = dataPointTime };

            Process process = new Process();

            outputStringBuilder = new StringBuilder();
            errorStringBuilder = new StringBuilder();

            try
            {
                process.StartInfo.FileName = exeFileName;
                process.StartInfo.WorkingDirectory = @"./";
                process.StartInfo.Arguments = "--format=json";
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.StartInfo.CreateNoWindow = false; //true;
                process.StartInfo.UseShellExecute = false;
                process.EnableRaisingEvents = false;

                Debug.WriteLine($"Process Starting");

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                var processExited = process.WaitForExit(PROCESS_TIMEOUT_MILLSECONDS);
                Debug.WriteLine($"Processes exited");

                if (processExited == false) // we timed out...
                {
                    process.Kill();
                    result.ErrorMessage = $"Process timed out and was killed";
                    result.IsError = true;
                    Debug.WriteLine($"{result.ErrorMessage}");
                }

                if (process.ExitCode != 0)
                {
                    result.ErrorMessage = $"Process exited with non-zero exit code of: {process.ExitCode}";
                    result.IsError = true;
                    Debug.WriteLine($"{result.ErrorMessage}");
                }

            }
            finally
            {
                Debug.WriteLine($"Closing and disposing of process");
                var time = DateTime.UtcNow.AddSeconds(10);
                while (DateTime.UtcNow.Ticks < time.Ticks)  // until time exceeded
                {
                    Thread.Sleep(100); // have a wee kip
                }
                process.CancelErrorRead();
                process.CancelOutputRead();
                process.Close();
                process.Dispose();

                // wait 5 seconds
            }

            if (outputStringBuilder.Length == 0 && errorStringBuilder.Length == 0)
            {
                result.ErrorMessage = $"No output was recieved at all";
                result.IsError = true;
                Debug.WriteLine($"No output was recieved");
            }

            else if (outputStringBuilder.Length == 0 && errorStringBuilder.Length > 0)
            {
                result.ErrorMessage = $"Error output was recieved";
                result.IsError = true;
                Debug.WriteLine($"Error output was recieved");
            }
            Debug.WriteLine($"Recording data in result");

            result.DataOutput = outputStringBuilder.ToString();
            result.ErrorOutput = errorStringBuilder.ToString();

            Debug.WriteLine($"Return result {result}");
            return Task.FromResult(result);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs eventArgs)
        {
            if (string.IsNullOrWhiteSpace(eventArgs.Data)) return;
            Debug.WriteLine($"Recieving data {eventArgs.Data}");
            outputStringBuilder.AppendLine(eventArgs.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs eventArgs)
        {
            if (string.IsNullOrWhiteSpace(eventArgs.Data)) return;
            Debug.WriteLine($"Recieving error data {eventArgs.Data}");
            errorStringBuilder.AppendLine(eventArgs.Data);
        }
    }

}
