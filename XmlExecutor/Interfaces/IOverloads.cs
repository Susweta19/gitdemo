using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExecutor.Interfaces
{
    public interface IOverload<B, N>
    {
        N Build(B a);
    }
}
