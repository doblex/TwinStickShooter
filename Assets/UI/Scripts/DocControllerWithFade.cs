using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DocControllerWithFade : DocController
{
    [Header("Transition Options")]
    [SerializeField] protected bool fadeInActive = true;
    [SerializeField] protected float fadeInDuration = 0.5f;
    [SerializeField] protected bool fadeOutActive = true;
    [SerializeField] protected float fadeOutDuration = 0.5f;


    protected Coroutine currentFadeRoutine;

    public override void ShowDoc(bool show)
    {
        if (currentFadeRoutine != null)
            StopCoroutine(currentFadeRoutine);

        if (show && fadeInActive)
        {
            currentFadeRoutine = StartCoroutine(FadeIn());
        }
        else if (!show && fadeOutActive)
        {
            currentFadeRoutine = StartCoroutine(FadeOut());
        }
    }

    protected virtual IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float startOpacity = Root.style.opacity.value;
        float endOpacity = 1f;

        Root.style.display = DisplayStyle.Flex;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startOpacity, endOpacity, elapsed / fadeInDuration);
            Root.style.opacity = newOpacity;
            yield return null;
        }

        Root.style.opacity = endOpacity;
    }

    protected virtual IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float startOpacity = Root.style.opacity.value;
        float endOpacity = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startOpacity, endOpacity, elapsed / fadeOutDuration);
            Root.style.opacity = newOpacity;
            yield return null;
        }

        Root.style.opacity = endOpacity;
        Root.style.display = DisplayStyle.None;
    }
}
