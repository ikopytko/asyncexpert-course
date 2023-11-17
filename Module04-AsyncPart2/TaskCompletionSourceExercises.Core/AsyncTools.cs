using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TaskCompletionSourceExercises.Core
{
    public class AsyncTools
    {
        public static Task<string> RunProgramAsync(string path, string args = "")
        {
            var tcs = new TaskCompletionSource<string>();
            var process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = new ProcessStartInfo(path, args)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            
            process.Exited += (sender, eventArgs) =>
            {
                var senderProcess = sender as Process;

                if (senderProcess.ExitCode != 0)
                {
                    var result = senderProcess?.StandardError.ReadToEnd();
                    tcs.SetException(new Exception(result));
                }
                else
                {
                    var result = senderProcess?.StandardOutput.ReadToEnd();
                    tcs.TrySetResult(result);
                }
                
                senderProcess?.Dispose();
            };

            process.Start();

            return tcs.Task;
        }
    }
}