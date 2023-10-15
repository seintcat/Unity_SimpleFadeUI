using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    [SerializeField]
    List<FadeData> fadeDatas = new List<FadeData>();
    [SerializeField]
    Image imageFade;
    [SerializeField]
    AspectRatioFitter fitter;

    IEnumerator fadeNow;
    WaitForSeconds fadeWait;
    int fadeIndex;
    Action actionInFade;

    private void Awake()
    {
        if (imageFade == null)
        {
            Error("Can't find fade image!");
            return;
        }
        if (fadeDatas.Count < 1)
        {
            Error("Can't find fade datas!");
            return;
        }
        if (fitter == null)
        {
            Error("Can't find aspect ratio fitter!");
            return;
        }

        fadeIndex = 0;
        imageFade.raycastTarget = false;
        imageFade.enabled = false;
        imageFade.color = Color.white;
    }

    public void FadeStart(Action action, int fadeDataIndex = -1)
    {
        // Reset
        actionInFade = action;
        imageFade.raycastTarget = true;
        imageFade.enabled = true;

        // Check fade data
        if (fadeDataIndex > -1)
        {
            fadeIndex = fadeDataIndex;
        }
        if(fadeDatas[fadeIndex] == null)
        {
            Error("Wrong fade data index!");
            return;
        }

        // Fade in
        if (fadeNow != null)
        {
            StopCoroutine(fadeNow);
        }
        if (fadeDatas[fadeIndex].sprite != null)
        {
            imageFade.sprite = fadeDatas[fadeIndex].sprite;
            fitter.aspectRatio = fadeDatas[fadeIndex].sprite.textureRect.width / fadeDatas[fadeIndex].sprite.textureRect.height;
        }
        else
        {
            imageFade.sprite = null;
        }
        fadeNow = FadeIn();

        // Check fade in skipped
        if (fadeDatas[fadeIndex].fadeOption == FadeOption.ClipFadeIn)
        {
            // Skip to fade out
            FadeOutReset();
            return;
        }
        StartCoroutine(fadeNow);
    }
    public void FadeStart(Action action, FadeData data) 
    {
        if (fadeDatas.Find(x => x == data) != null)
        {
            fadeIndex = fadeDatas.IndexOf(data);
        }
        FadeStart(action);
    }

    private IEnumerator FadeIn()
    {
        fadeWait = new WaitForSeconds(fadeDatas[fadeIndex].fadeIn.fadeTime);
        fadeDatas[fadeIndex].fadeIn.fadeColor.a = 0;
        imageFade.CrossFadeColor(fadeDatas[fadeIndex].fadeIn.fadeColor, 0, false, true);
        fadeDatas[fadeIndex].fadeIn.fadeColor.a = 1;
        imageFade.CrossFadeColor(fadeDatas[fadeIndex].fadeIn.fadeColor, fadeDatas[fadeIndex].fadeIn.fadeTime, false, true);
        yield return fadeWait;

        FadeOutReset();
    }
    private void FadeOutReset()
    {
        // Invoke action between fade in and out
        actionInFade.Invoke();

        // Check fade out skipped
        if (fadeDatas[fadeIndex].fadeOption == FadeOption.ClipFadeOut)
        {
            imageFade.enabled = false;
            return;
        }

        // Fade out
        if (fadeNow != null)
        {
            StopCoroutine(fadeNow);
        }
        fadeNow = FadeOut();
        StartCoroutine(fadeNow);
    }
    private IEnumerator FadeOut()
    {
        fadeWait = new WaitForSeconds(fadeDatas[fadeIndex].fadeOut.fadeTime);
        if (fadeDatas[fadeIndex].fadeOption == FadeOption.ClipFadeIn)
        {
            fadeDatas[fadeIndex].fadeOut.fadeColor.a = 1;
            imageFade.CrossFadeColor(fadeDatas[fadeIndex].fadeOut.fadeColor, 0, false, true);
        }
        fadeDatas[fadeIndex].fadeOut.fadeColor.a = 0;
        imageFade.CrossFadeColor(fadeDatas[fadeIndex].fadeOut.fadeColor, fadeDatas[fadeIndex].fadeOut.fadeTime, false, true);
        yield return fadeWait;

        // Fade done
        imageFade.raycastTarget = false;
        imageFade.enabled = false;
    }

    private void Error(string message)
    {
        Debug.LogError(message);
        Destroy(gameObject);
    }
}
