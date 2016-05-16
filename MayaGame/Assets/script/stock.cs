using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class stock : parameter {
	/*public Dictionary<string,float> parameters = new Dictionary<string,float>(){
		{"damage",0 },
		{"accuracy",0 },
		{"recoil",0},
		{"range",0},
		{"weight",0}
	};*/
	string[] useParams = new string[]{"accuracy","recoil","mobility","weight"};
	// Use this for initialization
	void Start () {
		partsType = PartsType.stock;
		base.useParams = useParams;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void DivideParm ()
	{	base.useParams = useParams;
		partsType = PartsType.stock;
		base.DivideParm ();
	}
}
