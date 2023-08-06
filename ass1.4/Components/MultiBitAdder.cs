using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements an adder, receving as input two n bit numbers, and outputing the sum of the two numbers
    class MultiBitAdder : Gate
    {
        //Word size - number of bits in each input
        public int Size { get; private set; }

        public WireSet Input1 { get; private set; }
        public WireSet Input2 { get; private set; }
        public WireSet Output { get; private set; }
        //An overflow bit for the summation computation
        public Wire Overflow { get; private set; }


        //decl
        FullAdder[] fa;

        public MultiBitAdder(int iSize)
        {
            Size = iSize;
            Input1 = new WireSet(Size);
            Input2 = new WireSet(Size);
            Output = new WireSet(Size);
            //your code here
            Overflow = new Wire();

            fa = new FullAdder[Size];
            for (int j = 0; j < fa.Length; j++)
                fa[j] = new FullAdder();

            Wire zero = new Wire();
            zero.Value = 0;

            fa[0].CarryInput.ConnectInput(zero);
            fa[0].ConnectInput1(Input1[0]);
            fa[0].ConnectInput2(Input2[0]);
            Output[0].ConnectInput(fa[0].Output);

            for (int i = 1; i < Size; i++)
            {
                fa[i].ConnectInput1(Input1[i]);
                fa[i].ConnectInput2(Input2[i]);
                fa[i].CarryInput.ConnectInput(fa[i - 1].CarryOutput);
                Output[i].ConnectInput(fa[i].Output);
            }

            Overflow.ConnectInput(fa[Size - 1].CarryOutput);

        }

        public override string ToString()
        {
            return Input1 + "(" + Input1.Get2sComplement() + ")" + " + " + Input2 + "(" + Input2.Get2sComplement() + ")" + " = " + Output + "(" + Output.Get2sComplement() + ")";
        }

        public void ConnectInput1(WireSet wInput)
        {
            Input1.ConnectInput(wInput);
        }
        public void ConnectInput2(WireSet wInput)
        {
            Input2.ConnectInput(wInput);
        }


        public override bool TestGate()
        {
            for(int i = 0; i < 50; i++)
            {
                Random rnd = new Random();

                int val1 = rnd.Next(0, (int)Math.Pow(2, Size) - 1);
                int val2 = rnd.Next(0, (int)Math.Pow(2, Size) - 1);

                Input1.SetValue(val1);
                Input2.SetValue(val2);

                if (Output.GetValue() != (val1 + val2) % (int)Math.Pow(2, Size))
                    return false;
                if ((val1 + val2) >= (int)Math.Pow(2, Size) && Overflow.Value != 1)
                    return false;
            }


            return true;
        }
    }
}
