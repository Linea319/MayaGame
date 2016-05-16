using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class upper : parameter {
	/*public Dictionary<string,float> parameters = new Dictionary<string,float>(){
		{"damage",0 },
		{"accuracy",0 },
		{"recoil",0},
		{"range",0},
		{"weight",0}
	};*/
	string[] useParams = new string[]{"damage","accuracy","range","weight"};
	public float rpm;
	// Use this for initialization
	void Start () {
		partsType = PartsType.upper;
		base.useParams = useParams;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override float ModifiParam ()
	{
		rpm = rpm+Random.Range(-150,150);
		return 0;
	}

	public override void DivideParm ()
	{	base.useParams = useParams;
		partsType = PartsType.upper;
		base.DivideParm ();
	}
}
