using System;
using PlayerSave;
using Scriptable;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class Skins : MonoBehaviour
    {
        public SkinItem[] SkinItems { get; private set; }

        public int FirstPlayerSkin { get; private set; }
        public int SecondPlayerSkin { get; private set; }

        public UnityAction OnPlayersSkinUpdate;
        public UnityAction OnSkinsUnlockUpdate;

        private void Awake()
        {
            SkinItems = Resources.LoadAll<SkinItem>("SkinItems");
            SortSkinsByPrice();
            SaveAndLoad.OnDataUpdate += SetupBySaveData;
        }

        private void SetupBySaveData()
        {
            SetSkinForPlayer(PlayerNum.Player1, SaveAndLoad.Data.firstPlayerSkin);
            SetSkinForPlayer(PlayerNum.Player2, SaveAndLoad.Data.secondPlayerSkin);

            var opened = SaveAndLoad.Data.openedSkins;
            for (var i = 0; i < opened.Length; i++)
            {
                SkinItems[i].unlocked = opened[i];
            }
            OnSkinsUnlockUpdate?.Invoke();
        }

        private void SortSkinsByPrice()
        {
            for (var i = 1; i < SkinItems.Length; i++)
            {
                var j = i;
                var item = SkinItems[i];
                while (j > 0 && (item.cost < SkinItems[j - 1].cost))
                {
                    (SkinItems[j - 1], SkinItems[j]) = (SkinItems[j], SkinItems[j - 1]);
                    j--;
                }
                SkinItems[j] = item;
            }
        }

        public Sprite GetPlayerSkin(PlayerNum playerNum)
        {
            return playerNum switch
            {
                PlayerNum.Player1 => SkinItems[FirstPlayerSkin].sprite,
                PlayerNum.Player2 => SkinItems[SecondPlayerSkin].sprite,
                _ => SkinItems[0].sprite
            };
        }

        public void SetSkinForPlayer(PlayerNum playerNum, int skinIdx)
        {
            if (skinIdx < 0 || skinIdx > SkinItems.Length) return;
            switch (playerNum)
            {
                case PlayerNum.Player1:
                    if (FirstPlayerSkin == skinIdx) return;
                    FirstPlayerSkin = skinIdx;
                    break;
                case PlayerNum.Player2:
                    if (SecondPlayerSkin == skinIdx) return;
                    SecondPlayerSkin = skinIdx;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerNum), playerNum, null);
            }
            OnPlayersSkinUpdate?.Invoke();
        }
    }
}
