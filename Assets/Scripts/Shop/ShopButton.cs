using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField]
    Tower tower;
    [SerializeField]
    Wall wall;

    [SerializeField]
    int ResourcePrice;

    TowerShop shop;

    private void Start()
    {
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
    }

    public void Wall()
    {
        shop.BuildWall(wall, ResourcePrice);
    }
    public void Tower()
    {
        shop.BuildTower(tower, ResourcePrice);
    }
}
