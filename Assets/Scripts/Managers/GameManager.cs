using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static Action<Color> Validate;

    [Header("Equations")]
    [SerializeField, Range(1, 10)] private int _nbSolutions;
    [Header("DISPLAYS")]
    [SerializeField] private GridLayoutGroup _numberButtonsGrid;
    [SerializeField] private VerticalLayoutGroup _solutionsDisplay;
    [SerializeField] private TextMeshProUGUI _currentEquationText;
    [SerializeField] private GameObject _numberButtonPrefab;
    [SerializeField] private GameObject _solutionDisplayPrefab;

    private static GameManager _instance;
    private EquationGenerator _equationGenerator;
    // Number buttons Objects
    private List<GameObject> _numberButtons;
    // Solutions displays objects
    private List<GameObject> _solutionsDisplays;
    // List of valid but not yet solved equations
    private List<string> _solutionEquations;
    private List<string> _equationsSolved;

    private List<NumButton> _pressedButtons;

    // Check if equation has valid format which is --> number operator number
    // Numbers can be more than one digit, operators are only +, -, *, /
    // https://discussions.unity.com/t/how-to-use-the-regex-class-to-check-for-string-with-case-insensitive/22150
    private Regex _validFormat = new Regex(@"^\s*(\d+)\s*([\+\-\*/])\s*(\d+)\s*$");

    private string _currentEquation = "";
    private bool _solutionFound = false;
    public int _solutionsFoundCount;
    private bool _isCheckingAnswer = false;
    private string _result;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _equationGenerator = new EquationGenerator(_nbSolutions);
    }

    public void GenerateNewPuzzle()
    {
        _equationGenerator.Generate();
        List<string> values = _equationGenerator.Values;
        List<int> solutions = _equationGenerator.Solutions;

        Shuffle(values);
        for (int i = 0; i < values.Count; i++)
        {
            GameObject currentNumberButton = Instantiate(_numberButtonPrefab, _numberButtonsGrid.transform);
            currentNumberButton.GetComponent<NumButton>().Value = values[i];
        }
        for (int i = 0;i < solutions.Count; i++)
        {
            GameObject currentSolutionDisplay = Instantiate(_solutionDisplayPrefab, _solutionDisplayPrefab.transform);
            currentSolutionDisplay.GetComponent<SolutionElement>().Value = solutions[i].ToString();
            _solutionsDisplays.Add(currentSolutionDisplay);
            _solutionEquations.Add(solutions[i].ToString());
        }

        ButtonListener();
    }
	
	private void AddToEquation(string value, NumButton buttonPressed)
	{
        if (_currentEquation.Length < 3)
        {
            _currentEquation += (value + " ");
            _currentEquationText.text = _currentEquation;

            // Track the clicked buttons
            _pressedButtons.Add(buttonPressed);
            foreach (NumButton pressedbtn in _pressedButtons)
            {
                pressedbtn.SetButtonActive(false);
            }
            return;
        }

        if (_isCheckingAnswer) return; // block input while evaluating

        _currentEquation += (value + " ");
        _currentEquationText.text = _currentEquation;

        // Track the clicked buttons
        _pressedButtons.Add(buttonPressed);
        foreach (NumButton pressedbtn in _pressedButtons)
        {
            pressedbtn.SetButtonActive(false);
        }

        if (_validFormat.IsMatch(_currentEquation))
        {
            CalculateEquation();
        }
        else
        {
            _currentEquationText.text = "WRONG";
            _currentEquationText.color = Color.red;
            ClearEquation(false);
        }
    }
    private void CalculateEquation()
    {
        _isCheckingAnswer = true;

        // Quick trick to evaluate a string math expression. Will not work for bigger application
        // https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable.compute?view=net-9.0
        _result = new DataTable().Compute(_currentEquation, null).ToString();

        // Check if _result of equation is in the solutions
        for (int i = 0; i < _solutionsDisplays.Count; i++)
        {
            SolutionElement solutionElement = _solutionsDisplays[i].GetComponent<SolutionElement>();
            string solutionElementValue = solutionElement.Value;

            if (_result == solutionElementValue && !_equationsSolved.Contains(solutionElementValue))
            {
                solutionElement.OnSolutionFound();
                _equationsSolved.Add(_solutionEquations[i]);
                _solutionFound = true;
            }
        }

        // Deactivate buttons if we found a solution
        if (_solutionFound == true)
        {
            // We add the pressed buttons to the used button and we deactivate the used buttons only. That way all the buttons the user press wont be deactivated button only the ones that found a solution
            foreach (NumButton pressedbtn in _pressedButtons)
            {
                pressedbtn.RightAnswer();
            }
            _currentEquationText.color = Color.green;
            // Triggers when the whole puzzle is solve
            if (_equationsSolved.Count == _solutionsDisplays.Count)
            {
                Invoke("PuzzleSolved", 2f);
            }
        }
        else
        {
            foreach (NumButton pressedbtn in _pressedButtons)
            {
                pressedbtn.WrongAnswer();
            }
            _currentEquationText.color = Color.red;
            ClearEquation(false);
        }
    }

    private void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }
	
	private void ButtonListener()
    {		
		foreach (GameObject button in _numberButtons)
		{
            NumButton buttonLogic = button.GetComponent<NumButton>();
			Button currentButton = button.GetComponent<Button>();
			
			currentButton.onClick.RemoveAllListeners();
			currentButton.onClick.AddListener(() => AddToEquation(buttonLogic.Value, buttonLogic));			
		}
    }
    public void ClearEquation(bool isAnswerRight)
    {
        _currentEquation = "";
        _currentEquationText.text = _currentEquation;
        _currentEquationText.color = Color.black;

        if (isAnswerRight)
        {
            foreach (NumButton pressedbtn in _pressedButtons)
            {
                pressedbtn.SetButtonActive(true);
            }
        }

        _pressedButtons.Clear();
        _solutionFound = false;
        _isCheckingAnswer = false;
    }
}
