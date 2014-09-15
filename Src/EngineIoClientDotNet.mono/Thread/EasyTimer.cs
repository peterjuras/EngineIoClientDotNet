﻿using EngineIoClientDotNet.Modules;
using Quobject.EngineIoClientDotNet.Modules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Quobject.EngineIoClientDotNet.Thread
{
    public class EasyTimer
    {
        private CancellationTokenSource ts;    

        public EasyTimer(CancellationTokenSource ts)
        {
            this.ts = ts;
        }

        public static EasyTimer SetTimeout(Action method, int delayInMilliseconds)
        {
            var ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            var task = Task.Delay(delayInMilliseconds,ct);
            var awaiter = task.GetAwaiter();

            awaiter.OnCompleted(
                () =>
                {
                    if (!ts.IsCancellationRequested)
                    {
                        method();
                    }
            });
           
            
            // Returns a stop handle which can be used for stopping
            // the timer, if required
            return new EasyTimer(ts);
        }

        internal void Stop()
        {
            var log = LogManager.GetLogger(Global.CallerName());
            log.Info("EasyTimer stop");
            if (ts != null)
            {
                ts.Cancel();                
            }           
        }
    }


}
