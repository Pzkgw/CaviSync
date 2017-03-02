namespace ServiceForClient
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
            this.processInstallerClient = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerClient = new System.ServiceProcess.ServiceInstaller();
            // 
            // processInstallerClient
            // 
            this.processInstallerClient.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.processInstallerClient.Password = null;
            this.processInstallerClient.Username = null;
            // 
            // serviceInstallerClient
            // 
            this.serviceInstallerClient.ServiceName = "CaviSyncClientService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.processInstallerClient,
            this.serviceInstallerClient});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller processInstallerClient;
        private System.ServiceProcess.ServiceInstaller serviceInstallerClient;
    }
}