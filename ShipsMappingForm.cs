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
        private Field field;
        private FieldViewer FieldView;
        private int[] CurCounters;  //amount of ships to be mapped
        private int ShipCounter;   //amount of cells of current ship to be mapped
        private ShipOrientation CurShipOrientation;
        private ShipType CurShipType;
        private Point LastPos; //the last pos of setting the ship
        private string CurShipId;
        private bool deleteMode;
        public delegate void SelectField(Field field);
        public SelectField ConfirmField;
        public ShipsMappingForm(SelectField FieldCallBack)
        {
            InitializeComponent();
            ConfirmField = FieldCallBack;
            field = new Field(10);
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
                        for (int x = xst; x <= e.ColumnIndex + 1 && x < field.FieldSize; ++x)
                            for (int y = yst; y <= e.RowIndex + 1 && y < field.FieldSize; ++y)
                            {
                                if (field.getCell(x, y).State == CellState.Occupied) return;  //if at least one cell around is specified then position of new ship is invalid
                            }
                        LastPos.X = e.ColumnIndex;
                        LastPos.Y = e.RowIndex;
                        CurShipId = e.ColumnIndex.ToString() + e.RowIndex.ToString();
                        field.setCell(e.ColumnIndex, e.RowIndex, new Field.Cell(CellState.Occupied, CurShipType, CurShipId));
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
                            for (int x = xst; x <= e.ColumnIndex + 1 && x < field.FieldSize; ++x)
                                for (int y = yst; y <= e.RowIndex + 1 && y < field.FieldSize; ++y)  //iterating over the cells around current cell,
                                {                                                               //all cells around must be free, excluding the previous deck
                                    if (field.getCell(x, y).State == CellState.Occupied)       //if the cell is occupied
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
                            field.setCell(e.ColumnIndex, e.RowIndex, new Field.Cell(CellState.Occupied, CurShipType, CurShipId));
                            FieldView.updateCellState(CellState.Occupied, new Point(e.ColumnIndex, e.RowIndex));
                            ShipCounter--;
                        }
                        else
                        {
                            //here check orientation & proximity,
                            for (int x = xst; x <= e.ColumnIndex + 1 && x < field.FieldSize; ++x)
                                for (int y = yst; y <= e.RowIndex + 1 && y < field.FieldSize; ++y) //iterating over all cells around current one
                                {
                                    if (field.getCell(x, y).State == CellState.Occupied)     //if occupied cell encountered
                                    {
                                        if (field.getCell(x, y).Shipid == CurShipId)                 //and if occupied cell have the same id as the current ship
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
                            field.setCell(e.ColumnIndex, e.RowIndex, new Field.Cell(CellState.Occupied, CurShipType, CurShipId));
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
                if (field.getCell(e.ColumnIndex, e.RowIndex).State == CellState.Occupied)
                {
                    ShipType sh_type = field.getCell(e.ColumnIndex, e.RowIndex).Type;
                    CurCounters[(byte)sh_type]++;
                    switch (field.getCell(e.ColumnIndex, e.RowIndex).Type)
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
                    if ((e.RowIndex - 1 >= 0 && field.getCell(e.ColumnIndex, e.RowIndex - 1).State == CellState.Occupied) ||
                       (e.RowIndex + 1 < field.FieldSize && field.getCell(e.ColumnIndex, e.RowIndex + 1).State == CellState.Occupied))
                    {
                        DeletingShipOrientation = ShipOrientation.Vertical;
                    }
                    if (DeletingShipOrientation != ShipOrientation.Vertical)
                    {
                        if ((e.ColumnIndex - 1 >= 0 && field.getCell(e.ColumnIndex - 1, e.RowIndex).State == CellState.Occupied) ||
                            (e.ColumnIndex + 1 < field.FieldSize && field.getCell(e.ColumnIndex + 1, e.RowIndex).State == CellState.Occupied))
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
                    this.Hide();
                    ConfirmField(field);
                    this.Close();
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
            if (ShipCounter == 0)
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
                        while ((pos >= 0) && field.getCell(ShipCell.X, pos).State == CellState.Occupied)    //move to the start
                        {
                            pos--;
                        }
                        pos++;
                        for (int y = pos; y < field.FieldSize && field.getCell(ShipCell.X, y).State != CellState.Free; ++y)   
                        {
                            DeletedCells++;
                            field.setCell(ShipCell.X, y, new Field.Cell(CellState.Free, ShipType.None, "-1"));
                            FieldView.updateCellState(CellState.Free, new Point(ShipCell.X, y));
                        }
                    }
                    break;
                case ShipOrientation.Horizontal:
                    {
                        int pos = ShipCell.X;
                        while ((pos >= 0) && field.getCell(pos, ShipCell.Y).State == CellState.Occupied)    //move to the start
                        {
                            pos--;
                        }
                        pos++;
                        for (int x = pos; x < field.FieldSize && field.getCell(x, ShipCell.Y).State != CellState.Free; ++x)
                        {
                            DeletedCells++;
                            field.setCell(x, ShipCell.Y, new Field.Cell(CellState.Free, ShipType.None, "-1"));
                            FieldView.updateCellState(CellState.Free, new Point(x, ShipCell.Y));
                        }
                    }
                    break;
                case ShipOrientation.None:
                    {
                        DeletedCells++;
                        field.setCell(ShipCell.X, ShipCell.Y, new Field.Cell(CellState.Free, ShipType.None, "-1"));
                        FieldView.updateCellState(CellState.Free, new Point(ShipCell.X, ShipCell.Y));
                    }
                    break;
            }
            FieldView.Update();
            //return DeletedCells;
        }
        private void Determineborders(ref int hst, ref int vst, ref int hend, ref int vend, Point Start, Point End)
        {
            vst = Start.Y == 0 ? 0 : Start.Y - 1;
            hst = Start.X == 0 ? 0 : Start.X - 1;
            vend = End.Y == field.FieldSize - 1 ? field.FieldSize - 1 : End.Y + 1;
            hend = End.X == field.FieldSize - 1 ? field.FieldSize - 1: End.X + 1;
        }
        private bool CheckShips(Point Start, Point End)  //returns false, if there are any ships in area around start and end
        {
            int hst, vst, hend, vend;
            hst = vst = hend = vend = 0; // borders of checking area
            Determineborders(ref hst, ref vst, ref hend, ref vend, Start, End);
            for (int i = hst; i <= hend; ++i)
                for (int j = vst; j <= vend; ++j)
                {
                    if (field.getCell(i, j).State != CellState.Free)
                    {
                        return false;
                    }
                }
            return true;
        }

        private void RandomizeBtn_Click(object sender, EventArgs e)
        {
            /*
             method generate ships, 
             first create start and end pos, then check if there are another ships nearby, if yes generate again, else plant ship to the field
             
             */
            ResetAll();
            Random Randomizer = new Random();
            bool generated = false;
            ShipOrientation Orientation = ShipOrientation.None;
            Point Start = new Point();
            Point End = new Point();
            while (!generated)
            {
                Orientation = (Randomizer.Next(2) == 0) ? ShipOrientation.Vertical : ShipOrientation.Horizontal;   //randomize orientation
                if (Orientation == ShipOrientation.Horizontal)
                {
                    Start.X = Randomizer.Next(7);
                    Start.Y = Randomizer.Next(10);
                    End.Y = Start.Y;
                    End.X = Start.X + 3;
                    if (CheckShips(Start, End)) generated = true;
                }
                else
                {
                    Start.X = Randomizer.Next(10);
                    Start.Y = Randomizer.Next(7);
                    End.Y = Start.Y + 3;
                    End.X = Start.X;
                    if (CheckShips(Start, End)) generated = true;
                }
            }
            //plant ship
            spawnShip(Start, End, ShipType.FourDeck);
            for (int i = 0; i < 2; ++i)
            {
                generated = false;
                while (!generated)
                {
                    Orientation = (Randomizer.Next(2) == 0) ? ShipOrientation.Vertical : ShipOrientation.Horizontal;
                    if (Orientation == ShipOrientation.Horizontal)
                    {
                        Start.X = Randomizer.Next(8);
                        Start.Y = Randomizer.Next(10);
                        End.Y = Start.Y;
                        End.X = Start.X + 2;
                        if (CheckShips(Start, End)) generated = true;
                    }
                    else
                    {
                        Start.X = Randomizer.Next(10);
                        Start.Y = Randomizer.Next(8);
                        End.Y = Start.Y + 2;
                        End.X = Start.X;
                        if (CheckShips(Start, End)) generated = true;
                    }
                }
                //plant ship
                spawnShip(Start, End, ShipType.ThreeDeck);
            }
            for (int i = 0; i < 3; ++i)
            {
                generated = false;
                while (!generated)
                {
                    Orientation = (Randomizer.Next(2) == 0) ? ShipOrientation.Vertical : ShipOrientation.Horizontal;
                    if (Orientation == ShipOrientation.Horizontal)
                    {
                        Start.X = Randomizer.Next(9);
                        Start.Y = Randomizer.Next(10);
                        End.Y = Start.Y;
                        End.X = Start.X + 1;
                        if (CheckShips(Start, End)) generated = true;
                    }
                    else
                    {
                        Start.X = Randomizer.Next(10);
                        Start.Y = Randomizer.Next(9);
                        End.Y = Start.Y + 1;
                        End.X = Start.X;
                        if (CheckShips(Start, End)) generated = true;
                    }
                }
                //plant ship
                spawnShip(Start, End, ShipType.TwoDeck);
            }
            for (int i = 0; i < 4; ++i)
            {
                generated = false;
                while (!generated)
                {
                    Start.X = Randomizer.Next(10);
                    Start.Y = Randomizer.Next(10);
                    if (CheckShips(Start, Start)) generated = true;
                }
                //plant ship
                spawnShip(Start, Start, ShipType.OneDeck);
            }

        }

        private void spawnShip(Point Start, Point End, ShipType ShType)
        {
            /*
             * method spawn ship to the field, and decrement counters bounded with this shiptype
             */
            switch (ShType)
            {
                case ShipType.FourDeck:

                    CurCounters[0]--;
                    FourLabel.Text = CurCounters[0].ToString();
                    for (int i = Start.X; i <= End.X; ++i)
                        for (int j = Start.Y; j <= End.Y; ++j)
                        {
                            field.setCell(i, j, new Field.Cell(CellState.Occupied, ShipType.FourDeck, Start.X.ToString() + Start.Y.ToString()));
                            FieldView.updateCellState(CellState.Occupied, new Point(i, j));
                        }

                    break;
                case ShipType.ThreeDeck:
                    CurCounters[1]--;
                    ThreeLabel.Text = CurCounters[1].ToString();
                    for (int i = Start.X; i <= End.X; ++i)
                        for (int j = Start.Y; j <= End.Y; ++j)
                        {
                            field.setCell(i, j, new Field.Cell(CellState.Occupied, ShipType.ThreeDeck, Start.X.ToString() + Start.Y.ToString()));
                            FieldView.updateCellState(CellState.Occupied, new Point(i, j));
                        }
                    break;
                case ShipType.TwoDeck:
                    CurCounters[2]--;
                    TwoLabel.Text = CurCounters[2].ToString();
                    for (int i = Start.X; i <= End.X; ++i)
                        for (int j = Start.Y; j <= End.Y; ++j)
                        {
                            field.setCell(i, j, new Field.Cell(CellState.Occupied, ShipType.TwoDeck, Start.X.ToString() + Start.Y.ToString()));
                            FieldView.updateCellState(CellState.Occupied, new Point(i, j));
                        }
                    break;
                case ShipType.OneDeck:
                    CurCounters[3]--;
                    OneLabel.Text = CurCounters[3].ToString();
                    field.setCell(Start.X, Start.Y, new Field.Cell(CellState.Occupied, ShipType.OneDeck, Start.X.ToString() + Start.Y.ToString()));
                    FieldView.updateCellState(CellState.Occupied, Start);

                    break;
            }

        }
        private void ShipsMappingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Owner.Show();
        }

        private void ResetAll()  //reset all to start pos
        {
            FieldView.Enabled = false;
            for (int i = 0; i < 4; ++i)
            {
                CurCounters[i] = i + 1;
            }
            this.Controls.Add(FieldView);
            FieldView.Enabled = false;
            ResetShipBtn.Enabled = false;
            ConfirmShipBtn.Enabled = false;
            deleteMode = false;
            Map1Btn.Enabled = true;
            Map2Btn.Enabled = true;
            Map3Btn.Enabled = true;
            Map4Btn.Enabled = true;
            OneLabel.Text = CurCounters[3].ToString();
            TwoLabel.Text = CurCounters[2].ToString();
            ThreeLabel.Text = CurCounters[1].ToString();
            FourLabel.Text = CurCounters[0].ToString();
            ShipCounter = 0;
            LastPos.X = -1;
            LastPos.Y = -1;
            for(int i = 0; i < field.FieldSize; ++i)
                for(int j = 0; j < field.FieldSize; ++j)
                {
                    field.setCell(i, j, new Field.Cell(CellState.Free, ShipType.None, "-1"));
                    FieldView.updateCellState(CellState.Free, new Point(i,j));
                }
        }
        private void ResetAllBtn_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
    }
}
