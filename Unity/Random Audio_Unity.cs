using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendContainer : MonoBehaviour
{
    public float MinTime = 2, MaxTime = 4;
    public float MinPitch = 1, MaxPitch = 1, MinVol = 1, MaxVol = 1;
    public bool Randomize;
    AudioSource RandomContainer;
    public AudioClip[] SoundClips;
    int new_ind, last_ind, i;

    void Start()
    {
        RandomContainer = gameObject.GetComponent<AudioSource>();
        CallAudio();

    }

    int Randomization(int ClipLength)
    {
        new_ind = Random.Range(0, ClipLength);

        while (new_ind == last_ind)
        {
            new_ind = Random.Range(0, ClipLength);
        }

        last_ind = new_ind;
        return new_ind;
    }
    void CallAudio()
    {
        Invoke("BlendClips", Random.Range(MinTime, MaxTime));
    }
    void BlendClips()
    {
        if (Randomize) i = Randomization(SoundClips.Length); else i = Random.Range(0, SoundClips.Length);
        RandomContainer.volume = Random.Range(MinVol, MaxVol);
        RandomContainer.pitch = Random.Range(MinPitch, MaxPitch);
        if (SoundClips.Length >0) RandomContainer.PlayOneShot(SoundClips[i]);
        CallAudio();
    }
}
