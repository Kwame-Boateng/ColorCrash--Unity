using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//Public Variables: ********************************************************************************
	public Sprite[] playerSprites = new Sprite[4]; //Array of sprites to change player colors
	public ObjectColor playerColor; //Enumeration of colors (Red, Pinkish, Purple, Blue)
	public float gameSpeed; //The moving speed of the player
	public float initialSpeed; //The initial speed of player at game start which is 0


	//Private variables: *********************************************************************************
	GameObject gameController; //GameController gameObject to access its script.
	GameCtrl gameCtrl; //Reference to GameController script
	SpriteRenderer sr; //Reference to Sprite Renderer
	Rigidbody2D rb; //Reference to RigidBody
	MyPersonalLibrary myPersonalLibrary; // My own personal library which contains functions and enums.
	Vector2 startPosition, sidePosition; //Player start position and x position
	Vector2 playerVelocity;
	int spriteIndex; //Random number to access sprites.
	float startYPosition = -5.9f; //Initial Y position of the player object
	public float sideDistance;
	CameraScript cameraScript;



	void Awake ()
	{
		myPersonalLibrary = new MyPersonalLibrary ();
		sr = GetComponent <SpriteRenderer> ();
		rb = GetComponent <Rigidbody2D> ();
		gameController = GameObject.FindWithTag ("GameController");
		gameCtrl = gameController.GetComponent <GameCtrl> ();
		cameraScript = GameObject.FindWithTag ("MainCamera").GetComponent <CameraScript> ();
	}

	void Start ()
	{
		SetStartPosition ();
		this.transform.position = startPosition; //Set the position of the player at game start
		SetOrChangeSprite (); //Set player color at beginning of game
		sidePosition = rb.position; //Set SidePosition of player at game start
		gameSpeed = 6.0f; //Set gamespeed to 0 at game start so player object do not move 
		initialSpeed = gameSpeed;
		cameraScript.SetPlayer ();
	}

	// Update is called once per frame
	void Update ()
	{
		MovePlayer ();
		moveLeftRight ();
	}

	/*void FixedUpdate ()
	{
		MovePlayer ();
	}*/

	//Set or change the sprites of the player game object at runtime
	public void SetOrChangeSprite ()
	{
		spriteIndex = Random.Range (0, playerSprites.Length);
		sr.sprite = playerSprites [spriteIndex];
		SetColor ();
	}

	//Set the color of the player game object to an enum type
	void SetColor ()
	{
		if (sr.sprite == playerSprites [0])
		{
			playerColor = ObjectColor.Red;
		}
		else if (sr.sprite == playerSprites [1])
		{
			playerColor = ObjectColor.Pinkish;
		}
		else if (sr.sprite == playerSprites [2])
		{
			playerColor = ObjectColor.Purple;
		}
		else if (sr.sprite == playerSprites [3])
		{
			playerColor = ObjectColor.Blue;
		}

	}

	//Function to move the player gameobject forward
	void MovePlayer ()
	{
		playerVelocity = new Vector2 (rb.velocity.x, 1.0f);
		transform.Translate (playerVelocity * gameSpeed * Time.deltaTime);
		//rb.velocity = playerVelocity * gameSpeed; //Might test later
	}

	//Funtion to move player game side by side
	void moveLeftRight ()
	{
		Vector2 sideTarget = rb.position;


		if (Input.GetKeyDown (KeyCode.LeftArrow) || Swipe.swipe == Swipe.SwipeDirection.left || ScreenSideTouch.side == ScreenSideTouch.SideTouched.leftSide) 
		{
			Swipe.ResetSwipe ();
			if (sidePosition.x > -2)
				sidePosition.x -= sideDistance;
		}
		else if (Input.GetKeyDown (KeyCode.RightArrow) || Swipe.swipe == Swipe.SwipeDirection.right || ScreenSideTouch.side == ScreenSideTouch.SideTouched.rightSide) 
		{
			ScreenSideTouch.ResetTouch ();
			Swipe.ResetSwipe ();
			if (sidePosition.x < 2)
				sidePosition.x += sideDistance;
		} 

		sideTarget.x = Mathf.MoveTowards (sideTarget.x, sidePosition.x, Time.deltaTime * (gameSpeed + 3.5f));
		rb.position = sideTarget; 
	}


	//Set Player speed depending on gameLevel
	public void SetGameSpeed ()
	{
		switch (gameCtrl.gameLevel) 
		{
		case GameLevel.Moderate:
			gameSpeed = (initialSpeed + 0.5f);
			break;
		case GameLevel.Hard:
			gameSpeed = (initialSpeed + 1.0f);
			break;
		case GameLevel.ExtremelyHard:
			gameSpeed = (initialSpeed + 1.5f);
			break;
		}
	}


	void SetStartPosition ()
	{
		switch (cameraScript.deviceAspect) 
		{
		case DeviceAspect.NewIphone:
			startPosition = new Vector2 (myPersonalLibrary.RandomPicker <float> (-0.7f, 0.7f), startYPosition);
			break;
		case DeviceAspect.OldIphone:
			startPosition = new Vector2 (myPersonalLibrary.RandomPicker <float> (-0.84f, 0.84f), startYPosition);
			break;
		case DeviceAspect.Ipad:
			startPosition = new Vector2 (myPersonalLibrary.RandomPicker <float> (-0.95f, 0.95f), startYPosition);
			break;
		case DeviceAspect.Android10by16:
			startPosition = new Vector2 (myPersonalLibrary.RandomPicker <float> (-0.78f, 0.78f), startYPosition);
			break;
		case DeviceAspect.Android3by5:
			startPosition = new Vector2 (myPersonalLibrary.RandomPicker <float> (-0.75f, 0.75f), startYPosition);
			break;
		}
	}

}


