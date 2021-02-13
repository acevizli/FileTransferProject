using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
// imports

namespace cs408Project_Client
{
    public partial class Form1 : Form
    {
        Socket Client;
        bool connected = false;
        string directory;
        public Form1()
        {
            // initialize form
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        // Connect button event handler
        private void connect_Click(object sender, EventArgs e)
        {
            Thread ReceiveFileData = new Thread(receiveFileList);
            if (!connected)
            {
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                string ip = this.ip.Text;
                int port;
                if (Int32.TryParse(this.port.Text, out port))
                {
                    try
                    {
                        
                        Client.Connect(ip, port); // Connect to server
                        connect.Text = "Disconnect";
                        upload.Enabled = true;
                        ListPublicFiles.Enabled = true;
                        List.Enabled = true;
                        Publish.Enabled = true;
                        Copy.Enabled = true;
                        Download.Enabled = true;
                        Delete.Enabled = true;
                        PublicDownload.Enabled = true;
                        connected = true;
                        logs.AppendText("Connected to the server!\n");
                        string name = this.name.Text;
                        Byte[] buffer = Encoding.Default.GetBytes(name);
                        try
                        {
                            // Send name to server
                            Client.Send(buffer);
                        }
                        catch
                        {
                            // Exception handling
                            logs.AppendText("Server has disconnected\n");
                            connect.Text = "Connect";
                            upload.Enabled = false;
                            ListPublicFiles.Enabled = false;
                            List.Enabled = false;
                            Publish.Enabled = false;
                            Copy.Enabled = false;
                            Download.Enabled = false;
                            Delete.Enabled = false;
                            PublicDownload.Enabled = false;
                            connected = false;
                            Client.Close();
                        }
                        while (connected)
                        {
                            try
                            {
                                // Listen to server for acceptance
                                Byte[] bufferAcceptance = new Byte[2];
                                Client.Receive(bufferAcceptance);
                                if (Encoding.Default.GetString(bufferAcceptance) == "1\0")
                                {
                                    // Success
                                    logs.AppendText("Server has accepted you.\n");
                                    ReceiveFileData.Start();
                                    break;
                                }
                                else
                                {
                                    // Error case: conflicting names
                                    logs.AppendText("Name is already taken. Choose another name!\n");
                                    connect.Text = "Connect";
                                    upload.Enabled = false;
                                    ListPublicFiles.Enabled = false;
                                    List.Enabled = false;
                                    Publish.Enabled = false;
                                    Copy.Enabled = false;
                                    Download.Enabled = false;
                                    Delete.Enabled = false;
                                    PublicDownload.Enabled = false;
                                    connected = false;

                                }
                            }
                            catch
                            {
                                // Error case: server disconnected
                                logs.AppendText("Server has disconnected\n");
                                connect.Text = "Connect";
                                upload.Enabled = false;
                                ListPublicFiles.Enabled = false;
                                List.Enabled = false;
                                Publish.Enabled = false;
                                Copy.Enabled = false;
                                Download.Enabled = false;
                                Delete.Enabled = false;
                                PublicDownload.Enabled = false;
                                connected = false;
                                Client.Close();
                            }
                        }

                    }
                    catch
                    {
                        logs.AppendText("Could not connect to the server!\n");
                    }
                }
            }
            else
            {
                // Return to initial state
                connected = false;
                Client.Close();
                upload.Enabled = false;
                ListPublicFiles.Enabled = false;
                List.Enabled = false;
                Publish.Enabled = false;
                Copy.Enabled = false;
                Download.Enabled = false;
                Delete.Enabled = false;
                PublicDownload.Enabled = false;

                connect.Text = "Connect";
                logs.AppendText("You have disconnected\n");
                ReceiveFileData.Abort();
            }
        }

        // Upload button event handler
        private void upload_Click(object sender, EventArgs e)
        {
            // Open file explorer
            var openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                string text = File.ReadAllText(fileName);
                string[] authorsList = fileName.Split('\\');

                fileName = authorsList[authorsList.Length-1]; // Get file name and extension
                // parser
                Byte[] nameData = Encoding.Default.GetBytes(fileName);
                Byte[] nameLength = BitConverter.GetBytes(nameData.Length);
                Byte[] fileData = Encoding.Default.GetBytes(text);
                int packetCount = (int)Math.Ceiling(fileData.Length / (40000.0 * 1024));
                Byte[] clientData;
                if (packetCount == 1)
                {
                    clientData = new Byte[9 + nameData.Length + fileData.Length];
                    fileData.CopyTo(clientData, 9 + nameData.Length);
                }
                else
                {
                    clientData = new Byte[9 + nameData.Length + 40000 * 1024];
                    fileData.Skip(0).Take(40000 * 1024).ToArray().CopyTo(clientData, 9 + nameData.Length);
                }
                nameLength.CopyTo(clientData, 1);
                BitConverter.GetBytes((int)Math.Ceiling(fileData.Length/1024.0)).CopyTo(clientData,5);
                nameData.CopyTo(clientData, 9);

                try
                {
                    // Send data
                    Client.Send(clientData);
                }
                catch
                {
                    // Error case: server disconnected
                    logs.AppendText("Server has disconnected\n");
                    connected = false;
                    connect.Text = "Connect";
                    upload.Enabled = false;
                    ListPublicFiles.Enabled = false;
                    List.Enabled = false;
                    Publish.Enabled = false;
                    Copy.Enabled = false;
                    Download.Enabled = false;
                    Delete.Enabled = false;
                    PublicDownload.Enabled = false;
                    Client.Close();
                }
                clientData[0] = (byte)1;
                for (int i = 0; i < packetCount-2; i++)
                {
                    fileData.Skip((i+1)*40000*1024).Take(40000*1024).ToArray().CopyTo(clientData, 9 + nameData.Length);
                    try
                    {
                        // Send data
                        Client.Send(clientData);
                    }
                    catch
                    {
                        logs.AppendText("Server has disconnected\n");
                        connected = false;
                        connect.Text = "Connect";
                        upload.Enabled = false;
                        Client.Close();
                        break;
                    }
                }
                if (packetCount > 1)
                {
                    Byte[] clientDataLast = new Byte[9+nameData.Length + fileData.Length - (packetCount - 1) * (40000 * 1024)];
                    fileData.Skip((packetCount - 1) * 40000*1024).Take(fileData.Length - (packetCount - 1) * 40000*1024).ToArray().CopyTo(clientDataLast, 9 + nameData.Length);
                    clientDataLast[0] = (byte)1;
                    nameLength.CopyTo(clientDataLast, 1);
                    BitConverter.GetBytes((int)Math.Ceiling(fileData.Length / 1024.0)).CopyTo(clientDataLast,5);
                    nameData.CopyTo(clientDataLast, 9);
                    try
                    {
                        // Send data
                        Client.Send(clientDataLast);
                    }
                    catch
                    {
                        // Error case: server disconnected
                        logs.AppendText("Server has disconnected\n");
                        connected = false;
                        connect.Text = "Connect";
                        upload.Enabled = false;
                        ListPublicFiles.Enabled = false;
                        List.Enabled = false;
                        Publish.Enabled = false;
                        Copy.Enabled = false;
                        Download.Enabled = false;
                        Delete.Enabled = false;
                        PublicDownload.Enabled = false;
                        Client.Close();
                    }
                }
                logs.AppendText(Encoding.Default.GetString(nameData) + " is sent.\n");
            }
            else
            {
                // Error case: invalid file
                logs.AppendText("Please choose suitible file\n");
            }
        }

