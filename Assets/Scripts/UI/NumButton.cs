using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumButton : MonoBehaviour
{
    [Header("COLORS")]
    [SerializeField] private Color _clickedColor;
    [SerializeField] private Color _deactivatedColor;
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _rightColor;

    private NumButtonsTween _tween;
    private RectTransform _rectTransform;
    private Color _baseColor;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _baseColor = _rectTransform.GetComponent<Image>().color;
        _tween = gameObject.GetComponent<NumButtonsTween>();
    }

    public void ButtonClicked()
    {
        _tween.OnClickTween(_clickedColor);
    }

    public void RightAnswer()
    {
        _tween.OnRightTween(_rightColor);
    }

    public void WrongAnswer()
    {
        _tween.OnWrongTween(_wrongColor);
    }

    public void ResetButton()
    {
        _tween.OnResetTween(_baseColor);
    }
}
