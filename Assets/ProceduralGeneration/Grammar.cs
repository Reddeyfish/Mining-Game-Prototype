/* Grammar is a class meant to provide procedural generation independent of function.
 * It uses a set of symbols and terminals to generate strings that follow certain rules.
 * 
 * Input string should be formatted as:
 * 	<terminals as a single string>
 * 	<symbols as a single string, starting symbol is assuming to be the first>
 * 	<Rule1>
 * 	<Rule2>
 * 	...
 * 
 * Rules should be formatted as:
 * 	<symbol><generation1>:<generation2>:<generation3> ...
 * 
 * To avoid non-terminating generations, Grammar requires a max iteration parameter.
 */

using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Grammar {
	
	private HashSet<char> terminals;					// Terminals. Only valid as right-hand-sides
	private HashSet<char> symbols;						// Non-terminals. These are valid left-hand-sides
	private Dictionary<char, HashSet<char[]>> rules;	// Rules. righthandsides generate lefthandsides.
	private char seed;
	
	public Grammar()
	{
		symbols = new HashSet<char>();
		terminals = new HashSet<char>();
		rules = new Dictionary<char, HashSet<char[]>>();
		seed = '\0';
	}

	/* Loads a grammar from a text file resource.
	 */
	public bool LoadFromFile(string filepath)
	{
		// Get grammar file as a TextAsset
		TextAsset data = (TextAsset) Resources.Load (filepath, typeof(TextAsset));
		
		// Attempt to load the grammar from the file's contents
		if (!LoadFromString (data.text))
		{
			Debug.Log ("Grammar file " + filepath + "failed to load!");
			return false;
		}
		return true;
	}

	/* Loads a grammar from a string. Expected format is described in detail at the top of the file.
	 * WARNING: this function may partially load a grammar before failing.
	 */
	public bool LoadFromString(string grammarstring)
	{
		terminals.Clear ();
		symbols.Clear ();
		rules.Clear ();
		
		// Attempt to read the string
		StringReader reader = new StringReader(grammarstring);
		
		// Failed to read the string
		if (reader == null)
		{
			Debug.Log ("Error loading grammar: unreadable!");
			return false;
		}
		
		// extract the characters in the first line and add them as symbols
		string line = reader.ReadLine ();
		if (line == null)
		{
			Debug.Log ("Error loading grammar: missing terminals!");
			return false;
		}
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == ':')
			{
				Debug.Log ("Error loading grammar: character ':' is reserved!");
				return false;
			}
			if (terminals.Contains(line[i]))
			{
				Debug.Log ("Error loading grammar: duplicate terminals!");
				return false;
			}
			terminals.Add(line[i]);
		}
		
		// extract the characters in the second line and add them as terminals
		line = reader.ReadLine ();
		if (line == null)
		{
			Debug.Log ("Error loading grammar: missing symbols!");
			return false;
		}
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == ':')
			{
				Debug.Log ("Error loading grammar: character ':' is reserved!");
				return false;
			}
			if (symbols.Contains(line[i]))
			{
				Debug.Log ("Error loading grammar: duplicate symbols!");
				return false;
			}
			if (terminals.Contains(line[i]))
			{
				Debug.Log ("Error loading grammar: has a symbol that is also a terminal!");
				return false;
			}
			symbols.Add(line[i]);
		}
		
		// now extract the rules
		line = reader.ReadLine();
		int count = 1;
		while (line != null)
		{
			char lhs = line[0];
			if (lhs == '\0')
			{
				Debug.Log ("Error loading grammar: rule " + count + " is missing lhs!");
				return false;
			}
			if (!symbols.Contains(lhs))
			{
				if (terminals.Contains(lhs))
				{
					Debug.Log ("Error loading grammar: rule " + count + " lhs must be a non-terminal!");
				}
				else
				{
					Debug.Log ("Error loading grammar: rule " + count + " lhs is not a valid symbol!");
				}
				return false;
			}
			
			// if the symbol doesn't have any rules yet, add a new rule. Otherwise, add to the existing rule.
			HashSet<char[]> rhs = new HashSet<char[]>();
			if (!rules.ContainsKey(lhs))
			{
				rules.Add(lhs, rhs);
			}
			else
			{
				rhs = rules[lhs];
			}

			// if the seed is the null character, use the first rule's generating symbol as the seed
			if (seed == '\0')
			{
				seed = line[0];
			}

			// remove the generating symbol from the beginning of the string
			line = line.Substring(1);
			
			// parse each generation separately. generations must be split by ':'
			string[] rhsArr = line.Split(':');
			for (int i = 0; i < rhsArr.Length; i++)
			{
				// convert into character array
				char[] genCharArr = rhsArr[i].ToCharArray();

				// make sure that the generations only use valid terminals and symbols
				for (int j = 0; j < rhsArr[i].Length; j++)
				{
					if (!symbols.Contains(genCharArr[j]) && !terminals.Contains (genCharArr[j]))
					{
						Debug.Log ("Error loading grammar: generation " + rhsArr[i] + " in rule " + count + " has an element " + rhsArr[i][j] + " that is not a symbol or terminal!");
						return false;
					}
				}

				// keep each generation from the same generating symbol unique
				if (!rhs.Contains(genCharArr))
				{
					rhs.Add(genCharArr);
				}
			}
			
			line = reader.ReadLine ();
			count++;
		}
		return true;
	}

	/* Clears loaded terminals, symbols and rules.
	 */
	public void Clear()
	{
		symbols.Clear();
		terminals.Clear();
		rules.Clear();
		seed = '\0';
	}

	/* Call recursive generation function on the seed symbol. User must provide a max depth and final terminating
	 * character to ensure the function halts.
	 */
	public string GenerateWord(int maxIterations, char finalTerminal)
	{
		if (seed == '\0')
		{
			Debug.Log("Grammar has no seed!");
			return null;
		}

		// return the result of recursive generation
		return GenerateRec (seed, 1, maxIterations, finalTerminal);
	}

	/* The main recursive generating algorithm. We have ensured that the recursive generation halts by forcing the
	 * user to specify a max number of iterations and a final terminating symbol.
	 */
	private string GenerateRec(char gen, int currIteration, int maxIterations, char finalTerminal)
	{
		List<char[]> generations = new List<char[]>();

		// if we are at maximum allowed string length, remove all non-terminals from possible generations
		if (currIteration == maxIterations)
		{
			foreach(char[] generation in rules[gen])
			{
				// check if there is a non-terminal in the current generation
				bool isTerminating = true;
				for (int i = 0; i < generation.Length; i++)
				{
					if (!terminals.Contains(generation[i]))
					{
						isTerminating = false;
						break;
					}
				}
				if (isTerminating)
				{
					generations.Add(generation);
				}
			}

			// if there are no terminating generations, return the finalTerminal
			if (generations.Count == 0)
			{
				return finalTerminal.ToString();
			}
		}
		else 
		{
			generations.AddRange(rules[gen]);
		}

		// select a random generation
		int index = Random.Range(0, generations.Count);

		// parse the new generation and see if we need to resolve any non-terminals
		string ret = "";
		foreach(char e in generations[index])
		{
			if (symbols.Contains(e))
			{
				ret += GenerateRec (e, currIteration + 1, maxIterations, finalTerminal);
			}
			else
			{
				ret += e;
			}
		}

		// finally, return the resulting string of terminals
		return ret;
	}

	/* Returns a string containing information about the grammar
	 */
	public string GetInfo()
	{
		string info = "";
		info += terminals.Count + "\tterminals\n\t";
		foreach (char terminal in terminals)
		{
			info += terminal + ", ";
		}
		info = info.Remove(info.Length - 2);
		info += "\n";
		
		info += symbols.Count + "\tsymbols\n\t";
		foreach (char symbol in symbols)
		{
			info += symbol + ", ";
		}
		info = info.Remove(info.Length - 2);
		info += "\n";
		
		info += rules.Count + "\trules\n";
		foreach (char key in rules.Keys)
		{
			info += "\t" + key + " (" + rules[key].Count + "): ";
			foreach (char[] gen in rules[key])
			{
				for (int i = 0; i < gen.Length; i++)
				{
					info += gen[i];
				}
				info += ", ";
			}
			info = info.Remove(info.Length - 2);
			info += "\n";
		}
		
		return info;
	}
}
