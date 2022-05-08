using System.Collections;
using UnityEngine;

namespace STP.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentFade = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            // StartCoroutine(FadeOutThenIn());
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        IEnumerator FadeOutThenIn()
        {
            yield return Fade(1f, 3f);
            print("Faded Out");
            yield return Fade(0f, 1f);
            print("Faded In");
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1f, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0f, time);
        }

        private Coroutine Fade(float target, float time)
        {
            if (currentFade != null)
            {
                StopCoroutine(currentFade);
            }
            currentFade = StartCoroutine(FadeRoutine(target, time));
            return currentFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
