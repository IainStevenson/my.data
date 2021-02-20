using System.Collections.Generic;

namespace My.Data.Bandwidth.Models
{
    /// <summary>
    /// GEneric <see cref="ICommandHandler{TInput, TOutput}"/> arguments wrapper.
    /// </summary>
    public interface ICommandHandlerArguments
    {        
        IDictionary<string, string> Arguments {  get;set;}
    }
}
