using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)

    class MultiBitOrGate : MultiBitGate
    {
        //your code here
        OrGate[] or;

        public MultiBitOrGate(int iInputCount)
            : base(iInputCount)
        {
            //your code here
            or = new OrGate[iInputCount];

            for (int i = 0; i < iInputCount-1; i++)
                or[i] = new OrGate();

            or[0].ConnectInput1(m_wsInput[0]);
            or[0].ConnectInput2(m_wsInput[1]);

            for (int i = 1; i < iInputCount - 1; i++)
            {
                or[i].ConnectInput1(or[i - 1].Output);
                or[i].ConnectInput2(m_wsInput[i + 1]);
            }

            this.Output.ConnectInput(or[iInputCount - 2].Output);
        }

        public override bool TestGate()
        {
            for (int i = 0; i < 10; i++)
            {
                //get random number in range [0, 2^inputcount]
                Random rand = new Random();
                int test = rand.Next(0, (int)Math.Pow(2, m_wsInput.Size) - 1);

                m_wsInput.SetValue(test);

                if (this.Output.Value == 0 && test != 0)
                    return false;
                if (this.Output.Value == 1 && test == 0)
                    return false;

            }

            //checking all 0;
            for (int i = 0; i < m_wsInput.Size; i++)
                m_wsInput[i].Value = 0;

            if (this.Output.Value != 0)
                return false;


            return true;
        }
    }
}
