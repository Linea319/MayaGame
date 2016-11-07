using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bag : NetworkBehaviour {
    public float moveDebuf = 0.7f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Kaisyu(Transform player)
    {
        player.GetComponent<FPSController>().SetBag(true,moveDebuf);
        NetworkServer.Destroy(gameObject);
    }
}
