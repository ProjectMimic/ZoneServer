using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneServer
{
    public class Timer
    {
        public int Remaining { get; private set; }
        public int Total { get; private set; }
        public bool Expired { get { return Remaining > 0; } }
        public float Percentage { get { return Remaining / (float)Total; } }

        public Timer()
        {
            Remaining = 0;
            Total = 0;
        }
        public Timer(int time)
        {
            Remaining = time;
            Total = time;
        }

        public void Reset(int time)
        {
            Total = time;
            Remaining = time;
        }

        public void Restart()
        {
            Remaining = Total;
        }

        public void Stop()
        {
            Remaining = 0;
            Total = 0;
        }

        public bool Update(int elapsed, bool shouldRestart = false)
        {
            if (Expired)
            {
                return false;
            }

            Remaining -= elapsed;
            if (Expired)
            {
                if (shouldRestart)
                {
                    Restart();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
