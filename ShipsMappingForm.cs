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
    public partial class ShipsMappingForm : Form
    {
        private enum ShipOrientation : byte
        {
            Vertical,
            Horizontal,
            None
        }
        private bool GameLaunchType;
        private Field _Field;
        private FieldViewer FieldView;
        private int[] CurCounters;  //amount of ships to be mapped
        private int ShipCounter;   //amount of cells of current ship to be mapped
        private ShipOrientation CurShipOrientation;
        private ShipType CurShipType;
        private Point LastPos; //the last pos of setting the ship // it'll help to identify collisions
        private string CurShipId;
        private bool deleteMode;
        public ShipsMappingForm(bool startmode)
        {
            InitializeComponent();
            GameLaunchType = startmode;
            _Field = new Field(10);
            FieldView = new FieldViewer(10, new System.Drawing.Point(458, 1), 50);
            FieldView.CellContentClick += CellClickHandler;
            FieldView.Enabled = false;
            CurCounters = new int[4];

            for (int i = 0; i < 4; ++i)
            {
                CurCounters[i] = i + 1;
            }
            this.Controls.Add(FieldView);
            FieldView.Enabled = false;
            ResetShipBtn.Enabled = false;
            ConfirmShipBtn.Enabled = false;
            deleteMode = false;
            //deleteship too
        }

        private void CellClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            if (!deleteMode)
            {
                if (ShipCounter > 0) //if there are still more decks of ship to map
                {
                    /*
                     if last pos - undefined
                     check whether around cells have enougth space, if not ignore this call
                     otherwise, set last pos, and mark cell at the field, decrement counters
                     */
                    int xst = e.ColumnIndex - 1;
                    int yst = e.RowIndex - 1;
                    if (xst < 0) xst++;          //if around cells is beyond the field increment the counter
                    if (yst < 0) yst++;
                    if (LastPos.X == -1 && LastPos.Y == -1)
                    {
                        for (int x = xst; x <= e.ColumnIndex + 1 && x < _Field.FieldSize; ++x)
                            for (int y = yst; y <= e.RowIndex + 1 && y < _Field.FieldSize; ++y)
                            {
                                if (_Field.getCell(x, y).State == CellState.Occupied) return;  //if at least one cell around is specified then position of new ship is invalid
                            }
                        LastPos.X = e.ColumnIndex;
                        LastPos.Y = e.RowIndex;
                        CurShipId = e.ColumnIndex.ToString() + e.RowIndex.ToString();
                        _Field.setCell(e.ColumnIndex, e.RowIndex, new Field.Cell(CellState.Occupied, false, CurShipType, CurShipId));
                        FieldView.updateCellState(CellState.Occupied, new Point(e.ColumnIndex, e.RowIndex));
                        ShipCounter--;

                    }
                    else
                    {
                        /*if last pos is specified,
                        check does the orientation specified,
                        if not check does the new deck is nearby the previous one, then specify orientation
                        */
                        bool DeckIsNear = false;
                        if (CurShipOrientation == ShipOrientation.None)
                        {
                            for (int x = xst; x <= e.ColumnIndex + 1 && x < _Field.FieldSize; ++x)
                                for (int y = yst; y <= e.RowIndex + 1 && y < _Field.FieldSize; ++y)  //iterating over the cells around current cell,
                                {                                                               //all cells around must be free, excluding the previous deck
                                    if (_Field.getCell(x, y).State == CellState.Occupied)       //if the cell is occupied
                                    {
                                        if (x == LastPos.X && y == LastPos.Y)                   //and that occupied cell - last pos
                                        {
                                            DeckIsNear = true;
                                            if ((e.ColumnIndex - 1 == x && e.RowIndex == y) || (e.ColumnIndex + 1 == x && e.RowIndex == y))      //determine the orientation
                                            {
                                                CurShipOrientation = ShipOrientation.Horizontal;
                                                continue;
                                            }
                                            if ((e.ColumnIndex == x && e.RowIndex - 1 == y) || (e.ColumnIndex == x && e.RowIndex + 1 == y))
                                            {
                                                CurShipOrientation = ShipOrientation.Vertical;
                                                continue;
                                            }
                                        }
                                        return;
                                    }
                                }
                            if (!DeckIsNear) return;
                            LastPos.X = e.ColumnIndex;
                            LastPos.Y = e.RowIndex;   //set cell to the field and decrement counter
                            _Field.setCell(e.ColumnIndex, e.RowIndex, new Field.Cell(CellState.Occupied, false, CurShipType, CurShipId));
                            FieldView.updateCellState(CellState.Occupied, new Point(e.ColumnIndex, e.RowIndex));
                            ShipCounter--;
                        }
                        else
                        {
                            //here check orientation & proximity,
                            for (int x = xst; x <= e.ColumnIndex + 1 && x < _Field.FieldSize; ++x)
                                for (int y = yst; y <= e.RowIndex + 1 && y < _Field.FieldSize; ++y) //iterating over all cells around current one
                                {
                                    if (_Field.getCell(x, y).State == CellState.Occupied)     //if occupied cell encountered
                                    {
                                        if (_Field.getCell(x, y).Shipid == CurShipId)                 //and if occupied cell have the same id as the current ship
                                        {
                                            DeckIsNear = true;
                                            switch (CurShipOrientation)                        //check the orientation
                                            {
                                                case ShipOrientation.Vertical: if ((e.ColumnIndex == x && e.RowIndex - 1 == y) || (e.ColumnIndex == x && e.RowIndex + 1 == y)) continue; break; //check both side simuataneosly
                                                case ShipOrientation.Horizontal: if ((e.ColumnIndex - 1 == x && e.RowIndex == y) || (e.ColumnIndex + 1 == x && e.RowIndex == y)) continue; break;
                                            }
                                        }
                                        return;   //if encounter unknown occupied cell
                                    }
                                }
                            if (!DeckIsNear) return;
                            LastPos.X = e.ColumnIndex;
                            LastPos.Y = e.RowIndex;   //set cell and decrement counter
                            _Field.setCell(e.ColumnIndex, e.RowIndex, new Field.Cell(CellState.Occupied, false, CurShipType, CurShipId));
                            FieldView.updateCellState(CellState.Occupied, new Point(e.ColumnIndex, e.RowIndex));
                            ShipCounter--;
                        }
                    }
                }
            }
            else
            {
                ShipOrientation DeletingShipOrientation = ShipOrientation.None;
                //check does the selected cell is occipied
                if (_Field.getCell(e.ColumnIndex, e.RowIndex).State == CellState.Occupied)
                {
                    ShipType sh_type = _Field.getCell(e.ColumnIndex, e.RowIndex).Type;
                    CurCounters[(byte)sh_type]++;
                    switch (_Field.getCell(e.ColumnIndex, e.RowIndex).Type)
                    {
                        case ShipType.FourDeck:
                            FourLabel.Text = CurCounters[(byte)sh_type].ToString();
                            break;
                        case ShipType.ThreeDeck:
                            ThreeLabel.Text = CurCounters[(byte)sh_type].ToString();
                            break;
                        case ShipType.TwoDeck:
                            TwoLabel.Text = CurCounters[(byte)sh_type].ToString();
                            break;
                        case ShipType.OneDeck:
                            OneLabel.Text = CurCounters[(byte)sh_type].ToString();
                            break;
                    }
                    //scan  ways
                    //first vertical
                    if ((e.RowIndex - 1 >= 0 && _Field.getCell(e.ColumnIndex, e.RowIndex - 1).State == CellState.Occupied) ||
                       (e.RowIndex + 1 < _Field.FieldSize && _Field.getCell(e.ColumnIndex, e.RowIndex + 1).State == CellState.Occupied))
                    {
                        DeletingShipOrientation = ShipOrientation.Vertical;
                    }
                    if (DeletingShipOrientation != ShipOrientation.Vertical)
                    {
                        if ((e.ColumnIndex - 1 >= 0 && _Field.getCell(e.ColumnIndex - 1, e.RowIndex).State == CellState.Occupied) ||
                            (e.ColumnIndex + 1 < _Field.FieldSize && _Field.getCell(e.ColumnIndex + 1, e.RowIndex).State == CellState.Occupied))
                        {
                            DeletingShipOrientation = ShipOrientation.Horizontal;
                        }
                        else
                        {
                            DeletingShipOrientation = ShipOrientation.None;
                        }
                    }
                    if (DeletingShipOrientation == ShipOrientation.None) DeleteShipFromOneDeck(new Point(e.ColumnIndex, e.RowIndex), DeletingShipOrientation);
                    else if (DeletingShipOrientation == ShipOrientation.Vertical)
                    {
                        DeleteShipFromOneDeck(new Point(e.ColumnIndex, e.RowIndex), DeletingShipOrientation);
                    }
                    else
                    {
                        DeleteShipFromOneDeck(new Point(e.ColumnIndex, e.RowIndex), DeletingShipOrientation);
                    }
                }
                deleteMode = false;
                FieldView.Enabled = false;
                //restore another buttons availability
                Map1Btn.Enabled = true;
                Map2Btn.Enabled = true;
                Map3Btn.Enabled = true;
                Map4Btn.Enabled = true;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            if (ShipCounter == 0)
            {
                for (int i = 0; i < CurCounters.Length; ++i)
                {
                    if (CurCounters[i] > 0)
                    {
                        MessageBox.Show("Вы расставили не все корабли", "Ошибка");
                        return;
                    }
                }
                if (MessageBox.Show("Завершить расстановку кораблей?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // init new form and start game
                } 
            }
            else
                MessageBox.Show("Вы расставили корабль", "Ошибка");
        }

        private void Map4Btn_Click(object sender, EventArgs e)
        {
            if (CurCounters[0] > 0) //if there are still more ships to map
            {
                CurShipOrientation = ShipOrientation.None;
                DisableButtons();
                ResetShipBtn.Enabled = true;
                ConfirmShipBtn.Enabled = true;
                LastPos.X = -1;
                LastPos.Y = -1;
                FieldView.Enabled = true;
                CurShipType = ShipType.FourDeck;
                ShipCounter = 4;
            }

        }

        private void Map3Btn_Click(object sender, EventArgs e)
        {
            if (CurCounters[1] > 0)
            {
                CurShipOrientation = ShipOrientation.None;
                DisableButtons();
                ResetShipBtn.Enabled = true;
                ConfirmShipBtn.Enabled = true;
                LastPos.X = -1;
                LastPos.Y = -1;
                FieldView.Enabled = true;
                CurShipType = ShipType.ThreeDeck;
                ShipCounter = 3;
            }
        }
        private void Map2Btn_Click(object sender, EventArgs e)
        {
            if (CurCounters[2] > 0)
            {
                CurShipOrientation = ShipOrientation.None;
                DisableButtons();
                ResetShipBtn.Enabled = true;
                ConfirmShipBtn.Enabled = true;
                LastPos.X = -1;
                LastPos.Y = -1;
                FieldView.Enabled = true;
                CurShipType = ShipType.TwoDeck;
                ShipCounter = 2;
            }
        }

        private void Map1Btn_Click(object sender, EventArgs e)
        {
            if (CurCounters[3] > 0)
            {
                CurShipOrientation = ShipOrientation.None;
                DisableButtons();
                ResetShipBtn.Enabled = true;
                ConfirmShipBtn.Enabled = true;
                LastPos.X = -1;
                LastPos.Y = -1;
                FieldView.Enabled = true;
                CurShipType = ShipType.OneDeck;
                ShipCounter = 1;
            }
        }
        private void DisableButtons()
        {
            Map1Btn.Enabled = false;
            Map2Btn.Enabled = false;
            Map3Btn.Enabled = false;
            Map4Btn.Enabled = false;
            DeleteShip.Enabled = false;
        }
        private void DeleteShip_Click(object sender, EventArgs e)
        {
            //when this mode if on, one can click on the ship and it will be deleted
            //after click all other buttons must be disabled 
            deleteMode = true;
            FieldView.Enabled = true;
            DisableButtons();

        }

        private void ConfirmShipBtn_Click(object sender, EventArgs e)
        {
            if(ShipCounter == 0)
            {
                FieldView.Enabled = false;
                ResetShipBtn.Enabled = false;
                ConfirmShipBtn.Enabled = false;
                CurCounters[(byte)CurShipType]--;
                switch (CurShipType)
                {
                    case ShipType.FourDeck:
                        FourLabel.Text = CurCounters[(byte)CurShipType].ToString();
                        break;
                    case ShipType.ThreeDeck:
                        ThreeLabel.Text = CurCounters[(byte)CurShipType].ToString();
                        break;
                    case ShipType.TwoDeck:
                        TwoLabel.Text = CurCounters[(byte)CurShipType].ToString();
                        break;
                    case ShipType.OneDeck:
                        OneLabel.Text = CurCounters[(byte)CurShipType].ToString();
                        break;
                }
                Map1Btn.Enabled = true;
                Map2Btn.Enabled = true;
                Map3Btn.Enabled = true;
                Map4Btn.Enabled = true;
                DeleteShip.Enabled = true;
            }
            else
            {
                MessageBox.Show("Вы не завершили расстановку корабля");
            }
        }
        //while editing the ship in progress one may cancel deck entreing and clear current ship deck
        //after this one can choose new ship to map
        private void ResetShipBtn_Click(object sender, EventArgs e)
        {
            //we know the orientation, and the last pos, thus send lastpos to delete method
            if (LastPos.X != -1 && LastPos.Y != -1)
            {
                CurCounters[(byte)CurShipType]++;
                DeleteShipFromOneDeck(LastPos, CurShipOrientation);               
            }
            FieldView.Enabled = false;
            ResetShipBtn.Enabled = false;
            DeleteShip.Enabled = false;
            ConfirmShipBtn.Enabled = false;
            Map1Btn.Enabled = true;
            Map2Btn.Enabled = true;
            Map3Btn.Enabled = true;
            Map4Btn.Enabled = true;
        }
        //returns am of deleted cells
        private void DeleteShipFromOneDeck(Point ShipCell, ShipOrientation Orientation)
        {
            int DeletedCells = 0;
            switch (Orientation)
            {
                case ShipOrientation.Vertical:
                    {
                        int pos = ShipCell.Y;
                        while ( (pos >= 0) && _Field.getCell(ShipCell.X, pos).State == CellState.Occupied)    //move to the start
                        {
                            pos--;                            
                        }
                        pos++;
                        for (int y = pos; y < _Field.FieldSize && _Field.getCell(ShipCell.X, y).State != CellState.Free; ++y)    //check
                        {
                            DeletedCells++;
                            _Field.setCell(ShipCell.X, y, new Field.Cell(CellState.Free, false, ShipType.None, "-1"));
                            FieldView.updateCellState(CellState.Free, new Point(ShipCell.X, y));
                        }
                    }
                    break;
                case ShipOrientation.Horizontal:
                    {
                        int pos = ShipCell.X;
                        while ((pos >= 0) && _Field.getCell(pos, ShipCell.Y).State == CellState.Occupied)    //move to the start
                        {
                            pos--;
                        }
                        pos++;
                        for (int x = pos; x < _Field.FieldSize && _Field.getCell(x, ShipCell.Y).State != CellState.Free; ++x)
                        {
                            DeletedCells++;
                            _Field.setCell(x, ShipCell.Y, new Field.Cell(CellState.Free, false, ShipType.None, "-1"));
                            FieldView.updateCellState(CellState.Free, new Point(x, ShipCell.Y));
                        }
                    }
                    break;
                case ShipOrientation.None:
                    {
                        DeletedCells++;
                        _Field.setCell(ShipCell.X, ShipCell.Y, new Field.Cell(CellState.Free, false, ShipType.None, "-1"));
                        FieldView.updateCellState(CellState.Free, new Point(ShipCell.X, ShipCell.Y));
                    }
                    break;
            }
            FieldView.Update();
            //return DeletedCells;
        }

        private void RandomizeBtn_Click(object sender, EventArgs e)
        {

        }

        private void ShipsMappingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Owner.Show();
        }
    }
}
