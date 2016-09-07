using UnityEngine;
using System.Collections;

public class GiziBuillboard : MonoBehaviour {
    Camera mainCam;
	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(mainCam.transform);
        transform.localRotation *= Quaternion.Euler(0, 180, 0);
	}
}
