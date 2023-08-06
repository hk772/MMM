using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a register that can maintain 1 bit.
    class SingleBitRegister : Gate
    {
        public Wire Input { get; private set; }
        public Wire Output { get; private set; }
        //A bit setting the register operation to read or write
        public Wire Load { get; private set; }

        MuxGate mux;
        DFlipFlopGate dff;

        public SingleBitRegister()
        {
            
            Input = new Wire();
            Load = new Wire();
            //your code here 
            Output = new Wire();

            mux = new MuxGate();
            dff = new DFlipFlopGate();

            mux.ConnectInput2(Input);
            mux.ConnectInput1(Output);
            mux.ConnectControl(Load);

            dff.ConnectInput(mux.Output);

            Output.ConnectInput(dff.Output);



        }

        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }

      

        public void ConnectLoad(Wire wLoad)
        {
            Load.ConnectInput(wLoad);
        }


        public override bool TestGate()
        {
            Input.Value = 1;
            Load.Value = 1;
            Clock.ClockUp();
            Clock.ClockDown();
            
            Input.Value = 1;
            Load.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Value != 1)
                return false;

            Input.Value = 0;
            Load.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Value != 1)
                return false;

            Input.Value = 1;
            Load.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Value != 1)
                return false;

            Input.Value = 0;
            Load.Value = 1;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Value != 1)
                return false;

            Input.Value = 1;
            Load.Value = 0;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Value != 0)
                return false;

            Input.Value = 0;
            Load.Value = 1;
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Value != 0)
                return false;

            return true;
        }
    }
}
