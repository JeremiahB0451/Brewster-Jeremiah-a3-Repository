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

        //references to all intersections on this road
        public Intersection[] intersections = new Intersection[10];

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
                //if upgrading: show the smaller road still underneath
                if (numLanes == 4) { 
                    if (vertical) {
                        DrawRoad(0, Window.Height, width/2, 2);
                    } else { 
                        DrawRoad(0, Window.Width, width/2, 2);
                    }
                }
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
                    //random variation relitive to spawn rate
                    float variation = spawnRate * 0.5f;
                    spawnTimer += Random.Float(-variation, variation);
                    SpawnCar();
                }
            }

            //check if each car should stop
            for (int i = 0; i < lanes.Length; i++) { //every lane
                for (int j = 0; j < lanes[i].Length; j++) { //every car
                    Car carRef1 = lanes[i][j];
                    bool stopped = false;
                    if (carRef1 != null) { 
                        for (int k = 0; k < lanes[i].Length; k++) { //every other car
                            Car carRef2 = lanes[i][k];
                            if (carRef2 != null && carRef2 != carRef1) {
                                //get the x and y differences in their positions at once; only need to use one at a time
                                Vector2 diff = carRef2.position - carRef1.position;
                                if (carRef1.direction == "down" && diff.Y < carRef1.StopDistance() && diff.Y > 0) {
                                    carRef1.Stop = true;
                                    stopped = true;
                                    break;
                                } else if (carRef1.direction == "up" && diff.Y > -carRef1.StopDistance() && diff.Y < 0) {
                                    carRef1.Stop = true;
                                    stopped = true;
                                    break;
                                } else if (carRef1.direction == "left" && diff.X > -carRef1.StopDistance() && diff.X < 0) {
                                    carRef1.Stop = true;
                                    stopped = true;
                                    break;
                                } else if (carRef1.direction == "right" && diff.X < carRef1.StopDistance() && diff.X > 0) {
                                    carRef1.Stop = true;
                                    stopped = true;
                                    break;
                                } else {
                                    carRef1.Stop = false;
                                }
                            }
                        }
                        //go to the next car to prevent the next section from starting this car if it was stopped
                        if (stopped) continue;
                        for (int k = 0; k < intersections.Length; k++) { //every intersection on this road
                            Intersection intersectionRef = intersections[k];
                            if (intersectionRef != null && intersectionRef.greenVert != vertical) { //only stop cars on the road where the light is red
                                //get the x and y differences in their positions at once; only need to use one at a time
                                Vector2 diff = intersectionRef.position - carRef1.position;
                                if (carRef1.direction == "down" && diff.Y - intersectionRef.height/2 <= carRef1.StopDistance() && diff.Y >= intersectionRef.height/2 && diff.Y > 0) {
                                    carRef1.Stop = true;
                                    break;
                                } else if (carRef1.direction == "up" && diff.Y + intersectionRef.height/2 >= -carRef1.StopDistance() && diff.Y >= intersectionRef.height/2 && diff.Y < 0) {
                                    carRef1.Stop = true;
                                    break;
                                } else if (carRef1.direction == "left" && diff.X + intersectionRef.width/2 >= -carRef1.StopDistance() && diff.X >= intersectionRef.width/2 && diff.X < 0) {
                                    carRef1.Stop = true;
                                    break;
                                } else if (carRef1.direction == "right" && diff.X - intersectionRef.width/2 <= carRef1.StopDistance() && diff.X >= intersectionRef.width/2 && diff.X > 0) {
                                    carRef1.Stop = true;
                                    break;
                                } else {
                                    carRef1.Stop = false;
                                }
                            }
                        }
                    }
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

            //draw dashed lane lines if nessisary
            if (numLanes == 4) {
                Draw.SetLineSize(2);
                Draw.SetLineColor(Color.White);
                float lineLength = 20f;
                //2 sets of lines
                for (int j = -1; j < 2; j+=2) { //j will only be -1 then 1; used to create 2 copies of the dashes on either side of the middle
                    for (int i = (int)(lineLength/2); i < length - lineLength; i+=(int)(lineLength*2)) { //starts half a line from the edge; sepertates each line by a line length
                        //MUST be seperate for each direction
                        if (direction == "down") {
                            Draw.LineSharp(point1.X + j * width / 4, point1.Y + i, point1.X + j * width / 4, point1.Y + i + lineLength);
                        } else if (direction == "up") {
                            Draw.LineSharp(point1.X + j * width / 4, point1.Y - i, point1.X + j * width / 4, point1.Y - i - lineLength);
                        } else if (direction == "right") {
                            Draw.LineSharp(point1.X + i, point1.Y + j * width / 4, point1.X + i + lineLength, point1.Y + j * width / 4);
                        } else {
                            Draw.LineSharp(point1.X - i, point1.Y + j * width / 4, point1.X - i - lineLength, point1.Y + j * width / 4);
                        }
                    }
                }
            }
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

        //changes the road from 2 lanes to 4 lanes
        public void Upgrade() { 
            if (numLanes == 2 && !building) { //don't do anything if it has already been upgraded or currectly building
                numLanes = 4;
                width *= 2;
                //should result in a somewhat buiser road
                speedLimit *= 1.5f;
                spawnRate /= 3.5f;
                //set the paramiters used for the building animation
                building = true;
                length = 0;
            }
        }
    }
}
