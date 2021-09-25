using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Lowering.Models
{
    public sealed class HardwareDevice
    {
        private readonly List<HardwareMethod> hardwareMethods;

        public string DeviceName { get; }
        public IReadOnlyList<HardwareMethod> HardwareMethods => hardwareMethods.AsReadOnly();

        public HardwareDevice(string deviceName, IList<HardwareMethod> hardwareMethods)
        {
            DeviceName = deviceName;
            this.hardwareMethods = (List<HardwareMethod>)hardwareMethods;
        }
    }
}
