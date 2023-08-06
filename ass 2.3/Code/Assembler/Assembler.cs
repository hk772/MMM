using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    public class Assembler
    {
        private const int WORD_SIZE = 16;

        private Dictionary<string, int[]> m_dControl, m_dJmp, m_dDest; //these dictionaries map command mnemonics to machine code - they are initialized at the bottom of the class
        private Dictionary<string, int> symbols;
        //more data structures here (symbol map, ...)

        public Assembler()
        {
            InitCommandDictionaries();
        }

        //this method is called from the outside to run the assembler translation
        public void TranslateAssemblyFile(string sInputAssemblyFile, string sOutputMachineCodeFile)
        {
            //read the raw input, including comments, errors, ...
            StreamReader sr = new StreamReader(sInputAssemblyFile);
            List<string> lLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lLines.Add(sr.ReadLine());
            }
            sr.Close();
            //translate to machine code
            List<string> lTranslated = TranslateAssemblyFile(lLines);
            //write the output to the machine code file
            StreamWriter sw = new StreamWriter(sOutputMachineCodeFile);
            foreach (string sLine in lTranslated)
                sw.WriteLine(sLine);
            sw.Close();
        }

        //translate assembly into machine code
        private List<string> TranslateAssemblyFile(List<string> lLines)
        {
            //implementation order:
            //first, implement "TranslateAssemblyToMachineCode", and check if the examples "Add", "MaxL" are translated correctly.
            //next, implement "CreateSymbolTable", and modify the method "TranslateAssemblyToMachineCode" so it will support symbols (translating symbols to numbers). check this on the examples that don't contain macros
            //the last thing you need to do, is to implement "ExpendMacro", and test it on the example: "SquareMacro.asm".
            //init data structures here 

            //expand the macros
            List<string> lAfterMacroExpansion = ExpendMacros(lLines);

            //first pass - create symbol table and remove lable lines
            CreateSymbolTable(lAfterMacroExpansion);

            //second pass - replace symbols with numbers, and translate to machine code
            List<string> lAfterTranslation = TranslateAssemblyToMachineCode(lAfterMacroExpansion);
            return lAfterTranslation;
        }

        
        //first pass - replace all macros with real assembly
        public List<string> ExpendMacros(List<string> lLines)
        {
            //You do not need to change this function, you only need to implement the "ExapndMacro" method (that gets a single line == string)
            List<string> lAfterExpansion = new List<string>();
            for (int i = 0; i < lLines.Count; i++)
            {
                //remove all redudant characters
                string sLine = CleanWhiteSpacesAndComments(lLines[i]);
                if (sLine == "")
                    continue;
                //if the line contains a macro, expand it, otherwise the line remains the same
                List<string> lExpanded = ExapndMacro(sLine);
                //we may get multiple lines from a macro expansion
                foreach (string sExpanded in lExpanded)
                {
                    lAfterExpansion.Add(sExpanded);
                }
            }
            return lAfterExpansion;
        }

        //expand a single macro line
        public List<string> ExapndMacro(string sLine)
        {
            List<string> lExpanded = new List<string>();

            if (IsCCommand(sLine))
            {
                string sDest, sCompute, sJmp;
                GetCommandParts(sLine, out sDest, out sCompute, out sJmp);
                //your code here - check for indirect addessing and for jmp shortcuts
                //read the word file to see all the macros you need to support

                /*Console.WriteLine($"its c command: {sLine}");
                Console.WriteLine($"sDest: {sDest}");
                Console.WriteLine($"sCompute: {sCompute}");
                Console.WriteLine($"sJmp: {sJmp}");
                */
                if (sLine.Substring(0,2).Equals("//"))
                {
                    return lExpanded;
                }   //comment

                else if (sCompute.Contains("++"))
                {
                    string label = sCompute.Substring(0, sCompute.Length - 2);
                    if (label.Equals("D") || label.Equals("A") || label.Equals("M"))
                    {
                        lExpanded.Add(label + "=" + label + "+1");
                    }
                    else
                    {
                        lExpanded.Add("@" + label);
                        //Console.WriteLine($"label: {label}");
                        lExpanded.Add("M=M+1");
                    }

                    
                }

                else if (sJmp.Contains(":"))
                {
                    string jmpLabel = sJmp.Substring(4);
                    lExpanded.Add("@" + jmpLabel);
                    lExpanded.Add(sCompute + ";" + sJmp.Substring(0,3));

                    
                }
                //it must be <something>=<something>
                else
                {
                    //compute can be D/M/A = <label>/<numer>
                    //               <label> = D/M/A/<number>
                    //               <label> = <lable>/<number>
                    if (sDest.Equals("D") && (char)sCompute[0] >= '0' && (char)sCompute[0] <= '9')
                    {   //D=<number>
                        int num = 0;
                        try { num = Int32.Parse(sCompute); }
                        catch (Exception) { }
                        lExpanded.Add("@" + num);
                        lExpanded.Add("D=A");
                    }
                    else if(sDest.Equals("D") && !sCompute.Equals("A") && !sCompute.Equals("M") && !m_dControl.ContainsKey(sCompute))
                    {   //D=<lable>
                        lExpanded.Add("@" + sCompute);
                        lExpanded.Add("D=M");
                    }
                    else if (!sDest.Equals("D") && !sDest.Equals("A") && !sDest.Equals("M") && !sDest.Equals(""))
                    {   //dest is a lable
                        if ((char)sCompute[0] >= '0' && (char)sCompute[0] <= '9')
                        {   //<lable> = <number>
                            int num = 0;
                            try { num = Int32.Parse(sCompute); }
                            catch (Exception) { }
                            lExpanded.Add("@" +  num);
                            lExpanded.Add("D=A");
                            lExpanded.Add("@" + sDest);
                            lExpanded.Add("M=D");
                        }
                        else if (sCompute.Equals("D"))
                        {   //<lable>=D
                            lExpanded.Add("@" + sDest);
                            lExpanded.Add("M=D");
                        }
                        else
                        {   //<lable> = <lable>
                            lExpanded.Add("@" + sCompute);
                            lExpanded.Add("D=M");
                            lExpanded.Add("@" + sDest);
                            lExpanded.Add("M=D");
                        }
                    }

                    
                }
            }

            if (lExpanded.Count == 0)
                lExpanded.Add(sLine);
            return lExpanded;
        }

        //second pass - record all symbols - labels and variables
        public void CreateSymbolTable(List<string> lLines)
        {
            symbols = new Dictionary<string, int>();

            string sLine = "";
            int lineNum = -1;
            for (int i = 0; i < lLines.Count; i++)
            {
                lineNum++;
                sLine = lLines[i];
                if (IsLabelLine(sLine))
                {
                    lineNum--;
                    //record label in symbol table
                    //do not add the label line to the result
                    //if (!symbols.Keys.Contains(sLine.Substring(1, sLine.Length-2)))
                    symbols[sLine.Substring(1, sLine.Length - 2)] = lineNum + 1;
                }
                else if (IsACommand(sLine))
                {
                    //may contain a variable - if so, record it to the symbol table (if it doesn't exist there yet...)
                    if (!((char)sLine[1] >= '0' && (char)sLine[1] <= '9'))
                    {
                        if (!symbols.Keys.Contains(sLine.Substring(1)))
                            symbols.Add(sLine.Substring(1), -1);
                    }
                }
                else if (IsCCommand(sLine))
                {
                    //do nothing here
                }
                else
                    throw new FormatException("Cannot parse line " + i + ": " + lLines[i]);
            }

            int val = 16;
            List<string> sKyes = symbols.Keys.ToList();
            foreach(string key in sKyes)
            {
                if (symbols[key] == -1)
                {
                    symbols[key] = val;
                    val++;
                }
                    
            }
          
        }


        
        //third pass - translate lines into machine code, replacing symbols with numbers
        public List<string> TranslateAssemblyToMachineCode(List<string> lLines)
        {
            InitCommandDictionaries();

            string sLine = "";
            List<string> lAfterPass = new List<string>();
            for (int i = 0; i < lLines.Count; i++)
            {
                sLine = lLines[i];
                if (IsACommand(sLine))
                {
                    //translate an A command into a sequence of bits
                    int num;
                    string binaty_num = "";
                    if (!((char)sLine[1] >= '0' && (char)sLine[1] <= '9'))
                    {
                        string label = sLine.Substring(1);
                        if (!symbols.ContainsKey(label))
                            throw new Exception($"invalid label: {label}");
                        else if (symbols[label] == -1)
                            throw new Exception($"uknown lable: {label}");

                        try
                        {
                            num = symbols[label];
                            binaty_num = ToBinary(num);
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        try
                        {
                            int result = Int32.Parse(sLine.Substring(1));
                            binaty_num = ToBinary(result);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("invalid number in a command");
                        }
                    }

                    lAfterPass.Add(binaty_num);
                }
                else if (IsCCommand(sLine))
                {
                    string sDest, sControl, sJmp;
                    GetCommandParts(sLine, out sDest, out sControl, out sJmp);
                    //translate an C command into a sequence of bits
                    //take a look at the dictionaries m_dControl, m_dJmp, and where they are initialized (InitCommandDictionaries), to understand how to you them here

                    int[] destbits = { };
                    int[] controlbits = { };
                    int[] jmpbits = { };

                    try { 
                        destbits = m_dDest[sDest]; }
                    catch(Exception e) {  }

                    try { controlbits = m_dControl[sControl]; }
                    catch (Exception e) {  }

                    try { jmpbits = m_dJmp[sJmp]; }
                    catch (Exception e) {  }

                    string cCommand = "1" + "000" + ToString(controlbits) +
                        ToString(destbits) + ToString(jmpbits);

                    //Console.WriteLine(cCommand);

                    lAfterPass.Add(cCommand);

                }
                else if (IsLabelLine(sLine))
                {
                    //donothing
                }
                else
                    throw new FormatException("Cannot parse line " + i + ": " + lLines[i]);
            }
            return lAfterPass;
        }

        //helper functions for translating numbers or bits into strings
        private string ToString(int[] aBits)
        {
            string sBinary = "";
            for (int i = aBits.Length-1; i >= 0; i--)
                sBinary += aBits[i];
            return sBinary;
        }

        private string ToBinary(int x)
        {
            string sBinary = "";
            for (int i = 0; i < WORD_SIZE; i++)
            {
                sBinary = (x % 2) + sBinary;
                x = x / 2;
            }
            return sBinary;
        }


        //helper function for splitting the various fields of a C command
        private void GetCommandParts(string sLine, out string sDest, out string sControl, out string sJmp)
        {
            if (sLine.Contains('='))
            {
                int idx = sLine.IndexOf('=');
                sDest = sLine.Substring(0, idx);
                sLine = sLine.Substring(idx + 1);
            }
            else
                sDest = "";
            if (sLine.Contains(';'))
            {
                int idx = sLine.IndexOf(';');
                sControl = sLine.Substring(0, idx);
                sJmp = sLine.Substring(idx + 1);

            }
            else
            {
                sControl = sLine;
                sJmp = "";
            }
        }

        private bool IsCCommand(string sLine)
        {
            return !IsLabelLine(sLine) && sLine[0] != '@';
        }

        private bool IsACommand(string sLine)
        {
            return sLine[0] == '@';
        }

        private bool IsLabelLine(string sLine)
        {
            if (sLine.StartsWith("(") && sLine.EndsWith(")"))
                return true;
            return false;
        }

        private string CleanWhiteSpacesAndComments(string sDirty)
        {
            string sClean = "";
            for (int i = 0 ; i < sDirty.Length ; i++)
            {
                char c = sDirty[i];
                if (c == '/' && i < sDirty.Length - 1 && sDirty[i + 1] == '/') // this is a comment
                    return sClean;
                if (c > ' ' && c <= '~')//ignore white spaces
                    sClean += c;
            }
            return sClean;
        }

        public int[] GetArray(params int[] l)
        {
            int[] a = new int[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[l.Length - i - 1] = l[i];
            return a;
        }
        private void InitCommandDictionaries()
        {
            m_dControl = new Dictionary<string, int[]>();


            m_dControl["0"] = GetArray(0, 0, 0, 0, 0, 0);
            m_dControl["1"] = GetArray(0, 0, 0, 0, 0, 1);
            m_dControl["D"] = GetArray(0, 0, 0, 0, 1, 0);
            m_dControl["A"] = GetArray(0, 0, 0, 0, 1, 1);
            m_dControl["!D"] = GetArray(0, 0, 0, 1, 0, 0);
            m_dControl["!A"] = GetArray(0, 0, 0, 1, 0, 1);
            m_dControl["-D"] = GetArray(0, 0, 0, 1, 1, 0);
            m_dControl["-A"] = GetArray(0, 0, 0, 1, 1, 1);
            m_dControl["D+1"] = GetArray(0, 0, 1, 0, 0, 0);
            m_dControl["A+1"] = GetArray(0, 0, 1, 0, 0, 1);
            m_dControl["D-1"] = GetArray(0, 0, 1, 0, 1, 0);
            m_dControl["A-1"] = GetArray(0, 0, 1, 0, 1, 1);
            m_dControl["A+D"] = GetArray(0, 0, 1, 1, 0, 0);
            m_dControl["D+A"] = GetArray(0, 0, 1, 1, 0, 0);
            m_dControl["D-A"] = GetArray(0, 0, 1, 1, 0, 1);
            m_dControl["A-D"] = GetArray(0, 0, 1, 1, 1, 0);
            m_dControl["A^D"] = GetArray(0, 0, 1, 1, 1, 1);
            m_dControl["A&D"] = GetArray(0, 1, 0, 0, 0, 0);
            m_dControl["AvD"] = GetArray(0, 1, 0, 0, 0, 1);
            m_dControl["A|D"] = GetArray(0, 1, 0, 0, 1, 0);

            m_dControl["M"] = GetArray(1, 0, 0, 0, 1, 1);
            m_dControl["!M"] = GetArray(1, 0, 0, 1, 0, 1);
            m_dControl["-M"] = GetArray(1, 0, 0, 1, 1, 1);
            m_dControl["M+1"] = GetArray(1, 0, 1, 0, 0, 1);
            m_dControl["M-1"] = GetArray(1, 0, 1, 0, 1, 1);
            m_dControl["M+D"] = GetArray(1, 0, 1, 1, 0, 0);
            m_dControl["D+M"] = GetArray(1, 0, 1, 1, 0, 0);
            m_dControl["D-M"] = GetArray(1, 0, 1, 1, 0, 1);
            m_dControl["M-D"] = GetArray(1, 0, 1, 1, 1, 0);
            m_dControl["M^D"] = GetArray(1, 0, 1, 1, 1, 1);
            m_dControl["M&D"] = GetArray(1, 1, 0, 0, 0, 0);
            m_dControl["MvD"] = GetArray(1, 1, 0, 0, 0, 1);
            m_dControl["M|D"] = GetArray(1, 1, 0, 0, 1, 0);



            m_dDest = new Dictionary<string, int[]>();
            m_dDest[""] = GetArray(0, 0, 0);
            m_dDest["M"] = GetArray(0, 0, 1);
            m_dDest["D"] = GetArray(0, 1, 0);
            m_dDest["A"] = GetArray(1, 0, 0);
            m_dDest["DM"] = GetArray(0, 1, 1);
            m_dDest["AM"] = GetArray(1, 0, 1);
            m_dDest["AD"] = GetArray(1, 1, 0);
            m_dDest["ADM"] = GetArray(1, 1, 1);


            m_dJmp = new Dictionary<string, int[]>();

            m_dJmp[""] = GetArray(0, 0, 0);
            m_dJmp["JGT"] = GetArray(0, 0, 1);
            m_dJmp["JEQ"] = GetArray(0, 1, 0);
            m_dJmp["JGE"] = GetArray(0, 1, 1);
            m_dJmp["JLT"] = GetArray(1, 0, 0);
            m_dJmp["JNE"] = GetArray(1, 0, 1);
            m_dJmp["JLE"] = GetArray(1, 1, 0);
            m_dJmp["JMP"] = GetArray(1, 1, 1);
        }
    }
}
