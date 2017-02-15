using System.ServiceProcess;
using System.Timers;
using LibClient;

namespace ServiceClient
{
    public partial class ServiceCli : ServiceBase
    {
        public Provider pro;

        private Timer tim;
        public ServiceCli()
        {
            InitializeComponent();
        }

        public void Start(string path)
        {
            if (pro != null) pro.Dispose();
            pro = new Provider();
            pro.Start(path);

            tim = new Timer();
            tim.Interval = 2000;
            tim.Elapsed += Tim_Elapsed;
            tim.Start();
        }

        private void Tim_Elapsed(object sender, ElapsedEventArgs e)
        {
            pro.DetectChanges();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
            tim.Stop();
            if (pro != null)
            {
                pro.Dispose();
            }
        }
    }
}
