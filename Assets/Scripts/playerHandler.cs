using UnityEngine;
using System.Collections;

public class playerHandler : MonoBehaviour {
	private inputController _ipc;

	private int _animState = Animator.StringToHash("animState");
	private Animator _animator;

	public bool _isGrounded;

	public bool _top = false; // top box collider detection

	public float tmpGameSpeed = 5.0f;

	// Use this for initialization
	void Start () {
		_animator = this.transform.GetComponent<Animator> ();
	}

	void FixedUpdate()
	{
		if (Time.time % 2 == 0) { // delay // Every fixed (2) seconds, game speed increases
			tmpGameSpeed += 0.5f;
		}

	}

	// Update is called once per frame
	void Update () {
		if (_isGrounded) {
			_animator.SetInteger(_animState, 0);
		}
		else if (!_isGrounded){
			_animator.SetInteger(_animState, 1);
		}

		print ("tmp:   " + tmpGameSpeed);
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		Collider2D collider = coll.collider;

		if (coll.gameObject.tag == "floor" && !_top && this.GetComponent<Rigidbody2D>().velocity.y == 0.0f) {
			if (GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed > tmpGameSpeed)
				GameObject.Find("Main Camera").GetComponent<levelCreator>().gameSpeed = tmpGameSpeed;

			levelCreator._setBlank = true;
			_isGrounded = true;

			Vector3 contactPoint = coll.contacts [0].point;
			Vector3 center = collider.bounds.center;

			_top = contactPoint.y < center.y;

			if (_top)
				collider.enabled = false;
		}
		else 
			_isGrounded = false;
	}

//	public void OnCollisionExit2D(Collision2D coll)
//	{
//		if (coll.gameObject.tag == "floor")
//			_isGrounded = false;
//	}

	public bool isGrounded()
	{
		return _isGrounded;
	}

	public void jump()
	{
		//this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (this.GetComponent<Rigidbody2D> ().velocity.x, 0.0f);
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (this.GetComponent<Rigidbody2D> ().velocity.x, 0.0f);
		this.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * 3000.0f);


//		//this.GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, 0.0f);
//		if (inputController.doubleJump) {
//			player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (player.GetComponent<Rigidbody2D> ().velocity.x, 0.0f);
//			player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 3000.0f));
//		}
////		if(this.GetComponent<Rigidbody2D>().velocity.y < -5.0f)
////			this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 5000.0f));
////		else if (this.GetComponent<Rigidbody2D>().velocity.y < 0.0f)
////			this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 4000.0f));
////		else
//		else {
//			player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (player.GetComponent<Rigidbody2D> ().velocity.x, 0.0f);
//			player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 3000.0f)); //Jump force
//		}
	}
}
