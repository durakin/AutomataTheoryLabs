using System;
using System.Collections.Generic;
using System.Linq;

namespace FSA
{
    public class Dfsa
    {
        private HashSet<string> _states;
        private readonly HashSet<string> _finalStates;
        private readonly HashSet<char> _alphabet;
        private readonly Dictionary<(string, char), string> _transitionTable;

        private string _currentState;
        private readonly string _initialState;

        public bool Iterate(string input, out string log)
        {
            log = "";
            _currentState = _initialState;
            if (input.Any(operation => !_alphabet.Contains(operation)))
                throw new ArgumentException("Input contains symbols besides elements of alphabet");
            foreach (var operation in input)
            {
                log += _currentState + " --" + operation + "-> ";
                _currentState = _transitionTable[(_currentState, operation)];
            }

            log += _currentState + "\n " + input + " "+ (_finalStates.Contains(_currentState) ?  "Accept" : "Reject");

            return _finalStates.Contains(_currentState);
        }

        public Dfsa(HashSet<char> alphabet, HashSet<string> states, string initialState,
            Dictionary<(string, char), string> transitionTable, HashSet<string> finalStates)
        {
            if (alphabet.Count == 0) throw new ArgumentException("Alphabet must not be empty");
            if (states.Count == 0) throw new ArgumentException("States set must not be empty");
            if (finalStates.Count == 0) throw new ArgumentException("At least one state must be final");
            if (!states.Contains(initialState))
                throw new ArgumentException("Current state must be an element of states set");
            if (finalStates.Count == 0 || finalStates.Any(state => !states.Contains(state)))
                throw new ArgumentException("Final states must not be empty and must be a subset of states set");
            if (transitionTable.Any(transition => !states.Contains(transition.Key.Item1) ||
                                                  !alphabet.Contains(transition.Key.Item2)
                                                  || !states.Contains(transition.Value)) || states.Any(i =>
                alphabet.Any(j => !transitionTable.ContainsKey((i, j)))))
                throw new ArgumentException(
                    "Table of transitions must completely cover cartesian product of states and alphabet and only");

            _states = states;
            _finalStates = finalStates;
            _alphabet = alphabet;
            _transitionTable = transitionTable;
            _initialState = initialState;
            _currentState = initialState;
        }
    }
}
