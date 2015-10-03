using UnityEngine;
using System.Collections;

public class inputController : MonoBehaviour {

	private bool isMobile = true; // Platform, if unity Editor set it to false
	private playerHandler _player;
	public bool grounded;

	public bool doubleJump;

	//public static bool doubleJump = false;

	// Use this for initialization
	void Start () {
		if (Application.isEditor)
			isMobile = false;
		_player = GameObject.Find("player").GetComponent<playerHandler>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.timeSinceLevelLoad >= 5.0f) { // Players are able to jump after 5 seconds
			if (isMobile) { // Touch for mobile platfor
				int tmpC = Input.touchCount;
				tmpC--;

				if (Input.GetTouch (tmpC).phase == TouchPhase.Began) {
					handleInteraction ();
				}
				if (Input.GetTouch (tmpC).phase == TouchPhase.Ended) {
					// Do nothing
				}
			} else { // this statement is for Unity Editor/PC
				if (Input.GetMouseButtonDown (0)) {
					handleInteraction ();
				}
				if (Input.GetMouseButtonUp (0)) {
					// Do nothing
				}

			}
		}
	}


	void handleInteraction()
	{
		if (_player.isGrounded()) {
			_player._isGrounded = false;
			_player.jump ();
			doubleJump = true;
		}
		else {
			if (doubleJump)
			{
				_player.jump();
				doubleJump = false;
			}
		}
	}
}
