using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
#endif


public class GameCtrl : MonoBehaviour 
{
	public GameObject [] playerPrefab; 
	public GameObject playerObject;
	public GameObject currentTile;
	public GameObject playerExplosion;
	public GameObject obstacleBarPrefab, LevelEntryPrefab;
	public Sprite[] soundBtnImage = new Sprite[2];
	public int gameMuted;
	float vertDistanceBtnTiles = 6.8f;
	public int prefabIndex; //Index for playerPrefab array
	Vector2 tilePosition;
	public GameLevel gameLevel;
	public int numOfEntryTriggered;
	public int score, bestScore, gameRated;
	public Text mainMenuBestScoreText, InGameScoreText, PDScoreText;
	public Button startButton, soundButton;
	public List <Button> ShopButtons = new List <Button> (); //Buttons for shopitems
	public List <GameObject> ObstacleBarPool = new List <GameObject> (); //List of pre created obstacle Bars
	public List <GameObject> LevelEntryPool = new List <GameObject> (); //List of pre created LevelEntry Bars
	bool newHighScore = false;



	// Use this for initialization
	void Awake () 
	{
		//PlayerPrefs.DeleteAll ();
		prefabIndex = PlayerPrefs.GetInt ("prefabIndex", 0);
		SetGameLevel ();
		gameRated = PlayerPrefs.GetInt ("gameRated", 0);
		gameMuted = PlayerPrefs.GetInt ("gameMuted", 0);
		bestScore = PlayerPrefs.GetInt ("bestScore");
		CreateObjects (true);

		#if UNITY_ANDROID
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();
		#endif

		//if the device is Iphone then authenticate user for IOS GameCenter.
		#if UNITY_IOS /////////////////////////////////////////////////////////////////////
		Social.localUser.Authenticate (success =>
		{
		Debug.Log (success ? "GameCenter Authentication Successful" : "GameCenter Authentication failed");
		}
		);	
		#endif /////////////////////////////////////////////////////////////////////////////
	}


	void Start ()
	{
		//Set various variable values *****************************************************
		score = 0;
		SetSoundButton ();
		UpdateScoresText ();
		//*********************************************************************************

		//if Device is Android Sign in to GooglePlay Services
		#if UNITY_ANDROID
		Social.localUser.Authenticate ((bool success) =>
		{
		if (success) {
		Debug.Log ("GooglePlay Authentication Successful");
		} else {
		Debug.Log ("GooglePlay Authentication failed");
		}
		});
		#endif

	} 


	//Function to set the game Level as player progresses in game
	public void SetGameLevel ()
	{
		if (numOfEntryTriggered <= 1) 
		{
			gameLevel = GameLevel.Beginning;
		}
		else if (numOfEntryTriggered == 2) 
		{
			gameLevel = GameLevel.Easy;
		}
		else if (numOfEntryTriggered > 2 && numOfEntryTriggered <= 5) 
		{
			gameLevel = GameLevel.Moderate;
		}
		else if (numOfEntryTriggered > 5 && numOfEntryTriggered <= 8) 
		{
			gameLevel = GameLevel.Hard;
		}
		else if (numOfEntryTriggered > 8) 
		{
			gameLevel = GameLevel.ExtremelyHard;
		}
	}


	//Spawn the PlayerPrefab gameobject
	public void SpawnPlayerPrefab ()
	{
		playerObject = (GameObject)Instantiate (playerPrefab [prefabIndex]);
	}


	//Function to set the best score after player dies
	public void SetBestScore ()
	{
		if (score > bestScore) 
		{
			bestScore = score;
			PlayerPrefs.SetInt ("bestScore", bestScore);
			ReportScoreToGameCenter (bestScore);
			newHighScore = true;
		}
	}


