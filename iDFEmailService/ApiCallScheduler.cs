using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iDFEmailService.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace iDFEmailService
{
    internal class ApiCallScheduler
    {
        private int Delay { get; set; } = -1;
        internal IConfiguration Configuration { get; }
        private MailClient.Client client { get; }
        private MailClient.Client clientDemo { get; }
        internal ApiCallScheduler(IConfiguration configuration)
        {
            Configuration = configuration;
            client = new MailClient.Client(Configuration);
            clientDemo = new MailClient.Client(Configuration, true);
        }

        internal async Task Start()
        {
            Log.Logger.Information(string.Format("Service Start {0}", DateTime.UtcNow));
            var lastSendDate = DateTime.UtcNow.Date;
            var remainingForDay = 1;
            int emailDelay = Configuration.GetSection("TimerConfig").GetSection("EmailDelay").Get<int>();
            int delay = emailDelay;
            while (true)
            {
                Log.Logger.Information(string.Format("Next schedule time {0}", DateTime.UtcNow.AddMilliseconds(delay)));
                await Task.Delay(delay);

                //if (remainingForDay > 0 || lastSendDate < DateTime.UtcNow.Date)
                // {
                try
                {
                    remainingForDay = await client.SendEmail();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(string.Format("Error on send email {0}", DateTime.UtcNow.Date), ex);
                }
                //lastSendDate = DateTime.UtcNow.Date;

                Log.Logger.Information(string.Format("Remaining send for today {0} is {1}", DateTime.UtcNow.Date, remainingForDay));
                //   }
                //   else
                //    {
                //        Log.Logger.Information(string.Format("Remaining send for today {0} is {1}. So Skip send at {2}", DateTime.UtcNow.Date, remainingForDay, DateTime.UtcNow));
                //    }
                try
                {
                    await client.CheckDelivery();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(string.Format("Error on delivery check {0}", DateTime.UtcNow.Date), ex);
                }
                ///
                try
                {
                    remainingForDay = await clientDemo.SendEmail();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(string.Format("Error on send email demo {0}", DateTime.UtcNow.Date), ex);
                }

                Log.Logger.Information(string.Format("For Demo: Remaining send for today {0} is {1}", DateTime.UtcNow.Date, remainingForDay));
                try
                {
                    await clientDemo.CheckDelivery();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(string.Format("Error on delivery check demo {0}", DateTime.UtcNow.Date), ex);
                }

                try
                {
                    await client.SendSMS();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(string.Format("Error on sending SMS {0}", DateTime.UtcNow.Date), ex);
                }


                try
                {
                    await clientDemo.SendSMS();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(string.Format("Error on sending SMS demo {0}", DateTime.UtcNow.Date), ex);
                }
            }
        }
    }
}
