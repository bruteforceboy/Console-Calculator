using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorParser
{
    class Program
    {
        // TODO: add comments!
        // add space between operators and operands e.g ( 2 + 3 + ( 5 * 6 ) )
        // uses BODMAS

        static string[] solve(int leftIndex, int rightIndex, string[] ops)
        {
            // computing multiplication/division first 
            // firstResult will contain equation after processing multiplication/division operations 
            Stack<String> firstResult = new Stack<String>(); 
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                if (firstResult.Count == 0)
                {
                    firstResult.Push(ops[i]);
                }
                else if (firstResult.Peek().Equals("*"))
                {
                    firstResult.Pop();
                    double lastNumber = Double.Parse(firstResult.Peek());
                    firstResult.Pop();
                    double currentNumber = Double.Parse(ops[i]);
                    double product = lastNumber * currentNumber;
                    firstResult.Push(product.ToString());
                }
                else if (firstResult.Peek().Equals("/"))
                {
                    firstResult.Pop();
                    double lastNumber = Double.Parse(firstResult.Peek());
                    firstResult.Pop();
                    double currentNumber = Double.Parse(ops[i]);
                    double quotient = lastNumber / currentNumber;
                    firstResult.Push(quotient.ToString());
                }
                else
                {
                    firstResult.Push(ops[i]);
                }
            }

            // addition/subtraction next 
            // secondResult will contain the final result after processing addition/subtraction operations

            Stack<String> finalResult = new Stack<String>();
            foreach (var op in firstResult) 
            {
                if (finalResult.Count == 0)
                {
                    finalResult.Push(op);
                }
                else if (finalResult.Peek().Equals("+"))
                {
                    finalResult.Pop();
                    double lastNumber = Double.Parse(finalResult.Peek());
                    finalResult.Pop();
                    double currentNumber = double.Parse(op);
                    double sum = lastNumber + currentNumber;
                    finalResult.Push(sum.ToString());
                }
                else if (finalResult.Peek().Equals("-"))
                {
                    finalResult.Pop();
                    double lastNumber = double.Parse(finalResult.Peek());
                    finalResult.Pop();
                    double currentNumber = double.Parse(op);
                    double difference = lastNumber - currentNumber;
                    finalResult.Push(difference.ToString());
                }
                else
                {
                    finalResult.Push(op);
                }
            }

            if (leftIndex == -1)
            {
                string[] res = new string[1];
                res[0] = finalResult.Peek();
                return res;
            }

            int newEquationLength = ops.Length - (rightIndex - leftIndex);
            string[] newEquation = new string[newEquationLength];
            for (int i = 0; i < leftIndex; i++)
            {
                newEquation[i] = ops[i];
            }
            newEquation[leftIndex] = finalResult.Peek();
            for (int i = leftIndex + 1; i < newEquationLength; i++)
            {
                newEquation[i] = ops[rightIndex + 1];
                rightIndex++;
            }
            return newEquation;
        }

        static bool isValidExpression(int left, int right, string[] ops)
        {
            // values in even positions should be numbers 
            // values in odd positions should be operators
            // number of values should be odd too

            for (int i = left + 1; i < right; i++)
            {
                if ((i - left - 1) % 2 == 0)
                {
                    if (!Double.TryParse(ops[i], out var parsedNumber))
                    {
                        return false;
                    }
                }
                else
                {
                    if (ops[i] != "+" && ops[i] != "-" && ops[i] != "/" && ops[i] != "*")
                    {
                        return false;
                    }
                }
            }

            if ((right - left - 1) % 2 == 0)
            {
                return false;
            }
            return true;
        }

        static bool validateBracketSequence(String[] ops)
        {
            int balance = 0;
            foreach (String op in ops)
            {
                if (op == "(")
                {
                    balance += 1;
                }
                else if (op == ")")
                {
                    balance -= 1;
                }
                if (balance < 0)
                {
                    return false;
                }
            }
            return (balance == 0);
        }
        static void invalidEquation()
        {
            Console.WriteLine("You entered an invalid equation");
            Console.ReadLine();
            Environment.Exit(0);
        }
        static double parseEquation(string equation)
        {
            equation = equation.TrimEnd(); // remove whitespaces from the end of the equation

            string[] ops = equation.Split(' ');

            if (!validateBracketSequence(ops))
            {
                invalidEquation();
            }

            Stack<int> openBracket = new Stack<int>();
            for (int i = 0; i < ops.Length; i++)
            {
                if (ops[i].Equals(")"))
                {
                    int lastOpenBracketIdx = openBracket.Peek();
                    openBracket.Pop();
                    if (!isValidExpression(lastOpenBracketIdx, i, ops))
                    {
                        invalidEquation();
                    }
                    ops = solve(lastOpenBracketIdx, i, ops);
                    i -= (i - lastOpenBracketIdx);
                } 
                else if (ops[i].Equals("("))
                {
                    openBracket.Push(i);
                }
            }
            if (!isValidExpression(-1, ops.Length, ops))
            {
                invalidEquation();
            }
            string[] finalAnswer = solve(-1, ops.Length, ops);
            return Double.Parse(finalAnswer[0]);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter an equation: ");
                string equation = Console.ReadLine();

                Console.WriteLine("Solution: " + parseEquation(equation));
                Console.ReadLine();
            }
        }
    }
}
