using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumButtonsTween : MonoBehaviour
{
    [Header("LEAN TWEEN PARAMETERS")]
    [Header("Hoover animation")]
    [SerializeField] private float _onHooverEnterDuration;
    [SerializeField] private float _onHooverLeaveDuration;
    [SerializeField] private float _onHooverScaleFactor;
    [Space]
    [Header("Click animation")]
    [SerializeField] private float _onClickAnimDuration;
    [SerializeField] private float _onClickMinScaleFactor;
    [Space]
    [Header("Wrong animation")]
    [SerializeField] private float _oneRotationDuration;
    [SerializeField] private float _maxRotationAngle;
    [Space]
    [Header("Right animation")]
    [SerializeField] private float _onRightAnimDuration;
    [SerializeField] private float _onRightMaxScaleFactor;

    private RectTransform _rectTransform;
    private Vector3 _baseScale;
    private Color _baseColor;


    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _baseScale = _rectTransform.localScale;
        _baseColor = _rectTransform.GetComponent<Image>().color;
    }

    public void OnHooverTween()
    {
        LeanTween.scale(_rectTransform, Vector3.one * _onHooverScaleFactor, _onHooverEnterDuration).setEase(LeanTweenType.easeOutQuart);
    }

    public void OnHooverLeaveTween()
    {
        LeanTween.scale(_rectTransform, Vector3.one, _onHooverLeaveDuration).setEase(LeanTweenType.easeOutBounce);
    }

    public void OnClickTween(Color color)
    {
        LeanTween.scale(_rectTransform, Vector3.one * _onClickMinScaleFactor, _onClickAnimDuration / 3).setEase(LeanTweenType.easeOutExpo);
        LeanTween.scale(_rectTransform, Vector3.one, _onClickAnimDuration / 2).setEase(LeanTweenType.easeOutElastic).setDelay(_onClickAnimDuration / 3);
        LeanTween.color(_rectTransform, color, 0f);
    }

    public void OnWrongTween(Color color)
    {
        LeanTween.color(_rectTransform, color, _oneRotationDuration);
        LeanTween.rotateAroundLocal(_rectTransform, _rectTransform.forward, _maxRotationAngle, _oneRotationDuration).setEase(LeanTweenType.easeInOutBounce);
        LeanTween.rotateAroundLocal(_rectTransform, _rectTransform.forward, -2 * _maxRotationAngle, _oneRotationDuration).setEase(LeanTweenType.easeInOutBounce).setDelay(_oneRotationDuration);
        LeanTween.rotateAroundLocal(_rectTransform, _rectTransform.forward, _maxRotationAngle, _oneRotationDuration).setEase(LeanTweenType.easeInOutBounce).setDelay(2 * _oneRotationDuration);
    }

    public void OnRightTween(Color color)
    {
        LeanTween.color(_rectTransform, color, 0f);
        LeanTween.scale(_rectTransform, _rectTransform.localScale * _onRightMaxScaleFactor, _onRightAnimDuration).setEase(LeanTweenType.easeInSine);
        LeanTween.rotateAroundLocal(_rectTransform, _rectTransform.forward, 1080f, _onRightAnimDuration).setEase(LeanTweenType.easeInSine);
        LeanTween.scale(_rectTransform, Vector3.one * 0.01f, 0.3f).setEase(LeanTweenType.easeOutExpo).setDelay(_onRightAnimDuration).setOnComplete(() => gameObject.SetActive(false));
    }

    public void OnResetTween(Color color)
    {
        LeanTween.color(_rectTransform, color, 0f);
        LeanTween.scale(_rectTransform, _rectTransform.localScale, 0f);
    }
}
