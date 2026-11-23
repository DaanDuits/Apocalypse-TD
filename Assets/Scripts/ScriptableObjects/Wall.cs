using UnityEngine;
using TowerDefense.Building;

[CreateAssetMenu(fileName = "NewWall", menuName = "TowerDefense/Wall")]
public class Wall : ScriptableObject
{
	[SerializeField] private new string name;

    [SerializeField] private int price;
    [SerializeField] private string info;
    [SerializeField] private Sprite icon;

    [SerializeField] private Structure wallPrefab;

    public string Name
    { get => name; }

    public int Price
    { get => price; }
    public string Info
    { get => info; }
    public Sprite Icon
    { get => icon; }

    public Structure WallPrefab
    { get => wallPrefab; }

}
