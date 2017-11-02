namespace SeaBattle
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
            this.label1 = new System.Windows.Forms.Label();
            this.AdapterName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.IpBox = new System.Windows.Forms.TextBox();
            this.PortBox = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ConncectBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PortBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChangeAdapterBtn
            // 
            this.ChangeAdapterBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ChangeAdapterBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChangeAdapterBtn.Location = new System.Drawing.Point(271, 56);
            this.ChangeAdapterBtn.Name = "ChangeAdapterBtn";
            this.ChangeAdapterBtn.Size = new System.Drawing.Size(148, 26);
            this.ChangeAdapterBtn.TabIndex = 2;
            this.ChangeAdapterBtn.Text = "Сменить адаптер";
            this.ChangeAdapterBtn.UseVisualStyleBackColor = false;
            this.ChangeAdapterBtn.Click += new System.EventHandler(this.ChangeAdapterBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Текущий адаптер:";
            // 
            // AdapterName
            // 
            this.AdapterName.AutoSize = true;
            this.AdapterName.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AdapterName.Location = new System.Drawing.Point(145, 16);
            this.AdapterName.Name = "AdapterName";
            this.AdapterName.Size = new System.Drawing.Size(12, 17);
            this.AdapterName.TabIndex = 4;
            this.AdapterName.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(19, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ip";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(125, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IpBox
            // 
            this.IpBox.Location = new System.Drawing.Point(22, 164);
            this.IpBox.Name = "IpBox";
            this.IpBox.Size = new System.Drawing.Size(100, 20);
            this.IpBox.TabIndex = 7;
            // 
            // PortBox
            // 
            this.PortBox.Location = new System.Drawing.Point(128, 163);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ChangeAdapterBtn);
            this.groupBox1.Controls.Add(this.AdapterName);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 107);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // ConncectBtn
            // 
            this.ConncectBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ConncectBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConncectBtn.Location = new System.Drawing.Point(284, 157);
            this.ConncectBtn.Name = "ConncectBtn";
            this.ConncectBtn.Size = new System.Drawing.Size(148, 26);
            this.ConncectBtn.TabIndex = 10;
            this.ConncectBtn.Text = "Подключиться";
            this.ConncectBtn.UseVisualStyleBackColor = false;
            // 
            // ConnectToTheGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 216);
            this.Controls.Add(this.ConncectBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.IpBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "ConnectToTheGameForm";
            this.Text = "Подключиться к игре";
            ((System.ComponentModel.ISupportInitialize)(this.PortBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ChangeAdapterBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label AdapterName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox IpBox;
        private System.Windows.Forms.NumericUpDown PortBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ConncectBtn;
    }
}