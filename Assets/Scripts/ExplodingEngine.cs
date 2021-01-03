using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ExplodingEngine : MonoBehaviour
{
    private IEnumerator explodeEngineFunction;
    private IEnumerator fadeEngineFunction;

    private Material material;
    private Light2D lt;

    private float intensity;
    private float intensityTarget = 5f;
    private float intensityTimeToMax = 6f;
    private float radiusTarget = 5f;
    private float originalRadius;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponentInChildren<SpriteRenderer>().material;
        foreach (Transform child in transform)
        { 
            lt = child.gameObject.GetComponent<Light2D>();
            if (lt != null)
            {
                break;
            }
        }
        Debug.Log("OK we have a " + lt);
        originalRadius = lt.pointLightOuterRadius;
    }

    public void Explode()
    { 
        if (material.HasProperty("GlowIntensity"))
        {
            Debug.Log("Starting Engine explosion coroutine");
            intensity = material.GetFloat("GlowIntensity");
            explodeEngineFunction = CoEngineExplode();
            fadeEngineFunction = CoEngineFade();
            StartCoroutine(explodeEngineFunction);
        }
    }

    public void FadeOut()
    {
        StopCoroutine(explodeEngineFunction);
        StartCoroutine(fadeEngineFunction);
    }

    private IEnumerator CoEngineFade()
    {
        while (intensity > 0)
        {
            intensity = Mathf.MoveTowards(intensity, 0f, Time.deltaTime);
            material.SetFloat("GlowIntensity", intensity);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CoEngineExplode()
    {
        float timeTotal = 0.001f;
        do
        {
            timeTotal += Time.deltaTime;
            intensity = 0.75f + intensityTarget * (timeTotal / intensityTimeToMax);
            lt.pointLightOuterRadius = originalRadius + ((timeTotal / intensityTimeToMax) * radiusTarget);
            material.SetFloat("GlowIntensity", intensity);
            //Debug.Log("Intensity is now at " + intensity);
        
            /*{
                material.SetFloat("GlowIntensity", 0.25f);
                lt.pointLightOuterRadius = 0.5f;
            }*/
            yield return new WaitForEndOfFrame();
        } while (true);
    }

}