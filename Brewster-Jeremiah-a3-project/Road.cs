using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MohawkGame2D
{
    public class Road
    {
        public float width = 50f;
        float length = 0f;
        public bool building = true;
        float buildSpeed = 100;
        public float position;
        public string direction; //up, right, down, or left
        public bool vertical; //seperate value just storing if the road is horisontal or vertical to make some if statments easier
        public int lanes = 2;

        public Road(Vector2 position, string direction)
        {
            //determine if its horisontal or vertical to store either the x or y position
            if (direction == "up" || direction == "down") {
                this.vertical = true;
                this.position = position.X;
            } else {
                this.vertical = false;
                this.position = position.Y;
            }

            this.direction = direction;
        }

        public void Update()
        {
            //increace the road length until it reaches the edge of the screen, then set 'building' to false
            if ((vertical && length < Window.Height) || (!vertical && length < Window.Width)) {
                length += buildSpeed * Time.DeltaTime;
            } else {
                building = false;
            }

            //draw the road from one edge of the screen based on the direction and length
            if (direction == "up") {
                DrawRoad(Window.Height, Window.Height - length, width, lanes);

            } else if (direction == "down") {
                DrawRoad(0, length, width, lanes);

            } else if (direction == "left") {
                DrawRoad(Window.Width, Window.Width - length, width, lanes);

            } else {
                DrawRoad(0, length, width, lanes);
            }
        }

        //draws the road (made of lines) from a start point to an end point
        //uses position as either the x or y coodinate so it only needs the other
        void DrawRoad(float start, float end, float width, int lanes) {
            Vector2 point1;
            Vector2 point2;

            //use position as either x or y
            if (vertical) {
                point1 = new Vector2(position, start);
                point2 = new Vector2(position, end);
            } else {
                point1 = new Vector2(start, position);
                point2 = new Vector2(end, position);
            }

            //draw the road
            Draw.SetLineSize(width);
            Draw.SetLineColor(Color.LightGray);
            Draw.LineSharp(point1, point2);
            //center line
            Draw.SetLineSize(2);
            Draw.SetLineColor(Color.Yellow);
            Draw.LineSharp(point1, point2);
        }
    }
}
