using ChatProject.Data;
using System.Threading.Tasks;

namespace ChatProject.Repositories.Implementation
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dataContext;

        public MessageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> NewMessage(Models.Message message)
        {
            var newMsg = _dataContext.Messages.Add(message);
            await _dataContext.SaveChangesAsync();
            return newMsg.Entity.Id;
        }
    }
}
