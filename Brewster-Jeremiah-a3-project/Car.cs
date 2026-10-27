using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MohawkGame2D
{
    public class Car
    {
        public Vector2 position;
        float length = 35f;
        float width = 14f;
        string direction;
        float speed;
        float acceleration = 50f;
        float gap = 10f;
        public Road road;
        Color carColor;
        Color[] colorChoices = [Color.Black, Color.Blue, Color.Cyan, Color.Green, Color.Magenta, Color.Red, Color.White, Color.Yellow];

        public Car(Road road, Vector2 position, string direction)
        {
            this.road = road;
            this.position = position;
            this.direction = direction;

            this.speed = road.speedLimit;

            //pick a random color from selected choices
            carColor = colorChoices[Random.Integer(colorChoices.Length)];
        }

        public void Update()
        {
            //move the car
            if (direction == "up") {
                position.Y -= speed * Time.DeltaTime;
            } else if (direction == "down") {
                position.Y += speed * Time.DeltaTime;
            } else if (direction == "left") {
                position.X -= speed * Time.DeltaTime;
            } else {
                position.X += speed * Time.DeltaTime;
            }
            
            DrawCar();
        }

        //draws the car in the correct orientation
        //origin is the back left corner
        void DrawCar() {
            Vector2 point0 = position; //back left
            Vector2 point1; //front left
            Vector2 point2; //front right
            Vector2 point3; //back right

            //determine the correct points based on the cars position
            if (direction == "up") {
                point1 = new Vector2(point0.X, point0.Y - length);
                point2 = new Vector2(point0.X + width, point0.Y - length);
                point3 = new Vector2(point0.X + width, point0.Y);
            } else if (direction == "right") {
                point1 = new Vector2(point0.X + length, point0.Y);
                point2 = new Vector2(point0.X + length, point0.Y + width);
                point3 = new Vector2(point0.X, point0.Y + width);
            } else if (direction == "down") {
                point1 = new Vector2(point0.X, point0.Y + length);
                point2 = new Vector2(point0.X - width, point0.Y + length);
                point3 = new Vector2(point0.X - width, point0.Y);
            } else {
                point1 = new Vector2(point0.X - length, point0.Y);
                point2 = new Vector2(point0.X - length, point0.Y - width);
                point3 = new Vector2(point0.X, point0.Y - width);
            }

            Draw.SetLineSize(0);
            Draw.SetFillColor(carColor);
            Draw.Quad(point0, point1, point2, point3);
        }
    }
}
