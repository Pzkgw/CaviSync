namespace ServiceForServer
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.processInstallerServer = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerServer = new System.ServiceProcess.ServiceInstaller();
            // 
            // processInstallerServer
            // 
            this.processInstallerServer.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.processInstallerServer.Password = null;
            this.processInstallerServer.Username = null;
            // 
            // serviceInstallerServer
            // 
            this.serviceInstallerServer.ServiceName = "CaviSyncServerService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.processInstallerServer,
            this.serviceInstallerServer});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller processInstallerServer;
        private System.ServiceProcess.ServiceInstaller serviceInstallerServer;
    }
}