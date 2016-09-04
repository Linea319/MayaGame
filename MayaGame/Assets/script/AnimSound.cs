using UnityEngine;
using System.Collections;

public class AnimSound : MonoBehaviour {
    public AudioSource[] audio;

    public void CallSound(int slot=0)
    {
        audio[slot].Play();
    }
}
