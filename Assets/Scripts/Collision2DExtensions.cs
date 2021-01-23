using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collision2DExtensions
{
    public static Vector2 GetImpactForce(this Collision2D collision)
    {
        Vector2 impulse = new Vector2(0f, 0f);

        foreach (ContactPoint2D point in collision.contacts)
        {
            impulse += point.normalImpulse * point.normal;
        }

        return impulse / Time.fixedDeltaTime;
    }
}
