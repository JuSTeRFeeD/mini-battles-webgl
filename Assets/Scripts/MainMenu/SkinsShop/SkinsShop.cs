using Managers;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using Zenject;

namespace MainMenu.SkinsShop
{
    public class SkinsShop : MonoBehaviour
    {
        [Inject] private Skins _skins;
        [Inject] private PlayersScore _playersScore;

        [SerializeField] private ConfirmPanel confirmPanel; 
        [SerializeField] private SkinShopItem skinShopItemPrefab;
        [SerializeField] private RectTransform container;

        [Header("Localization")]
        public LocalizedString notEnoughCoins;
        public LocalizedString whoWillWear;
        public LocalizedString youWillSpendCoins;
        public LocalizedString buy;
        public LocalizedString player;

        private SkinShopItem[] _shopItems;
        private int _index;
        
        private void Start()
        {
            var id = 0;
            _shopItems = new SkinShopItem[_skins.SkinItems.Length];
          
            foreach (var i in _skins.SkinItems)
            {
                var item = Instantiate(skinShopItemPrefab, container);
                _shopItems[id] = item;
                item.Init(this, id++);
                item.UpdateItem(i);
            }
        }

        public void HandleClick(int id)
        {
            _index = id;
            if (!_skins.SkinItems[id].unlocked) ToBuySkin();
            else ToSetSkin();
        }

        private void ToBuySkin()
        {
            if (_skins.SkinItems[_index].cost > _playersScore.Coins)
            {
                confirmPanel.Show("", "", notEnoughCoins.GetLocalizedString());
            }
            else
            {
                youWillSpendCoins.Add("coins", new IntVariable { Value = _skins.SkinItems[_index].cost});
                confirmPanel.Show(buy.GetLocalizedString(), "", youWillSpendCoins.GetLocalizedString());
                confirmPanel.FirstButtonClick += BuySkin;
            }
        }

        private void BuySkin()
        {
            confirmPanel.FirstButtonClick -= BuySkin;
            if (_playersScore.SpendCoins(_skins.SkinItems[_index].cost))
            {
                _skins.SkinItems[_index].unlocked = true;
                _shopItems[_index].UpdateItem(_skins.SkinItems[_index]);
                // TODO [SDK]: save
            }
            confirmPanel.Hide();
        }

        private void ToSetSkin()
        {
            confirmPanel.FirstButtonClick += EquipSkinToFirstPlayer;
            confirmPanel.SecondButtonClick += EquipSkinToSecondPlayer;
            confirmPanel.Show(
                player.GetLocalizedString() + " 1", 
                player.GetLocalizedString() + " 2", 
                whoWillWear.GetLocalizedString());
        }

        private void EquipSkinToFirstPlayer()
        {
            _skins.SetSkinForPlayer(PlayerNum.Player1, _index);     
            confirmPanel.Hide();
            confirmPanel.FirstButtonClick -= EquipSkinToFirstPlayer;
        }
        
        private void EquipSkinToSecondPlayer()
        {
            _skins.SetSkinForPlayer(PlayerNum.Player2, _index);
            confirmPanel.Hide();
            confirmPanel.SecondButtonClick -= EquipSkinToFirstPlayer;
        }
    }
}
