using UnityEngine;
using System.Collections;

public class lineController : MonoBehaviour {
    public LineRenderer line;
    public Transform[] positions;
	// Use this for initialization

	
	// Update is called once per frame
	void LateUpdate () {
        Vector3[] newPos = new Vector3[positions.Length];
        for(int i = 0; i < newPos.Length; i++)
        {
            newPos[i] = positions[i].position;
        }
        line.SetPositions(newPos);
    }
}
