using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShop : MonoBehaviour
{
    [SerializeField]
    ResourceCounter counter;

    [SerializeField]
    BuildTower buildT;

    [SerializeField]
    BuildWall buildW;

    public bool canBuild = true;

    public void BuildWall(Wall wall, int price)
    {
        if (counter.CheckRemovedResources(price) && canBuild)
        {
            StartCoroutine(buildW.MoveBuildWall(wall));
            counter.Removeresources(price);
        }
    }
    public void BuildTower(Tower tower, int price)
    {
        if (counter.CheckRemovedResources(price) && canBuild)
        {
            StartCoroutine(buildT.MoveBuildTower(tower));
            counter.Removeresources(price);
        }
    }
}
