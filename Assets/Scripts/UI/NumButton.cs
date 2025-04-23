using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public string Value
    {
        get { return GetComponentInChildren<TextMeshPro>().text; }
        set { GetComponentInChildren<TextMeshPro>().text = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _tween = GetComponent<NumButtonsTween>();
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

    public void DeactivateButton()
    {
        _tween.OnTweenDeactivate(_deactivatedColor);
    }

    public void ResetButton()
    {
        _tween.OnResetTween();
    }

    public void SetButtonActive(bool active)
    {
        GetComponent<Button>().interactable = active;
    }
}
