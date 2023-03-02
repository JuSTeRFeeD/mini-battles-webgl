using Managers;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class PlayerWithSkin : MonoBehaviour
    {
        [Inject] private Skins _skins;
        [SerializeField] private PlayerNum playerNum;
        [SerializeField] private SpriteRenderer skinRenderer;

        private void Start()
        {
            UpdateSkin();
            _skins.OnPlayersSkinUpdate += UpdateSkin;
        }

        private void OnDestroy()
        {
            _skins.OnPlayersSkinUpdate -= UpdateSkin;
        }

        private void UpdateSkin()
        {
            skinRenderer.sprite = _skins.GetPlayerSkin(playerNum);
        }
    }
}
