namespace Emyfreya.Steam.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct VirtualClass
{
    public nint VirtualTable;
}
