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
            SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            ColumnHeadersVisible = true;
            RowHeadersVisible = true;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeColumns = false;
            AllowUserToResizeRows = false;
            CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            BackgroundColor = System.Drawing.SystemColors.Control;
            RowHeadersWidth = 50;
            SelectionChanged += FieldViewer_SelectionChanged;
            ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            for(int i = 0; i < fieldsize; ++i)
            {
                Cols[i] = new System.Windows.Forms.DataGridViewButtonColumn();
                Cols[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                Cols[i].Width = CellWidth;
                Cols[i].HeaderText = "" + Convert.ToChar(i + 65);
                Cols[i].Name = "" + Convert.ToChar(i + 65);
                Cols[i].ReadOnly = true;
            }
            Columns.AddRange(Cols);
            Rows.Add(fieldsize);
            for(int i = 0; i < fieldsize; ++i)
            {
                Rows[i].HeaderCell.Value = i.ToString();
                Rows[i].Height = CellWidth;
                Rows[i].ReadOnly = true;
            }
            FieldSize = fieldsize;
            this.Location = Location;
            Size = new System.Drawing.Size((RowCount + 2) * CellWidth, (ColumnCount + 1) * CellWidth);
            ClearSelection();
        }

        private void FieldViewer_SelectionChanged(object sender, EventArgs e)
        {
            ClearSelection();
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
            ClearSelection();
        }
    }
    sealed public class Field
    {
        /*
         Cell is identified by 4 params
         Owner - Player/Enemy (only if cell is occupied by the ship)   does it really needed?
         Type - ship type (1,2,3,4 decks)
         shipid - identifies the ship among the others of the same type
             */
        public struct Cell
        {
            public CellState State;
            public bool ShipOwner;
            public ShipType Type;
            public string Shipid;
            public Cell(CellState State, bool Owner, ShipType Type, string shipid)
            {
                this.State = State;
                this.ShipOwner = Owner;
                this.Type = Type;
                this.Shipid = shipid;
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
