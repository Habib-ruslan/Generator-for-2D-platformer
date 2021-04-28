using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private float[] lowChance = { 48f, 30f, 20f, 2f };
    private float[] mediumChance = { 15f, 25f, 20f, 40f };
    private float[] highChance = { 10f, 20f, 20f, 50f };
    private float[] chance;
    private float[] defaultChance;
    private float ChanceBooster;
    private float lowChanceBooster = 1f;
    private float mediumChanceBooster = 0.75f;
    private float highChanceBooster = 0.25f;
    private float minChance = 3f;
    private float customChanceBooster = 0.5f;
    private List<GameObject[]> RoomVariant;
    private List<Vector3> Rooms;

    [Header("Параметры генерации")]
    public GameObject[] RoomVariantEnd; 
    public GameObject[] RoomVariant2;
    public GameObject[] RoomVariant3;
    public GameObject[] RoomVariant4;
    private Point[] StartPoint;
    public settings Length;
    [Header("For custom")]
    public float Chance1;
    public float Chance2;
    public float Chance3;

    private void Start()
    {
        Length = (settings) PlayerPrefs.GetInt("Length");
        print("Длина -" + Length);

        if (Length == settings.custom)
        {
            int _chance = PlayerPrefs.GetInt("Custom");
            Chance1 = _chance / 10000;
            Chance2 = (_chance % 10000) / 100;
            Chance3 = _chance % 100;
            print(Chance1 + "  " + Chance2 + "  " + Chance3);
        }

        StartPoint = GetComponent<Room>().Points;

        RoomVariant = new List<GameObject[]>
        {
            RoomVariantEnd,
            RoomVariant2,
            RoomVariant3,
            RoomVariant4
        };

        Rooms = new List<Vector3>();
        Rooms.Add(transform.position);

        switch (Length)
        {
            case settings.low: chance = lowChance; ChanceBooster = lowChanceBooster; break;
            case settings.medium: chance = mediumChance; ChanceBooster = mediumChanceBooster; break;
            case settings.high: chance = highChance; ChanceBooster = highChanceBooster; break;
            case settings.custom: chance = new float[] { Chance1, Chance2, Chance3, 100f - (Chance3+Chance2+Chance1)}; ChanceBooster = customChanceBooster; break;
        }
        defaultChance = chance;

        for (int i = 0; i < 4; i++)
        {
            int n = ChooseTypeRoom(true);
            int m = ChooseRoom(n, StartPoint[i].PointDirection);
            if (isAvailable(StartPoint[i].PointTransform.position))
            {
                GameObject CurrantRoom = Instantiate(RoomVariant[n][m], StartPoint[i].PointTransform.position, Quaternion.identity);
                Rooms.Add(CurrantRoom.transform.position);
                Generate(CurrantRoom, StartPoint[i].PointDirection);
            }
            chance = defaultChance;

        }
        print(Rooms.Count);

    }
    private void Generate(GameObject obj, direction dir)
    {
        BalanceChance();
        Room room = obj.GetComponent<Room>();
        dir = SwapDirection(dir);
        int k = room.Points.Length;
        if (k == 1) return;
        for (int i = 0; i < k; i++)
        {
            if (room.Points[i].PointDirection == dir) continue; 
            int n = ChooseTypeRoom(false);
            int m = ChooseRoom(n, room.Points[i].PointDirection);
            if (isAvailable(room.Points[i].PointTransform.position))
            {
                GameObject newRoom = Instantiate(RoomVariant[n][m], room.Points[i].PointTransform.position, Quaternion.identity);
                Rooms.Add(newRoom.transform.position);
                Generate(newRoom, room.Points[i].PointDirection);
            }

        }
    }
    private bool isAvailable(Vector3 pos)
    {
        foreach(var roomPos in Rooms)
        {
            if (roomPos == pos)
            {
                return false;
            }
        }
        return true;


    }
    private void BalanceChance()
    {
        chance[0] += ChanceBooster;
        if (chance[3] - minChance >= 0) chance[3] -= ChanceBooster;
        else if (chance[2] >= 2 * minChance) chance[2] -= ChanceBooster;
    }
    private int ChooseTypeRoom(bool isStartRoomPoint)
    {
        float k = Random.Range(0, 100f);
        if (k < chance[0])
        {
            if (isStartRoomPoint) return 1;
            else return 0;
        }
        for (int i = 1; i < 4; i++)
        {
            if (k < chance[i])  return i;
            k -= chance[i];
        }
        return 0;
    }
    private int ChooseRoom(int n, direction dir)
    {
        dir = SwapDirection(dir);
        int k = RoomVariant[n].Length;

        List<int> candidate = new List<int>();
        
        for (int i = 0; i < k; i++)
        {
            Room NewRoom = RoomVariant[n][i].GetComponent<Room>();
            for (int j = 0; j < NewRoom.Points.Length; j++)
            {
                if (NewRoom.Points[j].PointDirection == dir)
                {
                    candidate.Add(i);
                    break;
                }
            }
        }
        if(candidate.Count - 1 >=0) return candidate[Random.Range(0, candidate.Count - 1)];
        return 0;
    }
    private direction SwapDirection(direction dir)
    {
        switch (dir)
        {
            case direction.right: dir = direction.left; break;
            case direction.left: dir = direction.right; break;
            case direction.up: dir = direction.down; break;
            case direction.down: dir = direction.up; break;
        }
        return dir;
    }

}
