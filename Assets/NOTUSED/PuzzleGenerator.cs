using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
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
        solutions.Clear();
        validExpressions.Clear();
        buttonValues.Clear();

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



// This code doesnt work as good because I create 4 solution and try to create 4 random equation and each equation solve at least one solution
// The new method is the reverse, we generate 4 random equations and we check the solutions of each equations
/*
// Step 1: Generate 4 unique random 1-digit solutions
while (solutions.Count < 4)
{
    float rng = UnityEngine.Random.Range(1f, 9f);
    int rand = Convert.ToInt32(rng);
    if (!solutions.Contains(rand))
        solutions.Add(rand);
}

// Step 2: Generate 4 valid expression thatn match each solution
while (validExpressions.Count < 4)
{
    float af = UnityEngine.Random.Range(1f, 9f);
    int a = Convert.ToInt32(af);
    float bf = UnityEngine.Random.Range(1f, 9f);
    int b = Convert.ToInt32(bf);
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

    if (solutions.Contains(result))
    {
        var expr = new ExpressionPart { left = a, op = op, right = b };

        if (!validExpressions.Any(e => e.left == a && e.right == b && e.op == op))
        {
            validExpressions.Add(expr);
        }
    }
    */