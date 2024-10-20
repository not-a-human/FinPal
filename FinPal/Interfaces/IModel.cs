using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Interfaces
{
    public interface IModel
    {
        int Id { get; set; }
        bool Active { get; set; }
    }
}
