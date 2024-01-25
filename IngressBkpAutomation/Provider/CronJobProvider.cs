using IngressBkpAutomation.IProvider;

namespace IngressBkpAutomation.Provider
{
    public class CronJobProvider : ICronJobProvider
    {
        public async Task BackupAttendance()
        {
            Console.WriteLine($"{DateTime.Now.ToString()} - This is a Recurring job!");
        }
    }
}
