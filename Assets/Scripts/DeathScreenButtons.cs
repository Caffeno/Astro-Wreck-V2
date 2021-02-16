using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenButtons : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private ScoreDisplay scoreKeeper;
    [SerializeField] private TMPro.TMP_Text finalScoreText;

    private float fadeDuration = 1f;

    private void Start()
    {
        GameEvents.instance.onPlayerDeathEnter += OpenDeathScreen;
    }

    private void Update()
    {

    }

    public void OpenDeathScreen()
    {
        deathScreen.SetActive(true);
        finalScoreText.text += ScoreDisplay.scoreValue.ToString();
        scoreKeeper.gameObject.SetActive(false);
        ScreenFadeIn();
    }

    private void ScreenFadeIn()
    {
        List<Button> deathScreenButtons = new List<Button>();
        foreach (Transform child in deathScreen.transform)
        {
            Debug.Log(child.name);

            Image childImage = child.GetComponent<Image>();
            TMPro.TMP_Text childText;
            if (childImage == null)
            {
                childText = child.GetComponent<TMPro.TMP_Text>();

                StartCoroutine("FadeInText", childText);
            }
            else
            {
                StartCoroutine("FadeInImage", childImage);
                Button childButton = child.GetComponent<Button>();
                if (childButton != null)
                {
                    deathScreenButtons.Add(childButton);
                    childText = child.GetComponentInChildren<TMPro.TMP_Text>();
                    StartCoroutine("FadeInText", childText);

                }
            }
        }
    }
    
    private float smoothStartN(float x, float N)
    {
        return Mathf.Pow(x, N);
    }

    private IEnumerator FadeInImage(Image img)
    {
        float targetAlpha = img.color.a;
        float alpha;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        float fadeTime = 0f;
        
        yield return new WaitForSeconds(2.5f);
        do
        {
            fadeTime += Time.deltaTime;
            alpha = Mathf.Lerp(0, targetAlpha, fadeTime/ fadeDuration);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return new WaitForEndOfFrame();
        } while (alpha < targetAlpha);
    }

    private IEnumerator FadeInText(TMPro.TMP_Text text)
    {
        float targetAlpha = text.alpha;
        float alpha = 0;
        text.alpha = 0;
        float fadeTime = 0;
        yield return new WaitForSeconds(2.5f);
        do {
            fadeTime += Time.deltaTime;
            alpha = Mathf.Lerp(alpha, targetAlpha, smoothStartN(fadeTime/ fadeDuration, 5));
            text.alpha = alpha;
            yield return new WaitForEndOfFrame();
        } while (alpha < targetAlpha);
    }
}
