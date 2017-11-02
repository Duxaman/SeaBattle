using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SeaBattle
{
    public struct Point
    {
        public int X;
        public int Y;
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    public enum CellState : int
    {
        Occupied,
        Free,
        Missed,
        Hit
    }
    public enum ShipType: int
    {
        FourDeck,
        ThreeDeck,
        TwoDeck,
        OneDeck,
        None
    }
    sealed public class FieldViewer : System.Windows.Forms.DataGridView
    {
        public int FieldSize { get; private set; }
        public FieldViewer(int fieldsize, System.Drawing.Point Location, int CellWidth)
        {
            System.Windows.Forms.DataGridViewButtonColumn[] Cols = new System.Windows.Forms.DataGridViewButtonColumn[fieldsize];
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeColumns = false;
            AllowUserToResizeRows = false;
            AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            BackgroundColor = System.Drawing.SystemColors.Control;
            ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            for(int i = 0; i < fieldsize; ++i)
            {
                Cols[i] = new System.Windows.Forms.DataGridViewButtonColumn();
                Cols[i].Width = CellWidth;
                Cols[i].HeaderText = "" + Convert.ToChar(i + 65); //TODO: Fix letters
                Cols[i].Name = "" + Convert.ToChar(i + 65);
                Cols[i].ReadOnly = true;
            }
            Columns.AddRange(Cols);
            RowCount = fieldsize;
            for(int i = 0; i < fieldsize; ++i)
            {
                Rows[i].HeaderCell.Value = i;
                Rows[i].Height = CellWidth;
                Rows[i].ReadOnly = true;
            }
            FieldSize = fieldsize;
            this.Location = Location;
            Size = new System.Drawing.Size((RowCount + 1) * CellWidth, (ColumnCount + 1) * CellWidth); 
            
        }
        public void updateCellState(CellState state, Point pos)
        {
            if(pos.X < FieldSize && pos.Y < FieldSize)
            {
                switch(state)
                {
                    case CellState.Free : Rows[pos.Y].Cells[pos.X].Style.BackColor = System.Drawing.Color.White; break;
                    case CellState.Hit: Rows[pos.Y].Cells[pos.X].Style.BackColor = System.Drawing.Color.Red; break;
                    case CellState.Missed: Rows[pos.Y].Cells[pos.X].Style.BackColor = System.Drawing.Color.LightGray; break;
                    case CellState.Occupied: Rows[pos.Y].Cells[pos.X].Style.BackColor = System.Drawing.Color.Black; break;

                }
            }
        }
    }
    sealed public class Field
    {
        /*
         Cell is identified by 4 params
         Owner - Player/Enemy (only if cell is occupied by the ship)
         Type - ship type (1,2,3,4 decks)
         shipid - identifies the ship among the others of the same type
             */
        public struct Cell
        {
            public CellState State;
            public bool ShipOwner;
            public ShipType Type;
            string shipid;
            public Cell(CellState State, bool Owner, ShipType Type, string shipid)
            {
                this.State = State;
                this.ShipOwner = Owner;
                this.Type = Type;
                this.shipid = shipid;
            }
        }
        public Field(int Size)
        {
            FieldSize = Size;
            Cells = new Cell[Size,Size];
            for(int i = 0; i < Size; ++i)   //not sure do we need this
                for(int j = 0; j < Size; ++j)
                {
                    Cells[i, j] = new Cell(CellState.Free, false, ShipType.None, "-1");
                }
        }
        private Cell[,] Cells;

        public int FieldSize { get; private set; }
        public Cell getCell(int x, int y)
        {
            return Cells[x, y];
        }
        public void setCell(int x, int y, Cell C)
        {
            Cells[x, y] = C;
        }

    }
}
