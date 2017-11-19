namespace SeaBattle
{
    partial class AdapterChoosingForm
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
            this.AdapterBox = new System.Windows.Forms.ListBox();
            this.IpLab = new System.Windows.Forms.Label();
            this.MacLab = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CurAdapterLab = new System.Windows.Forms.Label();
            this.SetBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AdapterBox
            // 
            this.AdapterBox.FormattingEnabled = true;
            this.AdapterBox.Location = new System.Drawing.Point(31, 72);
            this.AdapterBox.Name = "AdapterBox";
            this.AdapterBox.Size = new System.Drawing.Size(573, 212);
            this.AdapterBox.TabIndex = 0;
            this.AdapterBox.SelectedIndexChanged += new System.EventHandler(this.AdapterBox_SelectedIndexChanged);
            // 
            // IpLab
            // 
            this.IpLab.AutoSize = true;
            this.IpLab.Location = new System.Drawing.Point(50, 306);
            this.IpLab.Name = "IpLab";
            this.IpLab.Size = new System.Drawing.Size(0, 13);
            this.IpLab.TabIndex = 1;
            // 
            // MacLab
            // 
            this.MacLab.AutoSize = true;
            this.MacLab.Location = new System.Drawing.Point(198, 306);
            this.MacLab.Name = "MacLab";
            this.MacLab.Size = new System.Drawing.Size(0, 13);
            this.MacLab.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Текущий адаптер";
            // 
            // CurAdapterLab
            // 
            this.CurAdapterLab.AutoSize = true;
            this.CurAdapterLab.Location = new System.Drawing.Point(141, 28);
            this.CurAdapterLab.Name = "CurAdapterLab";
            this.CurAdapterLab.Size = new System.Drawing.Size(0, 13);
            this.CurAdapterLab.TabIndex = 4;
            // 
            // SetBtn
            // 
            this.SetBtn.Location = new System.Drawing.Point(439, 301);
            this.SetBtn.Name = "SetBtn";
            this.SetBtn.Size = new System.Drawing.Size(75, 23);
            this.SetBtn.TabIndex = 5;
            this.SetBtn.Text = "Применить";
            this.SetBtn.UseVisualStyleBackColor = true;
            this.SetBtn.Click += new System.EventHandler(this.SetBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(529, 301);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Отменить";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // AdapterChoosingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 347);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SetBtn);
            this.Controls.Add(this.CurAdapterLab);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MacLab);
            this.Controls.Add(this.IpLab);
            this.Controls.Add(this.AdapterBox);
            this.Name = "AdapterChoosingForm";
            this.Text = "Выберите адаптер";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox AdapterBox;
        private System.Windows.Forms.Label IpLab;
        private System.Windows.Forms.Label MacLab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CurAdapterLab;
        private System.Windows.Forms.Button SetBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}