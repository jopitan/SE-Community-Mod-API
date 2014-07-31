using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SEServerExtender.Threads
{
    public class Worker
    {

        private volatile int interval;
        private volatile bool shutdown;

        public int Interval { 
            get {
                return interval;
            }
            set
            {
                interval = value;
            }
        }

        public Worker()
        {
            Interval = 5000;
            shutdown = false;
        }

        public Worker(int interval)
        {
            Interval = interval;
            shutdown = false;
        }

        public void Shutdown()
        {
            shutdown = true;
        }

        public void Reset()
        {
            shutdown = false;
        }

        public virtual void Work() {}

        public void Run()
        {
            Reset();
            while (!shutdown)
            {
                Work();
                Thread.Sleep(Interval);
            }
        }

    }
}
