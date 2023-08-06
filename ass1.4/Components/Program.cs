using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Components
{
    class Program
    {
        static void Main(string[] args)
        {
            //This is an example of a testing code that you should run for all the gates that you create
            /*
            //Create a gate
            AndGate and = new AndGate();
            Console.WriteLine(and + "");
            //Test that the unit testing works properly
            if (!and.TestGate())
                Console.WriteLine("bugbug");

            //orgate
            OrGate or = new OrGate();
            if(!or.TestGate())
                Console.WriteLine("bugbug");

            //xorgate
            XorGate xor = new XorGate();
            if (!xor.TestGate())
                Console.WriteLine("bugbug");

            //muxgate
            MuxGate mux = new MuxGate();
            if (!mux.TestGate())
                Console.WriteLine("bugbug");

            //demuxgate
            Demux demux = new Demux();
            if (!demux.TestGate())
                Console.WriteLine("demux bugbug");

            //multibitand
            MultiBitAndGate multiAnd = new MultiBitAndGate(4);
            if (!multiAnd.TestGate())
                Console.WriteLine("multiAnd bugbug");

            //multibitor
            MultiBitOrGate multiOr = new MultiBitOrGate(4);
            if (!multiOr.TestGate())
                Console.WriteLine("multiOr bugbug");

            //BitAnd
            BitwiseAndGate bitAnd = new BitwiseAndGate(4);
            if (!bitAnd.TestGate())
                Console.WriteLine("bitAnd bugbug");

            //BitNot
            BitwiseNotGate bitNot = new BitwiseNotGate(4);
            if (!bitNot.TestGate())
                Console.WriteLine("bitNot bugbug");

            //BitOr
            BitwiseOrGate bitOr = new BitwiseOrGate(4);
            if (!bitOr.TestGate())
                Console.WriteLine("bitOr bugbug");

            //BitMux
            BitwiseMux bitMux = new BitwiseMux(4);
            if (!bitMux.TestGate())
                Console.WriteLine("bitMux bugbug");

            //BitDeMux
            BitwiseDemux bitDeMux = new BitwiseDemux(4);
            if (!bitDeMux.TestGate())
                Console.WriteLine("bitDeMux bugbug");

            //BitMultiMux
            BitwiseMultiwayMux bitMultiMux = new BitwiseMultiwayMux(2, 4);
            if (!bitMultiMux.TestGate())
                Console.WriteLine("bitMultiMux bugbug");

            //BitMultiDemux
            BitwiseMultiwayDemux bitMultiDemux = new BitwiseMultiwayDemux(4, 2);
            if (!bitMultiDemux.TestGate())
                Console.WriteLine("bitMultiDemux bugbug");

            //halfAdder
            HalfAdder ha = new HalfAdder();
            if (!ha.TestGate())
                Console.WriteLine("half adder bugbug");

            //fullAdder
            FullAdder fa = new FullAdder();
            if (!fa.TestGate())
                Console.WriteLine("full adder bugbug");

            //multiAdder
            MultiBitAdder ma = new MultiBitAdder(4);
            if (!ma.TestGate())
                Console.WriteLine("multi adder bugbug");

            //Now we ruin the nand gates that are used in all other gates. The gate should not work properly after this.
            NAndGate.Corrupt = true;
            if (and.TestGate() || or.TestGate() || xor.TestGate() || mux.TestGate() || 
                    demux.TestGate() || multiAnd.TestGate() || multiOr.TestGate() ||
                    bitAnd.TestGate() || bitOr.TestGate())
                Console.WriteLine("bugbug");

            */

            //BitwiseMultiwayMux m = new BitwiseMultiwayMux(7, 3);


            //test set and get 2s compliment
            /*WireSet w = new WireSet(4);
            Random rnd = new Random();

            for(int i = 0; i < 20; i++)
            {
                w.Set2sComplement(rnd.Next(-8, 7));
                Console.WriteLine("get:2s: " + w.Get2sComplement());
            }*/

            /*ALU alu = new ALU(4);
            if (!alu.TestGate())
                Console.WriteLine("alu bugbug");*/

            SingleBitRegister sbr = new SingleBitRegister();
            if(!sbr.TestGate())
                Console.WriteLine("alu bugbug");

            MultiBitRegister mbr = new MultiBitRegister(4);
            if (!mbr.TestGate())
                Console.WriteLine("mbr bugbug");/*

            Memory m = new Memory(4, 4);
            if (!m.TestGate())
                Console.WriteLine("m bugbug");*/

            Counter c = new Counter(4);
            if (!c.TestGate())
                Console.WriteLine("counter bugbug");

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
