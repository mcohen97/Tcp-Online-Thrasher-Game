using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace Services
{
    class SessionsAdmin
    {
        private static readonly Lazy<SessionsAdmin> instance = new Lazy<SessionsAdmin>(() => new SessionsAdmin());
        private ICollection<Session> ActualSessions; 

        private SessionsAdmin()
        {
            ActualSessions = new List<Session>();
        }

        public static SessionsAdmin Instance {
            get {
                return instance.Value;
            }
        }

        public void AddSession(Session aSession) {
            ActualSessions.Add(aSession);
        }

        public void RemoveSession(Session aSession)
        {
            ActualSessions.Remove(aSession);
        }

        public Session GetUserSession(User connected) {
            return ActualSessions.First(u => u.Logged.Equals(connected));
        }
    }
}
