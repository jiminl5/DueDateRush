using UnityEngine;
using System.Collections;

public class levelCreator : MonoBehaviour {

	//Audio
	private int startingPitch = 1;
	AudioSource bg_song;

	public GameObject tilePos;
	private float startUpPosY;
	private const float tileWidth = 1.25f;
	private int heightLevel = 0;
	private GameObject tmpTile;

	private GameObject collectedTiles;
	private GameObject gameLayer;
	private GameObject bgLayer;

	public float gameSpeed = 5.0f;
	private float outofbounceX;
	private int blankCounter = 0; // count number of blanks
	private int middleCounter = 0; // count number of mid tile
	public static string lastTile = "right";

	public static bool _setBlank;

	//private bool collectAdded = false;

	private float outOfBounceY;
	private GameObject _player;

	private float startTime; // ingame timer

	private bool playerDead = false;

	void Awake(){
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		_setBlank = true;
		//Audio
		bg_song = GetComponent<AudioSource> ();
		bg_song.pitch = startingPitch;


		gameLayer = GameObject.Find("gameLayer");
		bgLayer = GameObject.Find("backgroundLayer");
		collectedTiles = GameObject.Find("tiles");

		for (int i = 0; i < 30; i++) {
			GameObject tmpG1 = Instantiate(Resources.Load("ground_left", typeof(GameObject))) as GameObject;
			tmpG1.transform.parent = collectedTiles.transform.FindChild("gLeft").transform;

			GameObject tmpG3 = Instantiate(Resources.Load("ground_right", typeof(GameObject))) as GameObject;
			tmpG3.transform.parent = collectedTiles.transform.FindChild("gRight").transform;

			GameObject tmpG4 = Instantiate(Resources.Load("blank", typeof(GameObject))) as GameObject;
			tmpG4.transform.parent = collectedTiles.transform.FindChild("gBlank").transform;

			GameObject tmpG5 = Instantiate(Resources.Load("collect", typeof(GameObject))) as GameObject;
			tmpG5.transform.parent = collectedTiles.transform.FindChild("collectables").transform;
			tmpG5.transform.position = Vector2.zero;
		}

		for (int i = 0; i < 50; i++) {
			GameObject tmpG2 = Instantiate(Resources.Load("ground_middle", typeof(GameObject))) as GameObject;
			tmpG2.transform.parent = collectedTiles.transform.FindChild("gMid").transform;
		}

		collectedTiles.transform.position = new Vector2(-60.0f, -20.0f);
		tilePos = GameObject.Find("startTilePosition");
		startUpPosY = tilePos.transform.position.y;

		outofbounceX = tilePos.transform.position.x - 5.0f;

		outOfBounceY = startUpPosY - 3.0f;
		_player = GameObject.Find ("player");

		fillScene();
		startTime = 0; //in game timer
	}
	
	// Update is called once per frame
	void Update()
	{

		if (Time.timeSinceLevelLoad >= 6)
			this.GetComponent<scoreScript> ().Points++;
	}

	void FixedUpdate () {
		if (startTime - Time.time % 2 == 0) { // delay // Every fixed (2) seconds, game speed increases
			gameSpeed += 0.5f;

			//Audio
			//if (gameSpeed % 5 == 0)
				bg_song.pitch += 0.001f;
		}
		//GameLayer, Background movement
		gameLayer.transform.position = new Vector2 (gameLayer.transform.position.x - gameSpeed * Time.deltaTime, 0);
		//bgLayer.transform.position = new Vector2 (bgLayer.transform.position.x - gameSpeed / 4 * Time.deltaTime, 0); // slower

		foreach (Transform child in gameLayer.transform) {
			if(child.position.x < outofbounceX)
			{
				switch (child.gameObject.name) {
				case "ground_left(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gLeft").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gLeft").transform;
					break;
				case "ground_right(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gRight").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gRight").transform;
					break;
				case "ground_middle(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gMid").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gMid").transform;
					break;
				case "blank(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gBlank").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gBlank").transform;
					break;

				case "collect(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("collectables").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("collectables").transform;
					break;

				case "Reward":
					GameObject.Find("Reward").GetComponent<crateScript>().inPlay = false;
					break;

