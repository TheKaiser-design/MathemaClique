using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using System;
using UnityEngine.VFX; // For string math equation

public class GameLogicV5 : MonoBehaviour
{
    public PuzzleGeneratorV3 generator;
    public List<string> values = new List<string>();
    public List<int> answers = new List<int>();


    public GameObject mainGame;
    public GameObject winUI;
    public GameObject equationText;

    // The button that are layed out in the middle of the screen
    public List<Button> buttonsLayout;
    // The solutions on the left side of the screen
    public List<GameObject> solutionText;
    private List<string> validSolution = new List<string>();
    private List<string> solutionSolved = new List<string>();
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
        GenerateNewPuzzle();
    }

    public void GenerateNewPuzzle()
    {
        generator.GeneratePuzzle();

        //Clear it before filling again
        validSolution.Clear();

        values = generator.buttonValues;
        answers = generator.solutions;

        // Shuffle the values list (values in button are shuffle)
        Shuffle(values);
        for (int i = 0; i < values.Count; i++)
        {
            buttonsLayout[i].GetComponentInChildren<TextMeshProUGUI>().text = values[i];
            buttonsLayout[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }

        // Generate the text where the solutions on the left side should appear
        for (int i = 0; i < answers.Count; i++)
        {
            solutionText[i].GetComponent<TextMeshProUGUI>().text = answers[i].ToString();
            solutionText[i].GetComponent<TextMeshProUGUI>().color = Color.black;
            string answerstring = answers[i].ToString();
            validSolution.Add(answerstring);
        }

        ButtonListener();
    }


    // Find what written on the button the user clicked
    void ButtonListener()
    {
        // Will give the string of what's written on which button and which button was pressed
        foreach (Button btn in buttonsLayout)
        {
            // clear previous listeners
            btn.onClick.RemoveAllListeners();

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

            foreach (Button pressedbtn in presseddButtons)
            {
                pressedbtn.interactable = false;
            }
            return;
        }


        if (currentEquation.Length >= 3)
        {
            if (isCheckingAnswer) return; // block input while evaluating

            currentEquation = currentEquation + value + " ";
            equationText.GetComponent<TextMeshProUGUI>().text = currentEquation;

            // Track the clicked buttons
            presseddButtons.Add(buttonClicked);
            foreach (Button pressedbtn in presseddButtons)
            {
                pressedbtn.interactable = false;
            }

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
                if (result == validSolution[i] && !solutionSolved.Contains(validSolution[i]))
                {
                    solutionText[i].GetComponent<TextMeshProUGUI>().color = Color.green;
                    equationText.GetComponent<TextMeshProUGUI>().color = Color.green;
                    solutionSolved.Add(validSolution[i]);
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
                    Invoke("PuzzleSolved", 2f);
                }
            }
            // Prompt the user if the solution to his equation is not in the solution of the problem
            if (solutionFound == false)
            {
                equationText.GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            // Clear equation text and color after a delay of 2 second
            Invoke("ShowResult", 0f);
        }

    }

    void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, list.Count);
            string temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
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
        Invoke("ClearRightEquation", 1f);
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
        foreach (Button pressedbtn in presseddButtons)
        {
            pressedbtn.interactable = true;
        }
        presseddButtons.Clear();
        EnableInput();

    }

    public void ClearRightEquation()
    {
        currentEquation = "";
        equationText.GetComponent<TextMeshProUGUI>().text = "";
        equationText.GetComponent<TextMeshProUGUI>().color = Color.black;
        presseddButtons.Clear();
        solutionFound = false;
        EnableInput();

    }

    void EnableInput()
    {
        // Enable input again
        isCheckingAnswer = false;
    }

    void PuzzleSolved()
    {
        winUI.SetActive(true);
        mainGame.SetActive(false);
    }

    public void RestartPuzzle()
    {
        values.Clear();
        answers.Clear();
        solutionSolved.Clear();
        usedButtons.Clear();
        presseddButtons.Clear();

        for (int i = 0; i < solutionText.Count; i++)
        {
            solutionText[i].GetComponent<TextMeshProUGUI>().color = Color.black;
        }
        foreach (Button btn in buttonsLayout)
        {
            btn.interactable = true;
        }

        ClearEquation();
    }

    public void ClearPuzzle()
    {
        values.Clear();
        answers.Clear();
        validSolution.Clear();
        solutionSolved.Clear();
        usedButtons.Clear();
        presseddButtons.Clear();

        for (int i = 0; i < solutionText.Count; i++)
        {
            solutionText[i].GetComponent<TextMeshProUGUI>().color = Color.black;
        }
        foreach (Button btn in buttonsLayout)
        {
            btn.interactable = true;
        }

        ClearEquation();
    }

    public void NextPuzzle()
    {
        ClearPuzzle();

        GenerateNewPuzzle();
    }
}