	//Function to change scene.
	public void RestartScene ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}


	//Function to set the Text value of score boards
	public void UpdateScoresText ()
	{
		//Set the text value for mainMenuBestScoreText
		mainMenuBestScoreText.text = "BEST SCORE " + bestScore.ToString ();
		
		//Set the Text value for InGameScoreText
		InGameScoreText.text = score.ToString ();
		
		//Set the Text Value for PDScoreText
		if (newHighScore == true) {
			PDScoreText.text = score.ToString () + "\nNEW BEST SCORE";
			newHighScore = false;
		} else {
			PDScoreText.text = "SCORE " + score.ToString () + "\nBEST " + bestScore.ToString ();
		}
	}


	public void SetSoundButton ()
	{
		if (gameMuted == 0) {
			soundButton.GetComponent <Image> ().sprite = soundBtnImage [0];
		} else if (gameMuted == 1) {
			soundButton.GetComponent <Image> ().sprite = soundBtnImage [1];
		}

	}


	//function to report score to IOS GameCenter.
	public void ReportScoreToGameCenter (long scoreToReport)
	{
		#if UNITY_IOS
		Debug.Log ("Score Reported Successfully");
		Social.ReportScore (scoreToReport, "CCBESTSCORE1", success => {
			Debug.Log (success ? "Score Reported Successfully" : "Score Report failed");
		}
		);
		#elif UNITY_ANDROID
		if (Social.localUser.authenticated) {
		Social.ReportScore (scoreToReport, "CgkI-J6Dy8MDEAIQAA", (bool success) =>
		{
		if (success) {
		Debug.Log ("Score Reported Successfully");

		} else {
		Debug.Log ("Score Report failed");
		}
		});
		}
		#endif
	}


	//Set the button feature of items in the Shop Menu
	public void SetAllShopItemFeatures ()
	{
		for (int count = 0; count < ShopButtons.Count; count++) 
		{
			ShopButtons [count].GetComponent <ShopItemManager> ().SetButtonFeatures ();
		}
	}

	public string GetSelectedShopItemName ()
	{
		string name = "";
		for (int count = 0; count < ShopButtons.Count; count++) 
		{
			ShopItemManager shopIM = ShopButtons [count].GetComponent <ShopItemManager> ();
			if (shopIM.IsSelected ()) {
				name = shopIM.itemName;
				break;
			}
		}

		return name;
	}


	//Create a list of object pool for obstacle bars
	public void CreateObjects (bool createBothTiles)
	{
		int ObstacleAmount = 99;
		int EntryLevelAmount = 4;

		//Create Obstacle Bars
		for (int i = 0; i < ObstacleAmount; i++) 
		{
			GameObject obstacleBar = (GameObject)Instantiate (obstacleBarPrefab);
			obstacleBar.SetActive (false);
			ObstacleBarPool.Add (obstacleBar);
		}

		if (createBothTiles == true) {
			//Create LevelEntry Bars
			for (int i = 0; i < EntryLevelAmount; i++) 
			{
				GameObject LevelEntry = (GameObject)Instantiate (LevelEntryPrefab);
				LevelEntry.SetActive (false);
				LevelEntryPool.Add (LevelEntry);
			}

		}

	}



	//Function to take gameObjects out of pool and spawn them on screen
	public void SpawnBars ()
	{	
		int numOfTilesToSpawn = 10;
		float xPos = 0.0f;
		GameObject temp = null;


		for (int i = 1; i <= numOfTilesToSpawn; i++) 
		{
			if ((i % 10) == 0 && i != 0) {
				temp = LevelEntryPool [0];
				LevelEntryPool.RemoveAt (0);
				temp.SetActive (true);
				temp.transform.position = new Vector2 (xPos, (currentTile.transform.position.y + vertDistanceBtnTiles));
				currentTile = temp;
			} else {
				temp = ObstacleBarPool [0];
				ObstacleBarPool.RemoveAt (0);
				temp.SetActive (true);
				temp.transform.position = new Vector2 (xPos, (currentTile.transform.position.y + vertDistanceBtnTiles));
				currentTile = temp;
			}

		}

		if (ObstacleBarPool.Count < 9) {
			CreateObjects (false);
		}

	}
		
}
