using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField]
    TowerBlueprint tower;
    [SerializeField]
    WallBlueprint wall;

    TowerShop shop;

    private void Start()
    {
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
    }

    public void Wall()
    {
        shop.Build(wall);
    }
    public void Tower()
    {
        shop.Build(tower);
    }
}
