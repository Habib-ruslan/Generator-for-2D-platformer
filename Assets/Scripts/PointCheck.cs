using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PointCheck : MonoBehaviour
{
    public GameObject self;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Room")
        {
            Destroy(self);
        }
    }

}
