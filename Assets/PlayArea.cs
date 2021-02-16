using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    Player ship;
    BoxCollider2D playZone;
    [SerializeField] private LayerMask wrapMask;

    float width;
    float height;

    private void Awake()
    {
        playZone = GetComponent<BoxCollider2D>();
        width = playZone.size.x;
        height = playZone.size.y;
    }
    // Start is called before the first frame update
    void Start()
    {
        ship = Player.instance;
       
    }
    Vector2 pos2D;
    // Update is called once per frame
    void Update()
    {
        transform.position = ship.gameObject.transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (wrapMask == (wrapMask | (1 << collision.gameObject.layer)))
        {
            Rigidbody2D otherRB = collision.gameObject.GetComponent<Rigidbody2D>();
            float rbX = otherRB.position.x;
            float rbY = otherRB.position.y;

            float xoffset = transform.position.x - rbX;
            float yoffset = transform.position.y - rbY;
            if (Mathf.Abs(xoffset) > width / 2)
            {
                rbX += Mathf.Sign(xoffset) * width;
            }
            if (Mathf.Abs(yoffset) > height / 2)
            {
                rbY += Mathf.Sign(yoffset) * height;
            }
            otherRB.interpolation = RigidbodyInterpolation2D.None;
            otherRB.position = new Vector2(rbX, rbY);
            otherRB.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
        else
        {
            GameObject.Destroy(collision.gameObject);
        }
    }
}
