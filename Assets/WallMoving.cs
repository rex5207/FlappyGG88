using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMoving : MonoBehaviour {

	public Rigidbody2D wall;
	//private float speed = -0.1f;
	// Use this for initialization
	void Start () {
		wall.velocity = Vector2.left * 6;
	}
	
	// Update is called once per frame
	void Update () {
		//wall.transform.position += new Vector3 (speed, 0, 0);
	}
}
