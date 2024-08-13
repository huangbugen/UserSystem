using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSystem.Web.Hub
{
    public interface IMessageCommunicator
    {
        Task SendMessageToAll(string channel, string content);
    }
}