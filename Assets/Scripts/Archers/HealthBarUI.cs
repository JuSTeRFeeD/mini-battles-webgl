using System.Text;
using TMPro;
using UnityEngine;

namespace Archers
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private TextMeshProUGUI healthText;

        private void Start()
        {
            UpdateBar();
            health.HealthUpdate += UpdateBar;
        }

        private void UpdateBar()
        {
            var sb = new StringBuilder();
            sb.Append("<size=25%>");
            sb.Append(health.CurHealth);
            sb.Append("/");
            sb.Append(health.MaxHealth);
            sb.Append(" ");
            sb.Append("<size=100%>");
            for (var i = 0; i < health.MaxHealth; i++)
            {
                if (i == health.CurHealth) sb.Append("<color=grey>");
                sb.Append("_");
            }
            healthText.text = sb.ToString();
        }
    }
}
