using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class MenuForm : Form
    {
        private FieldForm GameForm;
        private bool gamemode;
        public MenuForm()
        {
            InitializeComponent();
        }

        private void CreateGameBtn_Click(object sender, EventArgs e)
        {
            ShipsMappingForm StartForm = new ShipsMappingForm(ProcessField);
            gamemode = true;
            //StartForm.Owner = this;
            //StartForm.Show();
            this.Hide();
            ProcessField(new Field(10));          

        }

        private void ProcessField(Field NewField)
        {
            if(gamemode)
            {
                WaitForPlayerForm LookForParnter = new WaitForPlayerForm();
                LookForParnter.Owner = this;
                if(LookForParnter.ShowDialog() == DialogResult.OK)
                {
                    GameForm = new FieldForm(new Game(NewField, true));
                    GameForm.Owner = this;
                    GameForm.Show();
                    return;
                }

            }
            else
            {
                ConnectToTheGameForm ConnectForm = new ConnectToTheGameForm();
                ConnectForm.Owner = this;
                if (ConnectForm.ShowDialog() == DialogResult.OK)
                {
                    GameForm = new FieldForm(new Game(NewField, false));
                    GameForm.Owner = this;
                    GameForm.Show();
                    return;
                }
            }
            this.Show();

        }

        private void ConnectToGameBtn_Click(object sender, EventArgs e)
        {
            ShipsMappingForm StartForm = new ShipsMappingForm(ProcessField);
            gamemode = false;
            this.Hide();
            ProcessField(new Field(10));
            //StartForm.Owner = this;
            //StartForm.Show();
        }

        private void QuitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
