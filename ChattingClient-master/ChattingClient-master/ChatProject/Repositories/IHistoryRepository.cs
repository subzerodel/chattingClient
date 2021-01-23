using ChatProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProject.Repositories
{
    public interface IHistoryRepository
    {
        public void NewHistoryAsync(History history);
        public List<HistoryMessage> GetHistoriesByRoomId(int roomId);
    }
}
