using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeronNetwork : MonoBehaviour {
	private float input_dis_width;
	private float input_dis_height;

	//從input到hidden的wight
	private float[,] w_input_hidden = new float[2, 6];
	//從input到hidden的bias
	private float[] b_input_hidden = new float[6];

	//從hidden到output的wight
	private float[] w_hidden_output = new float[6];
	//從idden到output的bias
	private float b_hidden_output;

	private GameObject genetic;
	private int tag;

	void Start () {
		tag = this.gameObject.GetComponent<BirdAction> ().tag;
		genetic = GameObject.FindWithTag("GameSetting");
		//Debug.Log (tag);
		//genetic = this.gameObject.GetComponent<BirdAction> ().genetic;
		for (int i = 0; i < 6; i++) {
			w_input_hidden[0, i] = genetic.GetComponent<Genetic>().get_weight_input_hidden (tag, 0, i);
			w_input_hidden[1, i] = genetic.GetComponent<Genetic>().get_weight_input_hidden (tag, 1, i);
			w_hidden_output[i] = genetic.GetComponent<Genetic>().get_weight_hidden_output (tag, i);
			b_input_hidden[i] = genetic.GetComponent<Genetic>().get_bias_input_hidden (tag, i);
		}
		b_hidden_output = genetic.GetComponent<Genetic>().get_bias_hidden_output(tag);
	}
	
	// Update is called once per frame
	void Update () {
		input_dis_width = this.gameObject.GetComponent<BirdAction>().get_width ();
		input_dis_height = this.gameObject.GetComponent<BirdAction>().get_height ();
		if(input_dis_width != -100 && input_dis_height != -100)
			getOutput ();
	}

	void getOutput(){
		float[] hidden_layer = new float[6];
		float output = 0;
		for (int i = 0; i < 6; i++) {
			//Input to hidden
			hidden_layer[i] = input_dis_width * w_input_hidden[0, i] + input_dis_height * w_input_hidden[1, i] + b_input_hidden[i];
			output += hidden_layer [i] * w_hidden_output [i];
		}
		output += b_hidden_output;

		if (Sigmoid(output) >= 0.5) {
			this.gameObject.GetComponent<BirdAction>().Fly ();
		}
	}


	private float Sigmoid(float x){
		return 1 / (1 + Mathf.Exp(-x));
	}
		
}
