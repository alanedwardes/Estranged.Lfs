using System;

namespace Estranged.Lfs.Data
{
    [Flags]
    public enum LfsPermission : uint
    {
        None = 0x0,
        Read = 0x1,
        Write = 0x2
    }
}
