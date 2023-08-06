using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Counter : Gate
    {
        private int m_iValue;
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public Wire Set { get; private set; }
        public int Size { get; private set; }

        //The counter contains a register, and supports two possible operations:
        //1. Set = 0: Incrementing the current register value by 1 (++)
        //2. Set = 1: Setting the register to a new value

        MultiBitRegister mbr;
        MultiBitAdder mba;
        BitwiseMux mux;
        WireSet ONE;

        public Counter(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Set = new Wire();

            //your code here

            mbr = new MultiBitRegister(iSize);
            mba = new MultiBitAdder(iSize);
            mux = new BitwiseMux(iSize);
            ONE = new WireSet(iSize);
            ONE.Set2sComplement(1);

            mux.ConnectInput1(mba.Output);
            mux.ConnectInput2(Input);
            mux.ConnectControl(Set);

            mbr.ConnectInput(mux.Output);
            mbr.Load.Value = 1;

            mba.ConnectInput1(mbr.Output);
            mba.ConnectInput2(ONE);

            Output = new WireSet(iSize);
            Output.ConnectInput(mbr.Output);

            
        
        }

        public void ConnectInput(WireSet ws)
        {
            Input.ConnectInput(ws);
        }
        
        public void ConnectReset(Wire w)
        {
            Set.ConnectInput(w);
        }

        public override string ToString()
        {
            return Output.ToString();
        }

        

        public override bool TestGate()
        {
            Input.Set2sComplement(0);
            Set.Value = 1;
            Clock.ClockUp();
            Clock.ClockDown();
            Set.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();

            Set.Value = 0;

            if (Output.Get2sComplement() != 0)
                return false;
            
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 1)
                return false;

            Clock.ClockUp();
            Clock.ClockDown();

            if (Output.Get2sComplement() != 2)
                return false;

            Clock.ClockUp();
            Clock.ClockDown();

            if (Output.Get2sComplement() != 3)
                return false;

            Clock.ClockUp();
            Clock.ClockDown();

            if (Output.Get2sComplement() != 4)
                return false;

            Clock.ClockUp();
            Clock.ClockDown();

            if (Output.Get2sComplement() != 5)
                return false;



            return true;


        }
    }
}
