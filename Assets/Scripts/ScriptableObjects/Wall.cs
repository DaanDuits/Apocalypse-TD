using UnityEngine;

[CreateAssetMenu(fileName = "NewWall", menuName = "TowerDefense/Wall")]
public class Wall : ScriptableObject
{
	public string Name;

	public int price;

	public string info;

	public Sprite icon;

	public GameObject WallPrefab;
}
