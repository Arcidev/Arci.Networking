using Arci.Networking.Object.Attributes;

namespace Arci.Networking.Tests.UAP.ObjectTests.Objects
{
    [PacketClass(3)]
    public class TestObject3
    {
        [PacketProperty(1)]
        public TestObject1 Object1 { get; set; }

        [PacketProperty(2)]
        public TestObject2 Object2 { get; set; }
    }
}
