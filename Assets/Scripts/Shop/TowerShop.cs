using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShop : MonoBehaviour
{
    public ResourceCounter counter;

    [SerializeField]
    BuildWall buildW;
    [SerializeField]
    BuildTower buildT;

    public bool canBuild = true;

    public void Build(Tower tower) => Build(null, tower);
    public void Build(Wall wall) => Build(wall, null);
    public void Build(Wall wall, Tower tower)
    {
        if (tower != null)
        {
            if (counter.CheckRemovedResources(tower.price) && canBuild)
                StartCoroutine(buildT.MoveBuildTower(tower));
        }

        else
        {
            if (counter.CheckRemovedResources(wall.price) && canBuild)
                StartCoroutine(buildW.MoveBuildWall(wall, false));
        }
    }
}
