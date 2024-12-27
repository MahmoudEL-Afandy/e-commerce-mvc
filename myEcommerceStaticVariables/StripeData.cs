using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceUtilities
{
    public class StripeData
    {
        // this the second step for payment stripe implementation
        // then i will inject it in the program.cs to make this property take data from the json file

        public string Secretkey { get; set; }
        public string Publishablekey { get; set; }

    }
}
