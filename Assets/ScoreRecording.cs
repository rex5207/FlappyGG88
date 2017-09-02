using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecording : MonoBehaviour {

	public Text[] Text_score;
	public Text Text_generation;
	private static int score = 0; 
	private static int generation = 1; 
	private bool[] isdead = new bool[10];
	private int[] top4score = new int[4];

	private int top1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		score++;
		for (int i = 0; i < 10; i++) {
			if (!isdead [i]) {
				Text_score [i].text = i + ". Score:" + score;
			}
		}
	}

	public void resetscore(){
		generation++;
		Text_generation.text = "Generation: " + generation;
		score = 0;
		for (int i = 0; i < 10; i++) {
			isdead [i] = false;
		}
	}

	public int birddead(int tag){
		isdead [tag] = true;
		int temp = 0;
		int index = -1;
		for (int i = 0; i < 4; i++) {
			if (score - top4score [i] > temp) {
				temp = score - top4score [i];
				index = i;
			}
		}
		if (temp > 0 && index != -1) {
			top4score [index] = score;
			return index;
		} else
			return -1;
	}

	public int gettop1(){
		int index = 0;
		int max = 0;
		for (int i = 0; i < 4; i++) {
			if (top4score [i] > max) {
				index = i;
				max = top4score [i];
			}
		}
		top1 = index;
		return index;
	}

	public int gettop2(){
		int index = 0;
		int max = 0;
		for (int i = 0; i < 4; i++) {
			if (top4score [i] > max && i!= top1) {
				index = i;
				max = top4score [i];
			}
		}
		top1 = index;
		return index;
	}
}
