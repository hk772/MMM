using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a FullAdder, taking as input 3 bits - 2 numbers and a carry, and computing the result in the output, and the carry out.
    class FullAdder : TwoInputGate
    {
        public Wire CarryInput { get; private set; }
        public Wire CarryOutput { get; private set; }

        //your code here

        HalfAdder ha1;
        HalfAdder ha2;
        OrGate or;

        public FullAdder()
        {
            CarryInput = new Wire();
            //your code here
            CarryOutput = new Wire();

            ha1 = new HalfAdder();
            ha2 = new HalfAdder();
            or = new OrGate();

            ha1.ConnectInput1(Input1);
            ha1.ConnectInput2(Input2);

            ha2.ConnectInput1(ha1.Output);
            ha2.ConnectInput2(CarryInput);

            or.ConnectInput1(ha1.CarryOutput);
            or.ConnectInput2(ha2.CarryOutput);

            Output.ConnectInput(ha2.Output);
            CarryOutput.ConnectInput(or.Output);


        }


        public override string ToString()
        {
            return Input1.Value + "+" + Input2.Value + " (C" + CarryInput.Value + ") = " + Output.Value + " (C" + CarryOutput.Value + ")";
        }

        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            CarryInput.Value = 0;
            if (Output.Value != 0 || CarryOutput.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 0;
            CarryInput.Value = 1;
            if (Output.Value != 1 || CarryOutput.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            CarryInput.Value = 0;
            if (Output.Value != 1 || CarryOutput.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            CarryInput.Value = 1;
            if (Output.Value != 0 || CarryOutput.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            CarryInput.Value = 0;
            if (Output.Value != 1 || CarryOutput.Value != 0)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            CarryInput.Value = 1;
            if (Output.Value != 0 || CarryOutput.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            CarryInput.Value = 0;
            if (Output.Value != 0 || CarryOutput.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            CarryInput.Value = 1;
            if (Output.Value != 1 || CarryOutput.Value != 1)
                return false;


            return true;
        }
    }
}
