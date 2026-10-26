using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MohawkGame2D
{
    public class Intersection
    {
        public Road roadVert;
        public Road roadHor;
        float width;
        float height;
        Vector2 position;

        public Intersection(Road road1, Road road2)
        {
            //store which road is vertical and horisontal
            if (road1.vertical) {
                this.roadVert = road1;
                this.roadHor = road2;
            } else {
                this.roadVert = road2;
                this.roadHor = road1;
            }

            //find the position of the intersection based on the positions of the roads
            position = new Vector2(roadVert.position, roadHor.position);
        }

        public void Update()
        {
            //find the dimentions of the intersection based on the widths of the roads
            width = roadVert.width;
            height = roadHor.width;

            //draw the intersection
            int crosswalkSize = 5;
            Draw.SetLineSize(crosswalkSize);
            Draw.SetLineColor(Color.White);
            Draw.SetFillColor(Color.LightGray);
            Draw.Rectangle(position.X - width / 2, position.Y - height / 2, width, height);
        }
    }
}
