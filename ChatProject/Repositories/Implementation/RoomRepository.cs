using ChatProject.Data;
using ChatProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProject.Repositories.Implementation
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataContext _dataContext;

        public RoomRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async void CreateRoom(Room room)
        {
            _dataContext.Rooms.Add(room);
            await _dataContext.SaveChangesAsync();
        }

        public Room GetRoom(int roomId)
        {
            return _dataContext.Rooms.FirstOrDefault(room => room.Id == roomId);
        }
    }
}
