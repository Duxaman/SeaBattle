﻿namespace SeaBattle
{
    partial class ConnectToTheGameForm
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
            this.ChangeAdapterBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.IpBox = new System.Windows.Forms.TextBox();
            this.PortBox = new System.Windows.Forms.NumericUpDown();
            this.ConncectBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.IpEndPointBox = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PortBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ChangeAdapterBtn
            // 
            this.ChangeAdapterBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ChangeAdapterBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChangeAdapterBtn.Location = new System.Drawing.Point(50, 152);
            this.ChangeAdapterBtn.Name = "ChangeAdapterBtn";
            this.ChangeAdapterBtn.Size = new System.Drawing.Size(148, 26);
            this.ChangeAdapterBtn.TabIndex = 2;
            this.ChangeAdapterBtn.Text = "Настройки";
            this.ChangeAdapterBtn.UseVisualStyleBackColor = false;
            this.ChangeAdapterBtn.Click += new System.EventHandler(this.ChangeAdapterBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(47, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ip";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(153, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IpBox
            // 
            this.IpBox.Location = new System.Drawing.Point(50, 117);
            this.IpBox.Name = "IpBox";
            this.IpBox.Size = new System.Drawing.Size(100, 20);
            this.IpBox.TabIndex = 7;
            // 
            // PortBox
            // 
            this.PortBox.Location = new System.Drawing.Point(156, 116);
            this.PortBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.PortBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(52, 20);
            this.PortBox.TabIndex = 8;
            this.PortBox.Value = new decimal(new int[] {
            27000,
            0,
            0,
            0});
            // 
            // ConncectBtn
            // 
            this.ConncectBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ConncectBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConncectBtn.Location = new System.Drawing.Point(50, 184);
            this.ConncectBtn.Name = "ConncectBtn";
            this.ConncectBtn.Size = new System.Drawing.Size(148, 26);
            this.ConncectBtn.TabIndex = 10;
            this.ConncectBtn.Text = "Подключиться";
            this.ConncectBtn.UseVisualStyleBackColor = false;
            this.ConncectBtn.Click += new System.EventHandler(this.ConncectBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(78, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "Ваш Адрес:";
            // 
            // IpEndPointBox
            // 
            this.IpEndPointBox.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IpEndPointBox.Location = new System.Drawing.Point(3, 17);
            this.IpEndPointBox.Name = "IpEndPointBox";
            this.IpEndPointBox.Size = new System.Drawing.Size(250, 32);
            this.IpEndPointBox.TabIndex = 12;
            this.IpEndPointBox.Text = " ";
            this.IpEndPointBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusLabel.Location = new System.Drawing.Point(0, 59);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(253, 24);
            this.StatusLabel.TabIndex = 13;
            this.StatusLabel.Text = "Не подключен";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConnectToTheGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(255, 220);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.IpEndPointBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConncectBtn);
            this.Controls.Add(this.ChangeAdapterBtn);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.IpBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectToTheGameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Подключиться к игре";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectToTheGameForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PortBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ChangeAdapterBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox IpBox;
        private System.Windows.Forms.NumericUpDown PortBox;
        private System.Windows.Forms.Button ConncectBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label IpEndPointBox;
        private System.Windows.Forms.Label StatusLabel;
    }
}