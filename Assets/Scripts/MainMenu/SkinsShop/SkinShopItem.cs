using Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainMenu.SkinsShop
{
    public class SkinShopItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Image coinIcon;
        [SerializeField] private Image icon;

        private SkinsShop _skinsShop;
        private int _id;

        public void Init(SkinsShop skinsShop, int id)
        {
            _id = id;
            _skinsShop = skinsShop;
        }

        public void UpdateItem(SkinItem skin)
        {
            icon.sprite = skin.sprite;
            if (skin.unlocked)
            {
                icon.color = Color.white;
                coinIcon.enabled = false;
                costText.enabled = false;
            }
            else
            {
                icon.color = Color.black;
                costText.text = skin.cost.ToString();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _skinsShop.HandleClick(_id);
        }
    }
}