using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.PointerInputLibrary
{
    public class PointerManager
    {
        public Dictionary<uint, Touch> Touches { get; private set; } 
            = new Dictionary<uint, Touch>();

        Control target;

        public PointerManager()
        {
            this.target = null;
        }

        public PointerManager(Control target)
        {
            this.target = target;
        }

        public PointerManager(Control target, Native.TWF flags)
        {
            this.target = target;
            Native.RegisterTouchWindow(target.Handle, (int)flags);
        }

        public void HandleWndProc(ref Message m)
        {
            Touch t;
            switch(m.Msg)
            {
                case Touch.TOUCH_DOWN:
                //case Touch.MOUSE_LDOWN:
                //case Touch.MOUSE_RDOWN:
                    t = new Touch(m, target);
                    if (Touches.ContainsKey(t.Id))
                        Touches.Remove(t.Id);
                    Touches.Add(t.Id, t);
                    break;
                case Touch.TOUCH_UP:
                //case Touch.MOUSE_LUP:
                //case Touch.MOUSE_RUP:
                    t = new Touch(m, target);
                    if (Touches.ContainsKey(t.Id))
                        Touches.Remove(t.Id);
                    break;
                case Touch.TOUCH_MOVE:
                //case Touch.MOUSE_MOVE:
                    t = new Touch(m, target);
                    if (Touches.ContainsKey(t.Id))
                    {
                        Touches[t.Id].MoveTo(t);
                    }
                    else
                    {
                        Touches.Add(t.Id, t);
                    }
                    break;
            }
        }
    }
}
