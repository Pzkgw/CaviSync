using System.Windows;
using MainLib;

namespace CaviSyncServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            Utils.ExecuteCommand(
    System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() +
    "\\InstallUtil.exe /i /unattended " +
    "ServiceForServer.exe");

        }
    }
}
