using My.Data.Bandwidth.Models;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace My.Data.Bandwidth.Handlers
{

    public class SpeedTestHandler
    {
        private static readonly string exeFileName = @"./speedtest.exe";
        private static int PROCESS_TIMEOUT_MILLSECONDS = 240000;
        public Task<SpeedTestDataModel> Execute(DateTimeOffset dataPointTime)
        {

            var result = new SpeedTestDataModel() { TimeStamp = dataPointTime };

            Process process = new Process();
            StringBuilder outputStringBuilder = new StringBuilder();
            StringBuilder errorStringBuilder = new StringBuilder();

            try
            {
                process.StartInfo.FileName = exeFileName;
                process.StartInfo.WorkingDirectory = @"./";
                process.StartInfo.Arguments = "--format=json";
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.EnableRaisingEvents = false;
                process.OutputDataReceived += (sender, eventArgs) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data)) 
                        outputStringBuilder.AppendLine(eventArgs.Data);
                };
                process.ErrorDataReceived += (sender, eventArgs) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                        errorStringBuilder.AppendLine(eventArgs.Data);
                };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                var processExited = process.WaitForExit(PROCESS_TIMEOUT_MILLSECONDS);

                if (processExited == false) // we timed out...
                {
                    process.Kill();
                    result.ErrorMessage = $"Process timed out and was killed";
                    result.IsError = true;
                }
                else if (process.ExitCode != 0)
                {
                    result.ErrorMessage = $"Process exited with non-zero exit code of: {process.ExitCode} [{errorStringBuilder}]";
                    result.IsError = true;
                }
                else if ($"{outputStringBuilder}" == string.Empty && $"{errorStringBuilder}" == string.Empty)
                {
                    result.ErrorMessage = $"No output was recieved";
                    result.IsError = true;
                }

                else if ($"{outputStringBuilder}" == string.Empty && $"{errorStringBuilder}" != string.Empty)
                {
                    result.ErrorMessage = $"Error output was recieved";
                    result.IsError = true;
                }
            }
            finally
            {
                process.Close();
                process.Dispose();
            }
            result.DataOutput = outputStringBuilder.ToString();
            result.ErrorOutput = errorStringBuilder.ToString();
            return Task.FromResult(result);
        }
    }

}
