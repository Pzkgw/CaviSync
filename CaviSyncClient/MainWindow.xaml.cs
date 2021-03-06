﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MainLib;

namespace CaviSyncClient
{
    /// <summary>
    /// CAVI synchronization CLIENT application
    /// </summary>
    public partial class MainWindow : Window
    {

        const string
            serviceName = "CaviSyncClientService",
            serviceExe = "ServiceForClient.exe"
            ;
        public MainWindow()
        {
            InitializeComponent();

            btnDirRefresh_Click(null, null);

            textBoxIPServer.Text = "10.10.10.15";
            textBoxIPServer.Width = 130;
            textBoxIPServer.TextAlignment = TextAlignment.Center;

            UpdateDirectoryListButtons();

        }


        private void UpdateDirectoryListButtons()
        {
            const string add = "  Add  ", del = " Clear ";
            btn1.Content = (textBox1.Content.ToString().Length < 3) ? add : del;
            btn2.Content = (textBox2.Content.ToString().Length < 3) ? add : del;
            btn3.Content = (textBox3.Content.ToString().Length < 3) ? add : del;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // curatenie de primavara

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

            {
                string replyStatus = null;
                if (!Utils.ServerIpAddressDoesAnswer(textBoxIPServer.Text, ref replyStatus))
                {
                    System.Windows.MessageBox.Show(string.Format(
                        " Server IP does not answer.{0}{0} Reply status: {1}",
                        Environment.NewLine, replyStatus));
                    return;
                }
            }

            Optiuni.EndpointIP = textBoxIPServer.Text;
            RegEdit.ClientUpdate(Optiuni.EndpointIP);

            // Verific conexiunea:
            // portul e deschis si se pot trimite, primi mesaje
            {
                string rv = ServiceForClient.TestConexiune.DoEeet();

                if (rv != null)
                {
                    System.Windows.MessageBox.Show(string.Format("{0}{1}{1}{2}",
                        "Server does not answer.", Environment.NewLine, rv));
                    return;
                }
            }

            if (!Exec.SerIsOn(serviceName))
            {
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
                    serviceExe) == 0); // 

                /*
                started = Services.Install(
                                    Settings.serName, Settings.serName, Settings.serExecutabil);

                Services.SetDescriereServiciu(Settings.serName, Settings.serDesc);*/

                if (started)
                {
                    if (AtLeastOneSyncDirectoryAdded())
                        infoLbl.Content = "Service started";
                    else
                        infoLbl.Content = "Empty service started. Add some directory";
                }
                else
                    infoLbl.Content = "Service was not started";

                Exec.SerStart(serviceName);
                SetServiceGui(true);

            }
            else
            {
                infoLbl.Content = serviceName + " is already installed";
            }

        }

        private bool AtLeastOneSyncDirectoryAdded()
        {
            return (
                (textBox1.Content != null && textBox1.Content.ToString().Length > 2) ||
                (textBox2.Content != null && textBox2.Content.ToString().Length > 2) ||
                (textBox3.Content != null && textBox3.Content.ToString().Length > 2));
        }


        /// <summary>
        /// Refresh all GUI data
        /// </summary>
        private void btnDirRefresh_Click(object sender, RoutedEventArgs e)
        {

            {
                IPAddress ip = Utils.GetLocalIpAddress();
                if (ip != null) labelIp.Content = ip.ToString();
            }

            SetServiceGui(Exec.SerIsOn(serviceName));

            string someString;

            someString = RegEdit.ClientGetPath(1);
            textBox1.Content = (someString == null) ? RegEdit.emptyDirectory : someString;

            someString = RegEdit.ClientGetPath(2);
            textBox2.Content = (someString == null) ? RegEdit.emptyDirectory : someString;

            someString = RegEdit.ClientGetPath(3);
            textBox3.Content = (someString == null) ? RegEdit.emptyDirectory : someString;

        }


        private void btnDirSelect_Click(object sender, RoutedEventArgs e)
        {
            infoLbl.Content = string.Empty;

            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            string newTextBoxString = RegEdit.emptyDirectory;
            int idx = 0;

            bool folderAdd = btn.Content.ToString().Contains("d");

            if (folderAdd) // "Add"
            {
                var dialog = new FolderBrowserDialog();
                DialogResult result = dialog.ShowDialog();

                newTextBoxString = dialog.SelectedPath;
            }

            System.Windows.Controls.Label uiElem = null;
            switch (btn.Name[btn.Name.Length - 1])
            {
                case '1':
                    uiElem = textBox1;
                    idx = 1;
                    break;
                case '2':
                    uiElem = textBox2;
                    idx = 2;
                    break;
                case '3':
                    uiElem = textBox3;
                    idx = 3;
                    break;
                default:
                    break;
            }

            if (RegEdit.ClientUpdate(newTextBoxString, idx) && uiElem != null)
            {
                uiElem.Content = newTextBoxString;
            }
            else
            {
                infoLbl.Content = "Directory not added.It does not exist or he is already in the list";
            }

            UpdateDirectoryListButtons();
        }

        private void btnSerDel_Click(object sender, RoutedEventArgs e)
        {
            infoLbl.Content = string.Empty;
            Exec.SerDelete(serviceName);

            SetServiceGui(false);
        }
    }
}
