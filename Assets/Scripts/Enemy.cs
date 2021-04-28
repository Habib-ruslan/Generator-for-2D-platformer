using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    private float Point;
    public float Deviation;
    public float speed;
    public int damage;
    public int hp;

    private void Start()
    {
        Point = transform.position.x;
    }
    private void FixedUpdate()
    {
        Move();
        Alive();
    }
    private void Move()
    {
        transform.Translate(transform.right *speed*Time.fixedDeltaTime);
        if (transform.position.x > Point + Deviation || transform.position.x < Point - Deviation)
        {
            Flip();
        }
    }
    private void Flip()
    {
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
        speed *= -1;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag =="Player")
        {
            col.GetComponent<Player>().TakeDamage(damage);
        }
    }
    private void Alive()
    {
        if (hp <= 0) Destroy(transform.gameObject);
    }
    public void TakeDamage(int _damage, GameObject particle)
    {
        hp -= damage;
        GameObject effect = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
    }
}
