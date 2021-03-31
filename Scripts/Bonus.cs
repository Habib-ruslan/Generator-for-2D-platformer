using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bonus : MonoBehaviour
{
    public typeBonus type;
    public Animator anim;
    public int power;
    public GameObject self;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if(!anim.GetBool("active"))
            {
                anim.SetBool("active", true);
                Use(col);
            }
        }
    }
    public void AnimDestroy()
    {
        Destroy(self);
    }
    private void Use(Collider2D col)
    {
        switch (type)
        {
            case typeBonus.money: col.GetComponent<Player>().AddMoney(power); break;
            case typeBonus.heal: col.GetComponent<Player>().Heal(power); break;
        }
    }


}
