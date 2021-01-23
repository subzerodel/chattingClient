using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProject.Repositories.Implementation
{
    public interface IMessageRepository
    {
        public Task<int> NewMessage(Models.Message message);
    }
}
