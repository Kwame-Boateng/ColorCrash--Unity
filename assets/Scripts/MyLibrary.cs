using UnityEngine;
using System.Collections;

//Enumeration of colors of game objects
public enum ObjectColor 
{
	Red = 1, 
	Pinkish, 
	Purple, 
	Blue,
	Neutral
}

//Enumerations of gameplay levels
public enum GameLevel
{
	Beginning,
	Easy,
	Moderate,
	Hard, 
	ExtremelyHard
}

//Enumeration of Obstacle types
public enum ObstacleTypes
{
	MainObstacle,
	SpikeObstacle,
	ObstacleEntry,
	DisguisedEntry,
	LevelChecker
}

//Enumeration Device types
public enum DeviceAspect
{
	OldIphone,
	NewIphone,
	Ipad,
	Android3by5,
	Android10by16
}


public class MyPersonalLibrary
{

	//Returns a random parameter
	public T RandomPicker <T> (params T [] args)
	{
		T returnValue;
		int randomIndex;
		randomIndex = Random.Range (0, args.Length);
		returnValue = args [randomIndex];
		return returnValue; 
	}
}

