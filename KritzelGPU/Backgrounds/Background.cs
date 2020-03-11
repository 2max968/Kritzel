using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kritzel.Renderer;

namespace Kritzel.Backgrounds
{
    public abstract class Background
    {
        public abstract void Draw(Renderer.BaseRenderer r, PageFormat format, float border, Color mainColor, Color secondaryColor);
    }

    public class BackgroundNull : Background
    {
        public override void Draw(BaseRenderer r, PageFormat format, float border, Color mainColor, Color secondaryColor)
        {
            
        }
    }

    public class BName : Attribute
    {
        public string Name { get; private set; }
        public BName(string name)
        {
            this.Name = name;
        }
    }
}
