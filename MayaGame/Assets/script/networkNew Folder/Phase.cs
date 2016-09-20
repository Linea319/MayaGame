using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Phase : NetworkBehaviour {
    GamePhaseManager manager;
    public bool clearFlag;
    public string taskText;
    public GameObject[] enableObjs;
    public GameObject[] disableObjs;
    public Behaviour[] enableBehaves;
    public Behaviour[] disableBehaves;
    // Use this for initialization
    void Start() {
        
    }

    public virtual void StartPhasae()
    {
        manager = GetComponent<GamePhaseManager>();
        GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>().SetTaskText(taskText);
        for (int i = 0; i < enableObjs.Length; i++)
        {
            enableObjs[i].SetActive(true);
        }
        for (int i = 0; i < disableObjs.Length; i++)
        {
            disableObjs[i].SetActive(false);
        }
        for (int i  =0; i < enableBehaves.Length; i++)
        {
            enableBehaves[i].enabled = true;
        }
        for (int i = 0; i < disableBehaves.Length; i++)
        {
            disableBehaves[i].enabled = false;
        }
    }

    [ServerCallback]  
    public virtual void ClearPhase()
    {
        manager.NextPhase();
        this.enabled = false;
    }
}
