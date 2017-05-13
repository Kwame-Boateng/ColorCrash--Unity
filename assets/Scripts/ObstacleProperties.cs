using UnityEngine;
using System.Collections;


public class ObstacleProperties : MonoBehaviour 
{
	public ObjectColor tileColor;
	public ObstacleTypes tileType;
	GameObject gameController;
	GameObject thePlayer;
	GameCtrl gameCtrl;
	PlayerController playerController;
	GameObject uiFunctions;
	public GUIEventFunctions guiEnventFunctions;


	void Awake ()
	{
		gameController = GameObject.FindWithTag ("GameController");
		uiFunctions = GameObject.FindWithTag ("UI");
	}

	// Use this for initialization
	void Start ()
	{
		gameCtrl = gameController.GetComponent <GameCtrl> ();
		thePlayer = GameObject.FindWithTag ("Player");
		playerController = thePlayer.GetComponent <PlayerController> ();
		guiEnventFunctions = uiFunctions.GetComponent <GUIEventFunctions> ();
	}


	//Function to add Obstacle Bars to Trash List and trash them all
	//after a while
	IEnumerator PushBacktoPool ()
	{
		yield return new WaitForSeconds (0.7f);
		if (transform.parent != null) 
		{
			transform.parent.gameObject.SetActive (false);
			//gameCtrl.Trash.Add (transform.parent.gameObject);
		} 
		else 
		{
			gameObject.SetActive (false);
			gameCtrl.LevelEntryPool.Add (gameObject);
		}
	}

	/*void ReducePlays ()
	{
		gameCtrl.remainingPlays--;
		PlayerPrefs.SetInt ("plays", gameCtrl.remainingPlays);
	}*/

	void OnTriggerEnter2D (Collider2D other)
	{
		if (this.gameObject.tag == "Entry" && other.gameObject.tag == "Player") 
		{   
			gameCtrl.numOfEntryTriggered++;
			gameCtrl.SetGameLevel ();
			playerController.SetGameSpeed ();
			if (gameCtrl.numOfEntryTriggered >= 1) 
			{
				playerController.SetOrChangeSprite ();
				gameCtrl.SpawnBars (); 
			}

			gameController.GetComponent <AudioSource> ().Play ();
		}

		if ((this.gameObject.tag != "ObstacleSpikes") && (other.gameObject.tag == "Player")) {
			if ((playerController.playerColor == this.tileColor) || this.tileColor == ObjectColor.Neutral) {

				gameCtrl.score++;

				if (this.gameObject.tag == "ObstacleEntry") {
					gameCtrl.score++;
				}

				gameCtrl.UpdateScoresText ();
			}
			else 
			{

				Instantiate (gameCtrl.playerExplosion, other.transform.position, other.transform.rotation);
				Destroy (other.gameObject);
				StartCoroutine ("GameOver");
			}
		} 
		else
		{
			Instantiate (gameCtrl.playerExplosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
			StartCoroutine ("GameOver");
		}

		StartCoroutine ("PushBacktoPool");

	}


	IEnumerator GameOver ()
	{
		if (gameCtrl.bestScore < 90) {
			if (gameCtrl.score >= 90) {
				guiEnventFunctions.gameOverText.text = "Congratulations!!! You unlocked DeFalcon.";
			} 	
		}
		gameCtrl.SetBestScore ();
		gameCtrl.UpdateScoresText ();
		yield return new WaitForSeconds (0.5f);
		//Show ad between 3 gameplay intervals.

		guiEnventFunctions.ShowAd ();

		guiEnventFunctions.userInfaceAnimator.SetInteger ("Transition", guiEnventFunctions.playerDead);
		guiEnventFunctions.gameMusic.volume = (guiEnventFunctions.gameMusic.volume)/2.5f;
	}

}
