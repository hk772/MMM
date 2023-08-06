using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseMux : BitwiseTwoInputGate
    {
        public Wire ControlInput { get; private set; }

        //your code here
        MuxGate[] mux;

        public BitwiseMux(int iSize)
            : base(iSize)
        {
            ControlInput = new Wire();
            //your code here

            mux = new MuxGate[iSize];

            for(int j = 0; j < iSize; j++)
            {
                mux[j] = new MuxGate();

                mux[j].ConnectInput1(Input1[j]);
                mux[j].ConnectInput2(Input2[j]);
                mux[j].ConnectControl(ControlInput);

                Output[j].ConnectInput(mux[j].Output);
            }
        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }



        public override string ToString()
        {
            return "Mux " + Input1 + "," + Input2 + ",C" + ControlInput.Value + " -> " + Output;
        }




        public override bool TestGate()
        {
            for (int i = 0; i < 10; i++)
            {
                //get random number in range [0, 2^inputcount]
                Random rand = new Random();
                int test1 = rand.Next(0, (int)Math.Pow(2, Input1.Size) - 1);
                int test2 = rand.Next(0, (int)Math.Pow(2, Input1.Size) - 1);

                Input1.SetValue(test1);
                Input2.SetValue(test2);

                //control = 0
                ControlInput.Value = 0;

                //Console.WriteLine(ToString());

                for (int j = 0; j < Input1.Size; j++)
                {
                    if (Output[j].Value != Input1[j].Value)
                        return false;
                }

                //control = 1
                ControlInput.Value = 1;

                //Console.WriteLine(ToString());

                for (int j = 0; j < Input1.Size; j++)
                {
                    if (Output[j].Value != Input2[j].Value)
                        return false;
                }


            }

            return true;
        }
    }
}
