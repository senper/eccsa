using ApiCallerService.ApiClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCallerService
{
    internal class ApiCallScheduler
    {
        private int Delay { get; set; } = -1;
        internal IConfiguration Configuration { get; }
        private Client client { get; }
        internal ApiCallScheduler(IConfiguration configuration)
        {
            Configuration = configuration;
            client = new Client(Configuration);
        }

        internal async Task Start()
        {
            Log.Logger.Information(string.Format("Service Start {0}", DateTime.UtcNow));
            var today = DateTime.Today;
            DateTime? lastSuccessDate = null;
            int delay = 3600000;
            var nextExc = NextExcuteTime();
            while (true) 
            {
                var preExcureTime = PreExcuteTime();
                if (preExcureTime != null && lastSuccessDate == null)
                {
                    lastSuccessDate = await client.LastReadSuccess();
                }

                var scheduleTime = preExcureTime;
                if (( lastSuccessDate >= DateTime.Today || preExcureTime == null) &&
                    nextExc > DateTime.Now)
                {
                    scheduleTime = nextExc;
                    var temp = (nextExc - DateTime.Now).TotalMilliseconds;
                    if (temp < 0)
                    {
                        temp = (DateTime.Now - nextExc).TotalMilliseconds;
                    }
                    var nextDelay = (int)Math.Floor(temp);
                    if (nextDelay > delay) { nextDelay = delay; }
                    var nextTry = DateTime.Now.AddMilliseconds(nextDelay);
                    Log.Logger.Warning(string.Format("Next schedule time {0}", scheduleTime));
                    //Log.Logger.Warning(string.Format("Next try time {0}", nextTry));
                    await Task.Delay(nextDelay);
                    if (DateTime.Now < nextExc)
                    {
                        continue;
                    }
                }

                //var isAuthSuccessful = true;// await client.Authenticate();
                //if (isAuthSuccessful)
                //{
                //Log.Logger.Information(string.Format("successfully authenticated for schedule time {0} at {1}", scheduleTime, DateTime.UtcNow));
                var from = lastSuccessDate ?? DateTime.Today.AddDays(-1);
                Log.Logger.Information(string.Format("Reading data for schedule time {0} at {1}", scheduleTime, DateTime.UtcNow));
                var data = await client.ReadData(from);
                Log.Logger.Information(string.Format("successfully read data for schedule time {0} at {1}", scheduleTime, DateTime.UtcNow));
                if (data != null)
                {
                    var saveSuccess = await client.SaveData(data);
                    if (saveSuccess)
                    {
                        Log.Logger.Information(string.Format("successfully data saved for schedule time {0} at {1}", scheduleTime, DateTime.UtcNow));
                        lastSuccessDate = await client.LastReadSuccess();
                        nextExc = NextExcuteTime();
                    }
                    else
                    {
                        Log.Logger.Information(string.Format("Data not saved for schedule time {0} at {1}", scheduleTime, DateTime.UtcNow));
                    }
                }
                else
                {
                    Log.Logger.Warning(string.Format("Data was not read successfully for schedule time {0}", scheduleTime));
                }
                //}
                //else
                //{
                //    Log.Logger.Warning(string.Format("Unable to Authenticate for schedule time {0}", scheduleTime));
                //}
            }
        }
        private DateTime NextExcuteTime()
        {
            var timer = Configuration.GetSection("TimerConfig");
            var st = timer.GetSection("StartTime").Get<List<DateTime>>();
            if (st.Any(s => s >= DateTime.Now))
            {
                var now = DateTime.Now.AddSeconds(-5);
                var nextMin = st.Where(s => s >= now).OrderBy(s => s).FirstOrDefault();
                return nextMin;
            }
            else if (st.Any(s => s < DateTime.Now))
            {
                var nextMin = st.Where(s => s < DateTime.Now).OrderBy(s => s).FirstOrDefault();
                nextMin = nextMin.AddDays(1);
                return nextMin;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        private DateTime? PreExcuteTime()
        {
            var timer = Configuration.GetSection("TimerConfig");
            var st = timer.GetSection("StartTime").Get<List<DateTime>>();
            if (st.Any(s => s <= DateTime.Now))
            {
                var pre = st.Where(s => s <= DateTime.Now).OrderBy(s => s).FirstOrDefault();
                return pre;
            }
            return null;
        }
        private async Task<DateTime> SetDelay()
        {
            Delay = -1;
            var retryCount = 0;
            var nextMin = DateTime.MinValue;
            while (Delay < 0 && retryCount <= 10)
            {
                var timer = Configuration.GetSection("TimerConfig");
                var st = timer.GetSection("StartTime").Get<List<DateTime>>();
                var now = DateTime.Now;
                double diffInMillsec = -1;
                if (st.Any(s => s >= now))
                {
                    nextMin = st.Where(s => s >= DateTime.Now).OrderBy(s => s).FirstOrDefault();
                    var diff = nextMin - now;
                    diffInMillsec = diff.TotalMilliseconds;
                }
                else if (st.Any(s => s < now))
                {
                    nextMin = st.Where(s => s < now).OrderBy(s => s).FirstOrDefault();
                    nextMin = nextMin.AddDays(1);
                    var diff = nextMin - now;
                    diffInMillsec = diff.TotalMilliseconds;
                }
                Delay = (int)diffInMillsec;
                if (Delay < 0)
                {
                    Log.Logger.Warning("Next Execute time not found");
                    retryCount++;
                    var delay = (int)Math.Pow(10, retryCount);
                    Log.Logger.Information(string.Format("Retry after {0} seconds", delay / 1000));
                }
            }
            return nextMin;
        }

    }
}
