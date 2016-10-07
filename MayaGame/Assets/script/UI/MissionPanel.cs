using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;

public class MissionPanel : MonoBehaviour {
    public string[] scenes;
    public Sprite[] sceneImage;
    [TextAreaAttribute]
    public string[] sceneObject;
    [TextAreaAttribute]
    public string[] sceneOverview;
    public Text sceneName;
    public Image image;
    public Text objective;
    public Text overview;
    int cSlot;
    LobbyManager lobbyMng;

	// Use this for initialization
    
	void Start () {
        lobbyMng = transform.root.GetComponent<LobbyManager>();
       StartCoroutine(delayChange(0));
	}
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator delayChange(int slot)
    {
        yield return new WaitForEndOfFrame();
        ChangePlayScene(slot);
    }

    //[ServerCallback]
    public void ChangePlayScene(int slotChange)
    {
        //Debug.Log("a" + lobbyMng.isHost);
        

        if (lobbyMng.isHost) {
        cSlot += slotChange;
        cSlot = Mathf.Clamp(cSlot, 0, scenes.Length - 1);
        sceneName.text = scenes[cSlot];
        lobbyMng.playScene = scenes[cSlot];
        image.sprite = sceneImage[cSlot];
        objective.text = sceneObject[cSlot];
        overview.text = sceneOverview[cSlot];
        lobbyMng.hostPlayer.SendMission(gameObject, cSlot);
        }

    }

    //[ClientRpc]
    public void RpcChangeScene(int slot)
    {
        cSlot = slot;
        sceneName.text = scenes[cSlot];
        lobbyMng.playScene = scenes[cSlot];
        image.sprite = sceneImage[cSlot];
        objective.text = sceneObject[cSlot];
        overview.text = sceneOverview[cSlot];
    }
}
