using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDemo : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;
    public FadeUI fadeUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fade(int index)
    {
        fadeUI.FadeStart(new Action(FadeMiddle), index);
    }

    public void FadeMiddle()
    {
        source.PlayOneShot(clip);
    }
}
