using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSideTouch : MonoBehaviour 
{

	private float screenCenterX; //Horizontal center position of screen in use

	public enum SideTouched
	{
		none,
		leftSide,
		rightSide
	}

	public static SideTouched side;

	// Use this for initialization
	void Start () {
		side = SideTouched.none; //set touch to none
		screenCenterX = Screen.width * 0.5f; //save horizontal center position of screen
	}

	public static void ResetTouch ()
	{
		side = SideTouched.none;
	}
	
	// Update is called once per frame
	void Update () {

		//if there are any touches currently
		if (Input.touchCount > 0) {

			//get first touch
			Touch firstTouch = Input.GetTouch (0);

			//if a touch began in this frame 
			if (firstTouch.phase == TouchPhase.Began) {

				if (firstTouch.position.x > screenCenterX) {
					//move right
					side = SideTouched.rightSide;
				} else {
					//move left
					side = SideTouched.leftSide;
				}
					
			}
				
		}
	}
}
