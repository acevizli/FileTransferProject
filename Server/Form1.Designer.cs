namespace CS408_Project_Server
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
            this.Port_Label = new System.Windows.Forms.Label();
            this.Port_TextBox = new System.Windows.Forms.TextBox();
            this.Listen_Button = new System.Windows.Forms.Button();
            this.Status_Update = new System.Windows.Forms.RichTextBox();
            this.Directory_Button = new System.Windows.Forms.Button();
            this.Path_Text = new System.Windows.Forms.RichTextBox();
            this.Path_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Port_Label
            // 
            this.Port_Label.AutoSize = true;
            this.Port_Label.Location = new System.Drawing.Point(43, 70);
            this.Port_Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Port_Label.Name = "Port_Label";
            this.Port_Label.Size = new System.Drawing.Size(47, 17);
            this.Port_Label.TabIndex = 0;
            this.Port_Label.Text = "PORT";
            // 
            // Port_TextBox
            // 
            this.Port_TextBox.Location = new System.Drawing.Point(106, 67);
            this.Port_TextBox.Margin = new System.Windows.Forms.Padding(2);
            this.Port_TextBox.Name = "Port_TextBox";
            this.Port_TextBox.Size = new System.Drawing.Size(106, 22);
            this.Port_TextBox.TabIndex = 1;
            // 
            // Listen_Button
            // 
            this.Listen_Button.Enabled = false;
            this.Listen_Button.Location = new System.Drawing.Point(243, 63);
            this.Listen_Button.Margin = new System.Windows.Forms.Padding(2);
            this.Listen_Button.Name = "Listen_Button";
            this.Listen_Button.Size = new System.Drawing.Size(80, 25);
            this.Listen_Button.TabIndex = 2;
            this.Listen_Button.Text = "LISTEN";
            this.Listen_Button.UseVisualStyleBackColor = true;
            this.Listen_Button.Click += new System.EventHandler(this.Listen_Button_Click);
            // 
            // Status_Update
            // 
            this.Status_Update.Location = new System.Drawing.Point(46, 130);
            this.Status_Update.Margin = new System.Windows.Forms.Padding(2);
            this.Status_Update.Name = "Status_Update";
            this.Status_Update.Size = new System.Drawing.Size(588, 223);
            this.Status_Update.TabIndex = 5;
            this.Status_Update.Text = "";
            // 
            // Directory_Button
            // 
            this.Directory_Button.Location = new System.Drawing.Point(46, 379);
            this.Directory_Button.Margin = new System.Windows.Forms.Padding(2);
            this.Directory_Button.Name = "Directory_Button";
            this.Directory_Button.Size = new System.Drawing.Size(140, 25);
            this.Directory_Button.TabIndex = 6;
            this.Directory_Button.Text = "SET DIRECTORY";
            this.Directory_Button.UseVisualStyleBackColor = true;
            this.Directory_Button.Click += new System.EventHandler(this.Directory_Button_Click);
            // 
            // Path_Text
            // 
            this.Path_Text.Location = new System.Drawing.Point(323, 379);
            this.Path_Text.Margin = new System.Windows.Forms.Padding(2);
            this.Path_Text.Name = "Path_Text";
            this.Path_Text.Size = new System.Drawing.Size(311, 26);
            this.Path_Text.TabIndex = 7;
            this.Path_Text.Text = "";
            // 
            // Path_Label
            // 
            this.Path_Label.AutoSize = true;
            this.Path_Label.Location = new System.Drawing.Point(234, 383);
            this.Path_Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Path_Label.Name = "Path_Label";
            this.Path_Label.Size = new System.Drawing.Size(53, 17);
            this.Path_Label.TabIndex = 8;
            this.Path_Label.Text = "PATH: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 445);
            this.Controls.Add(this.Path_Label);
            this.Controls.Add(this.Path_Text);
            this.Controls.Add(this.Directory_Button);
            this.Controls.Add(this.Status_Update);
            this.Controls.Add(this.Listen_Button);
            this.Controls.Add(this.Port_TextBox);
            this.Controls.Add(this.Port_Label);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Port_Label;
        private System.Windows.Forms.TextBox Port_TextBox;
        private System.Windows.Forms.RichTextBox Status_Update;
        private System.Windows.Forms.Button Directory_Button;
        private System.Windows.Forms.RichTextBox Path_Text;
        private System.Windows.Forms.Label Path_Label;
        private System.Windows.Forms.Button Listen_Button;
    }
}

