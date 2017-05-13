using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;
using UnityEngine.SocialPlatforms;


public class GUIEventFunctions : MonoBehaviour 
{

	GameObject gameController; //Game Controller Object to access its class and functions
	public Canvas userInterface; //Game Object containing all UI Canvas to be able to animate them
	GameCtrl gameCtrl; //A class within the gameController gameObject to access its functions
	public int isShared; //To determine weather game has been shared by user to Facebook
	public int gamesPlayed; //Checks number of times user has played game
	public int removeAds; //Checks if ads is purchased.
	public Animator userInfaceAnimator; //Animator component on userInterface Gameobject
	//GameObject thePlayer;
	//PlayerController playerController;
	//float initialGameSpeed = 6.0f;
	public Text gameOverText, ShopItemMessage;
	public AudioSource gameMusic;

	//List of intergers for transition states **************************************
	int mainToGameInstruction = 1; int gameInstructionToMain = 2; int enterShop = 3;//
	int mainToGamePlay = 4; public int playerDead = 5; int exitShop = 6;                  
	//****************************************************************************//

	void Awake () 
	{
		//Initialize Facebook SDK ***********************************************
		/*if (!FB.IsInitialized) {
			FB.Init (InitCallback);
		} else {
			//If already initialized then signal an app activation App Event
			FB.ActivateApp ();
		}*/
		//***********************************************************************



		//Set Reference to the gameCtrl class ***********************************
		gameController = GameObject.FindWithTag ("GameController");
		gameCtrl = gameController.GetComponent <GameCtrl> ();
		//***********************************************************************
		 
		removeAds = PlayerPrefs.GetInt ("removeAds", 0);
		isShared = PlayerPrefs.GetInt ("isShared", 0);
		gamesPlayed = PlayerPrefs.GetInt ("gamesPlayed", 1);

		userInfaceAnimator = userInterface.GetComponent <Animator> (); //Reference to userInterface Animator Component
	}


	void Start ()
	{
		//thePlayer = GameObject.FindWithTag ("Player");
		//playerController = thePlayer.GetComponent <PlayerController> ();
		ShopItemMessage.text = gameCtrl.GetSelectedShopItemName();
		SetGameOverText ();
		SetSound ();
		gameMusic = GameObject.FindWithTag ("MainCamera").GetComponent <AudioSource> ();

		if (gamesPlayed == 1) {
			userInfaceAnimator.SetInteger ("Transition", mainToGameInstruction);
		}
	}

	/*//Function to callback after Init function
	void InitCallback ()
	{
		if (FB.IsInitialized) {
			//Signal an app activation App Event
			FB.ActivateApp ();
		} else {
			Debug.Log ("Failed to initialized the Facebook SDK");
		}
	}*/


	//Function to share game on Facebook
	/*public void FacebookShare ()
	{
		FB.ShareLink
		(new System.Uri ("https://itunes.apple.com/us/app/color-tile-challenge/id1185115591?mt=8"), 
			"Think You Can Hit 90 In This Game? Download For Free To Try #COLORTILECHALLENGE", 
			"Think you can you score 90 in the New Color Tile Challenge game?" +
			"\n Download to try it. #ColorTileChallenge #CHALLENGECTC90", null,
			ShareCallback
		);
	}


	//Called after the ShareLink function in FacebookShare. 
	void ShareCallback (IShareResult result)
	{
		if (result.Cancelled || !string.IsNullOrEmpty (result.Error)) {
			Debug.Log ("ShareLink Error: " + result.Error);
		} else if (!string.IsNullOrEmpty (result.PostId)) {
			//Print post identifier of the shared content
			Debug.Log (result.PostId);
		} else {
			//What to do if share was succeeded
			Debug.Log ("ShareLink successful");
			if (isShared == 0) {
				isShared = 1; PlayerPrefs.SetInt ("isShared", isShared);
				gameOverText.text = "You unlocked Mr Rocket. Thanks for sharing our game";
			}
		}
	}*/

	private const string FACEBOOK_APP_ID = "1790257867928392";
	private const string FACEBOOK_URL = "http://www.facebook.com/dialog/feed";
	string gameLink = "http://onelink.to/colorcrash";
	string title = "Click To Try This Addicting New Game #COLORCRASH!";
	string caption = "#COLORCRASH!";
	string description = "Download ColorCrash! For Free To Try";
	string picture = null;
	string redirect = "http://www.facebook.com/";



	void ShareToFacebook (string linkParameter, string nameParameter, string captionParameter, string descriptionParameter, string pictureParameter, string redirectParameter)
	{
		Application.OpenURL (FACEBOOK_URL + "?app_id=" + FACEBOOK_APP_ID +
			"&link=" + WWW.EscapeURL(linkParameter) +
			"&name=" + WWW.EscapeURL(nameParameter) +
			"&caption=" + WWW.EscapeURL(captionParameter) + 
			"&description=" + WWW.EscapeURL(descriptionParameter) + 
			"&picture=" + WWW.EscapeURL(pictureParameter) + 
			"&redirect_uri=" + WWW.EscapeURL(redirectParameter));
	}


	IEnumerator FacebookPostHelper ()
	{
		yield return new WaitForSeconds (0.15f);
		gameOverText.text = "You unlocked Mr Rocket. Thanks for sharing Color Crash!";

	}

	public void FacebookPost ()
	{
		ShareToFacebook (gameLink, title, caption, description, picture, redirect);
		if (isShared == 0) {
			isShared = 1; PlayerPrefs.SetInt ("isShared", isShared);
			StartCoroutine ("FacebookPostHelper");
		}
	}



