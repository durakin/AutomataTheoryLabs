using System;
using System.Collections.Generic;
using System.Linq;

namespace FSA
{
    public class Pda
    {
        private HashSet<string> _states;
        private readonly HashSet<string> _finalStates;
        private readonly HashSet<char> _alphabet;
        private readonly Dictionary<(string, char, char), (string, char[])> _transitionTable;
        private readonly Stack<char> _stack;

        private string _currentState;
        private readonly string _initialState;
        private readonly char _initialStackElement;

        public bool Iterate(string input, out string log)
        {
            log = "";
            _currentState = _initialState;
            _stack.Clear();
            _stack.Push(_initialStackElement);
            if (input.Any(operation => !_alphabet.Contains(operation)))
                throw new ArgumentException("Input contains symbols besides elements of alphabet");
            
            foreach (var operation in input)
            {
                var stackElement = _stack.Pop();
                log += _currentState + " (" + stackElement + ") " + " --" + operation + "-> ";
                var (newState, newElements) = _transitionTable[(_currentState, operation, stackElement)];
                _currentState = newState;
                foreach (var element in newElements)
                {
                    _stack.Push(element);
                }
            }

            log += _currentState + " (" + _stack.Peek() + ") "+ "\n " + input + " " +
                   (_finalStates.Contains(_currentState) ? "Accept" : "Reject");

            return _finalStates.Contains(_currentState);
        }

        public Pda(HashSet<char> alphabet, HashSet<string> states, string initialState,
            Dictionary<(string, char, char), (string, char[])> transitionTable, HashSet<string> finalStates)
        {
            if (alphabet.Count == 0) throw new ArgumentException("Alphabet must not be empty");
            if (states.Count == 0) throw new ArgumentException("States set must not be empty");
            if (finalStates.Count == 0) throw new ArgumentException("At least one state must be final");
            if (!states.Contains(initialState))
                throw new ArgumentException("Current state must be an element of states set");
            if (finalStates.Count == 0 || finalStates.Any(state => !states.Contains(state)))
                throw new ArgumentException("Final states must not be empty and must be a subset of states set");

            _states = states;
            _finalStates = finalStates;
            _alphabet = alphabet;
            _transitionTable = transitionTable;
            _initialState = initialState;
            _currentState = initialState;
        }
    }
}