using UnityEngine;
using TowerDefense.Building;

[CreateAssetMenu(fileName = "NewWall", menuName = "TowerDefense/Wall")]
public class WallBlueprint : ScriptableObject
{
	[SerializeField] private new string name;

    [SerializeField] private int price;
    [SerializeField] private string info;
    [SerializeField] private Sprite icon;

    [SerializeField] private Wall wallPrefab;

    public string Name
    { get => name; }

    public int Price
    { get => price; }
    public string Info
    { get => info; }
    public Sprite Icon
    { get => icon; }

    public Wall WallPrefab
    { get => wallPrefab; }

}
