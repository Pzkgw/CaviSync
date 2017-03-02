using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MainLib;

namespace CaviSyncClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        const string 
            serviceName = "CaviSyncClientService",
            serviceExe = "ServiceForClient.exe";
        public MainWindow()
        {
            InitializeComponent();

            btnDirRefresh_Click(null, null);

            //        Utils.ExecuteCommand(
            //System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() +
            //"\\InstallUtil.exe /i /unattended " +
            //"ServiceForServer.exe");

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // curatenie de primavara
            textBox1.TextChanged -= TextBox_TextChanged;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetServiceGui(bool v)
        {
            serLbl.Content = v ? "on" : "off";
            btnService.Visibility = v ? Visibility.Collapsed : Visibility.Visible;
            btnSerDel.Visibility = v ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnService_Click(object sender, RoutedEventArgs e)
        {

            if (!Exec.SerIsOn(serviceName))
            {
                //RegistryCon.Update(null, -1, Guid.Empty, pathProviderLocal);

                bool started = true;

                //  The switches (username, password, etc) must
                //be placed before the name of the service to be installed, 
                //otherwise the switches will not be used.I made this mistake
                //so I initially thought that these switches were not working :)

                //Use the /? or / help switch to learn more about the other options
                //that can be used in installing the service

                // InstallUtil.exe /? Service.exe

                // /unattended : will not prompt for username and password

                //VwSer.ProjectInstaller l = new VwSer.ProjectInstaller();
                //l.Install();
                started = (Utils.ExecuteCommand(
                    System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() +
                    "\\InstallUtil.exe /i /unattended " +
                    serviceExe) == 0); //  /user=GI\bogdan.visoiu /password=

                /*
                started = Services.Install(
                                    Settings.serName, Settings.serName, Settings.serExecutabil);

                Services.SetDescriereServiciu(Settings.serName, Settings.serDesc);*/

                infoLbl.Content = (" service" + (started ? "" : " was not") + " started");


                Exec.SerStart(serviceName);
                SetServiceGui(true);

            }
            else
            {
                infoLbl.Content = serviceName + " is already installed";
            }

        }

        /// <summary>
        /// Refresh all GUI data
        /// </summary>
        private void btnDirRefresh_Click(object sender, RoutedEventArgs e)
        {
            textBox1.TextChanged -= TextBox_TextChanged;

            {
                IPAddress ip = Utils.GetLocalIpAddress();
                if (ip != null) labelIp.Content = ip.ToString();
            }

            SetServiceGui(Exec.SerIsOn(serviceName));

            textBox1.Text = RegEdit.ServerGetPath();

            btnDirUpdate.Visibility = Visibility.Collapsed;

            textBox1.TextChanged += TextBox_TextChanged;

        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            btnDirUpdate.Visibility = Visibility.Visible;
        }

        private void btnDirUpdate_Click(object sender, RoutedEventArgs e)
        {
            RegEdit.ServerUpdate(textBox1.Text);
        }

        private void btnSerDel_Click(object sender, RoutedEventArgs e)
        {
            infoLbl.Content = string.Empty;
            Exec.SerDelete(serviceName);

            SetServiceGui(false);
        }
    }
}
