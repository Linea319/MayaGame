using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum ammoType{
	pistol,
	rifle_L,
	rifle_H,
	sniper,
	Laser
}

public class magazine : parameter {
	/*public Dictionary<string,float> parameters = new Dictionary<string,float>(){
		{"damage",0 },
		{"accuracy",0 },
		{"recoil",0},
		{"range",0},
		{"weight",0}
	};*/
	string[] useParams = new string[]{"damage","reload","weight"};

	public ammoType type;
	public int capacityDef;
	public GameObject hitEffect;
	int capacity;
	// Use this for initialization
	void Start () {
		partsType = PartsType.magazine;
		base.useParams = useParams;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void DivideParm ()
	{	base.useParams = useParams;
		partsType = PartsType.magazine;
		base.DivideParm ();
	}

	public override float ModifiParam ()
	{
		float magOffset = Random.Range(0.8f,1.2f);
		capacityDef = (int)(capacityDef*magOffset);
		specialParam = capacityDef.ToString();
		magOffset = -(magOffset-1f)*100;
		return magOffset;


	}
}
