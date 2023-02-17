using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yandex;
using Zenject;

public class PlayerControls : MonoBehaviour, IPointerClickHandler
{
    [Inject] private YandexSDK _yandexSDK;
    public PlayerNum playerNum = PlayerNum.Player1;

    public enum InputMode
    {
        SingleButton
    }
    public InputMode inputMode = InputMode.SingleButton;

    [Space] 
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color pressedColor = Color.yellow;
    [Space]
    [SerializeField] private SingleButtonParams singleButtonParams;

    public UnityAction SingleButtonPressed;

    private void Start()
    {
        singleButtonParams.container.gameObject.SetActive(_yandexSDK.CurrentDeviceType == YandexDeviceType.Desktop);
        singleButtonParams.halfScreenImageMobile.enabled = _yandexSDK.CurrentDeviceType == YandexDeviceType.Mobile;
        singleButtonParams.mobileAnimationButton.enabled = _yandexSDK.CurrentDeviceType == YandexDeviceType.Mobile;

        if (_yandexSDK.CurrentDeviceType == YandexDeviceType.Mobile)
        {
            InitInputMobile();
            return;
        }
        InputInputDesktop();
    }

    private void InitInputMobile()
    {
        if (inputMode == InputMode.SingleButton)
        {
            var sequence = DOTween.Sequence();
            singleButtonParams.mobileAnimationButton.transform.localScale = Vector3.zero;
            const float scaleDuration = 0.65f;
            sequence
                .SetDelay(0.5f)
                .Append(singleButtonParams.mobileAnimationButton.transform.DOScale(Vector3.one * 3f, scaleDuration))
                .Append(singleButtonParams.mobileAnimationButton.transform.DOScale(Vector3.zero, scaleDuration))
                .Append(singleButtonParams.mobileAnimationButton.transform.DOScale(Vector3.one * 3f, scaleDuration))
                .Append(singleButtonParams.mobileAnimationButton.transform.DOScale(Vector3.zero, scaleDuration));
        }
    }

    private void InputInputDesktop()
    {
        singleButtonParams.keyText.text = inputMode switch
                {
                    InputMode.SingleButton => playerNum == PlayerNum.Player1 ? "D" : "L",
                    _ => throw new ArgumentOutOfRangeException()
                };
    }

    private IEnumerator SwitchColor(Image image)
    {
        image.color = pressedColor;
        yield return new WaitForSeconds(0.2f);
        image.color = defaultColor;
    }

    private void Update()
    {
        switch (inputMode)
        {
            case InputMode.SingleButton:
                SingleButtonInput();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SingleButtonInput()
    {
        if (_yandexSDK.CurrentDeviceType == YandexDeviceType.Desktop && 
            Input.GetKeyDown(playerNum == PlayerNum.Player1 ? KeyCode.D : KeyCode.L))
        {
            SingleButtonPressed?.Invoke();
            StartCoroutine(nameof(SwitchColor), singleButtonParams.buttonSprite);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_yandexSDK.CurrentDeviceType == YandexDeviceType.Mobile)
        {
            SingleButtonPressed?.Invoke();
        }
    }
}

[Serializable]
public class SingleButtonParams
{
    [Tooltip("Half screen image to interact on mobile")]
    public Image halfScreenImageMobile;
    public RectTransform container;
    public Image buttonSprite;
    public TextMeshProUGUI keyText;
    [Space, Tooltip("Animating on mobile device on level start")] 
    public Image mobileAnimationButton;
}
