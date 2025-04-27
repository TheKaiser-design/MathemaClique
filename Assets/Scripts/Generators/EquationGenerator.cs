using System.Collections.Generic;
using UnityEngine;

public class EquationGenerator
{
    public struct ExpressionPart
    {
        public int leftOperand;
        public int rightOperand;
        public char op;
    }

    private List<int> solutions = new List<int>();
    private List<ExpressionPart> validExpressions = new List<ExpressionPart>();
    private List<string> values = new List<string>();
    private int solutionsCount;

    public List<int> Solutions => solutions;
    public List <ExpressionPart> ValidExpressions => validExpressions;
    public List <string> Values => values;

    public EquationGenerator(int solutionsCount)
    {
        this.solutionsCount = solutionsCount;
    }

    public void Generate()
    {
        char[] ops = { '+', '-', '*', '/' };

        while (validExpressions.Count < solutionsCount)
        {
            int a = Random.Range(1, 9);
            int b = Random.Range(1, 9);
            char currentOP = ops[Random.Range(0, ops.Length)];

            // Avoid bad math
            if (currentOP == '/' && (b == 0 || a % b != 0)) continue;
            if (currentOP == '-' && a - b < 0) continue; // optional: avoid negatives

            int result = currentOP switch
            {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' => a / b,
                _ => 0
            };

            // Avoid duplicate results (so each solution is unique)
            if (solutions.Contains(result)) continue;

            validExpressions.Add(new ExpressionPart { leftOperand = a, op = currentOP, rightOperand = b });
            solutions.Add(result);
        }

        // Extract numbers and operators from expressions
        foreach (var expr in validExpressions)
        {
            values.Add(expr.leftOperand.ToString());
            values.Add(expr.op.ToString());
            values.Add(expr.rightOperand.ToString());
        }
    }

    public void Reset()
    {
        solutions.Clear();
        validExpressions.Clear();
        values.Clear();
    }
}
