using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonTween : MonoBehaviour
{
    #region Delegates
    public static Action<ButtonType> AnimationCompleted;
    #endregion

    #region Variables
    public ButtonType buttonType;
    [Header("ANIMATION PARAMETERS")]
    [Header("Hoover")]
    [SerializeField] private Color _hooverColor;
    [SerializeField] private float _transitionduration;
    [Space]
    [Header("Click")]
    [SerializeField] private float _downAnimationDuration;
    [SerializeField, Range(0.1f, 1f)] private float _downAnimationScaleFactor;
    [SerializeField] private float _upAnimationDuration;

    private RectTransform _rectTransform;
    private Vector3 _baseScale;
    private Color _baseColor;
    private List<Action> _subscriptionsList;
    #endregion

    #region MonoBeahiour Flow
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _baseScale = _rectTransform.localScale;
        _baseColor = _rectTransform.GetComponent<Image>().color;
        _subscriptionsList = new List<Action>();
        InitializeSubscriptions();
    }
    #endregion

    #region ButtonAnimations
    public void OnHooverEnter()
    {
        LeanTween.color(_rectTransform, _hooverColor, _transitionduration).setEase(LeanTweenType.easeOutBack);
    }

    public void OnHooverLeave()
    {
        LeanTween.color(_rectTransform, _baseColor, _transitionduration).setEase(LeanTweenType.easeOutBack);
    }

    public void OnButtonClicked()
    {
        LeanTween.scale(_rectTransform, _rectTransform.localScale * _downAnimationScaleFactor, _downAnimationDuration).setEase(LeanTweenType.easeOutExpo);
        LeanTween.scale(_rectTransform, _baseScale, _upAnimationDuration).setEase(LeanTweenType.easeOutExpo).setDelay(_downAnimationDuration).setOnComplete(() => AnimationCompleted(buttonType));
    }
    #endregion

    #region Subscriptions
    private void InitializeSubscriptions()
    {
        switch (buttonType)
        {
            case ButtonType.CLEAR:
                InGameMenu.Clear += OnButtonClicked;
                _subscriptionsList.Add(InGameMenu.Clear);
                break;

            case ButtonType.RESTART:
                InGameMenu.Restart += OnButtonClicked;
                _subscriptionsList.Add(InGameMenu.Restart);
                break;

            case ButtonType.NEXT:
                InGameMenu.Next += OnButtonClicked;
                _subscriptionsList.Add(InGameMenu.Next);
                break;

            case ButtonType.MAINMENU:
                InGameMenu.MainMenu += OnButtonClicked;
                _subscriptionsList.Add(InGameMenu.MainMenu);
                break;

            case ButtonType.QUIT:
                InGameMenu.Quit += OnButtonClicked;
                _subscriptionsList.Add(InGameMenu.Quit);
                break;

            default:
                break;
        }
    }

    private void Subscribe(bool subscribe)
    {
        for (int i = 0;  i < _subscriptionsList.Count; i++)
        {
            if (subscribe)
            {
                _subscriptionsList[i] += OnButtonClicked;
            }
            else
            {
                _subscriptionsList[i] -= OnButtonClicked;
            }            
        }
    }
    #endregion

    #region Enable & Disable
    private void OnEnable()
    {
        Subscribe(true);
    }

    private void OnDisable()
    {
        Subscribe(false);
    }

    private void OnDestroy()
    {
        Subscribe(false);
    }
    #endregion
}
