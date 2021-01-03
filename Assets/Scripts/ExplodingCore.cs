using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ExplodingCore : MonoBehaviour
{
    private ExplodingEngine engineScript;
    private ExplodingDrill drillScript;

    private IEnumerator explosion;
    private IEnumerator pulse;
    private Material coreMaterial;
    private Light2D lt;

    private float intensity;
    private float timeFlashScale = 1;
    private float intensityRange = 0.25f;
    private float intensityFloor = 0.75f;
    private float intensityFloorTarget = 0.75f;
    private float intensityRangeTarget = .25f;
    private float timeScaleDelta = 0f;
    private float originalRadius;
    private float supernovaValue = 0f;
    private bool isSupernova = false;

    // Start is called before the first frame update
    void Start()
    {
        coreMaterial = GetComponentInChildren<SpriteRenderer>().material;

        // Need references to other components to tell them when we explode
        engineScript = GameObject.FindObjectOfType(typeof(ExplodingEngine)) as ExplodingEngine;
        drillScript = GameObject.FindObjectOfType(typeof(ExplodingDrill)) as ExplodingDrill;

        foreach (Transform child in transform)
        {
            lt = child.gameObject.GetComponent<Light2D>();
            if (lt != null)
            {
                break;
            }
        }
        Debug.Log("ExplodingCore: OK we have a " + lt);
        originalRadius = lt.pointLightOuterRadius;

        if (coreMaterial.HasProperty("GlowIntensity"))
        {
            Debug.Log("Hello World starting pulse");

            intensity = coreMaterial.GetFloat("GlowIntensity");
            pulse = CoCorePulse();
            StartCoroutine(pulse);
        }
    }


    public void Explode(float explosionForce)
    {
        Debug.Log("Hello World 1");

        explosion = CoExplosion(explosionForce);
        StartCoroutine(explosion);
        engineScript.Explode();
    }

    private IEnumerator CoCorePulse()
    {
        float timeTotal = 0f;
        while (true)
        {
            if (intensityRange != intensityRangeTarget)
            {
                intensityRange = Mathf.MoveTowards(intensityRange, intensityRangeTarget, Time.deltaTime * 5);
            }
            if (intensityFloor != intensityFloorTarget)
            {
                intensityFloor = Mathf.MoveTowards(intensityFloor, intensityFloorTarget, Time.deltaTime * 5);
            }
            if (timeScaleDelta != 0)
            {
                timeFlashScale += timeScaleDelta;
            }
            timeTotal += Time.deltaTime * timeFlashScale;
            intensity = intensityRange * (-1 * Mathf.Sin(timeTotal)) + intensityRange + intensityFloor;
            coreMaterial.SetFloat("GlowIntensity", intensity);
            if (isSupernova)
            {
                supernovaValue = Mathf.MoveTowards(supernovaValue, 30f, 3f * Time.deltaTime);
                lt.pointLightOuterRadius = supernovaValue;
            } else
            {
                lt.pointLightOuterRadius = originalRadius * (intensity / 2f) + originalRadius * 0.33f;
            }
            yield return new WaitForEndOfFrame();

        }
    }

    private IEnumerator CoCoreFade()
    {
        while (intensity > 0)
        {
            intensity = Mathf.MoveTowards(intensity, 0f, Time.deltaTime);
            coreMaterial.SetFloat("GlowIntensity", intensity);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CoExplosion(float explosionForce)
    {
        timeScaleDelta = Mathf.MoveTowards(timeScaleDelta, 1f, 2*Time.deltaTime);
        intensityRangeTarget = 0.9f;
        intensityFloorTarget = 0.6f;
        isSupernova = true;

        yield return new WaitForSecondsRealtime(2f);
        Collider2D[] obInRadius = Physics2D.OverlapCircleAll(gameObject.transform.position, 10f);
        foreach (Collider2D other in obInRadius)
        {

            Rigidbody2D otherRB = other.gameObject.GetComponent<Rigidbody2D>();
            if (otherRB != null && otherRB.gameObject != gameObject)
            {
                Vector3 displacement3D = other.transform.position - gameObject.transform.position;
                Vector2 displacement = new Vector2(displacement3D.x, displacement3D.y);

                otherRB.AddForce((displacement.normalized) * explosionForce * (Random.value * 0.75f + 0.75f) / Mathf.Pow(displacement.magnitude, 2f));
                otherRB.angularVelocity += (Random.value - .5f) * 420f * (Random.value * 1.5f + 0.5f);

                StartCoroutine(FrictionDrop(otherRB));

            }
        }
        StopCoroutine(pulse);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        StartCoroutine(CoCoreFade());
        engineScript.FadeOut();
        drillScript.FadeOut();
    }

    private IEnumerator FrictionDrop(Rigidbody2D body)
    {
        Debug.Log("Lowering Drag for " + body.gameObject.name);
        float vMax = 0;
        float vMin = 0;
        float aMax = 0;
        float aMin = 0;
        while (vMax == 0)
        {
            vMax = body.velocity.magnitude;
            vMin = vMax * 0.1f;
            aMax = body.angularVelocity;
            aMin = aMax * 0.1f;

            yield return new WaitForEndOfFrame();
        }
        float initalDrag = body.drag;
        float initalaDrag = body.angularDrag;
        while (body.drag != 0 || body.angularDrag != 0)
        {

            float drag = initalaDrag * (body.velocity.magnitude - vMin) / (vMax - vMin);
            float adrag = initalaDrag * (body.angularVelocity - aMin) / (aMax - aMin);

            drag = drag < 0.01 ? 0 : drag;
            adrag = adrag < 0.01 ? 0 : adrag;

            body.drag = drag;
            body.angularDrag = adrag;

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Drag Gone for " + body.gameObject.name);

    }
}
