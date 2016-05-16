using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class muzule : parameter {
	/*public Dictionary<string,float> parameters = new Dictionary<string,float>(){
		{"damage",0 },
		{"accuracy",0 },
		{"recoil",0},
		{"range",0},
		{"weight",0}
	};*/
	string[] useParams = new string[]{"damage","accuracy","recoil","range","weight"};
	public bool supplesser;
	public ParticleSystem muzuleFlash;
	public AudioSource shotsound;
	// Use this for initialization
	void Start () {
		partsType = PartsType.muzzle;
		base.useParams = useParams;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override float ModifiParam ()
	{
		if(supplesser){
			return -20f;
		}
		else{
			return 0;
		}
	}

	public override void DivideParm ()
	{	base.useParams = useParams;
		partsType = PartsType.muzzle;
		base.DivideParm ();
	}
}
