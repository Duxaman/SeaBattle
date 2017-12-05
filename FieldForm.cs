using System;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class FieldForm : Form
    {
        private Point PlayerSelect;
        private Game GameObject;
        private FieldViewer MyField;
        private FieldViewer EnemyField;
        private delegate void SafeFormClose(FieldForm form);
        private delegate void SafeLabelSet(Label label, string text);
        private delegate void SafeControlEnable(Control control, bool enabled);
        private delegate void SafeCellUpdate(FieldViewer Field, CellState State, Point pos);
        public FieldForm(Game GameObject)
        {
            InitializeComponent();
            this.GameObject = GameObject;
            MyField = new FieldViewer(10, new System.Drawing.Point(1, 1), 50);
            EnemyField = new FieldViewer(10, new System.Drawing.Point(756, 1), 50);
            EnemyField.SelectionEnabled = true;
            this.Controls.Add(MyField);
            for (int i = 0; i < GameObject.MyField.FieldSize; ++i)
                for (int j = 0; j < GameObject.MyField.FieldSize; ++j)
                {
                    MyField.updateCellState(GameObject.MyField.getCell(i, j).State, new Point(i, j));
                }
            this.Controls.Add(EnemyField);
            EnemyField.CellClick += new DataGridViewCellEventHandler(EnemyCellClick);
            Program.ConnectionManager.OnConnectionLost += new EventHandler<EventArgs>(OnConnectionLostHandler);
            Program.ConnectionManager.OnMessageReceive += new EventHandler<GameData>(GameObject.ReceiveMsg);
            GameObject.SendMessage += new EventHandler<GameData>(Program.ConnectionManager.SendMsg);
            GameObject.OnDataUpdate += new EventHandler<DataUpdateEventArguments>(UpdateData);
            GameObject.OnGameFinished += new EventHandler<bool>(OnGameFinished);
            GameObject.ChangeMove += new EventHandler(ChangeMove);
            updateEnemyLabels();
            updatePlayerLabels();
            updateMoveLabel();
            PlayerSelect = new Point(-1, -1);
            MyField.Enabled = false;
        }
        ///////////////////////CallBackHandlers-----------------------------------------------------------------------------------------------------------------------------
        private void OnConnectionLostHandler(object sender, EventArgs e)
        {
            /*
              after restoring connection server takes the move
             */
            if (MessageBox.Show("Соединение с партнером было потеряно. Хотите подключиться заново? ", "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                bool rep = true;
                bool Mode = Program.ConnectionManager.Mode; //mode will be reset to default if user close connection form without success
                while (rep)
                {
                    if (Mode)
                    {
                        WaitForPlayerForm WaitForm = new WaitForPlayerForm();
                        if (WaitForm.ShowDialog() == DialogResult.Cancel)
                        {
                            if (MessageBox.Show("Вы уверены что хотите завершить текущую игру?", "Потдверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                rep = false;
                                SafeClose(this);
                            }
                        }
                        else
                        {
                            rep = false;
                            GameObject.CurrentMove = true;
                            updateMoveLabel();
                            SafeEnable(EnemyField, true);

                        }
                    }
                    else
                    {
                        ConnectToTheGameForm ConnectForm = new ConnectToTheGameForm();
                        if (ConnectForm.ShowDialog() == DialogResult.Cancel)
                        {
                            if (MessageBox.Show("Вы уверены что хотите завершить текущую игру?", "Потдверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                rep = false;
                                SafeClose(this);
                            }
                        }
                        else
                        {
                            rep = false;
                            GameObject.CurrentMove = false;
                            updateMoveLabel();
                            SafeEnable(EnemyField, false);
                        }
                    }
                }
            }
            else
            {
                SafeClose(this);
            }

        }
        private void UpdateData(object sender, DataUpdateEventArguments e)
        {
            if (!e.LocalField)
            {
                SafeCellStateUpdate(EnemyField, GameObject.EnemyField.getCell(e.Cell.X, e.Cell.Y).State, e.Cell);
                if (e.labelUpdateRequired)
                {
                    updateEnemyLabels();
                }
            }
            else
            {
                SafeCellStateUpdate(MyField, GameObject.MyField.getCell(e.Cell.X, e.Cell.Y).State, e.Cell);
                if (e.labelUpdateRequired)
                {
                    updatePlayerLabels();
                }
            }
        }
        private void OnGameFinished(object sender, bool localPlayerWon)
        {
            Program.ConnectionManager.CloseAllConnections();
            MessageBox.Show(localPlayerWon ? "Победа!" : "Поражение!", "Результаты игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SafeClose(this);
        }

        ///////////////////////InterfaceUpdaters-----------------------------------------------------------------------------------------------------------------------------
        private void updateEnemyLabels()
        {
            SafeSet(E1Left, GameObject.EnemyCounters[3].ToString());
            SafeSet(E2Left, GameObject.EnemyCounters[2].ToString());
            SafeSet(E3Left, GameObject.EnemyCounters[1].ToString());
            SafeSet(E4Left, GameObject.EnemyCounters[0].ToString());
        }
        private void updatePlayerLabels()
        {
            SafeSet(P1Left, GameObject.MyCounters[3].ToString());
            SafeSet(P2Left, GameObject.MyCounters[2].ToString());
            SafeSet(P3Left, GameObject.MyCounters[1].ToString());
            SafeSet(P4Left, GameObject.MyCounters[0].ToString());
        }
        private void updateMoveLabel()
        {
            SafeSet(WhoIsMoveLabel, GameObject.CurrentMove ? "Ваш" : "Противника");
        }
        private void ChangeMove(object sender, EventArgs e)
        {
            if (GameObject.CurrentMove)
            {
                SafeEnable(EnemyField, true);
                SafeEnable(FireBtn, true);
            }
            else
            {
                SafeEnable(EnemyField, false);
            }
            updateMoveLabel();

        }
        private void lockinterface()
        {
            //wiil be called when move is cheking on the enemy side
            //it freezes the interface until game check all vars
            EnemyField.Enabled = false;
            FireBtn.Enabled = false;
        }
        private void FieldForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.ConnectionManager.CloseAllConnections();
            Application.DoEvents();
            Owner.Show();
        }
        ///////////////////////ButtonHandlers-----------------------------------------------------------------------------------------------------------------------------
        private void FireBtn_Click(object sender, EventArgs e)
        {
            if (GameObject.CurrentMove)
            {
                if (PlayerSelect.X != -1 && PlayerSelect.Y != -1)
                {
                    //if its not fired yet
                    if (GameObject.EnemyField.getCell(PlayerSelect.X, PlayerSelect.Y).State == CellState.Free)
                    {
                        WhoIsMoveLabel.Text = "Отправка..";
                        lockinterface();
                        GameObject.MakeMove(PlayerSelect);

                    }
                    else
                    {
                        MessageBox.Show("Недопустимый выбор");
                    }
                }
                else
                {
                    MessageBox.Show("Сначала выберите клетку на поле врага!");
                }
            }
        }
        private void EnemyCellClick(object sender, DataGridViewCellEventArgs e)
        {
            PlayerSelect.X = e.ColumnIndex;
            PlayerSelect.Y = e.RowIndex;
        }
        private void ResetChoiceBtn_Click(object sender, EventArgs e)
        {
            PlayerSelect.X = -1;
            PlayerSelect.Y = -1;
        }

        private void FieldForm_Load(object sender, EventArgs e)
        {

        }


        ///////////////////////SafeUpdaters-----------------------------------------------------------------------------------------------------------------------------
        private void SafeClose(FieldForm form)
        {
            if (form.InvokeRequired)
            {
                SafeFormClose CloseForm = new SafeFormClose(SafeClose);
                this.Invoke(CloseForm, form);
            }
            else
            {
                form.Close();
            }
        }
        private void SafeSet(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                SafeLabelSet SetLabel = new SafeLabelSet(SafeSet);
                this.Invoke(SetLabel, label, text);
            }
            else
            {
                label.Text = text;
            }
        }
        private void SafeEnable(Control control, bool Enabled)
        {
            if (control.InvokeRequired)
            {
                SafeControlEnable SafeEn = new SafeControlEnable(SafeEnable);
                this.Invoke(SafeEn, control, Enabled);
            }
            else
            {
                control.Enabled = Enabled;
            }
        }
        private void SafeCellStateUpdate(FieldViewer Viewer, CellState State, Point pos)
        {
            if (Viewer.InvokeRequired)
            {
                SafeCellUpdate UpdateState = new SafeCellUpdate(SafeCellStateUpdate);
                this.Invoke(UpdateState, Viewer, State, pos);
            }
            else
            {
                Viewer.updateCellState(State, pos);
            }
        }
    }
}
