using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
// imports

namespace CS408_Project_Server
{
    public partial class Form1 : Form
    {
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // server instance
        List<Tuple<Socket,string>> clients = new List<Tuple<Socket,string>>(); // socket list
        List<string> files = new List<string>();

        // server variables
        bool listen = false;
        bool terminating = false;
        string directory;
        string DBdirectory;

        public Form1()
        {
            // thread config
            DBdirectory = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + "\\DB.txt";
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        // Listen button event handler
        private void Listen_Button_Click(object sender, EventArgs e)
        {
            int Port;
            if (Int32.TryParse(Port_TextBox.Text, out Port))
            {
                // Success case: valid port
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, Port);
                server.Bind(endPoint);
                server.Listen(3);
                listen = true;
                Listen_Button.Enabled = false;
                Directory_Button.Enabled = false;
                // Start accepting clients
                Thread acceptClients = new Thread(Accept);
                acceptClients.Start();
                Status_Update.AppendText("Started listening on Port Number: " + Port + "\n");
            }
            else {
                // Error case: invalid port
                Status_Update.AppendText("Please Check Port Number\n");
            }
        }


        // Close button event handler
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listen = false;
            terminating = true;
            Environment.Exit(0);
        }


        // Handle incoming client
        private void Accept()
        {
            while (listen) {
                try
                {
                    Socket newClient = server.Accept();
                    string newName;
                    if (ReceiveName(newClient,out newName) == true)
                    {
                        // Success case: accept client
                        Tuple<Socket, string> t = new Tuple<Socket, string>(newClient, newName);
                        clients.Add(t);
                        Thread getFile = new Thread(() => receiveFile(t));
                        getFile.Start();
                    }
                    else
                    {
                        // Error case: conflicting client name
                        if (newClient.Connected)
                        {
                            RejectName(newClient);
                            Status_Update.AppendText("A newcomer Client has been rejected due to existing name\n");
                            newClient.Close();
                        }
                    }
                }
                catch {
                    // Error case: client acceptance exception
                    if (terminating)
                    {
                        listen = false;
                    }
                    else
                    {
                        listen = false;

                        Status_Update.AppendText("The socket stopped working.\n");
                    }

                }
                    
            }
        }

