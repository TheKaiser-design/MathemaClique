using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public static Action<RectTransform> OnHooverEnter;
    public static Action<RectTransform> OnHooverExit;
    public static Action<RectTransform, Color> OnButtonClicked;
    public static Action<RectTransform, Color> OnRightAnswer;
    public static Action<RectTransform, Color> OnWrongAnswer;
    public static Action<RectTransform, Color> OnResetButton;

    [Header("COLORS")]
    [SerializeField] private Color _clickedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _deactivatedColor;
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _rightColor;

    private RectTransform _rectTransform;
    private Color _baseColor;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _baseColor = _rectTransform.GetComponent<Image>().color;
    }

    public void HooverEnter()
    {
        OnHooverEnter?.Invoke(_rectTransform);
    }

    public void HooverExit()
    {
        OnHooverExit?.Invoke(_rectTransform);
    }

    public void ButtonClicked()
    {
        OnButtonClicked?.Invoke(_rectTransform, _clickedColor);
    }

    public void RightAnswer()
    {
        OnRightAnswer?.Invoke(_rectTransform, _rightColor);
    }

    public void WrongAnswer()
    {
        OnWrongAnswer?.Invoke(_rectTransform, _wrongColor);
    }

    public void ResetButton()
    {

    }
}
