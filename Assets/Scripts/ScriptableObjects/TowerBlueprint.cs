using UnityEngine;
using TowerDefense.Building;

[CreateAssetMenu(fileName = "NewTower", menuName = "TowerDefense/Tower")]
public class TowerBlueprint : ScriptableObject
{
    [SerializeField] private new string name;

    [SerializeField] private int price;
    [SerializeField][TextArea] private string info;
    [SerializeField] private Sprite icon;

    [SerializeField] private Structure towerPrefab;
    [SerializeField] private GameObject overlayPrefab;
    [SerializeField] private GameObject operatorPrefab;

    public string Name
    { get => name; }

    public int Price
    { get => price; }
    public string Info
    { get => info; }
    public Sprite Icon
    { get => icon; }

    public Structure TowerPrefab
    { get => towerPrefab; }
    public GameObject OverlayPrefab
    { get => overlayPrefab; }
    public GameObject OperatorPrefab
    { get => operatorPrefab; }
}
