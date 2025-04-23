using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleGeneratorV3 : MonoBehaviour
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

    public void GeneratePuzzle()
    {
        buttonValues.Clear();
        solutions.Clear();
        validExpressions.Clear();

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

}