using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PipeStream.Tests
{
    [TestClass]
    public class PipeTests
    {
        [TestMethod]
        public void TestOperator()
        {
            int[] input = { 1, 2, 3, 4, 5 };
            var output = new List<int>();
            
            var from = input.AsInputPipedStream();
            var to = output.AsPipedStream();
            Func<int, object> function = (i) => i * 2;
            for (int i = 0; i < input.Length; i++)
                to |= function | from;

            CollectionAssert.AreEqual(output, new[] { 2, 4, 6, 8, 10 });
        }

        [TestMethod]
        public void TestStream()
        {
            var input = new StringReader("abc\n  def  ");
            var output = new StringWriter();

            var inputStream = input.AsPipedStream();
            var outputStream = output.AsPipedStream();

            outputStream |= inputStream;

            Assert.AreEqual("abc\r\n", output.GetStringBuilder().ToString());

            outputStream |= "ho ho ho";

            Assert.AreEqual("abc\r\nho ho ho\r\n", output.GetStringBuilder().ToString());

            outputStream |= ((s) => s.Trim()) | inputStream;

            Assert.AreEqual("abc\r\nho ho ho\r\ndef\r\n", output.GetStringBuilder().ToString());
        }
    }
}
