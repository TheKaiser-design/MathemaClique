using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameLogic : MonoBehaviour
{
    public int[] solutionArray;

    Func<float, float, float>[] operationArray = {
    (a, b) => a + b,
    (a, b) => a - b,
    (a, b) => a * b,
    (a, b) => a / b
    };

    /*
     float result = operationArray[0](5, 3); // 5 + 3 = 8
    */



    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("ButtonSolution1").GetComponentInChildren<TextMeshProUGUI>().text = solutionArray[0].ToString();
        GameObject.Find("ButtonSolution2").GetComponentInChildren<TextMeshProUGUI>().text = solutionArray[1].ToString();
        GameObject.Find("ButtonSolution3").GetComponentInChildren<TextMeshProUGUI>().text = solutionArray[2].ToString();
        GameObject.Find("ButtonSolution4").GetComponentInChildren<TextMeshProUGUI>().text = solutionArray[3].ToString();
        XYCalculator();
    }


    void XYCalculator()
    {




        for (int i = 1; i < 13; i++)
        {
            GameObject.Find("ButtonLayout" + i.ToString()).GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
