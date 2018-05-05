using System;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Hermann
{
    /// <summary>
    /// Mac OS向けDebug listener.
    /// </summary>
    public class DebugListener : TraceListener
    {
        public override void Write(string message)
        {
            Console.Write(message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public override void Fail(string message)
        {
            Fail(message, String.Empty);
        }

        public override void Fail(string message, string detailMessage)
        {
            if (null == detailMessage)
                detailMessage = String.Empty;

            var outputMessage = new StringBuilder();
            outputMessage.AppendLine($"{message}: {detailMessage}");
            Console.WriteLine(outputMessage);
            Console.WriteLine("Stack Trace:");

            var trace = new StackTrace(true);
            foreach (var frame in trace.GetFrames())
            {
                MethodBase frameClass = frame.GetMethod();
                var stackMessage = $"  {frameClass.DeclaringType}.{frameClass.Name} {frame.GetFileName()}:{frame.GetFileLineNumber()}";
                outputMessage.AppendLine(stackMessage);
                Console.WriteLine("  {2}.{3} {0}:{1}",
                                   frame.GetFileName(),
                                   frame.GetFileLineNumber(),
                                   frameClass.DeclaringType,
                                   frameClass.Name);
            }

            throw new ApplicationException(outputMessage.ToString());
        }
    }
}
