using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/FadeData", fileName ="FadeData")]
public class FadeData : ScriptableObject
{
    public FadeOption fadeOption;
    public Sprite sprite;

    public FadeOnce fadeIn;
    public FadeOnce fadeOut;
}

[System.Serializable]
public struct FadeOnce
{
    public Color fadeColor;
    public float fadeTime;
}

public enum FadeOption
{
    Default,
    ClipFadeIn,
    ClipFadeOut,
}
