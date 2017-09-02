using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWall : MonoBehaviour {
	public GameObject[] myWallPrefab;
	public GameObject[] birds;
	GameObject myWall;

	// Use this for initialization
	void Start () {
		InvokeRepeating("autoCreate", 0, 1f);
	}

	void autoCreate(){
		int tag = Random.Range(0, 5);
		myWall = Instantiate (myWallPrefab [tag], this.gameObject.transform.position, Quaternion.identity);
		Destroy (myWall, 5);
		for (int i = 0; i < 10; i++) {
			if(birds[i] != null)
				birds[i].GetComponent<BirdAction>().addwallLocation(myWall.transform.GetChild(2).gameObject);
		}
	}
}
