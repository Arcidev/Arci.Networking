using Arci.Networking.Serialization.Attributes;

namespace Arci.Networking.Tests.ObjectTests.Objects
{
    [PacketClass(4)]
    public class TestObject4
    {
        [PacketProperty(1)]
        public TestObject1 Object1 { get; set; }

        public TestObject2 NotSerializedObject2 { get; set; }

        [PacketProperty(2)]
        public TestObject3 Object3 { get; set; }

        [PacketProperty(3)]
        public int? NullableInt { get; set; }

        [PacketProperty(4)]
        public int? NullableInt2 { get; set; }

        [PacketProperty(5)]
        public string String { get; set; }

        public string NotSerializedString { get; set; }
    }
}
