using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Calculator
{
    internal class FileName
    {
        private static string eval;

        private static double evaluatePostfix(List<string> postfix)
        {
            List<string> stack = new List<string>();

            for (int i = 0; i < postfix.Count; i++)
            {
                string s = postfix[i];
                if ("+-*/%^".Contains(s))
                {
                    switch (s)
                    {
                        case "+":
                            stack[stack.Count - 2] = Convert.ToString(Convert.ToDouble(stack[stack.Count - 1]) + Convert.ToDouble(stack[stack.Count - 2])); break;
                        case "-":
                            stack[stack.Count - 2] = Convert.ToString(Convert.ToDouble(stack[stack.Count - 2]) - Convert.ToDouble(stack[stack.Count - 1])); break;
                        case "*":
                            stack[stack.Count - 2] = Convert.ToString(Convert.ToDouble(stack[stack.Count - 1]) * Convert.ToDouble(stack[stack.Count - 2])); break;
                        case "/":
                            stack[stack.Count - 2] = Convert.ToString(Convert.ToDouble(stack[stack.Count - 2]) / Convert.ToDouble(stack[stack.Count - 1])); break;
                        case "%":
                            stack[stack.Count - 2] = Convert.ToString(Convert.ToDouble(stack[stack.Count - 2]) % Convert.ToDouble(stack[stack.Count - 1])); break;
                        case "^":
                            stack[stack.Count - 2] = Convert.ToString(Math.Pow(Convert.ToDouble(stack[stack.Count - 2]), Convert.ToDouble(stack[stack.Count - 1]))); break;
                        default:
                            break;
                    }
                    stack.RemoveAt(stack.Count - 1);
                }
                else
                {
                    stack.Add(s);
                }
            }
            return Convert.ToDouble(stack[0]);
        }

        private static void printList(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }

        }
        private static bool pop(string a, string b)
        {
            int prioA;
            int prioB;
            if (a == "(") { return false; }
            switch (a)
            {
                case "+": prioA = 1; break;
                case "-": prioA = 1; break;
                case "*": prioA = 2; break;
                case "/": prioA = 2; break;
                case "%": prioA = 2; break;
                case "^": prioA = 3; break;
                default: prioA = 1; break;
            }

            switch (b)
            {
                case "+": prioB = 1; break;
                case "-": prioB = 1; break;
                case "*": prioB = 2; break;
                case "/": prioB = 2; break;
                case "%": prioB = 2; break;
                case "^": prioB = 3; break;
                default: prioB = 0; break;
            }
            if (prioA < prioB) { return false; }
            return true;
        }
        private static void popIfT(List<string> parsed, List<string> postfix)
        {
            List<string> stack = new List<string>();
            for (int pIndex = 0; pIndex < parsed.Count; pIndex++)
            {
                if (parsed.Count == pIndex)
                {
                    return;
                }
                //Console.WriteLine("index: " + pIndex);
                //Console.WriteLine("parsed[pIndex]: " + parsed[pIndex]);
                //Console.WriteLine("parsed[pIndex][0]: " + parsed[pIndex][0]);
                if (char.IsDigit((char)parsed[pIndex][parsed[pIndex].Length - 1]))
                {
                    postfix.Add(parsed[pIndex]);
                }

                else if (parsed[pIndex] == "(")
                {
                    stack.Add("(");
                }

                else if ("+-*/^%".Contains(parsed[pIndex]))
                {
                    if (stack.Count == 0)
                    {
                        stack.Add(parsed[pIndex]);
                    }

                    else if (stack.Count > 0)
                    {
                        while (pop(stack[stack.Count - 1], parsed[pIndex]))
                        {
                            postfix.Add(stack[stack.Count - 1]);
                            stack.RemoveAt(stack.Count - 1);
                            if (stack.Count == 0)
                            {
                                break;
                            }
                        }
                        stack.Add(parsed[pIndex]);
                    }

                }
                else if (parsed[pIndex] == ")")
                {
                    while (!(stack[stack.Count - 1] == "("))
                    {
                        postfix.Add(stack[stack.Count - 1]);
                        stack.RemoveAt(stack.Count - 1);
                    }
                    //removes last (
                    stack.RemoveAt(stack.Count - 1);
                }
            }

            for (int i = stack.Count - 1; i >= 0; i--)
            {
                postfix.Add(stack[i]);
            }
        }


        public static void parse(string s, List<string> parsed)
        {
            bool isSign = true;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == ' ')
                {
                    continue;
                }

                if ('-' == c && isSign)
                {
                    bool negative = true;

                    //************************************
                    int j;
                    int z = 0;
                    int a = 0;

                    for (j = 1; s[i + j] == '-'; j++)
                    {
                        negative = !negative;
                    }

                    if (s[i + j] == '(')
                    {
                        parsed.Add("-1");
                        parsed.Add("*");
                    }

                    if (char.IsDigit(s[i + j]))
                    {
                        Console.WriteLine("heyo");
                        for (z = 0; char.IsDigit(s[i + j + z]); z++)
                        {
                            if (negative)
                            {
                                a = 1;
                            }
                            else
                            {
                                a = 0;
                            }
                            if (i + j + z + 1 >= s.Length)
                            {

                                z++;
                                break;
                            }
                        }
                        Console.WriteLine(z);
                        parsed.Add(s.Substring(i + j - a, z + a));
                        Console.WriteLine("Substring: " + parsed[parsed.Count - 1]);
                    }

                    isSign = false;
                    i += j + z - 1;
                    //***********************************

                    //*************************************
                    /*
                    if (char.IsDigit(s[i+1]))
                    {
                        int j = 1;

                        while (char.IsDigit(s[i + j]))
                        {
                            j++;
                            if (i + j >= s.Length)
                            {
                                break;
                            }
                        }
                        //Console.WriteLine(i + "" + j);
                        //Console.WriteLine(s.Substring(i, j));
                        parsed.Add(s.Substring(i, j));
                        //Console.WriteLine("parsed: " + parsed[0]);
                        isSign = false;
                        i += j - 1;
                    }
                    */
                    //**********************************************
                }
                else if ("()*-+/^%".Contains(c))
                {
                    parsed.Add(Convert.ToString(c));
                    isSign = true;
                    //Console.WriteLine(c);
                }

                else if (char.IsDigit(s[i]))
                {
                    int j = 0;

                    while (char.IsDigit(s[i + j]))
                    {
                        j++;

                        if (i + j >= s.Length)
                        {
                            break;
                        }
                        if (s[i + j] == '(')
                        {
                            parsed.Add("*");
                        }
                    }
                    //Console.WriteLine(s.Substring(i, j));
                    parsed.Add(s.Substring(i, j));
                    isSign = false;
                    i += j - 1;
                }
            }
            return;
        }
        private static void calculatorLoop()
        {
            Console.WriteLine("enter q to quit");
            bool cond = true;
            while (cond)
            {
                List<string> postfixNotation = new List<string>();
                List<string> parsed = new List<string>();
                bool acceptableInput = false;
                while (!acceptableInput)
                {
                    eval = Console.ReadLine();
                    if (eval == "q")
                    {
                        return;
                    }
                    for (int i = 0; i < eval.Length; i++)
                    {
                        if (char.IsDigit(eval[i]) || eval[i] == '(')
                        {
                            acceptableInput = true;
                        }
                    }
                }

                parse(eval, parsed);
                printList(parsed);
                //Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------");
                popIfT(parsed, postfixNotation);
                printList(postfixNotation);

                Console.WriteLine("= " + evaluatePostfix(postfixNotation));
            }
        }
        public static void Main(string[] args)
        {
            calculatorLoop();
        }
    }
}