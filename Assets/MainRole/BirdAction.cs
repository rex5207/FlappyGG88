using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAction : MonoBehaviour {
	static bool created = false;

	public Rigidbody2D bird;
	public Renderer flyrender;
	public int tag;

	private GameObject genetic;

	private Queue<GameObject> wallLocation = new Queue<GameObject>();
	private GameObject nextWall = null;

	// Use this for initialization
	void Start () {
		genetic = GameObject.FindWithTag("GameSetting");
	}
	

	void Update () {
		//飛
		if (Input.GetMouseButtonDown (0)) {
			Fly ();
		}

		if (nextWall == null) {
			nextWall = wallLocation.Dequeue ();
		} else {
			if (this.gameObject.transform.position.x >= nextWall.transform.position.x) {
				nextWall = wallLocation.Dequeue ();
			}
		}
	}

	//Bird飛時，換Texture
	IEnumerator flymode()
	{
		flyrender.enabled = true;
		yield return new WaitForSeconds(1);
		flyrender.enabled = false;
	}

	public void Fly(){
		bird.velocity = Vector3.up * 7;
		StartCoroutine(flymode());
	}

	public void OnTriggerEnter2D(Collider2D node) {
		genetic.GetComponent<Genetic>().push_rank (tag);
		Destroy(this.gameObject);
	}

	//經過了目前的wall，把目標設成下一個
	public void addwallLocation(GameObject wall){
		wallLocation.Enqueue(wall);
	}

	//回傳與wall的距離差
	public float get_width(){
		return nextWall == null ? -100 : nextWall.transform.position.x - this.gameObject.transform.position.x;
	}

	//回傳與wall的高度差
	public float get_height(){
		return  nextWall == null ? -100 : nextWall.transform.position.y - this.gameObject.transform.position.y;
	}
}
