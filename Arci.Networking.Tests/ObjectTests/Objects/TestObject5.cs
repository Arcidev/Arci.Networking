using Arci.Networking.Object.Attributes;
using System.Collections.Generic;

namespace Arci.Networking.Tests.ObjectTests.Objects
{
    [PacketClass(5)]
    public class TestObject5
    {
        [PacketProperty(1)]
        public List<int> ListOfInt { get; set; }

        [PacketProperty(2)]
        public string[] ArrayOfString { get; set; }

        [PacketProperty(3)]
        public List<TestObject3> ListOfObject { get; set; }
    }
}
