using UnityEngine;
using System.Collections;

public class shake : MonoBehaviour {
    public float dumper=1f;
    public float spring = 1f;
    Vector3 shakeVec;
    private Vector3 nowPos;
    private float power;
    private bool back;
	// Use this for initialization
	void Start () {
        ShakeStart(new Vector3(0f, 10f, 0f), 1f);
        Debug.Log("shake");
    }
	
	// Update is called once per frame
	void LateUpdate () {
	    if(power != 0)
        {
            if (!back)
            {
                nowPos = Vector3.Lerp(nowPos, shakeVec, power * dumper * Time.deltaTime);
                if ((nowPos - shakeVec).sqrMagnitude <= 0.0001f) back = true;
            }
            else
            {
                nowPos = Vector3.Lerp(nowPos, Vector3.zero, power * dumper * Time.deltaTime);
                if ((nowPos - Vector3.zero).sqrMagnitude <= 0.0001f) back = false;
            }

            transform.position += nowPos;
            Debug.Log("shake");
        }
	}

    IEnumerator ShakeStart(Vector3 vec, float time)
    {
        Debug.Log("shake");
        shakeVec = vec;
        power = shakeVec.magnitude;
        yield return new WaitForSeconds(time);
        shakeVec = Vector3.zero;
        power = 0;
        Debug.Log("shakeStop");

    }
}
