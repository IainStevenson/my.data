using System.Collections.Generic;

namespace My.Data.Bandwidth.Models
{
    /// <summary>
    /// Generic wrapper for a list of arguments for an action.
    /// </summary>
    public class CommandHandlerArguments : ICommandHandlerArguments
    {
        public IDictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();
    }

}
