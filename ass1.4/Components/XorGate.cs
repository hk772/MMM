﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the xor operation. To implement it, follow the example in the And gate.
    class XorGate : TwoInputGate
    {
        //your code here
        //(x+Y)*(x nand y)

        OrGate or;
        AndGate and;
        NAndGate nAnd;

        public XorGate()
        {
            //your code here
            or = new OrGate();
            and = new AndGate();
            nAnd = new NAndGate();

            or.ConnectInput1(Input1);
            or.ConnectInput2(Input2);

            nAnd.ConnectInput1(Input1);
            nAnd.ConnectInput2(Input2);

            and.ConnectInput1(or.Output);
            and.ConnectInput2(nAnd.Output);

            this.Output.ConnectInput(and.Output);
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(xor)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Xor " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }


        //this method is used to test the gate. 
        //we simply check whether the truth table is properly implemented.
        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0)
                return false;
            return true;
        }
    }
}
