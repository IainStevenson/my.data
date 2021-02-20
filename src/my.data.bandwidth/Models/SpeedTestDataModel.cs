using System;

namespace My.Data.Bandwidth.Models
{
    public class SpeedTestDataModel : ISpeedTestData
    {
        public object DataOutput { get; set; }
        public object ErrorOutput { get; set; }
        public bool IsError { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string ErrorMessage { get; set; }
    }
}
