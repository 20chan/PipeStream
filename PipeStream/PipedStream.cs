using System;
using System.IO;
using System.Collections.Generic;

namespace PipeStream
{
    public abstract class PipedStream<T>
    {
        public abstract T Read();
        public abstract void Write(T data);

        public static PipedStream<T> operator |(PipedStream<T> to, PipedStream<T> from)
        {
            to.Write(from.Read());
            return to;
        }

        public static PipedStream<T> operator |(PipedStream<T> to, PipedStream<object> from)
        {
            to.Write((T)from.Read());
            return to;
        }

        public static PipedStream<T> operator |(PipedStream<T> to, T data)
        {
            to.Write(data);
            return to;
        }

        public static PipedStream<object> operator |(Func<T, object> func, PipedStream<T> from)
        {
            return new DelegateStream<T, object>(func, from);
        }
    }

    class DelegateStream<T, F> : PipedStream<F>
    {
        Func<T, F> function;
        PipedStream<T> parentStream;
        public DelegateStream(Func<T, F> del, PipedStream<T> stream)
        {
            function = del;
            parentStream = stream;
        }

        public override F Read()
            => function(parentStream.Read());

        public override void Write(F data)
            => throw new Exception();
    }

    class TextReaderLineStream : PipedStream<string>
    {
        TextReader reader;

        public TextReaderLineStream(TextReader tr)
            => reader = tr;

        public override string Read()
            => reader.ReadLine();

        public override void Write(string data)
            => throw new NotSupportedException();
    }

    class TextWriterLineStream : PipedStream<string>
    {
        TextWriter writer;

        public TextWriterLineStream(TextWriter tw)
            => writer = tw;

        public override string Read()
            => throw new Exception();

        public override void Write(string data)
            => writer.WriteLine(data);
    }

    class EnumeratorReaderStream<T> : PipedStream<T>
    {
        IEnumerator<T> array;

        public EnumeratorReaderStream(IEnumerator<T> arr)
        {
            array = arr;
            arr.MoveNext();
        }

        public override T Read()
        {
            var cr = array.Current;
            array.MoveNext();
            return cr;
        }

        public override void Write(T data)
            => throw new Exception();
    }

    class ListWriterStream<T> : PipedStream<T>
    {
        IList<T> list;

        public ListWriterStream(IList<T> arr)
            => list = arr;

        public override T Read()
            => throw new Exception();

        public override void Write(T data)
            => list.Add(data);
    }
}
