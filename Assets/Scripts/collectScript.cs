using UnityEngine;
using System.Collections;

public class collectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 30.0f);
			GameObject.Find("Main Camera").GetComponent<scoreScript>().Points += 100;
		}
	}
}
