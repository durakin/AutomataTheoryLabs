using System;
using System.Collections.Generic;

namespace FSA
{
    class Program
    {
        static void Main(string[] args)
        {
            #region DFSA

            var dfsaAlphabet = new HashSet<char>
            {
                '0',
                '1'
            };
            var dfsaStates = new HashSet<string>
            {
                "q0",
                "q1",
                "q2",
                "q3",
                "q4",
                "q5",
                "q6",
                "q7",
                "q8",
                "q9",
                "q10",
                "q11",
                "q12",
                "q13",
                "q14"
            };
            var dfsaTransitionTable = new Dictionary<(string, char), string>
            {
                { ("q0", '0'), "q1" },
                { ("q0", '1'), "q8" },
                { ("q1", '0'), "q2" },
                { ("q1", '1'), "q5" },
                { ("q2", '0'), "q2" },
                { ("q2", '1'), "q3" },
                { ("q3", '0'), "q4" },
                { ("q3", '1'), "q3" },
                { ("q4", '0'), "q2" },
                { ("q4", '1'), "q3" },
                { ("q5", '0'), "q6" },
                { ("q5", '1'), "q7" },
                { ("q6", '0'), "q6" },
                { ("q6", '1'), "q5" },
                { ("q7", '0'), "q6" },
                { ("q7", '1'), "q7" },
                { ("q8", '0'), "q9" },
                { ("q8", '1'), "q10" },
                { ("q9", '0'), "q13" },
                { ("q9", '1'), "q14" },
                { ("q10", '0'), "q11" },
                { ("q10", '1'), "q10" },
                { ("q11", '0'), "q11" },
                { ("q11", '1'), "q12" },
                { ("q12", '0'), "q11" },
                { ("q12", '1'), "q10" },
                { ("q13", '0'), "q13" },
                { ("q13", '1'), "q14" },
                { ("q14", '0'), "q9" },
                { ("q14", '1'), "q14" }
            };
            var dfsaFinalStates = new HashSet<string>
            {
                "q2", "q5", "q9", "q10"
            };
            var dfsAutomata = new Dfsa(dfsaAlphabet, dfsaStates, "q0", dfsaTransitionTable, dfsaFinalStates);
            
            string dfsaLog;
            
            dfsAutomata.Iterate("0", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("10", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("11", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("100", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("101", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("110", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("111", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1000", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1001", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1010", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1011", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1100", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1101", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1110", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("1111", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("110011", out dfsaLog);
            Console.WriteLine(dfsaLog);
            dfsAutomata.Iterate("100110", out dfsaLog);
            Console.WriteLine(dfsaLog);
            
            #endregion
            
            #region NFSA

            var alphabet = new HashSet<char>
            {
                '0',
                '1'
            };
            var states = new HashSet<string>
            {
                "q0",
                "q1",
                "q2",
                "q3",
                "q4",
                "q5"
            };
            var transitionTable = new Dictionary<(string, char), HashSet<string>>
            {
                { ("q0", '0'), new HashSet<string> {"q0"} },
                { ("q0", '1'),  new HashSet<string> {"q0", "q1" } },
                { ("q1", '0'),  new HashSet<string> {"q2" } },
                { ("q1", '1'),  new HashSet<string> {"q2" } },
                { ("q2", '0'),  new HashSet<string> {"q3" } },
                { ("q2", '1'),  new HashSet<string> {"q3" } },
                { ("q3", '0'),  new HashSet<string> {"q4" } },
                { ("q3", '1'),  new HashSet<string> {"q4" } },
                { ("q4", '0'),  new HashSet<string> {"q5" } },
                { ("q4", '1'),  new HashSet<string> {"q5" } },
            };
            var finalStates = new HashSet<string>
            {
                "q1", "q2", "q3", "q4", "q5"
            };
            var automata = new Nfsa(alphabet, states, "q0", transitionTable, finalStates);
            string log;
            automata.Iterate("0", out log);
            Console.WriteLine(log);
            automata.Iterate("1", out log);
            Console.WriteLine(log);
            automata.Iterate("10000", out log);
            Console.WriteLine(log);
            automata.Iterate("100000", out log);
            Console.WriteLine(log);
            automata.Iterate("11000", out log);
            Console.WriteLine(log);
            automata.Iterate("110000", out log);
            Console.WriteLine(log);
            automata.Iterate("00001", out log);
            Console.WriteLine(log);
            automata.Iterate("000001", out log);
            Console.WriteLine(log);
            automata.Iterate("1100000", out log);
            Console.WriteLine(log);
            automata.Iterate("11000000", out log);
            Console.WriteLine(log);
            automata.Iterate("110000001", out log);
 
            Console.WriteLine(log);
            
            #endregion   
        }
    }
}