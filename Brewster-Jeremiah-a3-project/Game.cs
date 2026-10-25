// Include the namespaces (code libraries) you need below.
using System;
using System.ComponentModel;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {
        Road[] roads = new Road[20];
        Vector2 startPos;

        public void Setup()
        {
            Window.SetSize(800, 600);
            Window.SetTitle("Buildable Roads");
        }

        public void Update()
        {
            Window.ClearBackground(Color.DarkGray);

            //update every existing road
            for (int i = 0; i < roads.Length; i++) {
                if (roads[i] != null) {
                    roads[i].Update();
                }
            }

            //build a road when the mouse is dragged
            if (Input.IsMouseButtonPressed(MouseButton.Left)) {
                startPos = Input.GetMousePosition(); //when first pressed, store the current mouse position
            }
            if (Input.IsMouseButtonDown(MouseButton.Left)) {
                Arrow(startPos, Input.GetMousePosition()); //as long as the mouse is held down, draw the arrow indicatior
            }
            if (Input.IsMouseButtonReleased(MouseButton.Left)) {
                //when realeased, create a road
                //use Arrow() to determine the direction of the new road
                string direction = Arrow(startPos, Input.GetMousePosition());
                for (int i = 0; i < roads.Length; i++) {
                    //create a new road at the first avalable index in the array
                    if (roads[i] == null) {
                        roads[i] = new Road(startPos, direction);
                        break;
                    }
                }
            }
        }

        //draws an arrow between 2 points, snapped to the nearest cardinal direction
        //returns that direction as a string
        string Arrow(Vector2 start, Vector2 end) {
            string direction;
            Vector2 endSnapped;
            Vector2 arrowPoint2;
            Vector2 arrowPoint3;
            float arrowSize = 10;
            float xDiff = end.X - start.X;
            float yDiff = end.Y - start.Y;

            //detemine the direction to snap to
            //based on if the xDiff or yDiff are bigger (absolute), and if that one is positive or negitive
            if (xDiff > 0 && MathF.Abs(xDiff) > MathF.Abs(yDiff)) {
                direction = "right";
                endSnapped = new Vector2(end.X, start.Y);
                //each direction needs to set the other arrow points by adding or subtracting from the coodinates of endSnapped appropriately
                arrowPoint2 = new Vector2(endSnapped.X - arrowSize, endSnapped.Y - arrowSize);
                arrowPoint3 = new Vector2(endSnapped.X - arrowSize, endSnapped.Y + arrowSize);

            } else if (xDiff < 0 && MathF.Abs(xDiff) > MathF.Abs(yDiff)) {
                direction = "left";
                endSnapped = new Vector2(end.X, start.Y);
                //each direction needs to set the other arrow points by adding or subtracting from the coodinates of endSnapped appropriately
                arrowPoint2 = new Vector2(endSnapped.X + arrowSize, endSnapped.Y - arrowSize);
                arrowPoint3 = new Vector2(endSnapped.X + arrowSize, endSnapped.Y + arrowSize);

            } else if (yDiff > 0 && MathF.Abs(yDiff) > MathF.Abs(xDiff)) {
                direction = "down";
                endSnapped = new Vector2(start.X, end.Y);
                //each direction needs to set the other arrow points by adding or subtracting from the coodinates of endSnapped appropriately
                arrowPoint2 = new Vector2(endSnapped.X - arrowSize, endSnapped.Y - arrowSize);
                arrowPoint3 = new Vector2(endSnapped.X + arrowSize, endSnapped.Y - arrowSize);

            } else { //if xDiff and yDiff are equal (unlikely), it will default to up
                direction = "up";
                endSnapped = new Vector2(start.X, end.Y);
                //each direction needs to set the other arrow points by adding or subtracting from the coodinates of endSnapped appropriately
                arrowPoint2 = new Vector2(endSnapped.X - arrowSize, endSnapped.Y + arrowSize);
                arrowPoint3 = new Vector2(endSnapped.X + arrowSize, endSnapped.Y + arrowSize);
            }

            //draw the arrow
            Draw.SetLineSize(5);
            Draw.SetLineColor(Color.Black);
            Draw.Line(start, endSnapped); //main line
            //extruding lines
            Draw.Line(endSnapped, arrowPoint2);
            Draw.Line(endSnapped, arrowPoint3);

            return direction;
        }
    }

}
