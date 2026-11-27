using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TowerDefense.Health;
using TowerDefense.Utility;
using TowerDefense.Building;

namespace TowerDefense.Operator
{
    public class Mechanic : MonoBehaviour, ITowerOperator
    {
        [SerializeField] private float range;
        [SerializeField] private float repairRate;
        [SerializeField] private int repairAmount;
        [SerializeField] protected AudioClip clip;

        protected AudioSource _audioSrc;
        public float Range
        { get => range; }
        void Start()
        {
            _audioSrc = FindAnyObjectByType<AudioSource>();
            Button[] all = FindObjectsByType<Button>(FindObjectsSortMode.None);
            foreach (Button b in all)
            {
                if (b.name == "StartWaveB")
                {
                    StartCoroutine(RepairLoop());
                    break;
                }
            }
        }

        private IEnumerator RepairLoop()
        {
            float timer = 0f;
            while (true)
            {
                foreach (IHealable h in GetAllBuildingsInRange())
                {
                    h.GainHealth(repairAmount);
                }

                timer += Time.deltaTime * (1f / repairRate);
                timer = Mathf.Clamp01(timer);

                yield return null;

                if (timer == 1f)
                {
                    _audioSrc.PlayOneShot(clip, 0.2f);
                    timer = 0f;
                }
            }
        }

        private HashSet<IHealable> GetAllBuildingsInRange()
        {
            return Registry<IHealable>.Filter(h => Registry<Structure>.Items.Contains(h)).ToHashSet();
        }
    }
}
