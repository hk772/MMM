using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class represents an n bit register that can maintain an n bit number
    class MultiBitRegister : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        //A bit setting the register operation to read or write
        public Wire Load { get; private set; }

        //Word size - number of bits in the register
        public int Size { get; private set; }

        SingleBitRegister[] sbr;

        public MultiBitRegister(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Load = new Wire();
            //your code here
            
            sbr = new SingleBitRegister[Size];
            for (int i = 0; i < Size; i++)
            {
                sbr[i] = new SingleBitRegister();
                sbr[i].ConnectInput(Input[i]);
                sbr[i].ConnectLoad(Load);
                Output[i].ConnectInput(sbr[i].Output);
            }

        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        
        public override string ToString()
        {
            return Output.ToString();
        }


        public override bool TestGate()
        {
            Input.Set2sComplement(1);
            Load.Value = 1;
            Clock.ClockUp();
            Clock.ClockDown();

            Input.Set2sComplement(1);
            Load.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 1)
                return false;

            Input.Set2sComplement(0);
            Load.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 1)
                return false;

            Input.Set2sComplement(1);
            Load.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 1)
                return false;

            Input.Set2sComplement(0);
            Load.Value = 1;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 1)
                return false;

            Input.Set2sComplement(1);
            Load.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 0)
                return false;

            Input.Set2sComplement(0);
            Load.Value = 1;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 0)
                return false;

            return true;
        }
    }
}
