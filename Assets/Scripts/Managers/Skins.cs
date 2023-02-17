using System;
using Scriptable;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class Skins : MonoBehaviour
    {
        public SkinItem[] SkinItems { get; private set; }

        // TODO [SDK]: preload selected skin
        private int _firstPlayerSkin = 0;
        private int _secondPlayerSkin = 1;

        public UnityAction PlayersSkinUpdate;

        private void Start()
        {
            SkinItems = Resources.LoadAll<SkinItem>("SkinItems");
        }

        public Sprite GetPlayerSkin(PlayerNum playerNum)
        {
            return playerNum switch
            {
                PlayerNum.Player1 => SkinItems[_firstPlayerSkin].sprite,
                PlayerNum.Player2 => SkinItems[_secondPlayerSkin].sprite,
                _ => SkinItems[0].sprite
            };
        }

        public void SetSkinForPlayer(PlayerNum playerNum, int skinIdx)
        {
            if (skinIdx < 0 || skinIdx > SkinItems.Length) return;
            switch (playerNum)
            {
                case PlayerNum.Player1:
                    _firstPlayerSkin = skinIdx;
                    break;
                case PlayerNum.Player2:
                    _secondPlayerSkin = skinIdx;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerNum), playerNum, null);
            }
            PlayersSkinUpdate?.Invoke();
        }
    }
}
