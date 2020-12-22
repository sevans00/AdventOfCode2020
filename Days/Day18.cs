using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace AdventOfCode2020.Days
{
    public class Day18
    {

        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay18.txt";

        public static string[] testInput = new string[]
        {
            "1 + 2 * 3 + 4 * 5 + 6",
            "1 + (2 * 3) + (4 * (5 + 6))",
            "2 * 3 + (4 * 5)",
            "5 + (8 * 3 + 9 + 3 * 4 * 3)",
            "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
            "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2",
        };


        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;


            var expressions = inputStrings.Select(line => new Expression3(line)).ToList();

            for (int ii = 0; ii < inputStrings.Length; ii++)
            {
                Console.WriteLine($"{expressions[ii]}");
            }

            BigInteger sum = 0;
            foreach (var expression in expressions)
            {
                var value = (BigInteger)expression.Evaluate();
                //Console.WriteLine($"{value}");
                sum += value;
            }
            Console.WriteLine($"Sum: {sum}");
        }


        public class Expression3
        {
            public string line;
            public Expression3(string line)
            {
                this.line = line;
            }

            public override string ToString()
            {
                return $"Line:\t{line}\nEvaluate:\t{Evaluate()}";
            }

            public BigInteger Evaluate()
            {
                return Evaluate(line, '(', ')');
            }

            private BigInteger Evaluate(string statement, char openingBracket, char closingBracket)
            {
                //Evaluate all brackets:
                var firstBracket = statement.IndexOf(openingBracket);
                if (firstBracket != -1)
                {
                    var openingBrackets = 0;
                    for (int ii = firstBracket; ii < statement.Length; ii++)
                    {
                        if (statement[ii] == openingBracket)
                        {
                            openingBrackets++;
                        }
                        if (statement[ii] == closingBracket)
                        {
                            openingBrackets--;
                            if (openingBrackets == 0)
                            {
                                var endBracket = ii;
                                var previousStatement = statement.Substring(0, firstBracket);
                                var bracketStatement = statement.Substring(firstBracket + 1, (endBracket - 1) - firstBracket).Trim();
                                var nextStatement = statement.Substring(endBracket + 1).Trim();
                                
                                var bracketEvaluation = Evaluate(bracketStatement, openingBracket, closingBracket);
                                return Evaluate($"{previousStatement} {bracketEvaluation} {nextStatement}", openingBracket, closingBracket);
                            }
                        }
                    }
                }


                if (statement.Contains('*'))
                {
                    var multiplicands = statement.Split('*');
                    if (multiplicands.Length == 1)
                        return BigInteger.Parse(multiplicands[0]);

                    BigInteger product = 1;
                    foreach (var multiplicand in multiplicands)
                    {
                        product *= Evaluate(multiplicand, openingBracket, closingBracket);
                    }
                    return product;
                }

                if (statement.Contains('+'))
                {
                    var addicands = statement.Split('+');
                    if (addicands.Length == 1)
                        return BigInteger.Parse(addicands[0]);

                    BigInteger sum = 0;
                    foreach (var addicand in addicands)
                    {
                        sum += Evaluate(addicand, openingBracket, closingBracket);
                    }
                    return sum;
                }


                return BigInteger.Parse(statement.Trim());
            }
        }










        public class Expression2
        {
            public string line;
            public Expression2(string line)
            {
                this.line = line;
            }

            public override string ToString()
            {
                return $"Line:\t{line}\nEvaluate:\t{Evaluate()}";
            }

            public BigInteger Evaluate()
            {
                var reverseString = string.Concat(line.Reverse());

                return Evaluate(reverseString, ')', '(');
            }

            private BigInteger Evaluate(string statement, char openingBracket, char closingBracket)
            {
                if(statement[0] == openingBracket)
                {
                    var openingBrackets = 0;
                    for (int ii = 0; ii < statement.Length; ii++)
                    {
                        if (statement[ii] == openingBracket)
                        {
                            openingBrackets++;
                        }
                        if (statement[ii] == closingBracket)
                        {
                            openingBrackets--;
                            if (openingBrackets == 0)
                            {
                                var nextStatement = statement.Substring(ii+1).Trim();
                                var bracketStatement = statement.Substring(1, ii - 1).Trim();
                                if(nextStatement.Length == 0)
                                {
                                    return Evaluate(bracketStatement, openingBracket, closingBracket);
                                }
                                if (nextStatement[0] == '*')
                                    return Evaluate(bracketStatement, openingBracket, closingBracket) * Evaluate(nextStatement.Substring(1).Trim(), openingBracket, closingBracket);
                                if (nextStatement[0] == '+')
                                    return Evaluate(bracketStatement, openingBracket, closingBracket) + Evaluate(nextStatement.Substring(1).Trim(), openingBracket, closingBracket);
                                Console.WriteLine("Huh?");
                            }
                        }
                    }
                }

                var numbersAndOperators = statement.Split(' ');
                if (numbersAndOperators.Length > 1)
                {
                    var number = BigInteger.Parse(numbersAndOperators[0]);
                    var operatorChar = numbersAndOperators[1][0];

                    if (operatorChar == '*')
                        return number * Evaluate(string.Join(" ", numbersAndOperators.Skip(2)), openingBracket, closingBracket);
                    if (operatorChar == '+')
                        return number + Evaluate(string.Join(" ", numbersAndOperators.Skip(2)), openingBracket, closingBracket);
                    Console.WriteLine("Wat?");

                }
                return BigInteger.Parse(numbersAndOperators[0]);
            }
        }


        public class Expression
        {
            public int number1;
            public char operatorChar = '0';
            public int number2;
            public bool isBrackets;
            public Expression nextExpression = null;
            public Expression bracketExpression = null;
            public Expression(string line)
            {
                //Sub expression:
                //\(.+\)

                //Opening bracket!
                if (line[0] == ')')
                {
                    isBrackets = true;
                    var openingBrackets = 0;
                    for (int ii = 0; ii < line.Length; ii++)
                    {
                        if(line[ii] == ')')
                        {
                            openingBrackets++;
                        }
                        if(line[ii] == '(')
                        {
                            openingBrackets--;
                            if(openingBrackets == 1)
                            {
                                bracketExpression = new Expression(line.Substring(ii-1));
                                var subLine = line.Substring(1, ii - 1);

                                nextExpression = new Expression(subLine);
                            }
                        }
                    }
                    return;
                }

                var numbersAndOperators = line.Split(' ');
                if(numbersAndOperators.Length == 3)
                {
                    nextExpression = null;
                    number1 = int.Parse(numbersAndOperators[0]);
                    operatorChar = numbersAndOperators[1][0];
                    number2 = int.Parse(numbersAndOperators[2]);
                    return;
                }

                if(numbersAndOperators.Length == 1)
                {
                    number1 = int.Parse(numbersAndOperators[0]);
                    operatorChar = '0';
                    nextExpression = null;
                    return;
                }


                if (numbersAndOperators.Length == 2)
                {
                    number1 = int.Parse(numbersAndOperators[0]);
                    operatorChar = numbersAndOperators[1][0];
                    nextExpression = null;
                    return;
                }
                number1 = int.Parse(numbersAndOperators[0]);
                operatorChar = numbersAndOperators[1][0];
                nextExpression = new Expression(string.Join(" ", numbersAndOperators.Skip(2)));
            }
            private int Evaluate()
            {
                if (operatorChar == '0')
                {
                    return number1;
                }
                if (isBrackets)
                {
                    if (operatorChar == '*')
                        return bracketExpression.Evaluate() * nextExpression.Evaluate();
                    if (operatorChar == '*')
                        return bracketExpression.Evaluate() + nextExpression.Evaluate();
                }

                if (operatorChar == '*')
                {
                    if (nextExpression != null)
                    {
                        return number1 * nextExpression.Evaluate();
                    }
                    return number1 * number2;
                }
                if (nextExpression != null)
                {
                    return number1 + nextExpression.Evaluate();
                }
                return number1 + number2;
            }

            public override string ToString()
            {
                if(nextExpression == null)
                {
                    return $"{number1} {operatorChar} {number2} = {Evaluate()}";
                }
                return $"{number1} {operatorChar} [{nextExpression}] = {Evaluate()}";
            }

        }
    }

}