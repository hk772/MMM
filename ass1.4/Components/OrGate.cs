using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the or operation. To implement it, follow the example in the And gate.
    class OrGate : TwoInputGate
    {
        //your code here 
        // x + y = !(!x * !y)
        
        NotGate notX;
        NotGate notY;
        NAndGate nAnd;

        public OrGate()
        {
            //your code here 
            notX = new NotGate();
            notY = new NotGate();
            nAnd = new NAndGate();

            notX.ConnectInput(Input1);
            notY.ConnectInput(Input2);

            nAnd.ConnectInput1(notX.Output);
            nAnd.ConnectInput2(notY.Output);

            this.Output.ConnectInput(nAnd.Output);
        }


        public override string ToString()
        {
            return "Or " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }

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
            if (Output.Value != 1)
                return false;
            return true;
        }
    }

}
