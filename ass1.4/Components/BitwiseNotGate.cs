using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This bitwise gate takes as input one WireSet containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseNotGate : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public int Size { get; private set; }

        //your code here
        NotGate[] not;

        public BitwiseNotGate(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
             //your code here

            not = new NotGate[iSize];

            for (int j = 0; j < iSize; j++)
            {
                not[j] = new NotGate();

                not[j].ConnectInput(Input[j]);
                Output[j].ConnectInput(not[j].Output);
            }

        }

        public void ConnectInput(WireSet ws)
        {
            Input.ConnectInput(ws);
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(not)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Not " + Input + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int i = 0; i < 10; i++)
            {
                //get random number in range [0, 2^inputcount]
                Random rand = new Random();
                int test = rand.Next(0, (int)Math.Pow(2, Input.Size) - 1);

                Input.SetValue(test);

                //Console.WriteLine(ToString());

                for (int j = 0; j < Input.Size; j++)
                {
                    if (Input[j].Value == 1 && Output[j].Value != 0)
                        return false;
                    if (Input[j].Value == 0 && Output[j].Value != 1)
                        return false;
                }

            }

            return true;
        }
    }
}
