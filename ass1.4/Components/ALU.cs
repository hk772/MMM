using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class is used to implement the ALU
    class ALU : Gate
    {
        //The word size = number of bit in the input and output
        public int Size { get; private set; }

        //Input and output n bit numbers
        //inputs
        public WireSet InputX { get; private set; }
        public WireSet InputY { get; private set; }
        public WireSet Control { get; private set; }

        //outputs
        public WireSet Output { get; private set; }
        public Wire Zero { get; private set; }
        public Wire Negative { get; private set; }


        //your code here
        BitwiseMultiwayMux alu;

        WireSet ONE;
        WireSet ZERO;
        BitwiseMux xy1Mux;
        BitwiseNotGate xyNot;
        BitwiseMux xyNotxyMux;
        AndGate c2c3And;
        AndGate c2c3NotAnd;
        NotGate c2Not;
        OrGate xyNotxyControlOr;
        MultiBitAdder mbr1;
        BitwiseNotGate not1;
        MultiBitAdder mbr2;
        BitwiseMux oneNotoneMux;
        MultiBitAdder mbr3;
        BitwiseMux xyReverseMux;
        MultiBitAdder mbr4;
        BitwiseAndGate bitAnd;
        MultiBitOrGate multiAndOr;
        MultiBitOrGate multiAnd2Or;
        AndGate and;
        BitwiseMux multiAndMux;

        BitwiseOrGate bitOr;

        //MultiBitOrGate multiOr;
        //MultiBitOrGate multiOr2;
        OrGate or;
        BitwiseMux orMuxGate;

        MultiBitOrGate zeroOr;
        NotGate zeroNot;

        public ALU(int iSize)
        {
            Size = iSize;
            InputX = new WireSet(Size);
            InputY = new WireSet(Size);
            Control = new WireSet(6);
            Zero = new Wire();


            //Create and connect all the internal components
            alu = new BitwiseMultiwayMux(Size, 6);
            alu.ConnectControl(Control);

            ZERO = new WireSet(Size);
            ZERO.Set2sComplement(0);
            alu.Inputs[0].ConnectInput(ZERO);

            ONE = new WireSet(Size);
            ONE.Set2sComplement(1);
            alu.Inputs[1].ConnectInput(ONE);

            xy1Mux = new BitwiseMux(Size);
            xy1Mux.ConnectInput1(InputX);
            xy1Mux.ConnectInput2(InputY);
            xy1Mux.ConnectControl(Control[0]);

            alu.Inputs[2].ConnectInput(xy1Mux.Output);
            alu.Inputs[3].ConnectInput(xy1Mux.Output);

            xyNot = new BitwiseNotGate(Size);
            xyNot.ConnectInput(xy1Mux.Output);

            alu.Inputs[4].ConnectInput(xyNot.Output);
            alu.Inputs[5].ConnectInput(xyNot.Output);

            xyNotxyMux = new BitwiseMux(Size);
            //c2c3And = new AndGate();
            c2c3NotAnd = new AndGate();
            c2Not = new NotGate();
            xyNotxyControlOr = new OrGate();
            mbr1 = new MultiBitAdder(Size);

            //c2c3And.ConnectInput1(Control[2]);
            //c2c3And.ConnectInput2(Control[3]);

            c2Not.ConnectInput(Control[2]);

            c2c3NotAnd.ConnectInput1(Control[3]);
            c2c3NotAnd.ConnectInput2(c2Not.Output); ////change??

            //xyNotxyControlOr.ConnectInput1(c2c3And.Output);
            //xyNotxyControlOr.ConnectInput2(c2c3NotAnd.Output);

            xyNotxyMux.ConnectInput1(xyNot.Output);
            xyNotxyMux.ConnectInput2(xy1Mux.Output);
            xyNotxyMux.ConnectControl(c2c3NotAnd.Output);

            mbr1.ConnectInput1(ONE);
            mbr1.ConnectInput2(xyNotxyMux.Output);

            alu.Inputs[6].ConnectInput(mbr1.Output);
            alu.Inputs[7].ConnectInput(mbr1.Output);
            alu.Inputs[8].ConnectInput(mbr1.Output);
            alu.Inputs[9].ConnectInput(mbr1.Output);

            not1 = new BitwiseNotGate(Size);
            mbr2 = new MultiBitAdder(Size);
            oneNotoneMux = new BitwiseMux(Size);
            mbr3 = new MultiBitAdder(Size);

            not1.ConnectInput(ONE);
            mbr2.ConnectInput1(not1.Output);
            mbr2.ConnectInput2(ONE);

            oneNotoneMux.ConnectInput1(InputY);
            oneNotoneMux.ConnectInput2(mbr2.Output);
            oneNotoneMux.ConnectControl(Control[1]);

            mbr3.ConnectInput1(xy1Mux.Output);
            mbr3.ConnectInput2(oneNotoneMux.Output);

            alu.Inputs[10].ConnectInput(mbr3.Output);
            alu.Inputs[11].ConnectInput(mbr3.Output);
            alu.Inputs[12].ConnectInput(mbr3.Output);

            xyReverseMux = new BitwiseMux(Size);
            mbr4 = new MultiBitAdder(Size);

            xyReverseMux.ConnectInput1(InputY);
            xyReverseMux.ConnectInput2(InputX);
            xyReverseMux.ConnectControl(Control[0]);

            mbr4.ConnectInput1(xyReverseMux.Output);
            mbr4.ConnectInput2(mbr1.Output);

            alu.Inputs[13].ConnectInput(mbr4.Output);
            alu.Inputs[14].ConnectInput(mbr4.Output);


            bitAnd = new BitwiseAndGate(Size);
            bitAnd.ConnectInput1(InputX);
            bitAnd.ConnectInput2(InputY);

            alu.Inputs[15].ConnectInput(bitAnd.Output);

            multiAndOr = new MultiBitOrGate(Size);
            multiAnd2Or = new MultiBitOrGate(Size);
            and = new AndGate();
            multiAndMux = new BitwiseMux(Size);

            multiAndOr.ConnectInput(InputX);
            multiAnd2Or.ConnectInput(InputY);
            and.ConnectInput1(multiAndOr.Output);
            and.ConnectInput2(multiAnd2Or.Output);

            multiAndMux.ConnectInput1(ZERO);
            multiAndMux.ConnectInput2(ONE);
            multiAndMux.ConnectControl(and.Output);

            alu.Inputs[16].ConnectInput(multiAndMux.Output);

            bitOr = new BitwiseOrGate(Size);
            bitOr.ConnectInput1(InputX);
            bitOr.ConnectInput2(InputY);

            alu.Inputs[17].ConnectInput(bitOr.Output);

            //multiOr = new MultiBitOrGate(Size);
            //multiOr2 = new MultiBitOrGate(Size);
            or = new OrGate();

            //multiOr.ConnectInput(InputX);
            //multiOr2.ConnectInput(InputY);
            or.ConnectInput1(multiAndOr.Output);
            or.ConnectInput2(multiAnd2Or.Output);

            orMuxGate = new BitwiseMux(Size);
            orMuxGate.ConnectInput1(ZERO);
            orMuxGate.ConnectInput2(ONE);
            orMuxGate.ConnectControl(or.Output);

            alu.Inputs[18].ConnectInput(orMuxGate.Output);

            Output = new WireSet(Size);
            Output.ConnectInput(alu.Output);

            zeroOr = new MultiBitOrGate(Size);
            zeroNot = new NotGate();
            zeroOr.ConnectInput(alu.Output);
            zeroNot.ConnectInput(zeroOr.Output);
            Zero.ConnectInput(zeroNot.Output);

            Negative = new Wire();
            Negative.ConnectInput(alu.Output[Size - 1]);

        }

        public override bool TestGate()
        {
            Random random = new Random();

            for(int i = 0; i < 3; i++)
            {
                int val1 = random.Next(-(int)Math.Pow(2, Size - 1), (int)Math.Pow(2, Size-1) - 1);
                int val2 = random.Next(-(int)Math.Pow(2, Size - 1), (int)Math.Pow(2, Size-1) - 1);

                

                int[] correctOutsputs = { 0, 1, val1, val2, -val1-1, -val2-1, -val1, -val2,
                                        val1+1, val2+1, val1-1, val2-1, val1+val2, val1-val2,
                                        val2-val1};//14 first outs

                //int c = random.Next(0, 18);

                InputX.Set2sComplement(val1);
                InputY.Set2sComplement(val2);

                Console.WriteLine("numbers: val1 = " + val1 + ", val2 = " + val2);


                for (int j = 0; j < 19; j++)
                {
                    Control.SetValue(j);

                    if (j < 15 && alu.Output.Get2sComplement() != correctOutsputs[j])
                    { 
                        Console.WriteLine("error: " + j + ", output: " + Output.Get2sComplement());
                        //Console.WriteLine("output: " + j);
                    }
                    else if (j == 15)
                    {

                    }
                    else if (j == 16)
                    {
                        
                        Console.WriteLine("hello");
                    }
                    else if (j == 17)
                    {

                    }
                    else if (j == 18)
                    {

                    }

                    if (Output.Get2sComplement() == 0 && Zero.Value != 1 ||
                        Output.Get2sComplement() != 0 && Zero.Value == 1)
                        return false;

                    if (Output.Get2sComplement() < 0 && Negative.Value != 1 ||
                        Output.Get2sComplement() >= 0 && Negative.Value == 1)
                        return false;

                }

                
            }

            return true;
        }
    }
}
