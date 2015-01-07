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

        public Parser()
        {
        }

        public ParsedMessage? Parse(byte[] data)
        {
            _buffer.AddRange(data);
            int preambleIndex = 0;
            while (preambleIndex < _buffer.Count && _buffer[preambleIndex] != Constants.RTCM_PREAMBLE)
                preambleIndex++;

            if(preambleIndex >= _buffer.Count)
            {
                _buffer.Clear();
                return null;
            }

            _buffer.RemoveRange(0, preambleIndex + 1);
            byte[] bufferBytes = _buffer.ToArray();
            int messageLength = (int)BitUtil.GetUnsigned(bufferBytes, 14, 10);

            if (messageLength + 6 < _buffer.Count)
                return null;

            _buffer.RemoveRange(0, messageLength + 6);

            uint crcValue = BitConverter.ToUInt32(bufferBytes, messageLength + 3); 
            uint crcCalc = CRC24Q.crc24q(bufferBytes, messageLength + 6, 0);

            if (crcValue != crcCalc)
                throw new Exception("CRC is not valid");

            byte[] messageBytes = new byte[messageLength];
            Array.Copy(bufferBytes, 3, messageBytes, 0, messageLength);

            int messageNumber = (int)BitUtil.GetUnsigned(messageBytes, 0, 12);

            Enums.Messages messageEnum = Enums.Messages.Unknown;
            if (Enum.IsDefined(typeof(Enums.Messages), messageNumber))
                messageEnum = (Enums.Messages)(int)messageNumber;

            if (MESSAGE_DICTIONARY.ContainsKey(messageEnum))
            {
                object message = Activator.CreateInstance(MESSAGE_DICTIONARY[messageEnum], messageBytes);
                return new ParsedMessage(messageEnum, message);
            }

            return null;
        }

    }
}
