using UnityEngine;
using System.Collections;

public class FovSync : MonoBehaviour {
    Camera myCamera;
    Camera mainCam;
	// Use this for initialization
	void Start () {
        myCamera = GetComponent<Camera>();
        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        myCamera.fieldOfView = mainCam.fieldOfView;
	}
}
