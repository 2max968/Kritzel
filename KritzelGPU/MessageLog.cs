using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Main
{
    public class MessageLog : IEnumerable<MessageEntry>
    {
        public List<MessageEntry> Entries = new List<MessageEntry>();
        public MessageEntry this[int ind]
        {
            get
            {
                return Entries[ind];
            }
            set
            {
                Entries[ind] = value;
            }
        }

        public void Add(MessageType type, string formatter, params object[] args)
        {
            Entries.Add(new MessageEntry(type, formatter, args));
        }

        public IEnumerator<MessageEntry> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(MessageEntry message in this)
            {
                sb.AppendFormat("[{0}]\t{1}\n", message.Type.ToString(), message.Message);
            }
            return sb.ToString();
        }
    }

    public class MessageEntry
    {
        public MessageType Type;
        public string Message;

        public MessageEntry(MessageType type, string formatter, params object[] args)
        {
            this.Type = type;
            this.Message = string.Format(formatter, args);
        }
    }

    public enum MessageType
    {
        MSG,
        WARN,
        ERROR
    }
}
