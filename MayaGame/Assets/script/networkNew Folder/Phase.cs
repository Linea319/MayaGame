using UnityEngine;
using System.Collections;

public class Phase : MonoBehaviour {
    GamePhaseManager manager;
    public 
    bool clearFlag;
    // Use this for initialization
    void Start() {
        manager = GetComponent<GamePhaseManager>();
        StartPhasae();
    }

    public virtual void StartPhasae()
    {
        ClearPhase();
    }

    public virtual void ClearPhase()
    {
        manager.NextPhase();
        this.enabled = false;
    }
}