        // Handle close button event
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            try
            {
                Client.Close();
                Environment.Exit(0);
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        private void Download_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // set directory for incoming files
                    string dir = fbd.SelectedPath;
                    directory = dir;

                    string fileName = downloadTextBox.Text + ".txt";

                    Byte[] nameData = Encoding.Default.GetBytes(fileName);
                    Byte[] nameLength = BitConverter.GetBytes(0); // 0 means any request
                    Byte[] reqID = BitConverter.GetBytes(3); // 3 = download request
                                                             // Byte[] fileNameLen = BitConverter.GetBytes(nameData.Length);
                    Byte[] clientData = new Byte[9 + nameData.Length];


                    nameLength.CopyTo(clientData, 1); // request
                    reqID.CopyTo(clientData, 5); // length of the name of the copied file
                    nameData.CopyTo(clientData, 9);  // name of the file i wanna copy

                    try
                    {
                        // Send data
                        Client.Send(clientData);
                        logs.AppendText("Download request for " + fileName + " has been sent. \n");
                    }
                    catch (Exception ex)
                    {
                        // Error case: server disconnected
                        logs.AppendText("Server has disconnected\n");
                        logs.AppendText(ex.GetType().ToString());
                        connected = false;
                        connect.Text = "Connect";
                        upload.Enabled = false;
                        ListPublicFiles.Enabled = false;
                        List.Enabled = false;
                        Publish.Enabled = false;
                        Copy.Enabled = false;
                        Download.Enabled = false;
                        Delete.Enabled = false;
                        PublicDownload.Enabled = false;
                        Client.Close();
                    }
                }
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            string fileName = copyTextBox.Text;
            string user = name.Text;

