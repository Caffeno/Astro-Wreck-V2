using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingDrill : MonoBehaviour
{
    
    private IEnumerator fadeDrillFunction;
    private Material material;
    private float intensity;
    
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponentInChildren<SpriteRenderer>().material;
        if (material.HasProperty("GlowIntensity"))
        {
            intensity = material.GetFloat("GlowIntensity");   
        }
        fadeDrillFunction = CoDrillFade();   
    }

    
    public void FadeOut()
    {
        StartCoroutine(fadeDrillFunction);
    }

    private IEnumerator CoDrillFade()
    {
        while (intensity > 0)
        {
            intensity = Mathf.MoveTowards(intensity, 0f, Time.deltaTime);
            material.SetFloat("GlowIntensity", intensity);
            yield return new WaitForEndOfFrame();
        }
    }

}