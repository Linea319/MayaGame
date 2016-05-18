using UnityEngine;
using System.Collections;

public class CamStabilaizer : MonoBehaviour {
    public Transform target;
    public bool stabX;
    public bool stabY;
    public bool stabZ;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 rotVec = transform.eulerAngles;
        if (stabX)
        {
            rotVec.x = target.eulerAngles.x;
        }
        if (stabY)
        {
            rotVec.y = target.eulerAngles.y;
        }
        if (stabZ)
        {
            rotVec.z = target.eulerAngles.z;
        }
        Quaternion rot = Quaternion.Euler(rotVec);
        transform.rotation = rot;
    }
}
