using UnityEngine;
using System.Collections;

public class canvasCamera : MonoBehaviour {
    public string cameraName;
	// Use this for initialization
	void Start () {
        if(cameraName != null)
        {
            GetComponent<Canvas>().worldCamera = GameObject.Find(cameraName).GetComponent<Camera>();
        }
        //GetComponent<Canvas>().worldCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
