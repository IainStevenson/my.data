using My.Data.Bandwidth.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace My.Data.Bandwidth.Handlers
{
    public class PersistenceHandler<T> where T: ISpeedTestData
    {
        /// <summary>
        /// Persist the serialised result model to a file using the object timestamp as a name
        /// </summary>
        /// <param name="result">the result model instance</param>
        /// <returns><see cref="Task"/></returns>
        public Task Execute(T result)
        {
            var timeString = $"{result.TimeStamp:yyyy-MM-dd-T-HH-mm-ss-ffffff}";

            File.WriteAllText($"{timeString}.json", JsonConvert.SerializeObject(result));

            return Task.FromResult(0);
        }
    }
}
