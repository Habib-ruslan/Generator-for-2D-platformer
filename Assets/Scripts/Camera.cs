using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim.SetInteger("CutScene", PlayerPrefs.GetInt("Length"));
    }
    private void Update()
    {
        if (anim.GetInteger("CutScene") < 0) transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        else if (Input.GetKey(KeyCode.Space)) anim.SetInteger("CutScene", -1);
    }
}
