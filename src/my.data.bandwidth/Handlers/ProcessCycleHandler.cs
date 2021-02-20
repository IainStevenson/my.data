using My.Data.Bandwidth.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace My.Data.Bandwidth.Handlers
{
    public class ProcessCycleHandler : ICommandHandler<ICommandHandlerArguments, ICommandHandlerResults>
    {
        private static int ONE_SECOND_MILLSECONDS = 1000;
        private static int DATAPOINT_INTERVAL_SECONDS = 300;
        private int _intervalInSeconds;

        public ProcessCycleHandler( int intervalInSeconds)
        {
            _intervalInSeconds = intervalInSeconds * ONE_SECOND_MILLSECONDS;
        }

        /// <summary>
        /// Processes the result and determines the next start time then waits until that has expired.
        /// </summary>
        /// <param name="arguments">The result model instance</param>
        /// <returns><see cref="Task"/></returns>
        public Task<ICommandHandlerResults> Execute(ICommandHandlerArguments arguments)
        {
            var time = DateTimeOffset.Parse(arguments.Arguments["TimeStamp"]);
            var isError = bool.Parse(arguments.Arguments["IsError"]);

            if (!isError)
            {
                arguments.Arguments["TimeStamp"] = time.AddSeconds(_intervalInSeconds).ToString(); // await the next interval
            }
            else
            {
                arguments.Arguments["TimeStamp"] = DateTimeOffset.UtcNow.ToString(); // GO again now
            }

            while (DateTime.UtcNow.Ticks < time.Ticks)  // until time exceeded
            {
                Thread.Sleep(ONE_SECOND_MILLSECONDS); // have a wee kip
            }
            ICommandHandlerResults results = new CommandHandlerResults() { Results = arguments };
            return Task.FromResult(results);
        }
    
    }
}