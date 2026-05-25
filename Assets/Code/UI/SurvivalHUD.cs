using UnityEngine;
using UnityEngine.UI;
using ViaGen.Core;
using ViaGen.Player;

namespace ViaGen.UI
{
    public class SurvivalHUD : MonoBehaviour
    {
        [SerializeField] private Image oxygenBar;
        [SerializeField] private Image energyBar;

        private SurvivalStats _stats;

        private void Start()
        {
            _stats = FindFirstObjectByType<SurvivalStats>();
            if (oxygenBar != null) ViaGenIcons.CreateIconImage(oxygenBar.transform.parent, ViaGenIconId.Oxygen, new Vector2(28f, 28f));
            if (energyBar != null) ViaGenIcons.CreateIconImage(energyBar.transform.parent, ViaGenIconId.Energy, new Vector2(28f, 28f));
        }

        private void Update()
        {
            if (_stats == null) return;
            if (oxygenBar != null) oxygenBar.fillAmount = _stats.Oxygen / 100f;
            if (energyBar != null) energyBar.fillAmount = _stats.Energy / 100f;
        }
    }
}
