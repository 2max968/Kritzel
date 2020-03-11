using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel
{
    public enum PickerOrientation { Vertical, Horizontal};
    public enum PickerType { Click, Select, Check};

    public partial class PickerMenu : UserControl
    {
        public delegate void SelectionChangedEvent(PickerMenu sender, int e);
        public delegate void ClickItemEvent(PickerMenu sender, int ind);

        public event SelectionChangedEvent SelectionChanged;
        public event SelectionChangedEvent RightclickItem;
        public event ClickItemEvent ClickItem;

        const int SIZE = 64;

        public int Capacity { get; private set; } = 4;
        PickerOrientation orientation = PickerOrientation.Horizontal;
        public PickerOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; setSize(); }
        }
        public PickerType Type { get; set; } = PickerType.Select;
        public Bitmap[] contents = null;
        int selected = 0;
        Point cursor = new Point(-1, -1);

        public int Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                draw(CreateGraphics());
            }
        }

        public bool[] Checked { get; private set; } = new bool[] { false, false, false, false };

        public PickerMenu()
        {
            InitializeComponent();
            SetCapacity(4);

            this.MouseEnter += PickerMenu_MouseEnter;
            this.MouseLeave += PickerMenu_MouseLeave;
            this.MouseMove += PickerMenu_MouseMove;
            this.MouseClick += PickerMenu_MouseClick;
            this.Paint += PickerMenu_Paint;
        }

        private void PickerMenu_MouseClick(object sender, MouseEventArgs e)
        {
            if (Orientation == PickerOrientation.Horizontal)
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    Rectangle rect = new Rectangle(i * SIZE, 0, SIZE, this.Height);
                    if (rect.Contains(e.Location))
                    {
                        if (e.Button == MouseButtons.Left) select(i);
                        else if (e.Button == MouseButtons.Right)
                        {
                            RightclickItem?.Invoke(this, i);
                            return;
                        }
                        else return;
                        SelectionChanged?.Invoke(this, selected);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    Rectangle rect = new Rectangle(0, i * SIZE, this.Width, SIZE);
                    if (rect.Contains(e.Location))
                    {
                        if (e.Button == MouseButtons.Left) select(i);
                        else if (e.Button == MouseButtons.Right)
                        {
                            RightclickItem?.Invoke(this, i);
                            return;
                        }
                        else return;
                        SelectionChanged?.Invoke(this, selected);
                        break;
                    }
                }
            }
        }

        private void PickerMenu_MouseMove(object sender, MouseEventArgs e)
        {
            cursor = e.Location;
            draw(this.CreateGraphics());
        }

        private void PickerMenu_Paint(object sender, PaintEventArgs e)
        {
            draw(e.Graphics);
        }

        void draw(Graphics g)
        { 
            if(contents != null)
            {
                if (Orientation == PickerOrientation.Horizontal)
                {
                    for (int i = 0; i < contents.Length; i++)
                    {
                        Rectangle rect = new Rectangle(i * SIZE, 0, SIZE, this.Height);
                        Brush b = Brushes.White;
                        if (rect.Contains(cursor)) b = Brushes.LightGray;
                        if (isSelected(i)) b = Brushes.Tomato;
                        g.FillRectangle(b, rect);
                        Point picl = new Point(i * SIZE + (rect.Width - contents[i].Width) / 2,
                            (rect.Height - contents[i].Height) / 2);
                        g.DrawImage(contents[i], picl);
                    }
                }
                else
                {
                    for (int i = 0; i < contents.Length; i++)
                    {
                        Rectangle rect = new Rectangle(0, i * SIZE, this.Width, SIZE);
                        Brush b = Brushes.White;
                        if (rect.Contains(cursor)) b = Brushes.LightGray;
                        if (isSelected(i)) b = Brushes.Tomato;
                        g.FillRectangle(b, rect);
                        Point picl = new Point((rect.Width - contents[i].Width) / 2,
                            i * SIZE + (rect.Height - contents[i].Height) / 2);
                        g.DrawImage(contents[i], picl);
                    }
                }
            }
        }

        bool isSelected(int i)
        {
            if (Type == PickerType.Select)
                return i == selected;
            else if (Type == PickerType.Check)
                return Checked[i];
            else
                return false;
        }

        void select(int i)
        {
            if (Type == PickerType.Select)
                selected = i;
            else if (Type == PickerType.Check)
                Checked[i] = !Checked[i];
            else
                ClickItem?.Invoke(this, i);
        }

        private void PickerMenu_MouseLeave(object sender, EventArgs e)
        {
            setSize();
        }

        private void PickerMenu_MouseEnter(object sender, EventArgs e)
        {
            setSize();
        }

        void setSize()
        {
            int w = SIZE;// hover ? 94 : 64;
            Size nSize = this.Size;
            switch (Orientation)
            {
                case PickerOrientation.Horizontal:
                    nSize = new Size(SIZE * Capacity, w);
                    break;
                case PickerOrientation.Vertical:
                    nSize = new Size(w, SIZE * Capacity);
                    break;
            }
            /*if ((Anchor & AnchorStyles.Right) == AnchorStyles.Right && this.Width > nSize.Width)
                this.Location = new Point(this.Location.X + this.Width - nSize.Width, this.Location.Y);
            if ((Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom && this.Height > nSize.Height)
                this.Location = new Point(this.Location.X, this.Location.Y - this.Height + nSize.Height);*/
            this.Size = nSize;
            draw(CreateGraphics());
        }

        public void SetCapacity(int cap)
        {
            Capacity = cap;
            this.Checked = new bool[cap];
            for (int i = 0; i < cap; i++)
                this.Checked[i] = false;
            setSize();
            /*int w = this.Width;
            this.Size = new Size(64 * cap, 64);
            w -= this.Width;
            if((this.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
            {
                this.Location = new Point(this.Location.X + w, this.Location.Y);
            }*/
        }

        public void Fill(Bitmap[] bmps)
        {
            contents = bmps;
            SetCapacity(bmps.Length);
        }
    }
}
