using UnityEngine;

[System.Serializable]
public struct Loot 
{
    public GameObject obj;
    public float chance;
}
[RequireComponent(typeof(BoxCollider2D))]
public class Box : MonoBehaviour
{
    public Loot[] loot;
    public int lootCount;
    public void Open()
    {
        for (int i = 0; i < lootCount; i++)
        {
            Instantiate(loot[ChooseLoot()].obj, transform.position, Quaternion.identity);
        }
        Destroy(transform.gameObject);
    }
    private int ChooseLoot()
    {
        float rand = Random.Range(0, 100f);
        for(int i=0; i< loot.Length;i++)
        {
            if (rand <= loot[i].chance) return i;
            rand -= loot[i].chance;
        }
        return 0;
    }
}
