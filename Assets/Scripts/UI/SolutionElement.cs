using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SolutionElement : MonoBehaviour
{
    [SerializeField] private Color _solutionFoundColor;

    private SolutionElementTween _tween;

    public string Value
    {
        get { return GetComponentInChildren<TextMeshPro>().text; }
        set { GetComponentInChildren<TextMeshPro>().text = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _tween = GetComponent<SolutionElementTween>();
    }

    public void OnSolutionFound()
    {
        _tween.OnSolutionFoundTween(_solutionFoundColor);
    }
}
