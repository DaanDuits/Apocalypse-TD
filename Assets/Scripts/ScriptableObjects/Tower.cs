using UnityEngine;

[CreateAssetMenu(fileName = "NewTower", menuName = "TowerDefense/Tower")]
public class Tower : ScriptableObject
{
	public string Name;

	public int price;

	public string info;

	public Sprite icon;

	public GameObject TowerPrefab;

	public GameObject overlayPrefab;

	public GameObject shooterPrefab;
}
