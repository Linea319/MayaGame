using UnityEngine;
using System.Collections;

public class slotButton : MonoBehaviour {
	int number;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetNum(int num){
		number = num;
	}

	public void ApplyParts(){
		transform.SendMessageUpwards("ChangeParts",number);
	}
}
