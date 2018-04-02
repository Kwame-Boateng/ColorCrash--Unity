using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObstacleBarManager : MonoBehaviour 
{
	//DesignObstacle designObstacle;
	int randomIndex; 
	public GameObject [] tilePrefab;
	public GameObject [] EntryTilePrefab;
	public List <GameObject> Children = new List <GameObject> ();
	GameObject replacements;
	GameObject player;
	GameObject gameController;
	GameCtrl gameCtrl;
	PlayerController playerController; 
	CameraScript cameraScript;
	int enableCheck = 0; //This is to make sure that Obstacle design function doesnt run first time its created

	void Awake ()
	{
		//Get all the child objects (tiles) of the obstacle bar
		foreach (Transform child in transform) 
		{
			Children.Add (child.gameObject);
		}

		gameController = GameObject.FindWithTag ("GameController");
		gameCtrl = gameController.GetComponent <GameCtrl> ();

		cameraScript = GameObject.FindWithTag ("MainCamera").GetComponent <CameraScript> ();
	}

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent <PlayerController> ();

	}

	void OnEnable ()
	{
		enableCheck++;

		/*if (playerController == null) {
			player = GameObject.FindWithTag ("Player");
			playerController = player.GetComponent <PlayerController> ();
		}*/

		if (enableCheck > 1) {

			if (playerController == null) {
				player = GameObject.FindWithTag ("Player");
				playerController = player.GetComponent <PlayerController> ();
			}
			ObstacleDesign ();
		}
	}




	void ShuffleChildObjects ()
	{
		for (int index = 0; index < Children.Count; index++)
		{
			randomIndex = Random.Range (0, Children.Count);
			Vector2 tempPosition = Children[index].transform.position;
			Children[index].transform.position = Children[randomIndex].transform.position;
			Children[randomIndex].transform.position = tempPosition;
		}
	}

	void Replacetile (GameObject [] prefabs, bool matchPlayerColor)
	{
		int childrenIndex;
		childrenIndex = Random.Range (0, Children.Count);
		ObstacleProperties obstaclePropeties = Children [childrenIndex].GetComponent <ObstacleProperties> ();
		if (playerController.playerColor != obstaclePropeties.tileColor)
		{
			Vector2 tempPosition = Children[childrenIndex].transform.position;
			replacements = (GameObject)Instantiate (ChoosePrefab (prefabs, matchPlayerColor), 
				tempPosition, Children[childrenIndex].transform.rotation);
			replacements.transform.parent = Children [childrenIndex].transform.parent;
			Destroy (Children[childrenIndex].gameObject);
			Children.RemoveAt (childrenIndex);
			Children.Add (replacements);
		}
	}

	GameObject ChoosePrefab (GameObject [] prefabs, bool matchPlayerColor)
	{
		int randomIndex;
		GameObject chosenPrefab = null;

		if (matchPlayerColor == true) {
			foreach (GameObject prefab in prefabs) {
				ObstacleProperties newObstacleProperties = prefab.GetComponent <ObstacleProperties> ();
				if (newObstacleProperties.tileColor == playerController.playerColor) {
					chosenPrefab = prefab;
					break;
				}
			}
		} else {
			randomIndex = Random.Range (0, prefabs.Length);
			if (gameCtrl.gameLevel == GameLevel.Hard && prefabs.Length < 3) 
			{
				chosenPrefab = prefabs [0];
			} 
			else
			{
				chosenPrefab = prefabs [randomIndex];
			}
		}
		return chosenPrefab;
	}

	void EasyObstacleDesign ()
	{
		int randomNum;
		randomNum = Random.Range (1, 4);
		if (randomNum == 3) {
			ShuffleChildObjects ();
		} else if (randomNum < 3) 
		{
			Replacetile (tilePrefab, false);
			ShuffleChildObjects();
		}
	}

	void ModerateObstacleDesign ()
	{
		int randomNum;
		randomNum = Random.Range (1, 5);
		if (randomNum == 4) 
		{
			EasyObstacleDesign ();
		} else if (randomNum < 4) 
		{
			for (int index = 1; index <= 2; index++)
			{
				if (index == 1)
				{
					Replacetile (tilePrefab, false);
				} else if (index == 2) 
				{
					Replacetile (tilePrefab, true);
				}
			}
			ShuffleChildObjects ();
		}
	}

	void HardObstacleDesign ()
	{
		int randomNum;
		randomNum = Random.Range (1, 5);
		if (randomNum >= 3) 
		{
			ModerateObstacleDesign ();
		} else if (randomNum < 3) 
		{
			for (int index = 1; index <= 3; index++)
			{
				if (index == 1) 
				{
					Replacetile (tilePrefab, false);
				} else if (index == 2) 
				{
					Replacetile (tilePrefab, true);
				} else if (index == 3)
				{
					Replacetile (EntryTilePrefab, false);
				}
			}
			ShuffleChildObjects ();
		}
	}


	void SetSize ()
	{
		switch (cameraScript.deviceAspect) 
		{
		case DeviceAspect.NewIphone:
			transform.localScale = new Vector2 (1.0f, 1.0f);
			break;
		case DeviceAspect.OldIphone:
			transform.localScale = new Vector2 (1.19f, 1.0f);
			break;
		case DeviceAspect.Ipad:
			transform.localScale = new Vector2 (1.34f, 1.0f);
			break;
		case DeviceAspect.Android10by16:
			transform.localScale = new Vector2 (1.11f, 1.0f);
			break;
		case DeviceAspect.Android3by5:
			transform.localScale = new Vector2 (1.07f, 1.0f);
			break;
		}
	}

	void ObstacleDesign ()
	{
		//gameCtrl = gameController.GetComponent <GameCtrl> ();
		switch (gameCtrl.gameLevel)
		{
		case GameLevel.Beginning:
			ShuffleChildObjects ();
			break;
		case GameLevel.Easy:
			EasyObstacleDesign ();
			break;
		case GameLevel.Moderate:
			ModerateObstacleDesign ();
			break;
		case GameLevel.Hard:
		case GameLevel.ExtremelyHard:
			HardObstacleDesign ();
			break;
		}

		SetSize ();
	}
}
