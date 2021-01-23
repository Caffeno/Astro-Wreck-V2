using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{

    private Camera Camera;
    [SerializeField] private Player player;


    private void Start()
    {
        Camera = GetComponent<Camera>();
    }
    private void Update()
    {
        float inputV = Input.GetAxisRaw("Vertical");
        Camera.orthographicSize += Time.deltaTime * inputV;

        float inputH = Input.GetAxisRaw("Horizontal");
        player.GetComponent<Rigidbody2D>().rotation += inputH * Time.deltaTime * 30;
    }
}
