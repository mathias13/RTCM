using System;
using System.Collections.Generic;
using RTCM.MESSAGES;

namespace RTCM
{
    public struct ParsedMessage
    {
        private Enums.Messages _messageType;

        private object _message;

        public ParsedMessage(Enums.Messages messageType, object message)
        {
            _messageType = messageType;
            _message = message;
        }

        public Enums.Messages MessageType
        {
            get { return _messageType; }
        }

        public object Message
        {
            get { return _message; }
        }
    }

    public class Parser
    {
        protected readonly IDictionary<Enums.Messages, Type> MESSAGE_DICTIONARY = new Dictionary<Enums.Messages, Type>()
        {
            {Enums.Messages.MSG_1001, typeof(Message_1001)},
            {Enums.Messages.MSG_1002, typeof(Message_1002)},
            {Enums.Messages.MSG_1003, typeof(Message_1003)},
            {Enums.Messages.MSG_1004, typeof(Message_1004)},
            {Enums.Messages.MSG_1005, typeof(Message_1005)},
            {Enums.Messages.MSG_1006, typeof(Message_1006)}
        };

        private List<byte> _buffer = new List<byte>();

        private object _syncObject = new object();

        public Parser()
        {
        }

        public ParsedMessage? Parse(byte[] data)
        {
            lock (_syncObject)
            {
                _buffer.AddRange(data);
                int preambleIndex = 0;
                while (preambleIndex < _buffer.Count && _buffer[preambleIndex] != Constants.RTCM_PREAMBLE)
                    preambleIndex++;

                if (preambleIndex >= _buffer.Count)
                {
                    _buffer.Clear();
                    return null;
                }
                _buffer.RemoveRange(0, preambleIndex);
                if (_buffer.Count < 3)
                    return null;
                byte[] buffer = _buffer.ToArray();
                int messageLength = (int)BitUtil.GetUnsigned(buffer, 14, 10);

                if (messageLength + 6 > buffer.Length)
                    return null;

                _buffer.RemoveRange(0, messageLength + 6);
                uint crcValue = (uint)BitUtil.GetUnsigned(buffer, (messageLength + 3) * 8, 24);
                uint crcCalc = CRC24Q.crc24q(buffer, messageLength + 3, 0);

                if (crcValue != crcCalc)
                    throw new Exception("CRC is not valid");

                byte[] messageBytes = new byte[messageLength];
                Array.Copy(buffer, 3, messageBytes, 0, messageLength);

                int messageNumber = (int)BitUtil.GetUnsigned(messageBytes, 0, 12);

                Enums.Messages messageEnum = Enums.Messages.Unknown;
                if (Enum.IsDefined(typeof(Enums.Messages), messageNumber))
                    messageEnum = (Enums.Messages)(int)messageNumber;

                if (messageEnum == Enums.Messages.Unknown)
                    throw new Exception("MessageNumber: " + messageNumber + " cant be parsed, no valid C# type");
                if (MESSAGE_DICTIONARY.ContainsKey(messageEnum))
                {
                    object message = Activator.CreateInstance(MESSAGE_DICTIONARY[messageEnum], messageBytes);
                    return new ParsedMessage(messageEnum, message);
                }

                return null;
            }
        }
    }
}
