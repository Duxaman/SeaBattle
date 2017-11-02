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
            Up,
            Down,
            Left,
            Right,
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

            for(int i = 0; i < 4; ++i)
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
                        for (int x = xst; x < e.ColumnIndex && x < _Field.FieldSize; ++x)
                            for (int y = yst; y < e.RowIndex && y < _Field.FieldSize; ++y)
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
                        if (CurShipOrientation == ShipOrientation.None)
                        {
                            for (int x = xst; x < e.ColumnIndex && x < _Field.FieldSize; ++x)
                                for (int y = yst; y < e.RowIndex && y < _Field.FieldSize; ++y)  //iterating over the cells around current cell,
                                {                                                               //all cells around must be free, excluding the previous deck
                                    if (_Field.getCell(x, y).State == CellState.Occupied)       //if the cell is occupied
                                    {
                                        if (x == LastPos.X && y == LastPos.Y)                   //and that occupied cell - the last pos of current ship
                                        {
                                            if (e.ColumnIndex - 1 == x && e.RowIndex == y)      //determine the orientation
                                            {
                                                CurShipOrientation = ShipOrientation.Left;
                                                continue;
                                            }
                                            if (e.ColumnIndex == x && e.RowIndex - 1 == y)
                                            {
                                                CurShipOrientation = ShipOrientation.Up;
                                                continue;
                                            }
                                            if (e.ColumnIndex + 1 == x && e.RowIndex == y)
                                            {
                                                CurShipOrientation = ShipOrientation.Right;
                                                continue;
                                            }
                                            if (e.ColumnIndex == x && e.RowIndex + 1 == y)
                                            {
                                                CurShipOrientation = ShipOrientation.Down;
                                                continue;
                                            }
                                        }
                                        return;
                                    }
                                }
                            LastPos.X = e.ColumnIndex;
                            LastPos.Y = e.RowIndex;   //set cell to the field and decrement counter
                            _Field.setCell(e.ColumnIndex, e.RowIndex, new Field.Cell(CellState.Occupied, false, CurShipType, CurShipId));
                            FieldView.updateCellState(CellState.Occupied, new Point(e.ColumnIndex, e.RowIndex));
                            ShipCounter--;
                        }
                        else
                        {
                            //here check orientation & proximity,
                            for (int x = xst; x < e.ColumnIndex && x < _Field.FieldSize; ++x)
                                for (int y = yst; y < e.RowIndex && y < _Field.FieldSize; ++y) //iterating over all cells around current one
                                {
                                    if (_Field.getCell(x, y).State == CellState.Occupied)     //if occupied cell encountered
                                    {
                                        if (x == LastPos.X && y == LastPos.Y)                 //and if occupied cell is the previous deck
                                        {
                                            switch (CurShipOrientation)                        //check the orientation
                                            {
                                                case ShipOrientation.Down: if (e.ColumnIndex == x && e.RowIndex - 1 == y) continue; break;
                                                case ShipOrientation.Left: if (e.ColumnIndex - 1 == x && e.RowIndex == y) continue; break;
                                                case ShipOrientation.Right: if (e.ColumnIndex + 1 == x && e.RowIndex == y) continue; break;
                                                case ShipOrientation.Up: if (e.ColumnIndex == x && e.RowIndex - 1 == y) continue; break;
                                            }
                                        }
                                        return;   //if encounter unknown occupied cell
                                    }
                                }
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
                if(_Field.getCell(e.ColumnIndex,e.RowIndex).State == CellState.Occupied)
                {
                    ShipType sh_type = _Field.getCell(e.ColumnIndex, e.RowIndex).Type;
                    CurCounters[(byte)sh_type]++;
                    switch(_Field.getCell(e.ColumnIndex, e.RowIndex).Type)
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
                    if ((e.RowIndex - 1 >= 0 && _Field.getCell(e.ColumnIndex,e.RowIndex - 1).State == CellState.Occupied) ||
                       (e.RowIndex + 1 < _Field.FieldSize && _Field.getCell(e.ColumnIndex, e.RowIndex + 1).State == CellState.Occupied))
                    {
                        DeletingShipOrientation = ShipOrientation.Down;
                    }
                    if(DeletingShipOrientation != ShipOrientation.Down)
                    {
                        if ((e.ColumnIndex - 1 >= 0 && _Field.getCell(e.ColumnIndex - 1, e.RowIndex).State == CellState.Occupied) ||
                            (e.ColumnIndex + 1 < _Field.FieldSize && _Field.getCell(e.ColumnIndex + 1, e.RowIndex).State == CellState.Occupied))
                        {
                            DeletingShipOrientation = ShipOrientation.Right;
                        }
                        else
                        {
                            DeletingShipOrientation = ShipOrientation.None;
                        }
                    }
                    if (DeletingShipOrientation == ShipOrientation.None) DeleteShipFromLastPos(new Point(e.ColumnIndex, e.RowIndex), DeletingShipOrientation);
                    else if(DeletingShipOrientation == ShipOrientation.Down)
                    {
                        int y = e.RowIndex;
                        while ( (y - 1) >= 0 && _Field.getCell(e.ColumnIndex, y - 1).State == CellState.Occupied) --y;
                        DeleteShipFromLastPos(new Point(e.ColumnIndex, y), DeletingShipOrientation);
                    }
                    else
                    {
                        int x = e.ColumnIndex;
                        while ((x - 1) >= 0 && _Field.getCell(x - 1, e.RowIndex).State == CellState.Occupied) --x;
                        DeleteShipFromLastPos(new Point(x, e.RowIndex), DeletingShipOrientation);
                    }
                }
                deleteMode = false;
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
            ParentForm.Show(); //TODO: Figure out how to open menu back
            this.Close();
        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < CurCounters.Length; ++i)
            {
                if(CurCounters[i] > 0)
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

        private void Map4Btn_Click(object sender, EventArgs e)
        {
            if (CurCounters[0] > 0) //if there are still more ships to map
            {
                Map4Btn.Enabled = false;
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
                Map3Btn.Enabled = false;
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
                Map2Btn.Enabled = false;
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
                Map1Btn.Enabled = false;
                ResetShipBtn.Enabled = true;
                ConfirmShipBtn.Enabled = true;
                LastPos.X = -1;
                LastPos.Y = -1;
                FieldView.Enabled = true;
                CurShipType = ShipType.OneDeck;
                ShipCounter = 1;
            }
        }

        private void DeleteShip_Click(object sender, EventArgs e)
        {
            //when this mode if on, one can click on the ship and it will be deleted
            //after click all other buttons must be disabled 
            deleteMode = true;
            Map1Btn.Enabled = false;
            Map2Btn.Enabled = false;
            Map3Btn.Enabled = false;
            Map4Btn.Enabled = false;

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
                        Map4Btn.Enabled = true;
                        FourLabel.Text = CurCounters[(byte)CurShipType].ToString();
                        break;
                    case ShipType.ThreeDeck:
                        Map3Btn.Enabled = true;
                        ThreeLabel.Text = CurCounters[(byte)CurShipType].ToString();
                        break;
                    case ShipType.TwoDeck:
                        Map2Btn.Enabled = true;
                        TwoLabel.Text = CurCounters[(byte)CurShipType].ToString();
                        break;
                    case ShipType.OneDeck:
                        Map1Btn.Enabled = true;
                        OneLabel.Text = CurCounters[(byte)CurShipType].ToString();
                        break;
                }
               
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
            //we know the orientation, and the last pos, thus
            //reverse the proccess
            CurCounters[(byte)CurShipType]++;
            ShipCounter =  DeleteShipFromLastPos(LastPos, CurShipOrientation);
            FieldView.Enabled = false;
            ResetShipBtn.Enabled = false;
            ConfirmShipBtn.Enabled = false;
            switch (CurShipType)
            {
                case ShipType.FourDeck: Map4Btn.Enabled = true; break;
                case ShipType.ThreeDeck: Map3Btn.Enabled = true; break;
                case ShipType.TwoDeck: Map2Btn.Enabled = true; break;
                case ShipType.OneDeck: Map1Btn.Enabled = true; break;
            }
        }
        //returns am of deleted cells
        private int DeleteShipFromLastPos(Point LastPos, ShipOrientation Orientation)
        {
            int DeletedCells = 0;
            switch (Orientation)
            {
                case ShipOrientation.Down:
                    {
                        for (int y = LastPos.Y; y > 0 && _Field.getCell(LastPos.X, y).State != CellState.Free; --y)
                        {
                            DeletedCells++;
                            _Field.setCell(LastPos.X, y, new Field.Cell(CellState.Free, false, ShipType.None, "-1"));
                            FieldView.updateCellState(CellState.Free, new Point(LastPos.X, y));
                        }
                    }
                    break;
                case ShipOrientation.Up:
                    {
                        for (int y = LastPos.Y; y < _Field.FieldSize && _Field.getCell(LastPos.X, y).State != CellState.Free; ++y)
                        {
                            DeletedCells++;
                            _Field.setCell(LastPos.X, y, new Field.Cell(CellState.Free, false, ShipType.None, "-1"));
                            FieldView.updateCellState(CellState.Free, new Point(LastPos.X, y));
                        }
                    }
                    break;
                case ShipOrientation.Left:
                    {
                        for (int x = LastPos.X; x < _Field.FieldSize && _Field.getCell(x, LastPos.Y).State != CellState.Free; ++x)
                        {
                            DeletedCells++;
                            _Field.setCell(x, LastPos.Y, new Field.Cell(CellState.Free, false, ShipType.None, "-1"));
                            FieldView.updateCellState(CellState.Free, new Point(x, LastPos.Y));
                        }
                    }
                    break;
                case ShipOrientation.Right:
                    {
                        for (int x = LastPos.X; x > 0 && _Field.getCell(x, LastPos.Y).State != CellState.Free; --x)
                        {
                            DeletedCells++;
                            _Field.setCell(x, LastPos.Y, new Field.Cell(CellState.Free, false, ShipType.None, "-1"));
                            FieldView.updateCellState(CellState.Free, new Point(x, LastPos.Y));
                        }
                    }
                    break;
                case ShipOrientation.None:
                    {
                        DeletedCells++;
                        _Field.setCell(LastPos.X, LastPos.Y, new Field.Cell(CellState.Free, false, ShipType.None, "-1"));
                        FieldView.updateCellState(CellState.Free, new Point(LastPos.X, LastPos.Y));

                    }
                    break;
            }
            return DeletedCells;
        }

        private void RandomizeBtn_Click(object sender, EventArgs e)
        {

        }

        private void ShipsMappingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ParentForm.Show();
        }
    }
}
