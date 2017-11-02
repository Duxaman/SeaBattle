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
            this.InterfaceIp = new System.Windows.Forms.Label();
            this.ConfirmBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AdapterBox
            // 
            this.AdapterBox.FormattingEnabled = true;
            this.AdapterBox.Location = new System.Drawing.Point(12, 12);
            this.AdapterBox.Name = "AdapterBox";
            this.AdapterBox.Size = new System.Drawing.Size(552, 212);
            this.AdapterBox.TabIndex = 0;
            // 
            // InterfaceIp
            // 
            this.InterfaceIp.AutoSize = true;
            this.InterfaceIp.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InterfaceIp.Location = new System.Drawing.Point(12, 246);
            this.InterfaceIp.Name = "InterfaceIp";
            this.InterfaceIp.Size = new System.Drawing.Size(12, 17);
            this.InterfaceIp.TabIndex = 7;
            this.InterfaceIp.Text = ".";
            this.InterfaceIp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConfirmBtn
            // 
            this.ConfirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ConfirmBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfirmBtn.Location = new System.Drawing.Point(416, 241);
            this.ConfirmBtn.Name = "ConfirmBtn";
            this.ConfirmBtn.Size = new System.Drawing.Size(148, 26);
            this.ConfirmBtn.TabIndex = 11;
            this.ConfirmBtn.Text = "Выбрать";
            this.ConfirmBtn.UseVisualStyleBackColor = false;
            // 
            // AdapterChoosingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 299);
            this.Controls.Add(this.ConfirmBtn);
            this.Controls.Add(this.InterfaceIp);
            this.Controls.Add(this.AdapterBox);
            this.Name = "AdapterChoosingForm";
            this.Text = "Выбрать адаптер";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox AdapterBox;
        private System.Windows.Forms.Label InterfaceIp;
        private System.Windows.Forms.Button ConfirmBtn;
    }
}