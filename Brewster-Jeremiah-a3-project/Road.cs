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
        public int numLanes = 2;
        public float speedLimit = 100f;
        float spawnRate = 5f;
        float spawnTimer;

        //an array for each lane of cars
        public Car[][] lanes = [new Car[20], new Car[20], new Car[20], new Car[20]];

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
            spawnTimer = spawnRate;
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
                DrawRoad(Window.Height, Window.Height - length, width, numLanes);

            } else if (direction == "down") {
                DrawRoad(0, length, width, numLanes);

            } else if (direction == "left") {
                DrawRoad(Window.Width, Window.Width - length, width, numLanes);

            } else {
                DrawRoad(0, length, width, numLanes);
            }


            //spawn cars after finnished building
            if (!building) {
                spawnTimer += Time.DeltaTime;

                if (spawnTimer >= spawnRate) {
                    //reset timer and spawn a car
                    spawnTimer = 0f;
                    SpawnCar();
                }
            }
        }

        //draws the road (made of lines) from a start point to an end point
        //uses position as either the x or y coodinate so it only needs the other
        void DrawRoad(float start, float end, float width, int numLanes) {
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

        //spawns a car in a random lane
        public void SpawnCar() {
            //choose a random lane for the car to spawn in
            int laneNum = Random.Integer(numLanes);
            Vector2 carPos;
            string carDirection;

            //each possible lane needs to be handled seperately so that the car has the right position and direction
            if (laneNum == 0) { //botton or right lane
                float carOffset = position + 6;
                if (vertical) {
                    carDirection = "up";
                    carPos = new Vector2(carOffset, Window.Height + 50);
                } else {
                    carDirection = "right";
                    carPos = new Vector2(-50f, carOffset);
                }
            } else if (laneNum == 1) { //top or left lane
                float carOffset = position - 6;
                if (vertical) {
                    carDirection = "down";
                    carPos = new Vector2(carOffset, -50f);
                } else {
                    carDirection = "left";
                    carPos = new Vector2(Window.Width + 50, carOffset);
                }
            } else if (laneNum == 2) { //second botton or right lane
                float carOffset = position + 6 + 25;
                if (vertical) {
                    carDirection = "up";
                    carPos = new Vector2(carOffset, Window.Height + 50);
                } else {
                    carDirection = "right";
                    carPos = new Vector2(-50f, carOffset);
                }
            } else{ //second top or left lane
                float carOffset = position - 6 - 25;
                if (vertical) {
                    carDirection = "down";
                    carPos = new Vector2(carOffset, -50f);
                } else {
                    carDirection = "left";
                    carPos = new Vector2(Window.Width + 50, carOffset);
                }
            }

            //create a new car at the first avalable index in the array
            for (int i = 0; i < lanes[laneNum].Length; i++) {
                if (lanes[laneNum][i] == null) {
                    lanes[laneNum][i] = new Car(this, carPos, carDirection);
                    break;
                }
            }
        }
    }
}
