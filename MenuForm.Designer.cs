namespace SeaBattle
{
    partial class MenuForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.QuitBtn = new System.Windows.Forms.Button();
            this.CreateGameBtn = new System.Windows.Forms.Button();
            this.ConnectToGameBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.QuitBtn);
            this.groupBox1.Controls.Add(this.CreateGameBtn);
            this.groupBox1.Controls.Add(this.ConnectToGameBtn);
            this.groupBox1.Location = new System.Drawing.Point(183, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // QuitBtn
            // 
            this.QuitBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.QuitBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuitBtn.Location = new System.Drawing.Point(0, 93);
            this.QuitBtn.Name = "QuitBtn";
            this.QuitBtn.Size = new System.Drawing.Size(200, 51);
            this.QuitBtn.TabIndex = 2;
            this.QuitBtn.Text = "Выход";
            this.QuitBtn.UseVisualStyleBackColor = false;
            this.QuitBtn.Click += new System.EventHandler(this.QuitBtn_Click);
            // 
            // CreateGameBtn
            // 
            this.CreateGameBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CreateGameBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateGameBtn.Location = new System.Drawing.Point(0, 0);
            this.CreateGameBtn.Name = "CreateGameBtn";
            this.CreateGameBtn.Size = new System.Drawing.Size(200, 51);
            this.CreateGameBtn.TabIndex = 1;
            this.CreateGameBtn.Text = "Создать игру";
            this.CreateGameBtn.UseVisualStyleBackColor = false;
            this.CreateGameBtn.Click += new System.EventHandler(this.CreateGameBtn_Click);
            // 
            // ConnectToGameBtn
            // 
            this.ConnectToGameBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ConnectToGameBtn.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectToGameBtn.Location = new System.Drawing.Point(0, 47);
            this.ConnectToGameBtn.Name = "ConnectToGameBtn";
            this.ConnectToGameBtn.Size = new System.Drawing.Size(200, 51);
            this.ConnectToGameBtn.TabIndex = 0;
            this.ConnectToGameBtn.Text = "Подключиться к сопернику";
            this.ConnectToGameBtn.UseVisualStyleBackColor = false;
            this.ConnectToGameBtn.Click += new System.EventHandler(this.ConnectToGameBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Font = new System.Drawing.Font("Kunstler Script", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(142, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "Морской Бой";
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SeaBattle.Properties.Resources.sea_battle_by_lobzov_d3dznfw;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(561, 565);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Морской бой";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CreateGameBtn;
        private System.Windows.Forms.Button ConnectToGameBtn;
        private System.Windows.Forms.Button QuitBtn;
    }
}

