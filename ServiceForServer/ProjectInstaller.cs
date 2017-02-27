using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceForServer
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }




        public string GetContextParameter(string key)
        {
            string sValue = "";
            try
            {
                sValue = this.Context.Parameters[key].ToString();
            }
            catch
            {
                sValue = "";
            }
            return sValue;
        }


        // Override the 'OnBeforeInstall' method.
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            string username = GetContextParameter("user").Trim();
            string password = GetContextParameter("password").Trim();

            if (username != "")
                processInstallerServer.Username = username;
            if (password != "")
                processInstallerServer.Password = password;

            base.OnBeforeInstall(savedState);
        }








    }
}
