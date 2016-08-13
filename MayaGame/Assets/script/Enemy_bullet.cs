using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Enemy_bullet : NetworkBehaviour {
    public float lifeTime = 4;
    float startTime;
	// Use this for initialization
    [ServerCallback]
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
	if(Time.time > startTime + lifeTime)
        {
            NetworkServer.Destroy(gameObject);
        }
	}


}
