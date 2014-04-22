using System;

namespace Common
{
    public class Message
    {
        public Guid Id { get; set; }
        public Int32 Value { get; set; }

        public Message()
        {
            Id = Guid.NewGuid();
            Value = new Random(DateTime.Now.Millisecond).Next(0, 1000);
        }
    }
}