            Byte[] nameData = Encoding.Default.GetBytes(fileName);
            Byte[] nameLength = BitConverter.GetBytes(0); // 0 means any request
            Byte[] reqID = BitConverter.GetBytes(1); // 1 = copy request
           // Byte[] fileNameLen = BitConverter.GetBytes(nameData.Length);
            Byte[] clientData = new Byte[9 + nameData.Length];


            nameLength.CopyTo(clientData, 1); // request
            reqID.CopyTo(clientData, 5); // length of the name of the copied file
            nameData.CopyTo(clientData, 9);  // name of the file i wanna copy
            //nameData.CopyTo(clientData, 9 + nameData.Length);

            try
            {
                // Send data
                Client.Send(clientData);
                logs.AppendText("Copy request has been sent. \n");
            }
            catch (Exception ex)
            {
                // Error case: server disconnected
                logs.AppendText("Server has disconnected\n");
                logs.AppendText(ex.GetType().ToString());
                connected = false;
                connect.Text = "Connect";
                upload.Enabled = false;
                ListPublicFiles.Enabled = false;
                List.Enabled = false;
                Publish.Enabled = false;
                Copy.Enabled = false;
                Download.Enabled = false;
                Delete.Enabled = false;
                PublicDownload.Enabled = false;
                Client.Close();
            }

            
        }

        private void Delete_Click(object sender, EventArgs e)
        {

            string fileName = deleteTextBox.Text;

            Byte[] nameData = Encoding.Default.GetBytes(fileName);
            Byte[] nameLength = BitConverter.GetBytes(0); // 0 means any request
            Byte[] reqID = BitConverter.GetBytes(2); // 2 = Delete request
            Byte[] clientData = new Byte[9 + nameData.Length];

            nameLength.CopyTo(clientData, 1); // request
            reqID.CopyTo(clientData, 5); // length of the name of the deleted file
            nameData.CopyTo(clientData, 9);  // name of the file to be deleted
            try
            {
                // Send data
                Client.Send(clientData);
                logs.AppendText("Delete request has been sent. \n");
            }
            catch (Exception ex)
            {
                // Error case: server disconnected
                logs.AppendText("Server has disconnected\n");
                logs.AppendText(ex.GetType().ToString());
                connected = false;
                connect.Text = "Connect";
                upload.Enabled = false;
                ListPublicFiles.Enabled = false;
                List.Enabled = false;
                Publish.Enabled = false;
                Copy.Enabled = false;
                Download.Enabled = false;
                Delete.Enabled = false;
                PublicDownload.Enabled = false;
                Client.Close();
            }
        }

