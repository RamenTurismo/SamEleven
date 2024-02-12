namespace Emyfreya.Steam.Desktop.Models;

internal sealed class VirtualClassWrapper<T>
    where T : struct
{
    public T VirtualClass { get; }
    public nint InterfaceHandle { get; }

    public VirtualClassWrapper(T virtualClass, nint interfaceHandle)
    {
        VirtualClass = virtualClass;
        InterfaceHandle = interfaceHandle;
    }

    public TDelegate GetDelegate<TDelegate>(Func<T, nint> getDelegatePointer) where TDelegate : Delegate
    {
        return Marshal.GetDelegateForFunctionPointer<TDelegate>(getDelegatePointer(VirtualClass));
    }
}
