using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.mermaid
{
    internal class SynException: Exception
    {
        public SynException(string message):base(message) { }

        public SynException(string message, SynContext context): 
            base(ContextMessage(message, context)) {
            
        }

        static string ContextMessage(string message, SynContext context)
        {
            string buf = StringTail(context.Cache, 10) +
               context.TextBuilder.ToString(0, Math.Min(10, context.TextBuilder.Length));
            return string.Format("{0}{1} at line {2}", message,
                string.IsNullOrWhiteSpace(buf) ? "" : $" near {buf}",
                context.Line);
        }
        static string StringTail(string str, int length)
        {
            length = Math.Min(length, str.Length);
            return str.Substring(str.Length - length);
        }
    }
}
