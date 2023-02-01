﻿using System;
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
    } = 550;
    public int Height
    {
        get; set;
    } = 140;

    public Rectangle Name
    {
        get; set;
    } = new Rectangle(new Point(0, 0), new Size(180, 32));
    public Rectangle Item
    {
        get; set;
    } = new Rectangle(new Point(35, 110), new Size(180, 30));

    public Rectangle Ability
    {
        get; set;
    } = new Rectangle(new Point(0, 73), new Size(180, 32));
    public Rectangle RectangleMove
    {
        get; set;
    } = new Rectangle(new Point(345, 0), new Size(200, 140));
    public PokeRegion(Point point)
    {
        BasePoint = point;
    }

    //public Bitmap GetMoveBitmap(Bitmap bitmap1)
    //{

    //}
}
