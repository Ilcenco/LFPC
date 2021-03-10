using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NFAtoDFA
{
    class Program
    {
        static void Main(string[] args)
        {
            NFA nfa = new NFA();
            readFile(nfa);
            DFA dfa = new DFA();
            constructDFA(nfa, dfa);
            printDFA(dfa);
            Console.ReadLine();
        }
        static void readFile(NFA nfa)
        {
            string fileName;
            string line;

            Console.WriteLine("Enter your NFA files full location");
            fileName = Console.ReadLine();
            fileName = "C:\\Users\\IlcencoEugeniu\\Desktop\\Fibonacci Lab\\test\\test\\NFA.txt";
            StreamReader file = new StreamReader(@fileName);

            nfa.numberOfStates = int.Parse(file.ReadLine());

            string alphabet = file.ReadLine();

            foreach (char ch in alphabet)
            {
                nfa.alphabet.Add(ch.ToString());
            }

            string finalStates = file.ReadLine();
            List<string> finalStatesString = finalStates.Split().ToList();
            for (int i = 0; i < finalStatesString.Count(); i++)
            {
                if (finalStatesString[i] != "")
                    nfa.finalStates.Add(int.Parse(finalStatesString[i]));
            }

            nfa.initialState = int.Parse(file.ReadLine());

            for (int i = 0; i < nfa.numberOfStates; i++)
            {
                nfa.states.Add(new NFAState());
                nfa.states[i].state = i;
            }

            while ((line = file.ReadLine()) != null && line != "")
            {
                List<string> lineString = line.Split().ToList();
                NFATransition transition = new NFATransition();
                int state = int.Parse(lineString[0]);
                transition.transition = lineString[1];
                transition.transitionState = int.Parse(lineString[2]);

                for (int i = 0; i < nfa.states.Count(); i++)
                {
                    if (nfa.states[i].state == state)
                    {
                        nfa.states[i].transitions.Add(transition);
                    }
                }
            }
        }

        static void printDFA(DFA dfa)
        {
            Console.WriteLine("----Equivalent DFA----");
            Console.WriteLine(dfa.numberOfStates);
            for (int i = 0; i < dfa.alphabet.Count(); i++)
            {
                Console.Write(dfa.alphabet[i]);
            }
            Console.WriteLine(" ");
            for (int i = 0; i < dfa.finalStates.Count(); i++)
            {
                Console.Write(dfa.finalStates[i] + " ");
            }
            Console.WriteLine(" ");
            Console.WriteLine(dfa.initialState);

            for (int i = 0; i < dfa.states.Count(); i++)
            {
                for (int j = 0; j < dfa.states[i].transitions.Count(); j++)
                {
                    Console.WriteLine(dfa.states[i].states.Max()              + " "
                                    + dfa.states[i].transitions[j].transition + " "
                                    + dfa.states[i].transitions[j].transitionStates.Max());
                }
            }
        }

        static void constructDFA(NFA nfa, DFA dfa)
        {
            dfa.initialState = nfa.initialState;
            dfa.finalStates = nfa.finalStates;
            dfa.alphabet = nfa.alphabet;

            DFAState initalState = new DFAState();
            DFATransition dfaTransition = new DFATransition();
            initalState.states.Add(dfa.initialState);

            for (int i = 0; i < dfa.alphabet.Count(); i++)
            {
                dfaTransition = new DFATransition();
                dfaTransition.transition = dfa.alphabet[i];
                initalState.transitions.Add(dfaTransition);
            }

            for (int i = 0; i < initalState.transitions.Count(); i++)
            {
                for (int j = 0; j < nfa.states.Count(); j++)
                {
                    if (nfa.states[j].state == initalState.states[0])
                    {
                        for (int k = 0; k < nfa.states[j].transitions.Count(); k++)
                        {
                            if (nfa.states[j].transitions[k].transition == initalState.transitions[i].transition)
                            {
                                initalState.transitions[i].transitionStates.Add(nfa.states[j].transitions[k].transitionState);
                            }
                        }
                    }
                }
            }
            dfa.states.Add(initalState);


            //start of adding dfa states
            bool createNewState = true;
            while (createNewState == true)
            {
                createNewState = false;
                DFAState newDFAState = new DFAState();

                for (int i = 0; i < dfa.states.Count(); i++)
                {
                    for (int j = 0; j < dfa.states[i].transitions.Count(); j++)
                    {
                        bool visited = false;
                        for (int k = 0; k < dfa.states.Count(); k++)
                        {
                            if (dfa.states[k].states.SequenceEqual(dfa.states[i].transitions[j].transitionStates))
                            {
                                visited = true;
                                continue;
                            }
                        }

                        if (visited == false)
                        {
                            createNewState = true;
                            newDFAState.states = dfa.states[i].transitions[j].transitionStates;
                            break;
                        }
                        visited = false;
                    }

                    if (createNewState == true)
                    {
                        break;
                    }
                }

                if (createNewState == false)
                {
                    break;
                }

                DFATransition transition;
                for(int i = 0; i < dfa.alphabet.Count(); i++)
                {
                    transition = new DFATransition();
                    transition.transition = dfa.alphabet[i];
                    newDFAState.transitions.Add(transition);
                }

                for (int i = 0; i < newDFAState.transitions.Count(); i++)
                {
                    for (int j = 0; j < nfa.states.Count(); j++)
                    {
                        foreach (var s in newDFAState.states)
                        {
                            if (nfa.states[j].state == s)
                            {
                                foreach (var possibleTransition in nfa.states[j].transitions)
                                {
                                    if (possibleTransition.transition == newDFAState.transitions[i].transition)
                                    {
                                        newDFAState.transitions[i].transitionStates.Add(possibleTransition.transitionState);
                                    }
                                }
                            }
                        }
                    }
                }

                dfa.states.Add(newDFAState);
            }
        }
    }
}
