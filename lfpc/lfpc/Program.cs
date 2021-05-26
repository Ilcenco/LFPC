using System;
using System.Collections.Generic;

namespace lfpc
{
    class Program
    {
        static int n, n1, n2;
        static void Main(string[] args)
        {

            string[] prods = new string[10];
            string[] leads = new string[10];
            string[] trails = new string[10];
            string[] nonterms = new string[10];
            string[] terms = new string[10];

            char[,] op_table = new char[20,20];


            Console.WriteLine("Enter productions number and productions");
            n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n2; i++)
            {
                prods[i] = Console.ReadLine();
            }


            Console.WriteLine("Enter terminals number and terminals");
            n2 = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n2; i++)
            {
                terms[i] = Console.ReadLine();
            }
            terms[n2] = "$";
            n2++;


            Console.WriteLine("Enter non-terminals number and non-terminals");
            for (int i = 0; i < n1; i++)
            {
                Console.WriteLine("Enter Non-Terminal : ");
                nonterms[i] = Console.ReadLine();
                Console.WriteLine("Enter Leads of " + nonterms[i] + " : ");
                leads[i] = Console.ReadLine();
                Console.WriteLine("Enter Trails of " + nonterms[i] + " : ");
                trails[i] = Console.ReadLine();    
            }


            Console.WriteLine("Enter the rules, stop to exit");
            string rule = "";
            while(rule != "stop")
            {
                rule = Console.ReadLine();
                if (rule[0] == '1')
                {
                    int row = getPosition(terms, rule.Substring(2, 1), n2);
                    int column = getPosition(terms, rule.Substring(4, 1), n2);
                    op_table[row,column] = '=';
                }
                if (rule[0] == '2')
                {
                    int ntp = getPosition(nonterms, rule.Substring(4, 1), n1);
                    int row = getPosition(terms, rule.Substring(2, 1), n2);
                    for (int j = 0; j < leads[ntp].Length; j++)
                    {
                        int col = getPosition(terms, leads[ntp].Substring(j, 1), n2);
                        op_table[row,col] = '<';
                    }
                }
                if (rule[0] == '3')
                {
                    int col = getPosition(terms, rule.Substring(4, 1), n2);
                    int ntp = getPosition(nonterms, rule.Substring(2, 1), n1);
                    for (int j = 0; j < trails[ntp].Length; j++)
                    {
                        int row = getPosition(terms, trails[ntp].Substring(j, 1), n2);
                        op_table[row,col] = '>';
                    }
                }
            }


            for (int j = 0; j < leads[0].Length; j++)
            {
                int col = getPosition(terms, leads[0].Substring(j, 1), n2);
                op_table[n2 - 1,col] = '<';
            }
            for (int j = 0; j < trails[0].Length; j++)
            {
                int row = getPosition(terms, trails[0].Substring(j, 1), n2);
                op_table[row,n2 - 1] = '>';
            }


            Console.WriteLine("Grammar is: ");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(prods[i]);
            }

            for (int j = 0; j < n2; j++)
            {
                Console.WriteLine("\t" + terms[j]);
            }

            for (int i = 0; i < n2; i++)
            {
                Console.WriteLine(terms[i] + "\t");

                for (int j = 0; j < n2; j++)
                {
                    Console.WriteLine(op_table[i, j] + "\t");
                }

            }

            char c;
            string ip;
            Queue<string> op_stack = new Queue<string>();
            op_stack.Enqueue("$");


            Console.WriteLine("Enter string to parse");
            ip = Console.ReadLine();
            ip += "$";

            while (true)
            {
                foreach (var el in op_stack)
                {
                    Console.WriteLine(el);
                }
                Console.WriteLine("\t");
                Console.WriteLine(ip + "\t");
                int row = getPosition(terms, op_stack.Dequeue(), n2);
                int column = getPosition(terms, ip.Substring(0, 1), n2);
                if (op_table[row,column] == '<')
                {
                    op_stack.Enqueue("<");
                    op_stack.Enqueue(ip.Substring(0, 1));
                    ip = ip.Substring(1);
                    Console.WriteLine("\t");
                }
                else if (op_table[row,column] == '=')
                {
                    op_stack.Enqueue("=");
                    op_stack.Enqueue(ip.Substring(0, 1));
                    ip = ip.Substring(1);
                    Console.WriteLine("\t");
                }
                else if (op_table[row,column] == '>')
                {
                    string last;
                    do
                    {
                        op_stack.Dequeue();
                        last = op_stack.Dequeue();
                        op_stack.Dequeue();
                    } while (last != "<");
                    Console.WriteLine("\t");
                }
                else
                {
                    if (ip[0] == '$' && op_stack.Dequeue() == "$")
                    {
                        Console.WriteLine("Parsed");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Not Parsed");
                        break;
                    }
                }
            }

        }
        public static int getPosition(string[] arr, string q, int size)
        {
            for (int i = 0; i < size; i++)
            {
                if (q == arr[i])
                    return i;
            }
            return -1;
        }
    }
}
