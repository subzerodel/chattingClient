using ChatProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProject.Repositories
{
    public interface IRoomRepository
    {
        public void CreateRoom(Room room);
        public Room GetRoom(int roomId);
    }
}
