using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
    class Compiler
    {


        public Compiler()
        {
        }

        //reads a file into a list of strings, each string represents one line of code
        public List<string> ReadFile(string sFileName)
        {
            StreamReader sr = new StreamReader(sFileName);
            List<string> lCodeLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lCodeLines.Add(sr.ReadLine());
            }
            sr.Close();
            return lCodeLines;
        }



        //Computes the next token in the string s, from the begining of s until a delimiter has been reached. 
        //Returns the string without the token.
        private string Next(string s, char[] aDelimiters, out string sToken, out int cChars)
        {
            cChars = 1;
            sToken = s[0] + "";
            if (aDelimiters.Contains(s[0]))
                return s.Substring(1);
            int i = 0;
            for (i = 1; i < s.Length; i++)
            {
                if (aDelimiters.Contains(s[i]))
                    return s.Substring(i);
                else
                    sToken += s[i];
                cChars++;
            }
            return null;
        }

        //Splits a string into a list of tokens, separated by delimiters
        private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (aDelimiters.Contains(s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }

        //This is the main method for the Tokenizing assignment. 
        //Takes a list of code lines, and returns a list of tokens.
        //For each token you must identify its type, and instantiate the correct subclass accordingly.
        //You need to identify the token position in the file (line, index within the line).
        //You also need to identify errors, in this assignement - illegal identifier names.
        public List<Token> Tokenize(List<string> lCodeLines)
        {
            List<Token> lTokens = new List<Token>();
            //your code here
            int lineCount = 0;

            foreach(string line in lCodeLines)
            {
                int position = 0;
                string tmp = line;
                while (tmp.StartsWith("\t"))
                {
                    position += 4;
                    tmp = tmp.Substring(1);
                }

                if (tmp.StartsWith("//"))
                {
                    lineCount++;
                    //handle cpmment??
                }
                else
                {
                    char[] space = { ' ' };
                    char[] splitby = Token.Separators.Concat(Token.Parentheses).ToArray().Concat(space).ToArray();

                    List<string> afterSplit = Split(tmp, splitby);
                    string[] nospaces = tmp.Split(" ");

                    foreach(string s in afterSplit)
                    {
                        if (s.Length > 0)
                        {
                            if (Token.Statements.Contains(s))
                            {
                                lTokens.Add(new Statement(s, lineCount, position));
                            }
                            else if (Token.Constants.Contains(s))
                            {
                                lTokens.Add(new Constant(s, lineCount, position));
                            }
                            else if (s.Length == 1 && Token.Parentheses.Contains(s[0]))
                            {
                                lTokens.Add(new Parentheses(s[0], lineCount, position));
                            }
                            else if (Token.VarTypes.Contains(s))
                            {
                                lTokens.Add(new VarType(s, lineCount, position));
                            }
                            else if (s.Length == 1 && Token.Operators.Contains(s[0]))
                            {
                                lTokens.Add(new Operator(s[0], lineCount, position));
                            }
                            else if (s.Length == 1 && Token.Separators.Contains(s[0]))
                            {
                                lTokens.Add(new Separator(s[0], lineCount, position));
                            }
                            else if (s.Length > 0 && (s[0] >= 'a' && s[0] <= 'z') || (s[0] >= 'A' && s[0] <= 'Z'))
                            {
                                //hence its a identifier
                                lTokens.Add(new Identifier(s, lineCount, position));
                            }
                            else if (s.Equals(" "))
                            {

                            }
                            else if (isNumber(s))
                            {
                                lTokens.Add(new Number(s, lineCount, position));
                            }
                            else
                            {
                                //error
                                Token T = new Identifier(s, lineCount, position);
                                throw new SyntaxErrorException("invalid token", T);
                            }
                            position += s.Length;
                        }


                    }
                }

            }
            
            return lTokens;
        }

        
        bool isNumber(string s)
        {
            try
            {
                int i = Convert.ToInt32(s);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

