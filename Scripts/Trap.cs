using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Trap : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag=="Player")
        {
            col.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
