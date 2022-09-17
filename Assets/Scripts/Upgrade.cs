using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    Sprite upgradeSlotFull;
    [SerializeField]
    GameObject upgradeSlot;
    [SerializeField]
    GameObject upgradePanel;
    [SerializeField]
    GameObject shopPanel;

    TowerShop shop;

    TMP_Text text;

    UpgradePanelData currentData;
    private void Start()
    {
        shopPanel.SetActive(true);
        upgradePanel.SetActive(false);
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
        text = upgradePanel.transform.Find("PriceCounter").GetComponent<TMP_Text>();
    }

    public void SetPanel(UpgradePanelData data)
    {
        if (shop.canBuild)
        {
            currentData = data;
            for (int i = 0; i < upgradePanel.transform.Find("UpgradeAmount").childCount; i++)
            {
                Destroy(upgradePanel.transform.Find("UpgradeAmount").GetChild(i).gameObject);
            }
            for (int i = 0; i < data.upgradeAmount; i++)
            {
                GameObject slot = Instantiate(upgradeSlot, upgradePanel.transform.Find("UpgradeAmount"));
                if (i <= data.Level - 1)
                {
                    slot.GetComponent<Image>().sprite = upgradeSlotFull;
                }
            }
            upgradePanel.SetActive(true);
            shopPanel.SetActive(false);
            upgradePanel.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = data.icon;

            if (data.Upgrade == null)
            {
                upgradePanel.transform.Find("Upgrade").gameObject.SetActive(false); 
                text.gameObject.SetActive(false);
            }
            else
            {
                upgradePanel.transform.Find("Upgrade").gameObject.SetActive(true);
                text.gameObject.SetActive(true);
            }
            text.text = "Price: " + data.nextCost.ToString();
        }
    }
    public void UpgradeCurrent()
    {
        if (shop.counter.CheckRemovedResources(currentData.nextCost))
        {
            GameObject upgrade = Instantiate(currentData.Upgrade, currentData.transform.position, currentData.transform.rotation);
            shop.counter.Removeresources(currentData.nextCost);
            Destroy(currentData.gameObject);
            SetPanel(upgrade.GetComponent<UpgradePanelData>());
        }
    }

}
