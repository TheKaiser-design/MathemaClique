using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolutionElementTween : MonoBehaviour
{
    [Header("LEAN TWEEN PARAMETERS")]
    [SerializeField] private float _animDuration;
    private RectTransform _rectTransform;
    private Color _baseColor;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _baseColor = _rectTransform.GetComponent<Image>().color;
    }
    public void OnSolutionFoundTween(Color color)
    {
        LeanTween.color(_rectTransform, color, _animDuration / 3);
        LeanTween.color(_rectTransform, _baseColor, _animDuration / 3).setDelay(_animDuration / 3);
        LeanTween.color(_rectTransform, color, _animDuration / 3).setDelay(2 * _animDuration / 3);
        LeanTween.color(_rectTransform, new Color(0f, 0f, 0f, 0f), _animDuration / 3).setDelay(_animDuration);
    }

    public void OnResetTween()
    {
        LeanTween.color(_rectTransform, _baseColor, 0f);
    }
}
