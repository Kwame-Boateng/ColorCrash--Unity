using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
	GameObject player; //The player game object
	PlayerController playerController;
	float verticalOffset = 2.0f; //Distance between player and camera on y axis
	public DeviceAspect deviceAspect; //The type of IOS device depending on aspect ratio
	MyPersonalLibrary myPersonalLibrary;
	Camera mainCamera; //To access camera
	public Color darkBlue, blueBlack, greenBlue, darkishGrey, Grey;


	void Awake ()
	{

		SetDeviceType ();
		mainCamera = GetComponent<Camera>();
		myPersonalLibrary = new MyPersonalLibrary ();
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}

	void Start () 
	{
		//player = GameObject.FindWithTag ("Player");
		//playerController = player.GetComponent <PlayerController> ();
		mainCamera.backgroundColor = myPersonalLibrary.RandomPicker <Color> (darkBlue, blueBlack, greenBlue, darkishGrey, Grey);
		//SetPlayerSize ();
	}

	#if UNITY_ANDROID
	void Update ()
	{
		if (Input.GetKey(KeyCode.Escape)) {
		Application.Quit ();
		}
	}
	#endif

	void LateUpdate () 
	{
		//If player is alive then follow player
		if (player != null) {
			if (player.transform.position.y > -(verticalOffset)) {
				transform.position = new Vector3 (0, player.transform.position.y + verticalOffset, -10);
			}
		}

	}

	public void SetPlayer ()
	{
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent <PlayerController> ();
		SetPlayerSize ();
	}


	void SetDeviceType ()
	{
		Camera mainCamera = GetComponent<Camera> ();
		float aspect = mainCamera.aspect;

		if (aspect > 0.56f && aspect < 0.57f) {
			deviceAspect = DeviceAspect.NewIphone;
		} else if (aspect > 0.66f && aspect < 0.67f) {
			deviceAspect = DeviceAspect.OldIphone;
		} else if (aspect > 0.74f && aspect < 0.76f) {
			deviceAspect = DeviceAspect.Ipad;
		} else if (aspect > 0.59 && aspect < 0.61) {
			deviceAspect = DeviceAspect.Android3by5;
		} else if (aspect > 0.62 && aspect < 0.63) {
			deviceAspect = DeviceAspect.Android10by16;
		}
	}


	void SetPlayerSize ()
	{
		switch (deviceAspect)
		{
		case DeviceAspect.NewIphone:
			player.transform.localScale = new Vector2 (0.9f, 0.9f);
			playerController.sideDistance = 1.4f;
			break;
		case DeviceAspect.OldIphone:
			player.transform.localScale = new Vector2 (1.1f, 1.1f);
			playerController.sideDistance = 1.68f;
			break;
		case DeviceAspect.Ipad:
			player.transform.localScale = new Vector2 (1.2f, 1.2f);
			playerController.sideDistance = 1.9f;
			break;
		case DeviceAspect.Android10by16:
			player.transform.localScale = new Vector2 (1.1f, 1.1f);
			playerController.sideDistance = 1.56f;
			break;
		case DeviceAspect.Android3by5:
			player.transform.localScale = new Vector2 (1.1f, 1.1f);
			playerController.sideDistance = 1.5f;
			break;
		}
	}

}

