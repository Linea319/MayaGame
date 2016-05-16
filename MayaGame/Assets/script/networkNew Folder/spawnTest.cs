using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class spawnTest : NetworkBehaviour {
    public Transform spawnPos;
    public GameObject spawnPrefab;

	// Use this for initialization
	public override void  OnStartServer() {

        GameObject obj = (GameObject)Instantiate(spawnPrefab, spawnPos.position, spawnPos.rotation);
        NetworkServer.Spawn(obj);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
