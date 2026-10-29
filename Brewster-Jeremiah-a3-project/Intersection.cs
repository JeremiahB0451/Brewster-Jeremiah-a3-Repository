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
        public float width;
        public float height;
        public Vector2 position;
        public bool greenVert = true;
        float changeRate = 10f; //in seconds
        float changeTimer = 0f;

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

            //change the lights
            changeTimer += Time.DeltaTime;
            if (changeTimer >= changeRate) {
                greenVert = !greenVert;
                changeTimer = 0f;
            }

            //draw the intersection
            int crosswalkSize = 3;
            Draw.SetLineSize(crosswalkSize);
            Draw.SetLineColor(Color.White);
            Draw.SetFillColor(Color.LightGray);
            Draw.Rectangle(position.X - width / 2, position.Y - height / 2, width, height);
        }

        //draw the lights
        public void DrawLights() {
            Draw.SetLineSize(0);
            int transperency = 127;
            float angle = 30f; //half the angle
            Vector2 size = new Vector2(40, 40);
            //vertical lights
            if (greenVert) Draw.SetFillColor(0, 255, 0, transperency);
            else Draw.SetFillColor(255, 0, 0, transperency);
            Draw.Arc(new Vector2(position.X, position.Y - height / 2), size, 90 - angle, 90 + angle); //top
            Draw.Arc(new Vector2(position.X, position.Y + height / 2), size, 270 - angle, 270 + angle); //bottom
            //horisontal lights
            if (!greenVert) Draw.SetFillColor(0, 255, 0, transperency);
            else Draw.SetFillColor(255, 0, 0, transperency);
            Draw.Arc(new Vector2(position.X - width / 2, position.Y), size, 0 - angle, 0 + angle); //left
            Draw.Arc(new Vector2(position.X + width / 2, position.Y), size, 180 - angle, 180 + angle); //right
        }
    }
}
