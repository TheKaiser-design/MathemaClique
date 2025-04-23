using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTween : MonoBehaviour
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

    public void OnHooverTween(RectTransform rectTransform)
    {
        LeanTween.scale(rectTransform, Vector3.one * _onHooverScaleFactor, _onHooverEnterDuration).setEase(LeanTweenType.easeOutQuart);
    }

    public void OnHooverLeaveTween(RectTransform rectTransform)
    {
        LeanTween.scale(rectTransform, Vector3.one, _onHooverLeaveDuration).setEase(LeanTweenType.easeOutBounce);
    }

    public void OnClickTween(RectTransform rectTransform, Color color)
    {
        LeanTween.scale(rectTransform, Vector3.one * _onClickMinScaleFactor, _onClickAnimDuration / 3).setEase(LeanTweenType.easeOutExpo);
        LeanTween.scale(rectTransform, Vector3.one, _onClickAnimDuration / 2).setEase(LeanTweenType.easeOutElastic).setDelay(_onClickAnimDuration / 3);
        LeanTween.color(rectTransform, color, 0f);
    }

    public void OnWrongTween(RectTransform rectTransform, Color color)
    {
        LeanTween.color(rectTransform, color, _oneRotationDuration);
        LeanTween.rotateAroundLocal(rectTransform, rectTransform.forward, _maxRotationAngle, _oneRotationDuration).setEase(LeanTweenType.easeInOutBounce);
        LeanTween.rotateAroundLocal(rectTransform, rectTransform.forward, - 2 * _maxRotationAngle, _oneRotationDuration).setEase(LeanTweenType.easeInOutBounce).setDelay(_oneRotationDuration);
        LeanTween.rotateAroundLocal(rectTransform, rectTransform.forward, _maxRotationAngle, _oneRotationDuration).setEase(LeanTweenType.easeInOutBounce).setDelay(2 * _oneRotationDuration);
    }

    public void onRightTween(RectTransform rectTransform, Color color)
    {
        LeanTween.color(rectTransform, color, 0f);
        LeanTween.scale(rectTransform, rectTransform.localScale * _onRightMaxScaleFactor, _onRightAnimDuration).setEase(LeanTweenType.easeInSine);
        LeanTween.rotateAroundLocal(rectTransform, rectTransform.forward, 1080f, _onRightAnimDuration).setEase(LeanTweenType.easeInSine);
        LeanTween.scale(rectTransform, Vector3.one * 0.01f, 0.3f).setEase(LeanTweenType.easeOutExpo).setDelay(_onRightAnimDuration).setOnComplete(() => gameObject.SetActive(false));
    }

    public void OnDeactivateTween()
    {

    }

    public void OnResetTween(RectTransform rectTransform, Color color)
    {
        LeanTween.color(rectTransform, color, 0f);
        LeanTween.scale(rectTransform, rectTransform.localScale, 0f);
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        
    }

    public void OnDestroyTween()
    {
        // Tween behavior ?
    }
}
