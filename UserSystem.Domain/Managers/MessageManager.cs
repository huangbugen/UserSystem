using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSystem.Web.Hub;
using Volo.Abp.Domain.Services;

namespace UserSystem.Domain.Managers
{
    public class MessageManager : DomainService
    {
        private readonly IMessageCommunicator _messageCommunicator;
        public MessageManager(IMessageCommunicator messageCommunicator)
        {
            this._messageCommunicator = messageCommunicator;
        }

        public async Task BroadcastMessage(object obj)
        {
            string content = obj.ToString();
            await _messageCommunicator.SendMessageToAll("broadcast", content);
        }
    }

    public class Singleton<T> where T : class, new()
    {
        private static volatile T _instance;
        private static readonly object lockObject = new object();

        public static T GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}