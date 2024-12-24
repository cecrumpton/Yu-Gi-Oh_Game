using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Mediators
{
    public class DuelMatMediator : IMediator
    {
        private readonly Dictionary<string, Action<object?>> _eventHandlers = new();

        public void Register(string eventName, Action<object?> handler)
        {
            if (_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName] += handler;
            }
            else
            {
                _eventHandlers[eventName] = handler;
            }
        }

        public void Notify(object sender, string eventName, object? parameter = null)
        {
            if (_eventHandlers.TryGetValue(eventName, out var handlers))
            {
                handlers?.Invoke(parameter);
            }
        }
    }

}
