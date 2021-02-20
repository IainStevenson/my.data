using System.Threading.Tasks;

namespace My.Data.Bandwidth.Models
{
    /// <summary>
    /// Command pattern interface.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public interface ICommandHandler<TInput, TOutput> 
            where TInput : ICommandHandlerArguments 
        where TOutput : ICommandHandlerResults
    {
        Task<TOutput> Execute(TInput arguments);
    }
}
