using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField]
    Tower tower;
    [SerializeField]
    Wall wall;

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
