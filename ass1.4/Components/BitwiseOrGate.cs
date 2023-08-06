using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseOrGate : BitwiseTwoInputGate
    {
        //your code here
        OrGate[] or;
        public BitwiseOrGate(int iSize)
            : base(iSize)
        {
            //your code here
            or = new OrGate[iSize];

            for(int i = 0; i < iSize; i++)
            {
                or[i] = new OrGate();

                or[i].ConnectInput1(Input1[i]);
                or[i].ConnectInput2(Input2[i]);

                Output[i].ConnectInput(or[i].Output);
            }
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(or)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Or " + Input1 + ", " + Input2 + " -> " + Output;
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

                //Console.WriteLine(ToString());

                for (int j = 0; j < Input1.Size; j++)
                {
                    if (Output[j].Value == 1 && (Input1[j].Value == 0 && Input2[j].Value == 0))
                        return false;
                    if (Output[j].Value == 0 && (Input1[j].Value == 1 || Input2[j].Value == 1))
                        return false;
                }

            }

            return true;
        }
    }
}
