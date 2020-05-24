using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organaizer_v2.Model
{
    public interface IData { }

    public class InitializationViewModel : IData { }
    public class UserJoinIntoAccount : IData { }

    public class InvokeNotification : IData
    {
        public InvokeNotification(Notification notification)
        {
            Notification = notification;
        }

        public Notification Notification { get; private set; }
    }

    public class InvokeTimerTick : IData
    {
        public InvokeTimerTick(int time)
        {
            this.Time = time;
        }

        public InvokeTimerTick(int time, bool timerState) : this(time)
        {
            TimerState = timerState;
        }

        public int Time { get; private set; }
        public bool TimerState { get; private set; }
    }

    public class LoadNeccessaryData : IData { }
}
