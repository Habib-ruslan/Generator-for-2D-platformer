using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject self;
    public float deathTime;
    public LayerMask target;
    public float rayDistance;
    public GameObject particle;


    private void Start()
    {
        Destroy(self, deathTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.right * Time.fixedDeltaTime * speed, Space.World);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, rayDistance, target);
        if (ray)
        {
            ray.collider.GetComponent<Enemy>().TakeDamage(damage, particle);
            Destroy(self);
        }
    }

}

