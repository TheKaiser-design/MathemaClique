using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq; // For string math equation

public class GameLogicV3 : MonoBehaviour
{
    public GameObject mainGame;
    public GameObject winUI;
    public GameObject equationText;

    // The button that are layed out in the middle of the screen
    public List<Button> buttonsLayout;
    // The solutions on the left side of the screen
    public List<GameObject> solutionText;
    public List<string> validSolution = new List<string>();
    // The equation on top of the screen
    private string currentEquation = "";

    // Check if equation has valid format which is --> number operator number
    // Numbers can be more than one digit, operators are only +, -, *, /
    // https://discussions.unity.com/t/how-to-use-the-regex-class-to-check-for-string-with-case-insensitive/22150
    private Regex validFormat= new Regex (@"^\s*(\d+)\s*([\+\-\*/])\s*(\d+)\s*$");

    private bool solutionFound = false;
    private string result;

    // Button we pressed but havent solve an equation yet
    private List<Button> presseddButtons = new List<Button>();
    // Once the buttons are clicked on, the equation is solve and is one of our solution, we want to deactivate those button
    private List<Button> usedButtons = new List<Button>();

    // We dont want the user to input while we show the equation
    private bool isCheckingAnswer = false;
    // We check if the inputs the user gave looks like what we want it to be. If true, we'll say it's a valid format, otherwise we'll say it's not
    private bool isValidFormat(string input)
    {
        // Very basic logic, we check if it ends in a digit and contains an operator
        return input.Any(c => "+-*/".Contains(c))
            && char.IsDigit(input.Last());
    }



    // Start is called before the first frame update
    void Start()
    {
        GenerateSolutionText();
        ButtonListener();
    }

    void GenerateSolutionText()
    {
        // Generate the text where the solutions on the left side should appear
        for (int i = 0; i < validSolution.Count; i++)
        {
            solutionText[i].GetComponent<TextMeshProUGUI>().text = validSolution[i];
            solutionText[i].GetComponent<TextMeshProUGUI>().color = Color.black;
        }
    }

    // Find what written on the button the user clicked
    void ButtonListener()
    {
        // Will give the string of what's written on which button and which button was pressed
        foreach (Button btn in buttonsLayout)
        {
            string val = btn.GetComponentInChildren<TextMeshProUGUI>().text;
            btn.onClick.AddListener(() => AddToEquation(val, btn));
        }

    }

    // Add the string to the equation on top of the screen
    void AddToEquation(string value, Button buttonClicked)
    {

        // Don’t even try to evaluate unless there's at least 3 characters: "1+2"
        if (currentEquation.Length < 3)
        {
            currentEquation = currentEquation + value + " ";
            equationText.GetComponent<TextMeshProUGUI>().text = currentEquation;

            // Track the clicked buttons
            presseddButtons.Add(buttonClicked);
            return;
        }


        if (currentEquation.Length >= 3)
        {
            if (isCheckingAnswer) return; // block input while evaluating

            currentEquation = currentEquation + value + " ";
            equationText.GetComponent<TextMeshProUGUI>().text = currentEquation;

            // Track the clicked buttons
            presseddButtons.Add(buttonClicked);

            if (validFormat.IsMatch(currentEquation))
            {
                CalculateEquation();
            }
            else if (!isValidFormat(currentEquation))
            {
                ShowWRONG();
            }
        }



    }

    // Calculate the equation and check if it's within the solution. Will only do it if the equation is of the right format
    void CalculateEquation()
    {
        // Will only calculate equation if it has the right format
        if (validFormat.IsMatch(currentEquation))
        {
            // lock input
            isCheckingAnswer = true; 

            // Quick trick to evaluate a string math expression. Will not work for bigger application
            // https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable.compute?view=net-9.0
            result = new DataTable().Compute(currentEquation, null).ToString();

            // Check if result of equation is in the solutions
            for (int i = 0; i < validSolution.Count; i++)
            {
                if (result == validSolution[i])
                {
                    solutionText[i].GetComponent<TextMeshProUGUI>().color = Color.green;
                    equationText.GetComponent<TextMeshProUGUI>().color = Color.green;
                    solutionFound = true;

                }
            }

            // Deactivate buttons if we found a solution
            if (solutionFound == true)
            {
                // We add the pressed buttons to the used button and we deactivate the used buttons only. That way all the buttons the user press wont be deactivated button only the ones that found a solution
                foreach (Button pressedbtn in presseddButtons)
                {
                    usedButtons.Add(pressedbtn);
                }
                foreach (Button btn in usedButtons)
                {
                    btn.interactable = false;
                }
                // Triggers when the whole puzzle is solve
                if (usedButtons.Count == buttonsLayout.Count)
                {
                    winUI.SetActive(true);
                    mainGame.SetActive(false);
                }
            }
            // Prompt the user if the solution to his equation is not in the solution of the problem
            if (solutionFound == false)
            {
                equationText.GetComponent<TextMeshProUGUI>().color = Color.red;
                presseddButtons.Clear();
            }

            // Clear equation text and color after a delay of 2 second
            Invoke("ShowResult", 0f);
        }

    }

    void ShowResult()
    {
        currentEquation = currentEquation + " = " + result;
        equationText.GetComponent<TextMeshProUGUI>().text = currentEquation;

        if (solutionFound == false)
        {
            Invoke("ShowWRONG", 1f);
        }
        if (solutionFound == true)
        {
            Invoke("ShowRIGHT", 1f);
        }
    }

    void ShowRIGHT()
    {
        equationText.GetComponent<TextMeshProUGUI>().color = Color.green;
        equationText.GetComponent<TextMeshProUGUI>().text = "RIGHT";
        Invoke("ClearEquation", 1f);
    }

    void ShowWRONG()
    {
        equationText.GetComponent<TextMeshProUGUI>().color = Color.red;
        equationText.GetComponent<TextMeshProUGUI>().text = "WRONG";
        Invoke("ClearEquation", 1f);
    }


    // Clear the equation on top of the screen after it was calculated and we determine if it was a solution or not
    public void ClearEquation()
    {
        currentEquation = "";
        equationText.GetComponent<TextMeshProUGUI>().text = "";
        equationText.GetComponent<TextMeshProUGUI>().color = Color.black;
        solutionFound = false;
        presseddButtons.Clear();
        EnableInput();
    }

    void EnableInput()
    {
        // Enable input again
        isCheckingAnswer = false;
    }

    public void RestartPuzzle()
    {
        ClearEquation();
        GenerateSolutionText();
        usedButtons.Clear();
        presseddButtons.Clear();
        foreach (Button btn in buttonsLayout)
        {
            btn.interactable = true;
        }

    }
}
