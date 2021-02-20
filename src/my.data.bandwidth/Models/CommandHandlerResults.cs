namespace My.Data.Bandwidth.Models
{
    /// <summary>
    /// Generic wrapper for results from a <see cref="ICommandHandlerResults"/> command handler.
    /// </summary>
    public class CommandHandlerResults : ICommandHandlerResults
    {
        public dynamic Results { get; set; }
    }
}
