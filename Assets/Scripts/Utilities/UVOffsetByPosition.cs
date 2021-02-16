using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVOffsetByPosition : MonoBehaviour
{
    public float paralaxLevel = 10;

    private MeshRenderer renderer;
    private Material material;
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        material = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newOffset;
        newOffset.x = transform.position.x / transform.localScale.x / paralaxLevel;
        newOffset.y = transform.position.y / transform.localScale.y / paralaxLevel;

        material.SetTextureOffset("_MainTex", newOffset);
    }
}
