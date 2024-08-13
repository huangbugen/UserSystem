using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using UserSystem.Domain.Managers;
using Volo.Abp.AspNetCore.SignalR;

namespace UserSystem.Web.Hub
{
    public class MessageHub : AbpHub
    {
        private readonly MessageManager _messageManager;
        public MessageHub(MessageManager messageManager)
        {
            this._messageManager = messageManager;

        }
        // 消息广播
        public async Task Broadcast(string message)
        {
            await _messageManager.BroadcastMessage(message);
        }
    }
}