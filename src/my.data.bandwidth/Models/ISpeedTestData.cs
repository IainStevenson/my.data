using System;

namespace My.Data.Bandwidth.Models
{
    public interface ISpeedTestData
    {
        object DataOutput { get; set; }
        object ErrorOutput { get; set; }
        bool IsError { get; set; }
        DateTimeOffset TimeStamp { get; set; }
        string ErrorMessage { get; set; }
    }
}
