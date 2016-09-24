using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Phase_Cargo :Phase {

    public Transform cargo;
    public Transform goal;


    public override void StartPhasae()
    {
        base.StartPhasae();
        
    }

    [ServerCallback]
    void Update()
    {
        if((cargo.position-goal.position).sqrMagnitude < 0.5f)
        {
            cargo.GetComponent<Payload>().Stop();
            ClearPhase();
        }
    }
}
