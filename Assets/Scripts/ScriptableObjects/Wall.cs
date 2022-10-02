using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWall", menuName = "TowerDefense/Wall")]
public class Wall : ScriptableObject
{
    public string Name;
    public int price;

    public GameObject WallPrefab;
}
