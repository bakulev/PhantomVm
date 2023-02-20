using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cc.Anba.PhantomOs.VirtualMachine.Classes
{
    public class Codegen
    {
        private int next_label_no = 0;

        public Codegen()
        {

        }

        /// <summary>
        /// Create a new unique label name.
        /// </summary>
        /// <returns>Name of a new label</returns>
        public String getLabel()
        {
            return "_label_" + (next_label_no++).ToString();
        }
    }
}
