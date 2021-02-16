using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDisplay : MonoBehaviour
{
    [SerializeField] private float fadeDelay = 5;
    [SerializeField] private float fadeOutDuration = 2;


    private TMPro.TMP_Text text;
    private IEnumerator textFadeOutCoroutine;
    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    void Start()
    {
        GameEvents.instance.waveStartTrigger += DisplayWaveNumber;
        textFadeOutCoroutine = FadeOutText();
        StartCoroutine(textFadeOutCoroutine);
    }

    private void DisplayWaveNumber(int waveNumber)
    {
        text.text = "Wave " + waveNumber;
        text.alpha = 1;
        textFadeOutCoroutine = FadeOutText();
        StartCoroutine(textFadeOutCoroutine);
    }

    private IEnumerator FadeOutText()

    {
        Debug.Log("text fade Started");

        yield return new WaitForSeconds(fadeDelay);
        float remainingTime = fadeOutDuration;
        do
        {
            remainingTime -= Time.deltaTime;

            text.alpha = Mathf.Lerp(0, 1,  Mathf.Pow(remainingTime / fadeOutDuration, 5));
            yield return new WaitForEndOfFrame();
        } while (text.alpha != 0);
        Debug.Log("fully Faded text");

    }
}
