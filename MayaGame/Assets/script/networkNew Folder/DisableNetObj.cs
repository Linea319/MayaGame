using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DisableNetObj : NetworkBehaviour {

    [SerializeField]
    Behaviour[] behaviours;
    [SerializeField]
    Renderer[] activeMesh;

    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = false;
            }
            foreach (var mesh in activeMesh)
            {
                mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (isLocalPlayer)
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = focusStatus;
            }
            foreach (var mesh in activeMesh)
            {
                mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
    }
}
