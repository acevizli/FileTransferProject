
namespace cs408Project_Client
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.ip = new System.Windows.Forms.TextBox();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.connect = new System.Windows.Forms.Button();
            this.upload = new System.Windows.Forms.Button();
            this.downloadTextBox = new System.Windows.Forms.TextBox();
            this.copyTextBox = new System.Windows.Forms.TextBox();
            this.deleteTextBox = new System.Windows.Forms.TextBox();
            this.Download = new System.Windows.Forms.Button();
            this.Copy = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.List = new System.Windows.Forms.Button();
            this.ListPublicFiles = new System.Windows.Forms.Button();
            this.publicDownloadUsername = new System.Windows.Forms.TextBox();
            this.PublicDownload = new System.Windows.Forms.Button();
            this.Publish = new System.Windows.Forms.Button();
            this.publishText = new System.Windows.Forms.TextBox();
            this.publicDownloadFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "IP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(422, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Logs";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(41, 68);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(348, 22);
            this.name.TabIndex = 4;
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(231, 127);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(158, 22);
            this.port.TabIndex = 5;
            // 
            // ip
            // 
            this.ip.Location = new System.Drawing.Point(41, 127);
            this.ip.Name = "ip";
            this.ip.Size = new System.Drawing.Size(158, 22);
            this.ip.TabIndex = 6;
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(425, 64);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(348, 222);
            this.logs.TabIndex = 7;
            this.logs.Text = "";
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(42, 187);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(157, 43);
            this.connect.TabIndex = 8;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // upload
            // 
            this.upload.Enabled = false;
            this.upload.Location = new System.Drawing.Point(231, 183);
            this.upload.Name = "upload";
            this.upload.Size = new System.Drawing.Size(158, 47);
            this.upload.TabIndex = 9;
            this.upload.Text = "Upload a file";
            this.upload.UseVisualStyleBackColor = true;
            this.upload.Click += new System.EventHandler(this.upload_Click);
            // 
            // downloadTextBox
            // 
            this.downloadTextBox.Location = new System.Drawing.Point(194, 400);
            this.downloadTextBox.Name = "downloadTextBox";
            this.downloadTextBox.Size = new System.Drawing.Size(195, 22);
            this.downloadTextBox.TabIndex = 10;
            // 
            // copyTextBox
            // 
            this.copyTextBox.Location = new System.Drawing.Point(578, 329);
            this.copyTextBox.Name = "copyTextBox";
            this.copyTextBox.Size = new System.Drawing.Size(195, 22);
            this.copyTextBox.TabIndex = 14;
            // 
            // deleteTextBox
            // 
            this.deleteTextBox.Location = new System.Drawing.Point(580, 400);
            this.deleteTextBox.Name = "deleteTextBox";
            this.deleteTextBox.Size = new System.Drawing.Size(193, 22);
            this.deleteTextBox.TabIndex = 15;
            // 
            // Download
            // 
            this.Download.Enabled = false;
            this.Download.Location = new System.Drawing.Point(42, 390);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(120, 43);
            this.Download.TabIndex = 17;
            this.Download.Text = "Download";
            this.Download.UseVisualStyleBackColor = true;
            this.Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // Copy
            // 
            this.Copy.Enabled = false;
            this.Copy.Location = new System.Drawing.Point(424, 319);
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(120, 43);
            this.Copy.TabIndex = 18;
            this.Copy.Text = "Copy";
            this.Copy.UseVisualStyleBackColor = true;
            this.Copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // Delete
            // 
            this.Delete.Enabled = false;
            this.Delete.Location = new System.Drawing.Point(425, 390);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(119, 43);
            this.Delete.TabIndex = 19;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // List
            // 
            this.List.Enabled = false;
            this.List.Location = new System.Drawing.Point(231, 243);
            this.List.Name = "List";
            this.List.Size = new System.Drawing.Size(158, 43);
            this.List.TabIndex = 20;
            this.List.Text = "List";
            this.List.UseVisualStyleBackColor = true;
            this.List.Click += new System.EventHandler(this.List_Click);
            // 
            // ListPublicFiles
            // 
            this.ListPublicFiles.Enabled = false;
            this.ListPublicFiles.Location = new System.Drawing.Point(41, 243);
            this.ListPublicFiles.Name = "ListPublicFiles";
            this.ListPublicFiles.Size = new System.Drawing.Size(158, 43);
            this.ListPublicFiles.TabIndex = 21;
            this.ListPublicFiles.Text = "List Public Files";
            this.ListPublicFiles.UseVisualStyleBackColor = true;
            this.ListPublicFiles.Click += new System.EventHandler(this.ListPublicFiles_Click);
            // 
            // publicDownloadUsername
            // 
            this.publicDownloadUsername.Location = new System.Drawing.Point(264, 472);
            this.publicDownloadUsername.Name = "publicDownloadUsername";
            this.publicDownloadUsername.Size = new System.Drawing.Size(241, 22);
            this.publicDownloadUsername.TabIndex = 22;
            // 
            // PublicDownload
            // 
            this.PublicDownload.Enabled = false;
            this.PublicDownload.Location = new System.Drawing.Point(42, 462);
            this.PublicDownload.Name = "PublicDownload";
            this.PublicDownload.Size = new System.Drawing.Size(185, 43);
            this.PublicDownload.TabIndex = 23;
            this.PublicDownload.Text = "Public Download";
            this.PublicDownload.UseVisualStyleBackColor = true;
            this.PublicDownload.Click += new System.EventHandler(this.PublicDownload_Click);
            // 
            // Publish
            // 
            this.Publish.Enabled = false;
            this.Publish.Location = new System.Drawing.Point(41, 319);
            this.Publish.Name = "Publish";
            this.Publish.Size = new System.Drawing.Size(120, 43);
            this.Publish.TabIndex = 24;
            this.Publish.Text = "Publish";
            this.Publish.UseVisualStyleBackColor = true;
            this.Publish.Click += new System.EventHandler(this.Publish_Click);
            // 
            // publishText
            // 
            this.publishText.Location = new System.Drawing.Point(194, 329);
            this.publishText.Name = "publishText";
            this.publishText.Size = new System.Drawing.Size(195, 22);
            this.publishText.TabIndex = 25;
            // 
            // publicDownloadFile
            // 
            this.publicDownloadFile.AccessibleDescription = "";
            this.publicDownloadFile.AccessibleName = "";
            this.publicDownloadFile.Location = new System.Drawing.Point(532, 472);
            this.publicDownloadFile.Name = "publicDownloadFile";
            this.publicDownloadFile.Size = new System.Drawing.Size(241, 22);
            this.publicDownloadFile.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(261, 452);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 17);
            this.label5.TabIndex = 27;
            this.label5.Text = "Username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(529, 452);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 17);
            this.label6.TabIndex = 27;
            this.label6.Text = "File Name";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 559);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.publicDownloadFile);
            this.Controls.Add(this.publishText);
            this.Controls.Add(this.Publish);
            this.Controls.Add(this.PublicDownload);
            this.Controls.Add(this.publicDownloadUsername);
            this.Controls.Add(this.ListPublicFiles);
            this.Controls.Add(this.List);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Copy);
            this.Controls.Add(this.Download);
            this.Controls.Add(this.deleteTextBox);
            this.Controls.Add(this.copyTextBox);
            this.Controls.Add(this.downloadTextBox);
            this.Controls.Add(this.upload);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.ip);
            this.Controls.Add(this.port);
            this.Controls.Add(this.name);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.TextBox ip;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button upload;
        private System.Windows.Forms.TextBox copyTextBox;
        private System.Windows.Forms.TextBox deleteTextBox;
        private System.Windows.Forms.Button Download;
        private System.Windows.Forms.Button Copy;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button List;
        private System.Windows.Forms.TextBox downloadTextBox;
        private System.Windows.Forms.Button ListPublicFiles;
        private System.Windows.Forms.TextBox publicDownloadUsername;
        private System.Windows.Forms.Button PublicDownload;
        private System.Windows.Forms.Button Publish;
        private System.Windows.Forms.TextBox publishText;
        private System.Windows.Forms.TextBox publicDownloadFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

