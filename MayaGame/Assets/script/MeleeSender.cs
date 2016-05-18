using UnityEngine;
using System.Collections;

public class MeleeSender : MonoBehaviour {
    public Melee_Wepon root;
	


    void OnTriggerEnter(Collider col)
    {
        root.MeleeHit(col);
    }
}
