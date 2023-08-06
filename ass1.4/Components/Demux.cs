using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A demux has 2 outputs. There is a single input and a control bit, selecting whether the input should be directed to the first or second output.
    class Demux : Gate
    {
        public Wire Output1 { get; private set; }
        public Wire Output2 { get; private set; }
        public Wire Input { get; private set; }
        public Wire Control { get; private set; }

        //your code here
        // x * !c = z1      x * c = z2
        AndGate and1;
        AndGate and2;
        NotGate not;

        public Demux()
        {
            Input = new Wire();
            Output1 = new Wire();
            Output2 = new Wire();
            Control = new Wire();
            //your code here
            and1 = new AndGate();
            and2 = new AndGate();
            not = new NotGate();

            not.ConnectInput(Control);
            and1.ConnectInput1(Input);
            and1.ConnectInput2(not.Output);
            Output1.ConnectInput(and1.Output);

            and2.ConnectInput1(Input);
            and2.ConnectInput2(Control);
            Output2.ConnectInput(and2.Output);
        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }



        public override bool TestGate()
        {
            for(int x = 0; x < 2; x++)
            {
                for(int c = 0; c < 2; c++)
                {
                    Input.Value = x;
                    Control.Value = c;

                    if (c == 0 && !(Output1.Value == x && Output2.Value == 0))
                        return false;
                    else if(c == 1 && !(Output1.Value == 0 && Output2.Value == x))
                        return false;
                }
            }
            return true;
        }
    }
}
