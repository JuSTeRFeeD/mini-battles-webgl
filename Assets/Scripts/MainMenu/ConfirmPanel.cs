using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class ConfirmPanel : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private RectTransform content;
        [Space]
        [SerializeField] private TextMeshProUGUI titleTmp;
        [Space]
        [SerializeField] private Button firstButton;
        [SerializeField] private TextMeshProUGUI firstButtonTmp;
        [Space]
        [SerializeField] private Button secondButton;
        [SerializeField] private TextMeshProUGUI secondButtonTmp;
        [Space]
        [SerializeField] private RectTransform cancelButton;

        public UnityAction FirstButtonClick;
        public UnityAction SecondButtonClick;

        private void Start()
        {
            content.transform.localScale = Vector3.zero;
            background.enabled = false;
        }

        public void Show(string firstText, string secondText, string titleText = "", bool showCancel = true)
        {
            cancelButton.gameObject.SetActive(showCancel);
            titleTmp.enabled = titleText != string.Empty;
            titleTmp.text = titleText;
            firstButton.gameObject.SetActive(firstText != string.Empty);
            firstButtonTmp.text = firstText;
            secondButton.gameObject.SetActive(secondText != string.Empty);
            secondButtonTmp.text = secondText;
            
            background.enabled = true;
            content.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            background.enabled = false;
            content.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }

        public void InvokeFirstClick() => FirstButtonClick?.Invoke();
        public void InvokeSecondClick() => SecondButtonClick?.Invoke();
    }
}
