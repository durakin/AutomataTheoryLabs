using System;
using System.Collections.Generic;
using System.Linq;

namespace FSA
{
    public class Nfsa
    {
        private readonly HashSet<string> _states;
        private readonly HashSet<string> _finalStates;
        private readonly HashSet<char> _alphabet;
        private readonly Dictionary<(string, char), HashSet<string>> _transitionTable;

        private HashSet<string> _currentStates;
        private readonly string _initialState;

        public bool Iterate(string input, out string log)
        {
            log = "";
            _currentStates.Clear();
            _currentStates.Add(_initialState);
            if (input.Any(operation => !_alphabet.Contains(operation)))
                throw new ArgumentException("Input contains symbols besides elements of alphabet");

            foreach (var operation in input)
            {
                var newStates = new HashSet<string>();

                foreach (var newState in _currentStates.Where(newState =>
                    _transitionTable.ContainsKey((newState, operation))))
                {
                    newStates.UnionWith(_transitionTable[(newState, operation)]);
                }

                _currentStates = newStates;
            }

            //log += _finalStates.Contains(_currentState) ?  "Accept" : "Reject");
            log += input + " " + (_currentStates.Any(state => _finalStates.Contains(state)) ? "Accept" : "Reject");
            return _currentStates.Any(state => _finalStates.Contains(state));
        }


        public Nfsa(HashSet<char> alphabet, HashSet<string> states, string initialState,
            Dictionary<(string, char), HashSet<string>> transitionTable, HashSet<string> finalStates)
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
                                                  || transition.Value.Any(newState => !states.Contains(newState))))
                throw new ArgumentException(
                    "Table of transitions must only refer states of automate");

            _states = states;
            _finalStates = finalStates;
            _alphabet = alphabet;
            _transitionTable = transitionTable;
            _initialState = initialState;
            _currentStates = new HashSet<string> { initialState };
        }
    }
}