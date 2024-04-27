using System.Collections.Generic;
using System.Reflection;

namespace Input.Interface
{
    public interface ISetFireModes
    {
        void SetFireModes(List<MethodInfo> methodFireStates);
    }
}