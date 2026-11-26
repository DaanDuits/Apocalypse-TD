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

    public void Build(TowerBlueprint tower) => Build(null, tower);
    public void Build(WallBlueprint wall) => Build(wall, null);
    public void Build(WallBlueprint wall, TowerBlueprint tower)
    {
        if (tower != null)
        {
            if (counter.CheckRemovedResources(tower.Price) && canBuild)
                StartCoroutine(buildT.MoveBuildTower(tower));
        }

        else
        {
            if (counter.CheckRemovedResources(wall.Price) && canBuild)
                StartCoroutine(buildW.MoveBuildWall(wall, false));
        }
    }
}
