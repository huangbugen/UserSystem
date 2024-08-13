using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;

namespace UserSystem.Web.Hub
{
    public class MessageCommunicator : IMessageCommunicator, ITransientDependency
    {
        private readonly IHubContext<MessageHub> _context;
        public MessageCommunicator(IHubContext<MessageHub> context)
        {
            this._context = context;
        }

        public async Task SendMessageToAll(string channel, string content)
        {
            await _context.Clients.All.SendAsync(channel, content);
        }
    }
}