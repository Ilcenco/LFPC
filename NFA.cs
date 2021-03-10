using System.Collections.Generic;

namespace NFAtoDFA
{
    public class NFATransition
    {
        public string transition;
        public int transitionState;
    }
    public class NFAState
    {
        public int state;
        public List<NFATransition> transitions = new List<NFATransition>();
    }
    public class NFA
    {
        public int numberOfStates;
        public int initialState;
        public List<string> alphabet = new List<string>();
        public List<int> finalStates = new List<int>();
        public List<NFAState> states = new List<NFAState>();

    }

    public class DFATransition
    {
        public List<int> transitionStates = new List<int>();
        public string transition;
    }
    public class DFAState
    {
        public List<int> states = new List<int>();
        public List<DFATransition> transitions = new List<DFATransition>();
    }

    public class DFA
    {
        public int numberOfStates;
        public int initialState;
        public List<string> alphabet = new List<string>();
        public List<int> finalStates = new List<int>();
        public List<DFAState> states = new List<DFAState>();
    }
}
