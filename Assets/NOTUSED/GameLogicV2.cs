using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLogicV2 : MonoBehaviour
{
    public GameObject equation;



    // Start is called before the first frame update
    void Start()
    {
        string button1String = "+";
        GameObject.Find("ButtonLayout1").GetComponentInChildren<TextMeshProUGUI>().text = button1String;
        string button2String = "2";
        GameObject.Find("ButtonLayout2").GetComponentInChildren<TextMeshProUGUI>().text = button2String;
        string button3String = "-";
        GameObject.Find("ButtonLayout3").GetComponentInChildren<TextMeshProUGUI>().text = button3String;
        string button4String = "7";
        GameObject.Find("ButtonLayout4").GetComponentInChildren<TextMeshProUGUI>().text = button4String;
        string button5String = "6";
        GameObject.Find("ButtonLayout5").GetComponentInChildren<TextMeshProUGUI>().text = button5String;
        string button6String = "*";
        GameObject.Find("ButtonLayout6").GetComponentInChildren<TextMeshProUGUI>().text = button6String;
        string button7String = "2";
        GameObject.Find("ButtonLayout7").GetComponentInChildren<TextMeshProUGUI>().text = button7String;
        string button8String = "-";
        GameObject.Find("ButtonLayout8").GetComponentInChildren<TextMeshProUGUI>().text = button8String;
        string button9String = "3";
        GameObject.Find("ButtonLayout9").GetComponentInChildren<TextMeshProUGUI>().text = button9String;
        string button10String = "6";
        GameObject.Find("ButtonLayout10").GetComponentInChildren<TextMeshProUGUI>().text = button10String;
        string button11String = "9";
        GameObject.Find("ButtonLayout11").GetComponentInChildren<TextMeshProUGUI>().text = button11String;
        string button12String = "4";
        GameObject.Find("ButtonLayout12").GetComponentInChildren<TextMeshProUGUI>().text = button12String;


        string equationText = "132" ;
        equation.GetComponent<TextMeshProUGUI>().text = equationText;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
