using UnityEngine;
using System.Collections;

public class crateScript : MonoBehaviour {

	private float maxY;
	private float minY;
	private int direction = 1;

	public bool inPlay = true; //crate inplay
	private bool releaseCrate = false; //crate inplay is not released yet

	private SpriteRenderer crateRender;

	// Use this for initialization
	void Start () {
		maxY = this.transform.position.y + 0.5f;
		minY = maxY - 1.0f;

		crateRender = this.transform.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y + (direction * 0.05f));
		if (this.transform.position.y > maxY)
			direction = -1;
		if (this.transform.position.y < minY)
			direction = 1;

		if (!inPlay && !releaseCrate) // if inplay crate is destroyed and new one is not released yet then respawn Step.2
			respawn ();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			switch(crateRender.sprite.name)
			{
			case "crates_0":
				if (GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed > 5.0f)
				{
					GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed -= 1.0f;
					GameObject.Find("player").GetComponent<playerHandler>().tmpGameSpeed = GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed;
				}
				break;
			case "crates_1":
				levelCreator._setBlank = false;
				GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed *= 3.0f;
				GameObject.Find("player").GetComponent<Rigidbody2D>().AddForce(Vector2.up * 6000.0f); //** HUGE JUMP???
				break;
			case "crates_2": //TRAP
				GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed += 2.0f;
				GameObject.Find("player").GetComponent<playerHandler>().tmpGameSpeed = GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed;
				break;

			case "crates_3":
				break;
			}
		
			inPlay = false; // not existing after collision Step.1
			this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 30.0f);
		}
	}

	void respawn()
	{
		releaseCrate = true;
		Invoke ("placeCrate", (float)Random.Range (3, 5));

	}

	void placeCrate(){
		inPlay = true;
		releaseCrate = false;

		GameObject tmpTile = GameObject.Find ("Main Camera").GetComponent<levelCreator> ().tilePos;
		this.transform.position = new Vector2 (tmpTile.transform.position.x, tmpTile.transform.position.y + 7.5f);
		maxY = this.transform.position.y + 0.5f;
		minY = maxY - 1.0f;
	}
}
