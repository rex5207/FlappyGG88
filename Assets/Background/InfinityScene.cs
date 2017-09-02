using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityScene : MonoBehaviour {

	public Renderer background;
	private float speed = 0.0001f;
	// Update is called once per frame
	void Update () {
		float x = background.material.GetTextureOffset("_MainTex").x;
		background.material.SetTextureOffset ("_MainTex", new Vector2(x + speed, 0));
	}
}
