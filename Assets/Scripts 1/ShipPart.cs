using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class ShipPart : MonoBehaviour
{
    private IEnumerator fadePartFunction;
    protected Material material;
    protected float intensity;

    // Start is called before the first frame update


    public virtual void FadeOut()
    {
        material = GetComponent<SpriteRenderer>().material;
        if (material.HasProperty("GlowIntensity"))
        {
            intensity = material.GetFloat("GlowIntensity");
        }
        fadePartFunction = CoPartFade();
        StartCoroutine(fadePartFunction);
    }

    public virtual void Explode()
    {

    }

    private IEnumerator CoPartFade()
    {
        Light2D componentLight = GetComponentInChildren<Light2D>();

        while (intensity > 0)
        {
            intensity = Mathf.MoveTowards(intensity, 0f, Time.deltaTime);
            Debug.Log(gameObject.name + " intensity now at " + intensity);
            material.SetFloat("GlowIntensity", intensity);

            if (componentLight != null)
            {
                componentLight.pointLightOuterRadius *= 0.5f;
            }
            
            yield return new WaitForEndOfFrame();
        }
        if (componentLight != null)
        {
            componentLight.enabled = false;
        }
    }
}
