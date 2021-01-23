using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProject.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
