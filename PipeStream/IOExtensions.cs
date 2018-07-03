using System.IO;
using System.Collections.Generic;

namespace PipeStream
{
    public static class IOExtensions
    {
        public static PipedStream<string> AsPipedStream(this TextReader reader)
            => new TextReaderLineStream(reader);

        public static PipedStream<string> AsPipedStream(this TextWriter writer)
            => new TextWriterLineStream(writer);

        public static PipedStream<T> AsInputPipedStream<T>(this IEnumerable<T> arr)
            => new EnumeratorReaderStream<T>(arr.GetEnumerator());

        public static PipedStream<T> AsPipedStream<T>(this IList<T> list)
            => new ListWriterStream<T>(list);
    }
}
