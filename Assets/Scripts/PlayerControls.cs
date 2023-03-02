using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YandexProvider.Device;
using DeviceType = YandexProvider.Device.DeviceType;

public class PlayerControls : MonoBehaviour, IPointerDownHandler
{
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

    private DeviceType _deviceType = DeviceType.Mobile;

    private void Start()
    {
        #if !DEBUG
        _deviceType = Device.Type;
        #endif
        singleButtonParams.container.gameObject.SetActive(_deviceType == DeviceType.Desktop);
        singleButtonParams.halfScreenImageMobile.enabled = _deviceType == DeviceType.Mobile;
        singleButtonParams.mobileAnimationButton.enabled = _deviceType == DeviceType.Mobile;

        if (_deviceType == DeviceType.Mobile)
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
            var minScale = Vector3.one * 3;
            var maxScale = Vector3.one * 6;
            singleButtonParams.mobileAnimationButton.transform.localScale = minScale;
            const float scaleDuration = 0.5f;
            sequence
                .SetDelay(0.5f)
                .Append(singleButtonParams.mobileAnimationButton.transform.DOScale(maxScale, scaleDuration))
                .Append(singleButtonParams.mobileAnimationButton.transform.DOScale(minScale, scaleDuration))
                .Append(singleButtonParams.mobileAnimationButton.transform.DOScale(maxScale, scaleDuration))
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
        if (_deviceType == DeviceType.Desktop && 
            Input.GetKeyDown(playerNum == PlayerNum.Player1 ? KeyCode.D : KeyCode.L))
        {
            SingleButtonPressed?.Invoke();
            StartCoroutine(nameof(SwitchColor), singleButtonParams.buttonSprite);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_deviceType == DeviceType.Mobile)
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
