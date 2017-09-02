using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genetic : MonoBehaviour {
	private static Genetic instanceRef;

	static bool created = false;
	private Stack<int> rank = new Stack<int>();

	//從input到hidden的wight
	private float[,,] w_input_hidden = new float[10, 2, 6];
	//從input到hidden的bias
	private float[,] b_input_hidden = new float[10, 6];

	//從hidden到output的wight
	private float[,] w_hidden_output = new float[10, 6];
	//從idden到output的bias
	private float[] b_hidden_output = new float[10];


	//從input到hidden的wight
	float[,,] w_input_hidden_temp = new float[10, 2, 6];
	//從input到hidden的bias
	float[,] b_input_hidden_temp = new float[10, 6];

	//從hidden到output的wight
	float[,] w_hidden_output_temp = new float[10, 6];
	//從idden到output的bias
	float[] b_hidden_output_temp = new float[10];

	//從input到hidden的wight
	float[,,] w_input_hidden_top4 = new float[4, 2, 6];
	//從input到hidden的bias
	float[,] b_input_hidden_top4 = new float[4, 6];

	//從hidden到output的wight
	float[,] w_hidden_output_top4 = new float[4, 6];
	//從idden到output的bias
	float[] b_hidden_output_top4 = new float[4];

	private float mutateRate = 0.4f;

	// Use this for initialization
	void Awake()
	{
		if(instanceRef == null)
		{
			instanceRef = this;
			DontDestroyOnLoad(gameObject);
		}else
		{
			DestroyImmediate(gameObject);
		}
	}

	void Start(){
		for (int j = 0; j < 10; j++){
			for (int i = 0; i < 6; i++) {
				w_input_hidden[j, 0, i] = Random.Range(-5.0f, 5.0f);
				w_input_hidden[j, 1, i] = Random.Range(-5.0f, 5.0f);
				b_input_hidden[j, i] = Random.Range(-5.0f, 5.0f);
				w_hidden_output[j, i] = Random.Range(-5.0f, 5.0f);
			}
			b_hidden_output[j] = Random.Range(-5.0f, 5.0f);
		}
	}

	public void push_rank(int tag){
		rank.Push (tag);
		int index = this.gameObject.GetComponent<ScoreRecording> ().birddead (tag);
		if (index != -1) {
			for (int i = 0; i < 6; i++) {
				w_input_hidden_top4[index, 0, i] = w_input_hidden[tag, 0, i];
				w_input_hidden_top4[index, 1, i] = w_input_hidden[tag, 1, i];
				b_input_hidden_top4[index, i] = b_input_hidden[tag, i];
				w_hidden_output_top4[index, i] = w_hidden_output[tag, i];
			}
			b_hidden_output_top4[index] = b_hidden_output[tag];
		}


		if (rank.Count == 10) {
			this.gameObject.GetComponent<ScoreRecording> ().resetscore ();
			int[] top = new int[4];
			rank.Clear ();
			create_offspring ();
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	private void create_offspring(){

		//Top 4 直接複製到下一回合
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 6; j++) {
				w_input_hidden_temp[i, 0, j] = w_input_hidden_top4[i, 0, j];
				w_input_hidden_temp[i, 1, j] = w_input_hidden_top4[i, 1, j];
				b_input_hidden_temp[i, j] = b_input_hidden_top4[i, j];
				w_hidden_output_temp[i, j] = w_hidden_output_top4[i, j];
			}
			b_hidden_output_temp [i] = b_hidden_output_top4[i];
		}

		//Top 1 和 Top 2 crossover
		int temp = Random.Range (0, 1);
		int top1 = this.gameObject.GetComponent<ScoreRecording> ().gettop1 ();
		int top2 = this.gameObject.GetComponent<ScoreRecording> ().gettop2 ();
		temp = temp == 0 ? top1 : top2;
		for (int j = 0; j < 6; j++) {
			w_input_hidden_temp[4, 0, j] = w_input_hidden_top4[temp, 0, j];
			w_input_hidden_temp[4, 1, j] = w_input_hidden_top4[temp, 1, j];
			b_input_hidden_temp[4, j] = crossover(0, 1, j, 0);
			w_hidden_output_temp[4, j] = w_hidden_output_top4[temp, j];
		}
		b_hidden_output_temp [4] = crossover(top1, top2, 0, 1);


		//Top 4選其中兩個 crossover
		for (int i = 5; i <= 7; i++) {
			int p1 = Random.Range (0, 4);
			int p2 = Random.Range (0, 4);
			temp = Random.Range (0, 1);
			int myp = temp == 0 ? p1 : p2;
			for (int j = 0; j < 6; j++) {
				w_input_hidden_temp[i, 0, j] = w_input_hidden_top4[myp, 0, j];
				w_input_hidden_temp[i, 1, j] = w_input_hidden_top4[myp, 1, j];
				b_input_hidden_temp[i, j] = crossover(p1, p2, j, 0);
				w_hidden_output_temp[i, j] = w_hidden_output_top4[myp, j];
			}
			b_hidden_output_temp [i] = crossover(p1, p2, 0, 1);
		}

		//直接copy隨機兩個
		for (int i = 8; i <= 9; i++) {
			int p = Random.Range (0, 4);
			for (int j = 0; j < 6; j++) {
				w_input_hidden_temp[i, 0, j] = w_input_hidden_top4[p, 0, j];
				w_input_hidden_temp[i, 1, j] = w_input_hidden_top4[p, 1, j];
				b_input_hidden_temp[i, j] = b_input_hidden_top4[p, j];
				w_hidden_output_temp[i, j] = w_hidden_output_top4[p, j];
			}
			b_hidden_output_temp [i] = b_hidden_output_top4[p];
		}


		//前四個直接到下一回合
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 6; j++) {
				w_input_hidden[i, 0, j] = w_input_hidden_temp[i, 0, j];
				w_input_hidden[i, 1, j] = w_input_hidden_temp[i, 1, j];
				b_input_hidden[i, j] = b_input_hidden_temp[i, j];
				w_hidden_output[i, j] = w_hidden_output_temp[i, j];
			}
			b_hidden_output [i] = b_hidden_output_top4[i];
		}

		//增加變異
		for (int i = 4; i < 10; i++) {
			for (int j = 0; j < 6; j++) {
				w_input_hidden[i, 0, j] = mutate(w_input_hidden_temp[i, 0, j]);
				w_input_hidden[i, 1, j] = mutate(w_input_hidden_temp[i, 1, j]);
				b_input_hidden[i, j] = mutate(b_input_hidden_temp[i, j]);
				w_hidden_output[i, j] = mutate(w_hidden_output_temp[i, j]);
			}
			b_hidden_output [i] = mutate(b_hidden_output_temp[i]);
		}
	}

	private float mutate(float input){
		if (Random.Range(0.0f, 1.0f) < this.mutateRate) {
			float mutateFactor = 1.0f + ((Random.Range(0.0f, 1.0f) - 0.5f) * 3.0f + (Random.Range(0.0f, 1.0f) - 0.5f));
			input *= mutateFactor;
		}

		return input;
	}

	private float crossover(int p1, int p2, int index, int type){
		int flag = Random.Range (0, 1);
		if (type == 0) {
			if (flag == 0)
				return b_input_hidden_top4 [p1, index];
			else
				return b_input_hidden_top4 [p2, index];
		} else {
			if (flag == 0)
				return b_hidden_output_top4 [p1];
			else
				return b_hidden_output_top4 [p2];
		}
	}

	public float get_weight_input_hidden(int bird_tag, int n_input, int n_hidden){
		return w_input_hidden[bird_tag, n_input, n_hidden];
	}

	public float get_weight_hidden_output(int bird_tag, int n_hidden){
		return w_hidden_output[bird_tag, n_hidden];
	}

	public float get_bias_input_hidden(int bird_tag, int n_hidden){
		return b_input_hidden[bird_tag, n_hidden];
	}

	public float get_bias_hidden_output(int bird_tag){
		return b_hidden_output[bird_tag];
	}
}
