using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceServe
{
    public partial class ServiceServer : ServiceBase
    {
        public ServiceServer()
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
