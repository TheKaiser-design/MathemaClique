using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleGeneratorV2 : MonoBehaviour
{
    public List<int> solutions = new List<int>();             // The 4 correct answers
    public List<ExpressionPart> validExpressions = new List<ExpressionPart>(); // 1 per answer
    public List<string> buttonValues = new List<string>();    // 12 values for the buttons

    public struct ExpressionPart
    {
        public int left;
        public char op;
        public int right;
        public string ToEquation() => $"{left}{op}{right}";
    }

    public enum DifficultyLevel
    {
        Easy,
        Hard
    }

    public void GeneratePuzzle(DifficultyLevel level)
    {
        buttonValues.Clear();
        solutions.Clear();
        validExpressions.Clear();

        switch (level)
        {
            case DifficultyLevel.Easy:
                GenerateEasyPuzzle();
                break;
            case DifficultyLevel.Hard:
                GenerateHardPuzzle();
                break;
        }
    }

    public void GenerateEasyPuzzle()
    {

        char[] ops = { '+', '-', '*', '/' };


        // Generate 4 solution that match each expression
        while (validExpressions.Count < 4)
        {
            int a = UnityEngine.Random.Range(1, 9);
            int b = UnityEngine.Random.Range(1, 9);
            char op = ops[UnityEngine.Random.Range(0, ops.Length)];

            // Avoid bad math
            if (op == '/' && (b == 0 || a % b != 0)) continue;
            if (op == '-' && a - b < 0) continue; // optional: avoid negatives

            int result = op switch
            {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' => a / b,
                _ => 0
            };

            // Avoid duplicate results (so each solution is unique)
            if (solutions.Contains(result)) continue;

            validExpressions.Add(new ExpressionPart { left = a, op = op, right = b });
            solutions.Add(result);

        }

        // Extract numbers and operators from expressions
        foreach (var expr in validExpressions)
        {
            buttonValues.Add(expr.left.ToString());
            buttonValues.Add(expr.op.ToString());
            buttonValues.Add(expr.right.ToString());
        }
    }

    public void GenerateHardPuzzle()
    {
        buttonValues.Clear();
        solutions.Clear();
        validExpressions.Clear();

        char[] ops = { '+', '-', '*', '/' };
        Dictionary<int, List<ExpressionPart>> expressionsByResult = new();

        // Étape 1 : Générer plein d'expressions valides et les regrouper par résultat
        while (expressionsByResult.Count < 8)
        {
            int a = UnityEngine.Random.Range(1, 9);
            int b = UnityEngine.Random.Range(1, 9);
            char op = ops[UnityEngine.Random.Range(0, ops.Length)];

            // Validation math
            if (op == '/' && (b == 0 || a % b != 0)) continue;
            if (op == '-' && a - b < 0) continue;

            int result = op switch
            {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' => a / b,
                _ => 0
            };

            // Regrouper par résultat
            if (!expressionsByResult.ContainsKey(result))
            {
                expressionsByResult.Add(result, new List<ExpressionPart>());
            }

            var expr = new ExpressionPart { left = a, op = op, right = b };

            if (!expressionsByResult[result].Any(e => e.left == a && e.op == op && e.right == b))
            {
                expressionsByResult[result].Add(expr);
            }

        }

        // Étape 2 : Choisir 4 résultats différents comme solutions
        List<int> possibleResults = expressionsByResult.Keys.ToList();
        while (solutions.Count < 4)
        {
            int r = possibleResults[UnityEngine.Random.Range(0, possibleResults.Count)];

            // Avoid duplicate results (so each solution is unique)
            if (solutions.Contains(r)) continue;

            if (!solutions.Contains(r))
            {
                solutions.Add(r);
            }
        }

        // Étape 3 : Sélectionner au moins une expression pour chaque solution
        foreach (int sol in solutions)
        {
            var exprs = expressionsByResult[sol];
            foreach (var expr in exprs)
            {
                validExpressions.Add(expr);
            }
        }

        // Étape 4 : Ajouter les valeurs aux boutons
        foreach (var expr in validExpressions)
        {
            buttonValues.Add(expr.left.ToString());
            buttonValues.Add(expr.op.ToString());
            buttonValues.Add(expr.right.ToString());
        }
    }
}