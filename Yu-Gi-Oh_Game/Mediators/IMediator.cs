using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yu_Gi_Oh_Game.Mediators
{
    public interface IMediator
    {
        void Notify(object sender, string eventName, object? parameter = null);
    }

}
