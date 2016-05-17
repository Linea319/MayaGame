using UnityEngine;
using System.Collections;

public class CamStabilaizer : MonoBehaviour {
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
            rotVec.x = transform.root.eulerAngles.x;
        }
        if (stabY)
        {
            rotVec.y = transform.root.eulerAngles.y;
        }
        if (stabZ)
        {
            rotVec.z = transform.root.eulerAngles.z;
        }
        Quaternion rot = Quaternion.Euler(rotVec);
        transform.rotation = rot;
    }
}
