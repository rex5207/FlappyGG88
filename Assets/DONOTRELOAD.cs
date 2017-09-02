using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DONOTRELOAD : MonoBehaviour {
	static bool created = false;

	// Use this for initialization
	void Awake(){
		if (!created) {
			Debug.Log ("Initial");
			DontDestroyOnLoad (this.gameObject);
			created = true;
		} else {
			Destroy (this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