        // Handle incoming file
        private void receiveFile(Tuple<Socket,string> t)
        {
            bool connected = true;

            while (connected == true && terminating == false) {
                try
                {
                    // Set max file size
                    Byte[] clientFile = new Byte[50000*1024];
                    int dataLen = t.Item1.Receive(clientFile);
                    Boolean flag = BitConverter.ToBoolean(clientFile, 0);
                    if (!flag)
                    {
                        int fileNameLen = BitConverter.ToInt32(clientFile, 1);
                        if(fileNameLen == 0)
                        {
                            int request = BitConverter.ToInt32(clientFile, 5);
                            if (request == 0) ///////////// LIST PART ///////////////////
                            {
                                Thread sendFileData = new Thread(() => SendList(t));
                                sendFileData.Start();
                                sendFileData.Join();
                                continue;
                            } ////////////////// LIST PART ENDS HERE ////////////////////


                            else if (request == 1) ///////////////// COPY PART //////////////
                            {
                                
                                // fileNameLen= 0 means copy request
                                //int nameLen = BitConverter.ToInt32(clientFile, 5);
                                //name of the file to be copied
                                string file_name = Encoding.ASCII.GetString(clientFile, 9, dataLen - 9);
                                //name of the user who did the copy request
                                string path_c = @directory + "\\" + t.Item2 + file_name + ".txt";
                                int ij = 1;
                                string tmp = "";
                                Status_Update.AppendText(Encoding.ASCII.GetString(clientFile, 0, 50));
                                while (File.Exists(path_c))
                                {
                                    tmp = path_c; // keeping the path of the LAST COPY within the given name 
                                                // as a copy source for the requested file.
                                                // Construct path
                                    path_c = @directory + "\\" + t.Item2 + file_name + "-0" + ij.ToString() + ".txt";
                                    ij++;
                                }
                                string[] lines = File.ReadAllLines(DBdirectory);
                                string to_copy = t.Item2 + " File: " + file_name + ".txt";
                                


                                Byte[] res = new Byte[64];  // result of the copy operation
                                BitConverter.GetBytes(1).CopyTo(res, 0);
                                try
                                {
                                    string data_c = File.ReadAllText(tmp);
                                    
                                    using (StreamWriter sw = File.CreateText(path_c))
                                    {
                                        // Write file line
                                        sw.Write(data_c);
                                    }
                                    string DBdata_c = "";// = t.Item2 + " File: " + path_c.Substring(path_c.IndexOf(file_name)) + " Size: " + data_c.Length + " Private " + "Time: " + DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt") + "\n";
                                    for (int temp = 0; temp < lines.Length; temp++)
                                    {
                                        if (lines[temp].Contains(to_copy))
                                        {
                                            DBdata_c = lines[temp];
                                        }
                                    }
                                    string[] tmp_arr = DBdata_c.Split(' ');
                                    tmp_arr[2] = file_name + "-0" + (ij-1).ToString() + ".txt";
                                    tmp_arr[6] = "Time: " + DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt") + "\n";
                                    

                                    DBdata_c = String.Join(" ", tmp_arr, 0, 7);
                                    using (StreamWriter sw = File.AppendText(DBdirectory))
                                    {
                                        // Write file line
                                        sw.Write(DBdata_c);
                                    }
                                    files.Add(path_c);
                                    
                                    // Status update
                                    Status_Update.AppendText("From client " + t.Item2 + "copy request for file " + "\"" + file_name + ".txt\" executed.\n");
                                    Encoding.Default.GetBytes("1\0").CopyTo(res, 4); // COPY OPERATION SUCCEDEED
                                }
                                catch (Exception ex)
                                {
                                    bool auth_flg = false;
                                    Encoding.Default.GetBytes("0\0").CopyTo(res, 4); // COPY OPERATION FAILED
                                    Status_Update.AppendText("Copy operation failure!\n");
                                    for (int temp = 0; temp < lines.Length; temp++)
                                    {
                                        if (lines[temp].Contains(file_name))
                                        {
                                            auth_flg = true;
                                            Status_Update.AppendText($"User {t.Item2} has no YETKI to copy this file.\n");
                                            Encoding.Default.GetBytes("2\0").CopyTo(res, 4); // AUTH ERROR
                                            break;
                                        }
                                    }
                                    if (!auth_flg)
                                    {
                                        Status_Update.AppendText("No such file exists.\n");
                                    }
                                    
                                }

                                try
                                {
                                    // send response to client
                                    t.Item1.Send(res);
                                }
                                catch
                                {
                                    // Error case: server exceptance
                                    Status_Update.AppendText("A problem occured, check your connection\n");
                                    terminating = true;

                                    server.Close();
                                }
                                continue;
                            }
                            ///////////// COPY PART ENDS HERE ////////////////////////////
                            else if (request == 2) /////////////////////// DELETE PART STARTS HERE /////////////////////
                            {
                                Status_Update.AppendText("Delete request received\n");
                                string file_name = Encoding.ASCII.GetString(clientFile, 9, dataLen - 9);
                                string path_delete = @directory + "\\" + t.Item2 + file_name + ".txt";
                                Byte[] res = new Byte[64];  // result of the delete operation
                                BitConverter.GetBytes(2).CopyTo(res, 0);
                                try
                                {
                                    if (File.Exists(path_delete))
                                    {
                                        string to_delete = t.Item2 + " File: " + file_name + ".txt";
                                        File.Delete(path_delete); 
                                        Status_Update.AppendText("File Deleted\n");
                                        string[] lines = File.ReadAllLines(DBdirectory);
                                        for (int temp = 0; temp < lines.Length; temp++) {
                                            if (lines[temp].Contains(to_delete)) {
                                                lines[temp] = "d e l e t e d";
                                                break;
                                            }
                                        }

                                        File.WriteAllLines(DBdirectory, lines);
                                        Encoding.Default.GetBytes("1\0").CopyTo(res, 4); // Delete OPERATION SUCCEDEED

                                    }
                                    else
                                    {
                                        bool auth_flag = false;
                                        string[] lines = File.ReadAllLines(DBdirectory);
                                        for (int temp = 0; temp < lines.Length; temp++)
                                        {
                                            if (lines[temp].Contains(file_name))
                                            {
                                                auth_flag = true;
                                                break;
                                            }
                                        }
                                        if (auth_flag)
                                        {
                                            Status_Update.AppendText($"User {t.Item2} has no YETKI to delete this file.\n");
                                            Encoding.Default.GetBytes("2\0").CopyTo(res, 4); // Delete OPERATION FAILED
                                            //due to an authentication error
                                        }
                                        else
                                        {
                                            Status_Update.AppendText(path_delete);
                                            Status_Update.AppendText("File Not Found\n");
                                            Encoding.Default.GetBytes("0\0").CopyTo(res, 4); // Delete OPERATION FAILED
                                        }
                                    }
                                }
                                catch
                                {
                                    // Error case: server exceptance
                                    Status_Update.AppendText("A problem occured, check your connection\n");
                                    terminating = true;

                                    server.Close();
                                }
                                try
                                {
                                    // send response to client
                                    t.Item1.Send(res);
                                }
                                catch
                                {
                                    // Error case: server exceptance
                                    Status_Update.AppendText("A problem occured, check your connection\n");
                                    terminating = true;

                                    server.Close();
                                }
                                continue;
                            }
                            //////////// DELETE PART ENDS HERE ///////////////////////////////
                            else if (request == 3) ///////////// UPLOAD PART ///////////////////
                            {
                                try
                                {
                                    string uploadFileName = Encoding.ASCII.GetString(clientFile, 9, dataLen - 9);
                                    if(!File.Exists(@directory + "\\"+t.Item2 + uploadFileName))
                                    {
                                        Byte[] buffer = new byte[9];
                                        BitConverter.GetBytes(3).CopyTo(buffer, 0);
                                        t.Item1.Send(buffer);
                                        continue;
                                    }
                                    string text = File.ReadAllText(@directory + "\\" +t.Item2+ uploadFileName);
                                    Byte[] nameData = Encoding.Default.GetBytes(uploadFileName);
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
                                    BitConverter.GetBytes(3).CopyTo(clientData, 0);
                                    nameLength.CopyTo(clientData, 5);
                                    nameData.CopyTo(clientData, 9);

                                    try
                                    {
                                        // Send data
                                        t.Item1.Send(clientData);
                                    }
                                    catch
                                    {
                                        Status_Update.AppendText("Client has disconnected\n");
                                        t.Item1.Close();
                                    }
                                    clientData[4] = (byte)1;
                                    for (int j = 0; j < packetCount - 2; j++)
                                    {
                                        fileData.Skip((j + 1) * 40000 * 1024).Take(40000 * 1024).ToArray().CopyTo(clientData, 9 + nameData.Length);
                                        try
                                        {
                                            // Send data
                                            
                                            t.Item1.Send(clientData);
                                            
                                        }
                                        catch
                                        {
                                            Status_Update.AppendText("Client has disconnected\n");
                                            t.Item1.Close();
                                        }
                                    }
                                    if (packetCount > 1)
                                    {
                                        Byte[] clientDataLast = new Byte[9 + nameData.Length + fileData.Length - (packetCount - 1) * (40000 * 1024)];
                                        fileData.Skip((packetCount - 1) * 40000 * 1024).Take(fileData.Length - (packetCount - 1) * 40000 * 1024).ToArray().CopyTo(clientDataLast, 9 + nameData.Length);
                                        BitConverter.GetBytes(3).CopyTo(clientDataLast, 0);
                                        clientDataLast[4] = (byte)1;
                                        nameLength.CopyTo(clientDataLast, 5);
                                        nameData.CopyTo(clientDataLast, 9);
                                        try
                                        {
                                            // Send data
                                            t.Item1.Send(clientDataLast);
                                            Status_Update.AppendText("Last\n");
                                        }
                                        catch
                                        {
                                            Status_Update.AppendText("Client has disconnected\n");
                                            t.Item1.Close();
                                        }
                                    }
                                    Status_Update.AppendText(Encoding.Default.GetString(nameData) + " is sent.\n");
                                }
                                catch
                                {
                                    Status_Update.AppendText("Invalid download request received\n");
                                    t.Item1.Close();
                                }

                                continue;
                            } ////////////////// UPLOAD PART ENDS HERE ////////////////////
                            else if (request == 4)//////////////////////// Make Public Part Starts Here///////////////////////
                            {
                                Status_Update.AppendText("Make Public request received\n");
                                string file_name = Encoding.ASCII.GetString(clientFile, 9, dataLen - 9);
                                string to_match = t.Item2 + " File: " + file_name + ".txt";
                                Byte[] res = new Byte[64];  // result of the make public operation
                                BitConverter.GetBytes(4).CopyTo(res, 0);
                                try
                                {
                                        string[] lines = File.ReadAllLines(DBdirectory);
                                        int temp = 0;
                                        for (; temp < lines.Length; temp++)
                                        {
                                            if (lines[temp].Contains(to_match))
                                            {
                                                string[] line = lines[temp].Split(' ');
                                                line[5] = "Public";
                                                lines[temp] = string.Join(" ", line);
                                                Encoding.Default.GetBytes("1\0").CopyTo(res, 4);
                                                break;
                                            }
                                        }


                                    File.WriteAllLines(DBdirectory, lines);

                                        if (temp == lines.Length)
                                        {
                                            Encoding.Default.GetBytes("0\0").CopyTo(res, 4); // Make Public OPERATION Failed
                                        }

                                }
                                catch
                                {
                                    // Error case: server exceptance
                                    Status_Update.AppendText("A problem occured, check your connection\n");
                                    terminating = true;

                                    server.Close();
                                }
                                try
                                {
                                    // send response to client
                                    t.Item1.Send(res);
                                }
                                catch
                                {
                                    // Error case: server exceptance
                                    Status_Update.AppendText("A problem occured, check your connection\n");
                                    terminating = true;

                                    server.Close();
                                }
                                continue;
                            }
                            else if(request == 5)
                            {
                                Thread sendPublic = new Thread(() => sendPublicList(t));
                                sendPublic.Start();
                                sendPublic.Join();
                                continue;
                            }
                            else if (request == 6) ///////////// PUBLIC UPLOAD PART ///////////////////
                            {
                                try
                                {
                                    string uploadFileName = Encoding.ASCII.GetString(clientFile, 9, dataLen - 9);
                                    if (!File.Exists(@directory + "\\" + uploadFileName))
                                    {
                                        Byte[] buffer = new byte[9];
                                        BitConverter.GetBytes(6).CopyTo(buffer, 0);
                                        t.Item1.Send(buffer);
                                        continue;
                                    }
                                    //Status_Update.AppendText("gümrük girişi\n");
                                    // gümrük girişi

                                    string[] lines = File.ReadAllLines(DBdirectory);
                                    bool foundFlag = false;
                                    Byte[] failData = new Byte[9];
                                    BitConverter.GetBytes(6).CopyTo(failData, 0);

                                    for (int j = 0; j < lines.Length; j++)
                                    {
                                        try // throws exception for the null lines, just ignore them.
                                        {
                                            string[] tmp = lines[j].Split(' ');

                                            if (tmp[0] + tmp[2] == uploadFileName)
                                            {
                                                if (tmp[5] != "Private")
                                                {
                                                    foundFlag = true;
                                                }
                                                break;
                                            }
                                        }
                                        catch { continue; }
                                    }

                                    if (!foundFlag)
                                    {
                                        // send fail (not found) response
                                        t.Item1.Send(failData);
                                        continue;
                                    }
                                    //Status_Update.AppendText("gümrük çıkışı\n");

                                    // gümrük çıkışı

                                    string text = File.ReadAllText(@directory + "\\" + uploadFileName);
                                    Byte[] nameData = Encoding.Default.GetBytes(uploadFileName);
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
                                    BitConverter.GetBytes(6).CopyTo(clientData, 0);
                                    nameLength.CopyTo(clientData, 5);
                                    nameData.CopyTo(clientData, 9);

                                    try
                                    {
                                        // Send data
                                        
                                        t.Item1.Send(clientData);
                                        
                                    }
                                    catch
                                    {
                                        Status_Update.AppendText("Client has disconnected, soket battı\n");
                                        t.Item1.Close();
                                    }
                                    clientData[4] = (byte)1;
                                    for (int j = 0; j < packetCount - 2; j++)
                                    {
                                        fileData.Skip((j + 1) * 40000 * 1024).Take(40000 * 1024).ToArray().CopyTo(clientData, 9 + nameData.Length);
                                        try
                                        {
                                            // Send data

                                            t.Item1.Send(clientData);
                                            

                                        }
                                        catch
                                        {
                                            Status_Update.AppendText("Client has disconnected\n");
                                            
                                            t.Item1.Close();
                                        }
                                    }
                                    if (packetCount > 1)
                                    {
                                        Byte[] clientDataLast = new Byte[9 + nameData.Length + fileData.Length - (packetCount - 1) * (40000 * 1024)];
                                        fileData.Skip((packetCount - 1) * 40000 * 1024).Take(fileData.Length - (packetCount - 1) * 40000 * 1024).ToArray().CopyTo(clientDataLast, 9 + nameData.Length);
                                        BitConverter.GetBytes(6).CopyTo(clientDataLast, 0);
                                        clientDataLast[4] = (byte)1;
                                        nameLength.CopyTo(clientDataLast, 5);
                                        nameData.CopyTo(clientDataLast, 9);
                                        try
                                        {
                                            // Send data
                                            t.Item1.Send(clientDataLast);
                                            Status_Update.AppendText("Last\n");
                                        }
                                        catch
                                        {
                                            Status_Update.AppendText("Client has disconnected\n");
                                            t.Item1.Close();
                                        }
                                    }
                                    Status_Update.AppendText(Encoding.Default.GetString(nameData) + " is sent.\n");
                                }
                                catch(Exception e)
                                {
                                    Status_Update.AppendText(e.GetType().ToString());
                                    Status_Update.AppendText("Invalid download request received\n");
                                    t.Item1.Close();
                                }

                                continue;
                            } ///////////// PUBLIC UPLOAD PART END HERE ///////////////////
                        }
                        int size = BitConverter.ToInt32(clientFile, 5);
                        string fileName = Encoding.ASCII.GetString(clientFile, 9, fileNameLen);
                        string data = Encoding.ASCII.GetString(clientFile, 9 + fileNameLen, dataLen - 9 - fileNameLen);
                        fileName = fileName.Substring(0, fileName.IndexOf(".txt"));
                        string path = @directory + "\\" + t.Item2 + fileName + ".txt";
                        int i = 1;
                        while (File.Exists(path))
                        {
                            // Construct path
                            path = @directory + "\\" + t.Item2 + fileName + "-0" + i.ToString() + ".txt";
                            i++;
                        }
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            // Write file line
                            sw.Write(data);
                        }
                        string DBdata =t.Item2 +" File: "+ path.Substring(path.IndexOf(fileName))+" Size: "+size+"KB Private "+ "Time: "+DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt")+"\n";
                        using (StreamWriter sw = File.AppendText(DBdirectory))
                        {
                            // Write file line
                            sw.Write(DBdata);
                        }
                        files.Add(path);
                        // Status update
                        Status_Update.AppendText("From client " + t.Item2 + " file " + "\"" + fileName + ".txt\" received.\n");
                    }
                    else
                    {
                        int fileNameLen = BitConverter.ToInt32(clientFile, 1);
                        string fileName = Encoding.ASCII.GetString(clientFile, 9, fileNameLen);
                        string data = Encoding.ASCII.GetString(clientFile, 9 + fileNameLen, dataLen - 9 - fileNameLen);
                        fileName = fileName.Substring(0, fileName.IndexOf(".txt"));
                        string path = @directory + "\\" + t.Item2 + fileName;
                        path = files.FindLast(x=>x.Contains(path));
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            // Write file line
                            sw.Write(data);
                        }

                    }
                }
                catch
                {
                    // Error case: file transfer abortion
                    t.Item1.Close();
                    Status_Update.AppendText("Client "+t.Item2+" has left!\n");
                    clients.Remove(t);
                    connected = false;
                }

            }

        }

        // Receive name handler
        private bool ReceiveName(Socket thisClient,out string name) {
            bool connect = true;
            name = "";
            while (connect && !terminating) {
                try
                {
                    Byte[] buffer = new Byte[64];
                    // get incomming name
                    thisClient.Receive(buffer);
                    string incomingName = Encoding.Default.GetString(buffer);
                    incomingName = incomingName.Substring(0, incomingName.IndexOf("\0"));
                    foreach (Tuple<Socket,string> check in clients) {
                        // check conflicting names
                        if (incomingName.Equals(check.Item2))
                        {
                            return false;
                        }
                    }
                    name = incomingName.ToString();

                    Byte[] ack = Encoding.Default.GetBytes("1\0");
                    try
                    {
                        // send response to client
                        thisClient.Send(ack);
                    }
                    catch
                    {
                        // Error case: server exceptance
                        Status_Update.AppendText("A problem occured, check your connection\n");
                        terminating = true;

                        server.Close();
                        return false;
                    }
                    Status_Update.AppendText("Client: " + incomingName + " has joined the server\n");
                    return true;
                }
                catch
                {
                   // Error case: client disconnected
                    Status_Update.AppendText("Newcomer Client Left before recieving Name\n");
                    thisClient.Close();
                    connect = false;

                }
            }
            return false;
        }

        // Reject name handler
        private void RejectName(Socket thisClient) {
            string message = "0\0";
            Byte[] buffer = Encoding.Default.GetBytes(message);
            try
            {
                thisClient.Send(buffer);
            }
            catch {
                Status_Update.AppendText("A problem occured, check your connection\n");
                terminating = true;
                
                server.Close();
            }
        }

        // Directory button event handler
        private void Directory_Button_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // set directory for incoming files
                    string dir = fbd.SelectedPath;
                    directory = dir;
                    Path_Text.AppendText(dir);
                    Directory_Button.Enabled = false; // disable folder button
                    Listen_Button.Enabled = true; // enable listen button
                }
            }
        }

        private void SendList(Tuple<Socket,string> t)
        {
            string[] lines = File.ReadAllLines(DBdirectory);
            string fileData = "";
            for(int i = 0;i<lines.Length;i++)
            {
                string[] authorsList = lines[i].Split(' ');
                if(authorsList[0].Equals(t.Item2))
                {
                    authorsList[5] = "";
                    authorsList[0] = "";
                    fileData += string.Join(" ", authorsList)+"\n";
                }
            }
            Byte[] buffer = Encoding.Default.GetBytes(fileData);
            Byte[] buffer2 = new Byte[5000 * 1024];
            buffer.CopyTo(buffer2, 4);
            try
            {
                t.Item1.Send(buffer2);
                Status_Update.AppendText("File list of " + t.Item2 + " is sent.\n");
            }
            catch
            {
                Status_Update.AppendText("A problem occured, check your connection\n");
                terminating = true;

                server.Close();
            }
        }
        private void sendPublicList(Tuple<Socket,string> t)
        {
            string[] lines = File.ReadAllLines(DBdirectory);
            string fileData = "";
            for(int i  =0;i<lines.Length;i++)
            {
                try
                {
                    string[] authorsList = lines[i].Split(' ');
                    if (authorsList[5].Equals("Public"))
                    {
                        authorsList[5] = "";
                        authorsList[0] = "From: " + authorsList[0];
                        fileData += string.Join(" ", authorsList) + "\n";
                    }
                }
                catch { continue; }
            }
            Byte[] buffer = Encoding.Default.GetBytes(fileData);
            Byte[] buffer2 = new Byte[5000 * 1024];
            BitConverter.GetBytes(5).CopyTo(buffer2, 0);
            buffer.CopyTo(buffer2, 4);
            try
            {
                t.Item1.Send(buffer2);
                Status_Update.AppendText("Public list is sent to " + t.Item2 + ".\n");
            }
            catch
            {
                Status_Update.AppendText("A problem occured, check your connection\n");
                terminating = true;

                server.Close();
            }
        }
    }
}
