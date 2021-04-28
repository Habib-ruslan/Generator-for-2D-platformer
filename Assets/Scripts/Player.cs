using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private int maxJump = 2; 
    private int JumpCount;
    private bool isRight = true;

    private int money = 0;
    private int hp;
    private bool isInvuln = false;
    private bool AttackReady = true;

    private Text Money;
    private Text Hp;
    private GameObject Camera;

    private Animator anim;
    private Quaternion Offset;
    public GameObject menuWindow;
    
    [Header("Передвижение")]
    public float speed;
    public float jumpPow;
    public Transform checkGround;
    public Vector2 SizeOfCheckGround;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsBox;

    [Header("Боевая система")]
    public int MaxHp;
    public GameObject bullet;
    public Transform AttackPoint;
    public GameObject Particle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Find();
        UpdateHp();
        UpdateMoney();
        hp = MaxHp;
        ResetJumps();
    }

    private void Update()
    {
        Move();
        Jump();
        if (Input.GetButton("Fire1")) Attack();
        else if (Input.GetButton("Escape")) menuWindow.SetActive(true);
    }
    private void FixedUpdate()
    {
        if (onGround())
        {
            ResetJumps();
        }
        OpenBox();
    }
    private bool onGround()
    {
        return Physics2D.OverlapBox(checkGround.position, SizeOfCheckGround, 0f, WhatIsGround);
    }
    private void OpenBox()
    {
        Collider2D[] boxes = Physics2D.OverlapBoxAll(checkGround.position, SizeOfCheckGround, 0f, WhatIsBox);
        for(int i =0; i < boxes.Length; i++)
        {
            boxes[i].GetComponent<Box>().Open();
        }
    }
    private void ResetJumps()
    {
        JumpCount = maxJump;
    }
    private void Move()
    {
        float X = Input.GetAxis("Horizontal");
        if (X !=0)
        {
            rb.velocity = new Vector2(X*speed, rb.velocity.y);
            if ((X < 0 && isRight) || ((X > 0) && !isRight)) Flip();
        }
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && JumpCount > 0)
        {
            rb.velocity = Vector2.up * jumpPow;
            JumpCount--;
        }
    }
 
    private void Flip()
    {
        isRight = !isRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(checkGround.position, SizeOfCheckGround);
    }
    public void AddMoney(int _money)
    {
        money += _money;
        UpdateMoney();
    }
    public void Heal(int heal)
    {
        hp += heal;
        UpdateHp();
    }
    public void TakeDamage(int OtherDamage)
    {
        if (!isInvuln)
        {
            anim.SetBool("hurt", true);
            hp -= OtherDamage;
            UpdateHp();
            Alive();
            StartCoroutine(Invuln());
        }
    }
    private IEnumerator Invuln()
    {
        isInvuln = true;
        yield return new WaitForSeconds(2f);
        anim.SetBool("hurt", false);
        isInvuln = false;
    }
    private void Attack()
    {
        if (AttackReady)
        {
            StartCoroutine(AttackReload());
            if (transform.localScale.x < 0)
            {
                Offset.z += 180f;
                Instantiate(bullet, AttackPoint.position, Offset);
                Offset.z -= 180f;
            }
            else
            {
                Instantiate(bullet, AttackPoint.position, Offset);
            }
        }
    }
    private IEnumerator AttackReload()
    {
        AttackReady = false;
        yield return new WaitForSeconds(0.5f);
        AttackReady = true;
    }
    private void Find()
    {
        Money = GameObject.Find("Canvas/Money").GetComponent<Text>();
        Hp = GameObject.Find("Canvas/Hp").GetComponent<Text>();
        Camera = GameObject.Find("MainCamera");
    }

    private void UpdateHp()
    {
        Hp.text = hp.ToString();
    }
    private void UpdateMoney()
    {
        Money.text = money.ToString();
    }
    private void Alive()
    {
        if (hp <= 0)
        {
            anim.SetBool("die", true);
        }
    }
    public void AnimDestroy()
    {
        Destroy(transform.gameObject);
    }
    public void AnimCreateParticle()
    {
        GameObject effect = Instantiate(Particle, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
    }

}
