using UnityEngine;
using System.Collections;

public class AnimEventSender : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AnimEvent(string eventName)
    {
        Debug.Log(eventName);
        transform.root.BroadcastMessage(eventName);
    }
}
