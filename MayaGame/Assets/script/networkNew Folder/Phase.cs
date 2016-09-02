using UnityEngine;
using System.Collections;

public class Phase : MonoBehaviour {
    GamePhaseManager manager;
    public bool clearFlag;
    public GameObject[] enableObjs;
    public GameObject[] disableObjs;
    public Behaviour[] enableBehaves;
    public Behaviour[] disableBehaves;
    // Use this for initialization
    void Start() {
        manager = GetComponent<GamePhaseManager>();
    }

    public virtual void StartPhasae()
    {
        for(int i = 0; i < enableObjs.Length; i++)
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

    public virtual void ClearPhase()
    {
        manager.NextPhase();
        this.enabled = false;
    }
}
