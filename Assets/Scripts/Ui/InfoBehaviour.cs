using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBehaviour : MonoBehaviour
{
	public TMP_Text uiName;

	public Image image;

	public TMP_Text info;

	public GameObject shopPanel;

	public GameObject infoPanel;

	private void Start()
	{
	}

	public void SetPanel(Wall wall)
	{
		infoPanel.SetActive(value: true);
		shopPanel.SetActive(value: false);
		image.transform.rotation = Quaternion.Euler(0f, 0f, -45f);
		uiName.text = wall.name;
		image.sprite = wall.Icon;
		info.text = wall.Info;
	}

	public void SetPanel(Tower tower)
	{
		infoPanel.SetActive(value: true);
		shopPanel.SetActive(value: false);
		image.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		uiName.text = tower.name;
		image.sprite = tower.Icon;
		info.text = tower.Info;
	}

	public void ExitPanel()
	{
		infoPanel.SetActive(value: false);
		shopPanel.SetActive(value: true);
	}

	private void Update()
	{
	}
}