        private void List_Click(object sender, EventArgs e)
        {
            Byte[] buffer = new Byte[9];
            try
            {
                // Send data
                Client.Send(buffer);
            }
            catch
            {
                // Error case: server disconnected
                logs.AppendText("Server has disconnected\n");
                connected = false;
                connect.Text = "Connect";
                upload.Enabled = false;
                ListPublicFiles.Enabled = false;
                List.Enabled = false;
                Publish.Enabled = false;
                Copy.Enabled = false;
                Download.Enabled = false;
                Delete.Enabled = false;
                PublicDownload.Enabled = false;
                Client.Close();
            }

            
        }
        private void receiveFileList()
        {
            while (connected)
            {
                try
                {
                    // Listen to server for acceptance
                    Byte[] bufferList = new Byte[50000 * 1024];
                    int size = Client.Receive(bufferList);
                    int requestResponse = BitConverter.ToInt32(bufferList, 0);
                    if (requestResponse == 0)
                    {
                        string fileListData = Encoding.Default.GetString(bufferList, 4, size - 4);
                        logs.AppendText("Your files:\n");
                        logs.AppendText(fileListData);
                    }
                    else if (requestResponse == 1)
                    {
                        if (Encoding.Default.GetString(bufferList, 4, 2) == "1\0")
                        {
                            // Success
                            logs.AppendText("Requested copy operation is succeded.\n");
                        }
                        else if (Encoding.Default.GetString(bufferList, 4, 2) == "2\0")
                        {
                            // Authentication error
                            logs.AppendText("Restricted access, you can't copy this file.\n");
                        }
                        else
                        {
                            // Error case: no such file in server
                            logs.AppendText("Copy operation failed, no such file exists!\n");
                        }
                    }
                    else if (requestResponse == 2)
                    {
                        if (Encoding.Default.GetString(bufferList, 4, 2) == "1\0")
                        {
                            // Success
                            logs.AppendText("Requested Delete operation is succeded.\n");
                        }
                        else if (Encoding.Default.GetString(bufferList, 4, 2) == "2\0")
                        {
                            // Authentication error
                            logs.AppendText("Restricted access, you can't delete this file.\n");
                        }
                        else
                        {
                            // Error case: no such file in server
                            logs.AppendText("Delete operation failed, no such file exists!\n");
                        }
                    }
                    else if (requestResponse == 3 || requestResponse == 6)
                    {
                        Boolean flag = BitConverter.ToBoolean(bufferList, 4);
                        if (!flag)
                        {
                            int fileNameLen = BitConverter.ToInt32(bufferList, 5);
                            if (fileNameLen == 0)
                            {
                                logs.AppendText("Unsuccessful download operation. No such file.\n");
                                continue;
                            }
                            string fileName = Encoding.ASCII.GetString(bufferList, 9, fileNameLen);
                            string data = Encoding.ASCII.GetString(bufferList, 9 + fileNameLen, size - 9 - fileNameLen);
                            string path = @directory + "\\" + fileName;
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                // Write file line
                                sw.Write(data);
                            }

                            logs.AppendText(fileName + " is received.\n");
                        }
                        else
                        {
                            int fileNameLen = BitConverter.ToInt32(bufferList, 5);
                            string fileName = Encoding.ASCII.GetString(bufferList, 9, fileNameLen);
                            string data = Encoding.ASCII.GetString(bufferList, 9 + fileNameLen, size - 9 - fileNameLen);
                            string path = @directory + "\\" + fileName;
                            using (StreamWriter sw = File.AppendText(path))
                            {
                                // Write file line
                                sw.Write(data);
                            }

                        }
                    }
                    else if (requestResponse == 4)
                    {
                        if (Encoding.Default.GetString(bufferList, 4, 2) == "1\0")
                        {
                            // Success
                            logs.AppendText("Make Public operation is succeded.\n");
                        }
                        else
                        {
                            // Error case: no such file in server
                            logs.AppendText("Make Public operation failed, no such file exists!\n");
                        }
                    }
                    else if (requestResponse == 5)
                    {
                        string fileListData = Encoding.Default.GetString(bufferList, 4, size - 4);
                        logs.AppendText("Public files:\n");
                        logs.AppendText(fileListData);
                    }
                }
                catch
                {
                    // Error case: server disconnected
                    if(connected) logs.AppendText("Server has disconnected\n");
                    connect.Text = "Connect";
                    upload.Enabled = false;
                    ListPublicFiles.Enabled = false;
                    List.Enabled = false;
                    Publish.Enabled = false;
                    Copy.Enabled = false;
                    Download.Enabled = false;
                    Delete.Enabled = false;
                    PublicDownload.Enabled = false;
                    connected = false;
                    Client.Close();
                }
            }
        }

        private void PublicDownload_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // set directory for incoming files
                    string dir = fbd.SelectedPath;
                    directory = dir;

                    string fileName = publicDownloadUsername.Text + publicDownloadFile.Text + ".txt";

                    Byte[] nameData = Encoding.Default.GetBytes(fileName);
                    Byte[] nameLength = BitConverter.GetBytes(0); // 0 means any request
                    Byte[] reqID = BitConverter.GetBytes(6); // 6 = public download request
                                                             // Byte[] fileNameLen = BitConverter.GetBytes(nameData.Length);
                    Byte[] clientData = new Byte[9 + nameData.Length];


                    nameLength.CopyTo(clientData, 1); // request
                    reqID.CopyTo(clientData, 5); // length of the name of the copied file
                    nameData.CopyTo(clientData, 9);  // name of the file i wanna copy

                    try
                    {
                        // Send data
                        Client.Send(clientData);
                        logs.AppendText("Download request for " + fileName + " has been sent. \n");
                    }
                    catch (Exception ex)
                    {
                        // Error case: server disconnected
                        logs.AppendText("Server has disconnected\n");
                        logs.AppendText(ex.GetType().ToString());
                        connected = false;
                        connect.Text = "Connect";
                        upload.Enabled = false;
                        ListPublicFiles.Enabled = false;
                        List.Enabled = false;
                        Publish.Enabled = false;
                        Copy.Enabled = false;
                        Download.Enabled = false;
                        Delete.Enabled = false;
                        PublicDownload.Enabled = false;
                        Client.Close();
                    }
                }
            }
        }

        private void ListPublicFiles_Click(object sender, EventArgs e)
        {
            Byte[] buffer = new Byte[9];
            BitConverter.GetBytes(5).CopyTo(buffer, 5);
            try
            {
                // Send data
                Client.Send(buffer);
            }
            catch
            {
                // Error case: server disconnected
                logs.AppendText("Server has disconnected\n");
                connected = false;
                connect.Text = "Connect";
                upload.Enabled = false;
                ListPublicFiles.Enabled = false;
                List.Enabled = false;
                Publish.Enabled = false;
                Copy.Enabled = false;
                Download.Enabled = false;
                Delete.Enabled = false;
                PublicDownload.Enabled = false;
                Client.Close();
            }
        }

        private void Publish_Click(object sender, EventArgs e)
        {
            string to_make_public = publishText.Text;
            Byte[] nameData = Encoding.Default.GetBytes(to_make_public);
            Byte[] nameLength = BitConverter.GetBytes(0); // 0 means any request
            Byte[] reqID = BitConverter.GetBytes(4); // 2 = Make Public request
            Byte[] clientData = new Byte[9 + nameData.Length];

            nameLength.CopyTo(clientData, 1); // request
            reqID.CopyTo(clientData, 5); // length of the name of the file to be made public
            nameData.CopyTo(clientData, 9);  // name of the file to be made public
            try
            {
                // Send data
                Client.Send(clientData);
                logs.AppendText("Request to make file public sent \n");
            }
            catch (Exception ex)
            {
                // Error case: server disconnected
                logs.AppendText("Server has disconnected\n");
                logs.AppendText(ex.GetType().ToString());
                connected = false;
                connect.Text = "Connect";
                upload.Enabled = false;
                ListPublicFiles.Enabled = false;
                List.Enabled = false;
                Publish.Enabled = false;
                Copy.Enabled = false;
                Download.Enabled = false;
                Delete.Enabled = false;
                PublicDownload.Enabled = false;
                Client.Close();
            }
        }
    }
}