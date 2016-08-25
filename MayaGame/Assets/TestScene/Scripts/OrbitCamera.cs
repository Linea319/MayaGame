using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
    public float r = 4.0f;
    public float angularSpeed = -0.25f; //0.5f;
    public float y = 3.0f;

    public Transform lookAt;

    Transform cachedTransform;

    // Use this for initialization
    void Start()
    {
        cachedTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos;
        pos.x = r * Mathf.Cos(Time.timeSinceLevelLoad * angularSpeed);
        pos.y = y;
        pos.z = r * Mathf.Sin(Time.timeSinceLevelLoad * angularSpeed);
        cachedTransform.position = pos;
        cachedTransform.LookAt(lookAt != null ? lookAt.position : Vector3.zero);
    }
}
