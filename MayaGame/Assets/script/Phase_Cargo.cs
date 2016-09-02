using UnityEngine;
using System.Collections;

public class Phase_Cargo :Phase {

    public Transform cargo;
    public Transform goal;

	void Update()
    {
        if((cargo.position-goal.position).sqrMagnitude < 0.5f)
        {
            ClearPhase();
        }
    }
}