				default:
					Destroy(child.gameObject);
					break;
				}
			}
		}

		if (gameLayer.transform.childCount < 50) {
			spawnTile ();
			//print (gameSpeed);
			//print (bg_song.pitch);
		}
		if (_player.transform.position.y < outOfBounceY) {
			killPlayer();
		}

		print (_setBlank);
		print (gameSpeed);
	}

	private void killPlayer()
	{
		if (playerDead)
			return;
		playerDead = true;
		this.GetComponent<scoreScript>().sendToHighScore ();
		//Application.LoadLevel (0);
		Invoke ("reloadScene", 1);
	}

	private void reloadScene()
	{
		Application.LoadLevel (0);
	}

	private void spawnTile()
	{
		float tileBalance = 10.0f; // Balance out of bound exception error

		float blankBalance = 10.0f;

		if (blankCounter > 0) {
			setTile("blank");
			blankCounter--;
			return;
		}
		if (middleCounter > 0) {
			//////////////////////
			spawnCollectables();
			//////////////////////
			setTile("middle");
			middleCounter--;
			return;
		}

		if (lastTile == "blank") {
			changeHeight ();
			setTile ("left");
//			middleCounter = (int)Random.Range(gameSpeed / 5.0f, gameSpeed + 3.0f); // random ranged size of tiles (mid)
			if (gameSpeed < blankBalance) {
				float randMin = gameSpeed / 5.0f;
				float randMax = gameSpeed + 3.0f;
				middleCounter = (int)Random.Range (randMin, randMax);
			} else 
				middleCounter = (int)Random.Range (blankBalance / 5.0f, blankBalance + 3.0f);
		} else if (lastTile == "right" && _setBlank) {
			//blankCounter = (int)Random.Range (gameSpeed / 2.5f, gameSpeed / 1.01f);//(int)Random.Range(2,5); // random ranged size of empty space
			if (gameSpeed < tileBalance) {
				float randMin = gameSpeed / 2.5f;
				float randMax = gameSpeed / 1.01f;
				blankCounter = (int)Random.Range (randMin, randMax);
			} else 
				blankCounter = (int)Random.Range (tileBalance / 2.5f, tileBalance / 1.01f);
		}
		// Newly Added
		else if (lastTile == "right" && !_setBlank) {
			setTile ("left");
			//			middleCounter = (int)Random.Range(gameSpeed / 5.0f, gameSpeed + 3.0f); // random ranged size of tiles (mid)
			if (gameSpeed < blankBalance) {
				float randMin = gameSpeed / 5.0f;
				float randMax = gameSpeed + 3.0f;
				middleCounter = (int)Random.Range (randMin, randMax);
			} else 
				middleCounter = (int)Random.Range (blankBalance / 5.0f, blankBalance + 3.0f);
		}

		else if(lastTile == "middle")
		{
			setTile("right");
		}
	}

	private void changeHeight()
	{
		int newHeightLevel = (int)Random.Range (0, 4);
		if (newHeightLevel < heightLevel)
			heightLevel--;
		else if (newHeightLevel > heightLevel)
			heightLevel++;
	}

	private void fillScene()
	{
		for (int i = 0; i < 30; i++)
		{
			setTile("middle");
		}
		setTile ("right");
	}

	public void setTile(string type){

		switch (type) {
		case "left":
			tmpTile = collectedTiles.transform.FindChild("gLeft").transform.GetChild(0).gameObject;
			break;
		case "right":
			tmpTile = collectedTiles.transform.FindChild("gRight").transform.GetChild(0).gameObject;
			break;
		case "middle":
			tmpTile = collectedTiles.transform.FindChild("gMid").transform.GetChild(0).gameObject;
			break;
		case "blank":
			tmpTile = collectedTiles.transform.FindChild("gBlank").transform.GetChild(0).gameObject;
			break;
		}

		tmpTile.transform.parent = gameLayer.transform;
		tmpTile.transform.position = new Vector2(tilePos.transform.position.x + (tileWidth), startUpPosY + (heightLevel * tileWidth));

		tilePos = tmpTile;

		lastTile = type;
	}

	private void spawnCollectables()
	{
		GameObject newCollect = collectedTiles.transform.FindChild("collectables").transform.GetChild(0).gameObject;
		newCollect.transform.parent = gameLayer.transform;

		newCollect.transform.position = new Vector2(tilePos.transform.position.x + tileWidth, startUpPosY + (heightLevel * tileWidth + (tileWidth * 3.0f)));

		return;
	}
}
