using ChatProject.Data;
using ChatProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace ChatProject.Repositories.Implementation
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly DataContext _dataContext;

        public HistoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<HistoryMessage> GetHistoriesByRoomId(int roomId)
        {
            return _dataContext.HistoryList.Where(history => history.RoomId == roomId).Join(_dataContext.Messages,
                   h => h.MessageId,
                   m => m.Id,
                   (h,m) => new HistoryMessage(){
                        Username = m.Username,
                        Text = m.Text,
                        RoomId = h.RoomId
                   }).ToList();
        }

        public async void NewHistoryAsync(History history)
        {
            _dataContext.HistoryList.Add(history);
            await _dataContext.SaveChangesAsync();
        }
    }
}
