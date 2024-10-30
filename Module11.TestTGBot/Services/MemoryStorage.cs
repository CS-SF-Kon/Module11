using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module11.TestTGBot.Models;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace Module11.TestTGBot.Services
{
    public class MemoryStorage : IStorage
    {
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        public Session GetSession(long ChatID)
        {
            if (_sessions.ContainsKey(ChatID))
                return _sessions[ChatID];

            var newSession = new Session() { selectedItem = "count"};
            _sessions.TryAdd(ChatID, newSession);
            return newSession;
        }
    }
}
