using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taurus.FindFiles.FileFilter
{
    public enum FileFilterOperator
    {
        Equals = 0,
        GreaterThan = 1,
        LessThan = 2,
        Between = 3,
        Like = 4,
        Contains = 5,
        NotEquals = 6,
        GreaterThanEquals = 7,
        LessThanEquals = 8

    }
}
