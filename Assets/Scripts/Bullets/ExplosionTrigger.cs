using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public List<Collider2D> Colliders { get; private set; } = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Colliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Colliders.Remove(collision);
    }
}
