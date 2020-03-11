using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PointerInputLibrary
{
    public enum TouchDevice { Mouse, Finger, Pen};

    public class Touch
    {
        public const int TOUCH_MOVE = 581;
        public const int TOUCH_DOWN = 585;
        public const int TOUCH_UP = 586;
        public const int TOUCH_ELEMENT = 787;

        public uint Id { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public TouchDevice TouchDevice { get; private set; }
        public long Pressure { get; private set; } = -1;
        public PenFlags PenFlags { get; private set; } = PenFlags.NONE;
        public bool Down { get; private set; } = true;
        public int Param;
        public List<Point> Trail { get; } = new List<Point>();

        public Touch(Message msg, Control target = null)
            : this(msg.WParam.ToInt32(), msg.LParam.ToInt32(), target)
        {

        }

        public Touch(Int32 wparam, Int32 lparam, Control target = null)
        {
            Id = (uint)(wparam & 0xffff);
            X = (short)lparam;
            Y = (short)(lparam >> 16);
            if(target != null)
            {
                Point pt = new Point(X, Y);
                pt = target.PointToClient(pt);
                X = pt.X;
                Y = pt.Y;
            }

            int param = (wparam >> 16) & 0xff;
            TouchDevice = TouchDevice.Finger;
            this.Param = param;

            PointerInfo.POINTER_PEN_INFO pInfo;
            if (PointerInfo.GetPointerPenInfo(Id, out pInfo))
            {
                TouchDevice = TouchDevice.Pen;
                if (System.Environment.Is64BitProcess)
                {
                    Pressure = (long)pInfo.x64pressure;
                    PenFlags = pInfo.x64penFlags;
                }
                else
                {
                    Pressure = (long)pInfo.x86pressure;
                    PenFlags = pInfo.x86penFlags;
                }
                if(Pressure == 0) Down = false;
            }
        }

        public void MoveTo(Touch t)
        {
            Trail.Insert(0, new Point(X, Y));
            if (Trail.Count > 5) Trail.RemoveAt(Trail.Count - 1);
            X = t.X;
            Y = t.Y;
            Pressure = t.Pressure;
            PenFlags = t.PenFlags;
            Down = t.Down;
        }
    }
}
