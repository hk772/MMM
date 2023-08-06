using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseDemux : Gate
    {
        public int Size { get; private set; }
        public WireSet Output1 { get; private set; }
        public WireSet Output2 { get; private set; }
        public WireSet Input { get; private set; }
        public Wire Control { get; private set; }

        //your code here
        Demux[] demux;

        public BitwiseDemux(int iSize)
        {
            Size = iSize;
            Control = new Wire();
            Input = new WireSet(Size);
            Output1 = new WireSet(Size);
            Output2 = new WireSet(Size);

            //your code here
            demux = new Demux[iSize];

            for(int j = 0; j < iSize; j++)
            {
                demux[j] = new Demux();

                demux[j].ConnectInput(Input[j]);
                demux[j].ConnectControl(Control);

                Output1[j].ConnectInput(demux[j].Output1);
                Output2[j].ConnectInput(demux[j].Output2);
            }

        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public override string ToString()
        {
            return "Mux " + Input + ",C" + Control.Value + " -> " + Output1 + ", " + Output2;
        }

        public override bool TestGate()
        {
            for(int i = 0; i < 10; i++)
            {
                //get random number in range [0, 2^inputcount]
                Random rand = new Random();
                int test = rand.Next(0, (int)Math.Pow(2, Input.Size) - 1);

                Input.SetValue(test);

                //control = 0
                Control.Value = 0;

                //Console.WriteLine(ToString());

                for (int j = 0; j < Input.Size; j++)
                {
                    if (Output1[j].Value != Input[j].Value || Output2[j].Value != 0)
                        return false;
                }

                //control = 1
                Control.Value = 1;

                //Console.WriteLine(ToString());

                for (int j = 0; j < Input.Size; j++)
                {
                    if (Output2[j].Value != Input[j].Value || Output1[j].Value != 0)
                        return false;
                }
            }
            
            return true;
        }
    }
}
