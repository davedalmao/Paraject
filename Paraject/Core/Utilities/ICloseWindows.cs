using System;

namespace Paraject.Core.Utilities
{
    public interface ICloseWindows
    {
        Action Close { get; set; }
    }
}
