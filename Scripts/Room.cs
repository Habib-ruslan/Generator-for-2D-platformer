using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Point
{
    public Transform PointTransform;
    public direction PointDirection;
    public bool isLock;
}


public class Room : MonoBehaviour
{
    public Point[] Points;

}
