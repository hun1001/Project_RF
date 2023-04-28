using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BufferQueue
{
    private static Queue<byte[]> _bufferQueue = new Queue<byte[]>();

    public static void Enqueue(byte[] buffer)
    {
        _bufferQueue.Enqueue(buffer);
    }

    public static byte[] Dequeue()
    {
        return _bufferQueue.Dequeue();
    }

    public static int Count => _bufferQueue.Count;

}

