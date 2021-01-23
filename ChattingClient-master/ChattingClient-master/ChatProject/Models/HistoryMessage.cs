using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProject.Models
{
    public class HistoryMessage
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public int RoomId { get; set; }
    }
}
