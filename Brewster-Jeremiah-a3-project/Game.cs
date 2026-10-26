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
        Intersection[] intersections = new Intersection[60];
        Vector2 mouseStartPos;
        Road roadRef;

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

            //update every existing intersection
            for (int i = 0; i < intersections.Length; i++) {
                if (intersections[i] != null) {
                    intersections[i].Update();
                }
            }

            //update every existing car
            //must be after intersections so that they are drawn on top
            for (int i = 0; i < roads.Length; i++) { //for each road
                if (roads[i] != null) {
                    for (int j = 0; j < roads[i].numLanes; j++) { //for each lane
                        for (int k = 0; k < roads[i].lanes[j].Length; k++) { //for each car
                            Car carRef = roads[i].lanes[j][k];
                            if (carRef != null) {
                                carRef.Update();
                                //if the car is offscreen, delete it
                                if (carRef.position.X > Window.Width + 50 || carRef.position.X < -50 || carRef.position.Y > Window.Height + 50 || carRef.position.Y < -50) {
                                    carRef = null;
                                }
                            }
                        }
                    }
                }
            }

            //build a road when the mouse is dragged
            if (Input.IsMouseButtonPressed(MouseButton.Left)) {
                mouseStartPos = Input.GetMousePosition(); //when first pressed, store the current mouse position
            }
            if (Input.IsMouseButtonDown(MouseButton.Left)) {
                Arrow(mouseStartPos, Input.GetMousePosition()); //as long as the mouse is held down, draw the arrow indicatior
            }
            if (Input.IsMouseButtonReleased(MouseButton.Left)) {
                //when realeased, create a road
                //use Arrow() to determine the direction of the new road
                string direction = Arrow(mouseStartPos, Input.GetMousePosition());
                for (int i = 0; i < roads.Length; i++) {
                    //create a new road at the first avalable index in the array
                    if (roads[i] == null) {
                        roads[i] = new Road(mouseStartPos, direction);
                        roadRef = roads[i];
                        break;
                    }
                }

                //create intersections if nessisary
                for (int i = 0; i < roads.Length; i++) {
                    if (roads[i] != null) {
                        if (roads[i].vertical != roadRef.vertical) { //check if they are perpindicular
                            for (int j = 0; j < intersections.Length; j++) {
                                //create a new intersection at the first avalable index in the array
                                if (intersections[j] == null) {
                                    intersections[j] = new Intersection(roads[i], roadRef);
                                    break;
                                }
                            }
                        }
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
