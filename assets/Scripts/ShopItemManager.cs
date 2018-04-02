using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopItemManager : MonoBehaviour {

	public int itemID; //Id for item this will be set to gameCtrl.prefabIndex when selected
	GameObject uiFunctions; //uiFunction is the object that holds GUIEventFunction class and functions
	GUIEventFunctions guiEventFunctions; // A class within uiFunctions to access its functions
	GameObject gameController; //Game Controller Object to access its class and functions
	GameCtrl gameCtrl; //A class within the gameController gameObject to access its functions
	public Color transparent, whitish;
	public string itemName, unlockMessage;
	public Text ShopItemMessage;


	void Awake ()
	{
		//Set Reference to the GUIEventFunctions class **********************************
		uiFunctions = GameObject.FindWithTag ("UI");
		guiEventFunctions = uiFunctions.GetComponent <GUIEventFunctions> ();                 
		//*******************************************************************************

		//Set Reference to the gameCtrl class ***********************************
		gameController = GameObject.FindWithTag ("GameController");
		gameCtrl = gameController.GetComponent <GameCtrl> ();
		//***********************************************************************
	}


	// Use this for initialization
	void Start () 
	{
		SetButtonFeatures ();
	}



	//Whether item has been selected or not
	public bool IsSelected () 
	{
		if (itemID == gameCtrl.prefabIndex) {
			return true;
		} else {
			return false;
		}
	}


	//Whether item is locked or unlocked
	public bool IsUnlocked ()
	{
		bool unlocked = false;

		switch (itemID) 
		{
		case 0:
			unlocked = true;
			break;
		case 1:
			if (gameCtrl.gameRated == 1) {
				unlocked = true;
			} else {
				unlocked = false;
			}
			break;
		case 2:
			if (guiEventFunctions.isShared == 1) {
				unlocked = true;
			} else {
				unlocked = false;
			}
			break;
		case 3:
			if (gameCtrl.bestScore >= 90) {
				unlocked = true;
			} else {
				unlocked = false;
			}
			break;
		}

		return unlocked;
	}

	//Set the features of the Button
	public void SetButtonFeatures ()
	{
		Image buttonImage = this.gameObject.GetComponent<Image> ();
		Image bgImage = this.transform.parent.GetComponent <Image> ();


		if (IsUnlocked ()) {
			buttonImage.color = Color.white;
		} else if (!IsUnlocked ()){
			buttonImage.color = Color.black;
		}


		//Change color of button if it is the active Button
		if (IsSelected ()) {
			bgImage.color = whitish;
		} else if (!IsSelected ()){
			bgImage.color = transparent;
		}

	}


	public void ButtonOnClick ()
	{
		if (IsUnlocked ()) { 
			gameCtrl.prefabIndex = itemID; 
			PlayerPrefs.SetInt ("prefabIndex", gameCtrl.prefabIndex);
			ShopItemMessage.text = itemName;
			gameCtrl.SetAllShopItemFeatures (); 
			//gameCtrl.RestartScene ();
		} else {
			ShopItemMessage.text = unlockMessage;
		}
	}
}
