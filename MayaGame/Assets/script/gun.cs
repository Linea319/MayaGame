using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gun : parameter {
	public string[] useParams = new string[]{"damage","accuracy","recoil","range","mobility","reload","weight"};
	public parameter[] parts;
	public muzule muzuleP;
	public upper upperP;
	public magwell magwellP;
	public magazine magazineP;


	// Use this for initialization
	void Start () {

		//CombineParam();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CombineParam(){
		parts = GetComponentsInChildren<parameter>();
		for(int i=0;i<useParams.Length;i++){
			parameters[useParams[i]] =0;
		}
		foreach(parameter item in parts){
			for(int i=0;i<useParams.Length;i++){
				parameters[useParams[i]] +=  item.parameters[useParams[i]];

			}
		}

		muzuleP = GetComponentInChildren<muzule>();
		upperP = GetComponentInChildren<upper>();
		magwellP = GetComponentInChildren<magwell>();
		magazineP = GetComponentInChildren<magazine>();
		partsName = "gun";
		//PartsSave 
		//PartsSave.SaveParts(this);
		Debug.Log(parts.Length);
	}
}
