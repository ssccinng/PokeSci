using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeOCRSV;
internal class PokeRegion
{
    public Point BasePoint
    {
        get; set;
    } = new Point();

    public int Width
    {
        get; set;
    } = 480;
    public int Height
    {
        get; set;
    } = 140;

    public Rectangle Name
    {
        get; set;
    }
    public Rectangle Item
    {
        get; set;
    }

    public PokeRegion(Point point)
    {
        BasePoint = point;
    }
}
