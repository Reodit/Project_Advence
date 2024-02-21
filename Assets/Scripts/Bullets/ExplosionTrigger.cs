using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public List<Collider2D> Colliders { get; private set; } = new List<Collider2D>();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Colliders.Add(collision);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        Colliders.Remove(collision);
    }
}
