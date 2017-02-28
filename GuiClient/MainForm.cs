using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MainLib;
using System.Diagnostics;

namespace FileServerWinClient
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Handles the Load event of the Form1 control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Form1_Load(object sender, EventArgs e)
		{
			RefreshFileList();
		}

		/// <summary>
		/// Refreshes the file list.
		/// </summary>
		private void RefreshFileList()
		{
			StorageFileInfo[] files = null;

			using (FileRepositoryServiceClient client = new FileRepositoryServiceClient())
			{
				files = client.List(null);
			}

			FileList.Items.Clear();

			int width = FileList.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;

			float[] widths = { .2f, .7f, .1f };

			for (int i = 0; i < widths.Length; i++)
				FileList.Columns[i].Width = (int)((float)width * widths[i]);

			foreach (var file in files)
			{
				ListViewItem item = new ListViewItem(Path.GetFileName(file.VirtualPath));

				item.SubItems.Add(file.VirtualPath);

				float fileSize = (float)file.Size / 1024.0f;
				string suffix = "Kb";

				if (fileSize > 1000.0f)
				{
					fileSize /= 1024.0f;
					suffix = "Mb";
				}
				item.SubItems.Add(string.Format("{0:0.0} {1}", fileSize, suffix));

				FileList.Items.Add(item);
			}
		}

		private void DownloadButton_Click(object sender, EventArgs e)
		{

			if (FileList.SelectedItems.Count == 0)
			{
				MessageBox.Show("You must select a file to download");
			}
			else
			{
				ListViewItem item = FileList.SelectedItems[0];

				// Strip off 'Root' from the full path
				string path = item.SubItems[1].Text;

				// Ask where it should be saved
				SaveFileDialog dlg = new SaveFileDialog()
				{
					RestoreDirectory = true,
					OverwritePrompt = true,
					Title = "Save as...",
					FileName = Path.GetFileName(path)
				};

				dlg.ShowDialog(this);

				if (!string.IsNullOrEmpty(dlg.FileName))
				{
					// Get the file from the server
					using (FileStream output = new FileStream(dlg.FileName, FileMode.Create))
					{
						Stream downloadStream;

						using (FileRepositoryServiceClient client = new FileRepositoryServiceClient())
						{
							downloadStream = client.GetFile(path);
						}

						downloadStream.CopyTo(output);
					}

					//Process.Start(dlg.FileName);
				}
			}

		}

		private void UploadButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog()
			{
				Title = "Select a file to upload",
				RestoreDirectory = true,
				CheckFileExists = true
			};

			dlg.ShowDialog();

			if (!string.IsNullOrEmpty(dlg.FileName))
			{
                string virtualPath = Path.GetFileName(dlg.FileName);

                Optiuni.dirClient = Path.GetDirectoryName(dlg.FileName);

                FileInfo fi = null;
                long fileSize = 0;

                FileUploadMessage fum = new FileUploadMessage()
                {
                    VirtualPath = virtualPath,
                    LastWriteTimeUtcTicks = 0
                };

                if (File.Exists(dlg.FileName))
                {
                    fi = new FileInfo(dlg.FileName);
                    fum.LastWriteTimeUtcTicks = fi.LastWriteTimeUtc.Ticks;
                    fileSize = fi.Length;
                    fi = null;
                }                

                using (Stream uploadStream = new FileStream(dlg.FileName, FileMode.Open))
				{
					using (FileRepositoryServiceClient client = new FileRepositoryServiceClient())
					{                        
                        string ip = Utils.GetLocalIpAddress().ToString();
                        int port = Optiuni.EndpointPort;
                        client.SendConnectionInfo(ip, port, Path.GetDirectoryName(dlg.FileName));

                        fum.DataStream = uploadStream;

                        if (client.GetPreUploadCheckResult(ip,
                            Optiuni.GetDirClient(), fum.VirtualPath, fum.LastWriteTimeUtcTicks, fileSize))
                        {
                            client.PutFile(fum);
                        }
					}
				}

				RefreshFileList();
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{

			if (FileList.SelectedItems.Count == 0)
			{
				MessageBox.Show("You must select a file to delete");
			}
			else
			{
				string virtualPath = FileList.SelectedItems[0].SubItems[1].Text;

				using (FileRepositoryServiceClient client = new FileRepositoryServiceClient())
				{
					client.DeleteFile(virtualPath);
				}

				RefreshFileList();
			}

		}

	}
}
