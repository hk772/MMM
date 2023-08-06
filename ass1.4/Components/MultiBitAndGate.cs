using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)
    class MultiBitAndGate : MultiBitGate
    {
        //your code here
        AndGate[] and;
        public MultiBitAndGate(int iInputCount)
            : base(iInputCount)
        {
            //your code here
            and = new AndGate[iInputCount-1];
            for(int i = 0; i < iInputCount-1; i++)
            {
                and[i] = new AndGate();
            }

            and[0].ConnectInput1(m_wsInput[0]);
            and[0].ConnectInput2(m_wsInput[1]);

            for(int i = 1; i < iInputCount-1; i++)
            {
                and[i].ConnectInput1(and[i-1].Output);
                and[i].ConnectInput2(m_wsInput[i+1]);
            }

            this.Output.ConnectInput(and[iInputCount - 2].Output);
        }


        public override bool TestGate()
        {
            
            for(int i = 0; i < 10; i++)
            {
                //get random number in range [0, 2^inputcount]
                Random rand = new Random();
                int test = rand.Next(0, (int)Math.Pow(2, m_wsInput.Size) - 1);
               
                m_wsInput.SetValue(test);

                if(this.Output.Value == 0 && test == (int)Math.Pow(2, m_wsInput.Size) - 1)
                    return false;
                if (this.Output.Value == 1 && test != (int)Math.Pow(2, m_wsInput.Size) - 1)
                    return false;

            }

            //checking all 1ns;
            for (int i = 0; i < m_wsInput.Size; i++)
                m_wsInput[i].Value = 1;

            if (this.Output.Value != 1)
                return false;


            return true;
        }

    }
}
