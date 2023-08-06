using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a memory unit, containing k registers, each of size n bits.
    class Memory : SequentialGate
    {
        //The address size determines the number of registers
        public int AddressSize { get; private set; }
        //The word size determines the number of bits in each register
        public int WordSize { get; private set; }

        //Data input and output - a number with n bits
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        //The address of the active register
        public WireSet Address { get; private set; }
        //A bit setting the memory operation to read or write
        public Wire Load { get; private set; }

        //your code here
        MultiBitRegister[] nbr;
        BitwiseMultiwayMux bMux;
        BitwiseMultiwayDemux dmux;
        WireSet Load2;

        public Memory(int iAddressSize, int iWordSize)
        {
            AddressSize = iAddressSize;
            WordSize = iWordSize;

            Input = new WireSet(WordSize);
            Output = new WireSet(WordSize);
            Address = new WireSet(AddressSize);
            Load = new Wire();

            //your code here
            nbr = new MultiBitRegister[(int)Math.Pow(2, AddressSize)];
            bMux = new BitwiseMultiwayMux(iWordSize, iAddressSize);
            dmux = new BitwiseMultiwayDemux(1, iAddressSize);

            for (int i = 0; i < nbr.Length; i++)
            {
                nbr[i] = new MultiBitRegister(WordSize);
                nbr[i].ConnectInput(Input);
                nbr[i].Load.ConnectInput(dmux.Outputs[i][0]);
                bMux.Inputs[i].ConnectInput(nbr[i].Output);
            }

            dmux.ConnectControl(Address);
            Load2 = new WireSet(1);
            Load2[0].ConnectInput(Load);
            dmux.ConnectInput(Load2);

            bMux.ConnectControl(Address);

            Output.ConnectInput(bMux.Output);


        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectAddress(WireSet wsAddress)
        {
            Address.ConnectInput(wsAddress);
        }


        public override void OnClockUp()
        {
        }

        public override void OnClockDown()
        {
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override bool TestGate()
        {
            Input.Set2sComplement(1);
            Load.Value = 1;
            Address.SetValue(0);
            Clock.ClockUp();
            Clock.ClockDown();

            Input.Set2sComplement(1);
            Load.Value = 0;
            Address.SetValue(0);
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 1)
                return false;

            Input.Set2sComplement(0);
            Load.Value = 0;
            Address.SetValue(0);
            Clock.ClockUp();
            Clock.ClockDown();
            if (Output.Get2sComplement() != 1)
                return false;

            return true;


        }
    }
}
