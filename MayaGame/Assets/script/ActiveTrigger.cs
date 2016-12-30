using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTrigger : MonoBehaviour
{
    public GameObject[] activeObjs;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.CompareTag("Player"))
        {
            for(int x = 0; x < activeObjs.Length; x++)
            {
                activeObjs[x].SetActive(true);
            }
        }
    }
}