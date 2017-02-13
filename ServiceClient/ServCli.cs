using System.ServiceProcess;

namespace ServiceClient
{
    public partial class ServiceCli : ServiceBase
    {
        public ServiceCli()
        {
            InitializeComponent();
        }

        public void Start(string path)
        {

        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
