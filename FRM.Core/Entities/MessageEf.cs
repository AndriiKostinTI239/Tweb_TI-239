using System;

namespace FRM.Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public string Text { get; set; }   
        public DateTime Timestamp { get; set; } 
    }
}