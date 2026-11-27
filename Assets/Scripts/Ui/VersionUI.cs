using UnityEngine;
using TMPro;

namespace TowerDefense
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionUI : MonoBehaviour
    {
        private TMP_Text _version;

        private void Start()
        {
            _version = GetComponent<TMP_Text>();
            _version.text = "Version: " + Application.version + "\n" + "Unity Version: " + Application.unityVersion;
        }
    }
}
