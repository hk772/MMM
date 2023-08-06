using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A mux has 2 inputs. There is a single output and a control bit, selecting which of the 2 inpust should be directed to the output.
    class MuxGate : TwoInputGate
    {
        public Wire ControlInput { get; private set; }
        //your code here
        //((x*x) * !c) + ((y*y) * c)

        AndGate xAnd;
        AndGate yAnd;
        AndGate ycAnd;
        AndGate xcAnd;
        NotGate cNot;
        OrGate or;

        public MuxGate()
        {
            ControlInput = new Wire();

            //your code here
            xAnd = new AndGate();
            yAnd = new AndGate();
            ycAnd = new AndGate();
            xcAnd = new AndGate();
            cNot = new NotGate();
            or = new OrGate();

            or.ConnectInput1(xcAnd.Output);
            or.ConnectInput2(ycAnd.Output);

            xcAnd.ConnectInput1(xAnd.Output);
            xcAnd.ConnectInput2(cNot.Output);

            cNot.ConnectInput(ControlInput);

            xAnd.ConnectInput1(Input1);
            xAnd.ConnectInput2(Input1);

            ycAnd.ConnectInput1(yAnd.Output);
            ycAnd.ConnectInput2(ControlInput);

            yAnd.ConnectInput1(Input2);
            yAnd.ConnectInput2(Input2);

            this.Output.ConnectInput(or.Output);
        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }


        public override string ToString()
        {
            return "Mux " + Input1.Value + "," + Input2.Value + ",C" + ControlInput.Value + " -> " + Output.Value;
        }



        public override bool TestGate()
        {
            for(int x = 0; x < 2; x++)
            {
                for(int y = 0; y < 2; y++)
                {
                    for (int c = 0; c < 2; c++)
                    {
                        Input1.Value = x;
                        Input2.Value = y;
                        ControlInput.Value = c;

                        if (c == 0 && Output.Value != x)
                            return false;
                        else if (c == 1 && Output.Value != y)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
