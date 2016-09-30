using UnityEngine;
using System.Collections;

public class Atachment : MonoBehaviour {

    public Vector3 offset;
    public GunParameter hosei;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void initialize(Wepon wep)
    {
        transform.localPosition = offset;
        wep.pa.Add(hosei);
        wep.initialize();
    }
}
