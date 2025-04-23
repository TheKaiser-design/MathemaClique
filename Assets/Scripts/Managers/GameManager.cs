using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    [SerializeField] private GameObject _numberButtonPrefab;
    [SerializeField] private GameObject _solutionDisplayPrefab;

    private static GameManager _instance;
    private EquationGenerator _equationGenerator;
    // Number buttons Objects
    private List<GameObject> numberButtons;
    // Solutions displays objects
    private List<GameObject> solutionsDisplays;
    // List of valid but not yet solved equations
    private List<string> validEquations;
    private List<string> equationsSolved;

    private List<NumButton> pressedButtons;
    private List<NumButton> unpressedButtons;

    // Check if equation has valid format which is --> number operator number
    // Numbers can be more than one digit, operators are only +, -, *, /
    // https://discussions.unity.com/t/how-to-use-the-regex-class-to-check-for-string-with-case-insensitive/22150
    private Regex validFormat = new Regex(@"^\s*(\d+)\s*([\+\-\*/])\s*(\d+)\s*$");

    private string currentEquation = "";
    private bool solutionFound = false;
    private bool isCheckingAnswer = false;
    private string result;

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

    // Update is called once per frame
    void Update()
    {
        
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
        }



    }
    private bool isValidFormat(string input)
    {
        return input.Any(c => "+-*/".Contains(c)) && char.IsDigit(input.Last());
    }

    private void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, list.Count);
            string temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