	IEnumerator RateGameHelper ()
	{
		yield return new WaitForSeconds (0.15f);
		gameOverText.text = "You unlocked Happy Kite. Thanks for your review";

	}


	//Function to take user to device app marketplace to Rate Game.
	public void RateGame ()
	{
		#if UNITY_IOS //if the device is an Iphone. /////////////////////
		Application.OpenURL ("https://itunes.apple.com/us/app/color-crash/id1221762855?mt=8");
		#elif UNITY_ANDROID
		Application.OpenURL("market://details?id=com.CasualGames.ColorTileChallenge");
		#endif
		if (gameCtrl.gameRated == 0) {
			gameCtrl.gameRated = 1; 
			PlayerPrefs.SetInt ("gameRated", gameCtrl.gameRated);
			StartCoroutine ("RateGameHelper");
		}
	}

	/*public void GoToFacebookPage ()
	{
		Application.OpenURL ("fb://profile/969992883105774");
	}*/


	//Function to show Leadeboard 
	public void ShowLeaderboard ()
	{
		Social.ShowLeaderboardUI ();
	}


	//Function to Show ads in Game
	public void ShowAd ()
	{
		#if UNITY_ADS ////////////////////////////////////////////////////////////////////////////////
		if (gamesPlayed % 3 == 0 && removeAds == 0) {
			
			if (Advertisement.IsReady ()) //If the ad is ready to show
				{
				Advertisement.Show ();
				}

		}

		#elif !UNITY_ADS
		Debug.Log ("Device Does not support Unity Ads");
		#endif ////////////////////////////////////////////////////////////////////////////////////////
	}


	//Function for Advertisement.Show callback
	/*void HandleAdResult (ShowResult result)
	{
		#if UNITY_ADS //////////////////////////////////////////////////////////////////////////////
		switch (result)
		{
		case ShowResult.Finished:
		gameCtrl.remainingPlays = gameCtrl.refill;
		//gameCtrl.SetStartButtonImage ();
		gameCtrl.SetLivesText ();
		PlayerPrefs.SetInt ("plays", gameCtrl.remainingPlays);
		break;
		case ShowResult.Skipped:
		break;
		case ShowResult.Failed:
		Debug.Log ("The ad failed to show");
		break;
		}
		#endif //////////////////////////////////////////////////////////////////////////////////
	}*/


	IEnumerator StartGamePlay ()
	{
		yield return new WaitForSeconds (0.7f);
		//playerController.gameSpeed = initialGameSpeed;
		//playerController.initialSpeed = initialGameSpeed;
		gameCtrl.SpawnPlayerPrefab ();
		gameCtrl.SpawnBars ();
	}

	//Functionality for Start button
	public void StartGame ()
	{
		/*if (gameCtrl.remainingPlays < 1) {
			ShowRewardedAd ();
		} else {*/
			userInfaceAnimator.SetInteger ("Transition", mainToGamePlay);
			//gameMusic.Play ();
			StartCoroutine ("StartGamePlay");
		//}
	}


	public void ViewGameInstructions ()
	{
		//Transition to Game Instructions
		userInfaceAnimator.SetInteger ("Transition", mainToGameInstruction);
	}


	public void ExitGameInstructions ()
	{
		//Transition back to Main Menu
		userInfaceAnimator.SetInteger ("Transition", gameInstructionToMain);
	}


	public void EnterShop ()
	{
		//Hide Menu
		userInfaceAnimator.SetInteger ("Transition", enterShop);
	}


	public void ExitGameShop ()
	{
		userInfaceAnimator.SetInteger ("Transition", exitShop);
	}

	public void PlayAgain ()
	{
		gamesPlayed++;
		PlayerPrefs.SetInt ("gamesPlayed", gamesPlayed);
		gameCtrl.RestartScene ();
	}


	/*public void PauseGame ()
	{
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
			gameMusic.volume = 0.02f;
		} 

		if (pauseMenu.enabled == false) {
			pauseMenu.enabled = true;
		}
	}*/


	/*public void UnPauseGame ()
	{
		if (pauseMenu.enabled == true) {
			pauseMenu.enabled = false;
		}

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
			gameMusic.volume = 0.4f;
		}
	}*/


	void SetGameOverText ()
	{
		if (gamesPlayed >= 2 && gameCtrl.bestScore >= 10) {
			if (gameCtrl.gameRated == 0) {
				gameOverText.text = "Rate Game to unlock Happy Kite";
			} else if (isShared == 0) {
				gameOverText.text = "Share Game to unlock Mr Rocket";
			} else if (gameCtrl.bestScore < 90) {
				gameOverText.text = "Score 90 or more to unlock DeFalcon";
			} else {
				gameOverText.text = "Try Again";
			}
		} else {
			gameOverText.text = "Try Again";
		}
	}


	public void SoundMute ()
	{
		if (gameCtrl.gameMuted == 0) {
			AudioListener.volume = 0.0f;
			gameCtrl.gameMuted = 1;
			PlayerPrefs.SetInt ("gameMuted", gameCtrl.gameMuted);
			gameCtrl.SetSoundButton ();
		} else if (gameCtrl.gameMuted == 1) {
			AudioListener.volume = 1.0f;
			gameCtrl.gameMuted = 0;
			PlayerPrefs.SetInt ("gameMuted", gameCtrl.gameMuted);
			gameCtrl.SetSoundButton ();
		}

	}


	void SetSound ()
	{
		if (gameCtrl.gameMuted == 1) {
			AudioListener.volume = 0.0f;
		} else if (gameCtrl.gameMuted == 0) {
			AudioListener.volume = 1.0f;
		}
	}




}



