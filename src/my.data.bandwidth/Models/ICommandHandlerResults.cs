namespace My.Data.Bandwidth.Models
{
    /// <summary>
    /// Generic <see cref="ICommandHandler{TInput, TOutput}"/> results wrapper.
    /// </summary>
    public interface ICommandHandlerResults
    {
        dynamic Results { get; set; }
    }
}