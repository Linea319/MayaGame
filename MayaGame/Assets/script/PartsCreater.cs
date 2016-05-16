using UnityEngine;
using System.Collections;

public class PartsCreater : MonoBehaviour {
	public enum PartsType{
		muzule,
		handguard,
		upper,
		lower,
		magwell,
		grip,
		stock,
		magazine,
	}
	public PartsType parts;
	public GameObject[] dropParts;
	public float dropRate;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Drop(){
		if(Random.Range(0,100)>dropRate){
			return;
		}
	}
}
