using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembler a = new Assembler();
            //to run tests, call the "TranslateAssemblyFile" function like this:
            //string sourceFileLocation = the path to your source file
            //string destFileLocation = the path to your dest file
            //a.TranslateAssemblyFile(sourceFileLocation, destFileLocation);
            //a.TranslateAssemblyFile(@"Add.asm", @"Add.hack");
            //You need to be able to run two translations one after the other
            //a.TranslateAssemblyFile(@"Max.asm", @"Max.hack");

            string sourceFileLocation = "C:/Users/hagai/OneDrive/מסמכים/semester e/מבנה מערכות מחשוב/ass 2.3/Code/Assembly examples/Max.asm";
            string destFileLocation = "C:/Users/hagai/OneDrive/מסמכים/semester e/מבנה מערכות מחשוב/ass 2.3/Code/Assembly examples/test.txt";
            a.TranslateAssemblyFile(sourceFileLocation, destFileLocation);



            /*
            Assembler b = new Assembler();
            List<string> lines = new List<string>();
            //lines.Add("0;jmp");
            //lines.Add("M=D+1;JGT");
            //lines.Add("M=D+1");
            //lines.Add("A=D+1;JGT:END");
            //lines.Add("x++");
            //lines.Add("label++");
            //lines.Add("D++");
            lines.Add("i=3");
            lines.Add("x=3");
            lines.Add("D=i");
            lines.Add("@x");
            lines.Add("D=D-M");
            lines.Add("D;JEQ:END");
            lines.Add("x++");
            lines.Add("(END)");



            List<string> afterMacros = b.ExpendMacros(lines);

            b.CreateSymbolTable(afterMacros);

            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }

            foreach (string line in afterMacros)
            {
                Console.WriteLine(line);
            }

            List<string> machinecode = b.TranslateAssemblyToMachineCode(afterMacros);

            foreach (string line in machinecode)
            {
                Console.WriteLine(line);
            }







            //TODO: remove this just for testing
            Console.ReadLine();
            */
        }
    }
}
