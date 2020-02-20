using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMessages.Models.Mess
{
    public class Message : IComparable<Message>
    {
        public string UserId { get; set; }
        public string MessageBody { get; set; }
        public int TimeStamp { get; set; }

        public Message(string userId, string messsge, int time)
        {
            UserId = userId;
            MessageBody = messsge;
            TimeStamp = time;
        }

        public Message() { }

        public int CompareTo(Message other)
        {
            if (other == null)
                return 1;
            else
                return this.UserId.CompareTo(other.UserId);
        }
    }
}