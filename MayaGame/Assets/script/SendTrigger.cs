using UnityEngine;
using System.Collections;

public class SendTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter");
        transform.root.SendMessage("EnterNear", other);
    }
}
