using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    [Header("COLORS")]
    [SerializeField] private Color _clickedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _deactivatedColor;
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _rightColor;

    private ButtonTween _tween;
    private RectTransform _rectTransform;
    private Color _baseColor;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _baseColor = _rectTransform.GetComponent<Image>().color;
        _tween = gameObject.GetComponent<ButtonTween>();
    }

    public void HooverEnter()
    {
        _tween.OnHooverTween(_rectTransform);
    }

    public void HooverExit()
    {
        _tween.OnHooverLeaveTween(_rectTransform);
    }

    public void ButtonClicked()
    {
        _tween.OnClickTween(_rectTransform, _clickedColor);
    }

    public void RightAnswer()
    {
        _tween.onRightTween(_rectTransform, _rightColor);
    }

    public void WrongAnswer()
    {
        _tween.OnWrongTween(_rectTransform, _wrongColor);
    }

    public void ResetButton()
    {

    }
}
