using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace tokenizr.net.compression
{
  public class Compression : ICompression
  {
    public string Compress(string source)
    {
      byte[] buffer = Encoding.UTF8.GetBytes(source);
      byte[] gzBuffer = null;
      using (MemoryStream ms = new MemoryStream())
      {
        using (GZipStream zip = new GZipStream(ms, CompressionLevel.Optimal, true))
        {
          zip.Write(buffer, 0, buffer.Length);
        }

        ms.Position = 0;

        byte[] compressed = new byte[ms.Length];
        ms.Read(compressed, 0, compressed.Length);

        gzBuffer = new byte[compressed.Length + 4];
        Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
      }
      return Convert.ToBase64String(gzBuffer);
    }

    public string Decompress(string source)
    {
      byte[] gzBuffer = Convert.FromBase64String(source);
      using (MemoryStream ms = new MemoryStream())
      {
        int msgLength = BitConverter.ToInt32(gzBuffer, 0);
        ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

        byte[] buffer = new byte[msgLength];

        ms.Position = 0;
        using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
        {
          zip.Read(buffer, 0, buffer.Length);
        }

        return Encoding.UTF8.GetString(buffer);
      }
    }
  }
}
