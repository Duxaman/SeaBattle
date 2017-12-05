namespace SeaBattle
{
    partial class WaitForPlayerForm
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
            this.ConfirmBtn = new System.Windows.Forms.Button();
            this.ChooseAdapter = new System.Windows.Forms.Button();
            this.PlayerList = new System.Windows.Forms.ListBox();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.IpEndPointBox = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(78, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ожидаем соперника:";
            // 
            // ConfirmBtn
            // 
            this.ConfirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ConfirmBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfirmBtn.Location = new System.Drawing.Point(224, 236);
            this.ConfirmBtn.Name = "ConfirmBtn";
            this.ConfirmBtn.Size = new System.Drawing.Size(76, 26);
            this.ConfirmBtn.TabIndex = 12;
            this.ConfirmBtn.Text = "Играть";
            this.ConfirmBtn.UseVisualStyleBackColor = false;
            this.ConfirmBtn.Click += new System.EventHandler(this.ConfirmBtn_Click);
            // 
            // ChooseAdapter
            // 
            this.ChooseAdapter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ChooseAdapter.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseAdapter.Location = new System.Drawing.Point(12, 236);
            this.ChooseAdapter.Name = "ChooseAdapter";
            this.ChooseAdapter.Size = new System.Drawing.Size(96, 26);
            this.ChooseAdapter.TabIndex = 13;
            this.ChooseAdapter.Text = "Настройки";
            this.ChooseAdapter.UseVisualStyleBackColor = false;
            this.ChooseAdapter.Click += new System.EventHandler(this.ChooseAdapter_Click);
            // 
            // PlayerList
            // 
            this.PlayerList.FormattingEnabled = true;
            this.PlayerList.Location = new System.Drawing.Point(12, 121);
            this.PlayerList.Name = "PlayerList";
            this.PlayerList.Size = new System.Drawing.Size(288, 95);
            this.PlayerList.TabIndex = 14;
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RefreshBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshBtn.Location = new System.Drawing.Point(114, 236);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(104, 26);
            this.RefreshBtn.TabIndex = 15;
            this.RefreshBtn.Text = "Обновить";
            this.RefreshBtn.UseVisualStyleBackColor = false;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(111, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "Ваш адрес:";
            // 
            // IpEndPointBox
            // 
            this.IpEndPointBox.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IpEndPointBox.Location = new System.Drawing.Point(12, 26);
            this.IpEndPointBox.Name = "IpEndPointBox";
            this.IpEndPointBox.Size = new System.Drawing.Size(288, 37);
            this.IpEndPointBox.TabIndex = 17;
            this.IpEndPointBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WaitForPlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 271);
            this.Controls.Add(this.IpEndPointBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.PlayerList);
            this.Controls.Add(this.ChooseAdapter);
            this.Controls.Add(this.ConfirmBtn);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WaitForPlayerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ожидание соперника";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WaitForPlayerForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ConfirmBtn;
        private System.Windows.Forms.Button ChooseAdapter;
        private System.Windows.Forms.ListBox PlayerList;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label IpEndPointBox;
    }
}