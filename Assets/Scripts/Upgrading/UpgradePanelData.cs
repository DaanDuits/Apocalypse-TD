using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelData : MonoBehaviour
{
    public Sprite icon;
    public int upgradeAmount;
    public int Level;

    public GameObject Upgrade;

    public int nextCost;

    Upgrade panel;

    private void Start()
    {
        panel = GameObject.Find("Upgrade").GetComponent<Upgrade>();
    }

    private void OnMouseDown()
    {
        panel.SetPanel(this);
    }
}
