using System;
using RTCM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UINT6_IN_BYTE()
        {
            uint testBits = 0xAB;
            uint expectedResult = 0x2A;
            byte[] buffer = buffer = BitConverter.GetBytes(testBits);
            uint result = BitUtil.GetUnsigned(buffer, 2, 6);
            Assert.AreEqual(expectedResult, result, "Result not correct");
        }

        [TestMethod]
        public void UINT6_SPAN_BYTE()
        {
            uint testBits = 0xFD5F;
            uint expectedResult = 0x2A;
            byte[] buffer = buffer = BitConverter.GetBytes(testBits);
            uint result = BitUtil.GetUnsigned(buffer, 5, 6);
            Assert.AreEqual(expectedResult, result, "Result not correct");
        }

        [TestMethod]
        public void UINT16_BYTE_CORRECT()
        {
            uint testBits = 0xAAAAFF;
            uint expectedResult = 0xAAAA;
            byte[] buffer = buffer = BitConverter.GetBytes(testBits);
            uint result = BitUtil.GetUnsigned(buffer, 8, 16);
            Assert.AreEqual(expectedResult, result, "Result not correct");
        }

        [TestMethod]
        public void UINT16_SPAN_BYTE()
        {
            uint testBits = 0x2AAABFF;
            uint expectedResult = 0xAAAA;
            byte[] buffer = buffer = BitConverter.GetBytes(testBits);
            uint result = BitUtil.GetUnsigned(buffer, 10, 16);
            Assert.AreEqual(expectedResult, result, "Result not correct");            
        }

        [TestMethod]
        public void INT16_BYTE_CORRECT()
        {
            uint testBits = 0x8002;
            int expectedResult = -32766;
            byte[] buffer = buffer = BitConverter.GetBytes(testBits);
            int result = BitUtil.GetSigned(buffer, 0, 16);
            Assert.AreEqual(expectedResult, result, "Result not correct");
        }

        [TestMethod]
        public void INT16_SPAN_BYTE()
        {
            uint testBits = 0x3C0025;
            int expectedResult = -32764;
            byte[] buffer = buffer = BitConverter.GetBytes(testBits);
            int result = BitUtil.GetSigned(buffer, 3, 16);
            Assert.AreEqual(expectedResult, result, "Result not correct");
        }

        [TestMethod]
        public void UINT16_BYTE_CORRECT_WRITE()
        {
            byte[] buffer = new byte[5];
            BitUtil.WriteUnsigned(ref buffer, 236, 8, 16);
            ushort value = BitConverter.ToUInt16(buffer, 1);
            Assert.AreEqual(236, value, "Result not correct");
        }
         
        [TestMethod]
        public void UINT16_SPAN_BYTE_WRITE()
        {
            byte[] buffer = new byte[5];
            buffer[0] = 0xff;
            buffer[1] = 0xff;
            buffer[2] = 0xff;
            BitUtil.WriteUnsigned(ref buffer, 236, 2, 16);
            uint value = BitConverter.ToUInt32(buffer, 0);
            Assert.AreEqual((uint)0xFC03B3, value, "Result not correct");
        }

        [TestMethod]
        public void UINT14_SPAN_BYTE_WRITE_2()
        {
            byte[] buffer = new byte[5];
            buffer[0] = 0xff;
            buffer[1] = 0xff;
            buffer[2] = 0xff;
            BitUtil.WriteUnsigned(ref buffer, 236, 2, 14);
            uint value = BitConverter.ToUInt32(buffer, 0);
            Assert.AreEqual((uint)0xFF03B3, value, "Result not correct");
        }

        [TestMethod]
        public void INT16_SPAN_BYTE_WRITE()
        {
            byte[] buffer = new byte[5];
            buffer[0] = 0xff;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            BitUtil.WriteSigned(ref buffer, -236, 2, 16);
            uint value = BitConverter.ToUInt32(buffer, 0);
            Assert.AreEqual((uint)0x3FC53, value, "Result not correct");
        }

        [TestMethod]
        public void INT14_SPAN_BYTE_WRITE_2()
        {
            byte[] buffer = new byte[5];
            buffer[0] = 0xff;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            BitUtil.WriteSigned(ref buffer, -236, 2, 14);
            uint value = BitConverter.ToUInt32(buffer, 0);
            Assert.AreEqual((uint)0xFC53, value, "Result not correct");
        }
    
        [TestMethod]
        public void WRITE_AND_READBACK()
        {
            byte[] buffer = new byte[3];
            BitUtil.WriteUnsigned(ref buffer, 92, 14, 10);
            uint test = BitUtil.GetUnsigned(buffer, 14, 10);
            Assert.AreEqual(test, (uint)92);
        }
    }
}
