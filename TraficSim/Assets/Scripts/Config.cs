using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {

	// Hand modificable Global variables declaration
	public static int INT_ROAD_SIZE 		 		  = 50;  	// The size / 10 of the road
	public static int INT_NB_ROADS  		 		  = 3;   	// Number of roads
	public static int INT_SPEED_LIMIT_KMH	 		  = 120; 	// All cars speed limit in KM/H
	public static float FLT_CARS_DENSITY_SEC 		  = 0.02f;	// The number of seconds before car spawn
	public static float FLT_DRIVERS_SPEED_FACTOR	  = 0.4f;	// The random speed factor for each driver
	public static float FLT_AHEAD_CAR_DETECTION_DIST  = 0.5f;	// Distance for the ahead cars detection
	public static float FLT_DRIVER_SAFETY_DIST		  = 0.2f;	// The distance when the driver starts to slow down (HAS TO BE LOWER THAN THE DETECTION DISTANCE)
	public static float FLT_DRIVER_ACCELERATION_SPEED = 0.02f;	// The acceleration speed when the driver speeds up the car
	public static float FLT_DRIVER_DECELERATION_SPEED = 0.06f;	// The deceleration speed when the driver slows down the car
	public static int INT_OUTPUT_REFRESH			  = 5;		// The output text will refresh every x seconds

	// Non hand modificable global variables
	public static int INT_CARS_OUTPUT = 0;						// The amount of cars passing on the road
}
