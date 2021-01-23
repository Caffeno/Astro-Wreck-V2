using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CenterOfMassTracker : MonoBehaviour
{

    private Rigidbody2D ship;
    private Rigidbody2D teatherTarget;
    [SerializeField] private GameObject centerOfMassCamera;


    void Start()
    {
        ship = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (teatherTarget != null)
        {
            SetPosition();
        }
    }

    public void Attach(Rigidbody2D target)
    {
        teatherTarget = target;
        centerOfMassCamera.SetActive(true);
        SetPosition();
    }

    public void Detach()
    {
        teatherTarget = null;
        centerOfMassCamera.SetActive(false);
    }

    private void SetPosition()
    {
        Vector2 offset = teatherTarget.position - ship.position;
        float massFactor = 1 - (ship.mass / (ship.mass + teatherTarget.mass));
        transform.position = ship.position + offset * massFactor;
    }
}
