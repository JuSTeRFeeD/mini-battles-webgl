using System;
using UnityEngine;

namespace MainMenu
{
    public class PanelsManager : MonoBehaviour
    {
        [SerializeField] private RectTransform levelsListPanel;
        [SerializeField] private RectTransform skinsShopPanel;
        [SerializeField] private RectTransform settingsPanel;
        
        [Serializable]
        public enum MainMenuPanel
        {
            LevelsList,
            SkinsShop,
            Settings,
        }

        private void Start()
        {
            ToPanel(0);
        }

        public void ToPanel(int panelIdx)
        {
            var panel = (MainMenuPanel)panelIdx;
            levelsListPanel.gameObject.SetActive(panel == MainMenuPanel.LevelsList);
            skinsShopPanel.gameObject.SetActive(panel == MainMenuPanel.SkinsShop);
            settingsPanel.gameObject.SetActive(panel == MainMenuPanel.Settings);
        }
    }
}
