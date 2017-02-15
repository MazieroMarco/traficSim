using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {

	// Hand modificable global variables
	public static int INT_ROAD_SIZE 		 		  = 30;  			   // The size / 10 of the road
	public static int INT_NB_ROADS  		 		  = 3;   			   // Number of roads
	public static int INT_SPEED_LIMIT_KMH	 		  = 120; 			   // All cars speed limit in KM/H
	public static int INT_OUTPUT_REFRESH			  = 5;				   // The output text will refresh every x seconds
	public static float FLT_CARS_DENSITY_SEC 		  = 0.2f;			   // The number of seconds before car spawn
	public static float FLT_DRIVERS_SPEED_FACTOR_KMH  = 10;	    		   // The random speed factor for each driver
	public static float FLT_AHEAD_CAR_DETECTION_DIST  = 2f;				   // Distance for the ahead cars detection
	public static float FLT_DRIVER_SAFETY_DIST		  = 0.2f;			   // The distance when the driver starts to slow down (HAS TO BE LOWER THAN THE DETECTION DISTANCE)
	public static float FLT_DRIVER_ACCELERATION_SPEED = 0.03f;			   // The acceleration speed when the driver speeds up the car
	public static float FLT_DRIVER_DECELERATION_SPEED = 0.08f;			   // The deceleration speed when the driver slows down the car
	public static float FLT_SECURITY_DIST_CHANGE_LANE = 4f;			 	   // The security distance to verify on the next lane before changing
	public static int INT_GRAPHS_NB_DATA			  = 31;				   // The amout of data to display in the graphs


	// Non hand modificable global variables
	public static int INT_SPEED_LIMIT_KMH_TRUCK	 	  = 80; 			   // All cars speed limit in KM/H
	public static int INT_CARS_OUTPUT_LEFT 			  = 0;				   // The amount of cars passing on the left roads
	public static int INT_CARS_OUTPUT_RIGHT 		  = 0;				   // The amount of cars passing on the right roads
	public static List<Road> LI_GAME_ROADS			  = new List<Road>();  // Contains all the roads
	public static bool BLN_IS_INTERFACE_ACTIVE		  = false;			   // Used to know if the interface is activated or not
	public static float FLT_TIME_OF_DAY				  = 0;				   // The time of the day (from 0.0 to 1.0)
	public static List<float> LI_LEFT_OUTPUTS		  = new List<float>(); // Contains all the outputs averages during the last 30 seconds
	public static List<float> LI_RIGHT_OUTPUTS		  = new List<float>(); // Contains all the outputs averages during the last 30 seconds
	public static bool BLN_ACTIVATE_SHOW_GRAPHS		  = true;			   // When true the user can see the output graphs

	// Cheats variables
	public static bool BLN_CAR_CONTROL				  = false;			   // The cheat to control a car
}
