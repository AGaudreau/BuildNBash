using System.Collections.Generic;
using System;
using System.Text;

public class ByteBuffer: IDisposable
{
  // Member variables
  private List<byte> buffer;
  private byte[] readBuffer;
  private int readPosition;
  private bool bufferUpdated = false;
  private bool disposedValue = false; // detects redudant calls to dispose

  // Construction
  public ByteBuffer() {
    buffer = new List<byte>();
    readPosition = 0;
  }

  // utility functions
  public long GetReadPosition() {
    return readPosition;
  }
  public byte[] ToArray() {
    return buffer.ToArray();
  }
  public int Count() {
    return buffer.Count;
  }
  public int Length() {
    return Count() - readPosition;
  }
  public void Clear() {
    buffer.Clear();
    readPosition = 0;
  }

  // Write methods
  public void WriteBytes(byte[] input) {
    buffer.AddRange(input);
    bufferUpdated = true;
  }
  public void WriteShort(short input) {
    buffer.AddRange(BitConverter.GetBytes(input));
    bufferUpdated = true;
  }
  public void WriteInteger(int input) {
    buffer.AddRange(BitConverter.GetBytes(input));
    bufferUpdated = true;
  }
  public void WriteFloat(float input) {
    buffer.AddRange(BitConverter.GetBytes(input));
    bufferUpdated = true;
  }
  public void WriteLong(long input) {
    buffer.AddRange(BitConverter.GetBytes(input));
    bufferUpdated = true;
  }
  public void WriteString(string input) {
    buffer.AddRange(BitConverter.GetBytes(input.Length));
    buffer.AddRange(Encoding.ASCII.GetBytes(input));
    bufferUpdated = true;
  }

  // Read methods
  public int ReadInteger(bool peek = true) {
    if (buffer.Count > readPosition) {
      if (bufferUpdated) {
        readBuffer = buffer.ToArray();
        bufferUpdated = false;
      }
      int returnValue = BitConverter.ToInt32(readBuffer, readPosition);
      if (peek && buffer.Count > readPosition) {
        readPosition += 4;
      }
      return returnValue;
    } else {
      throw new Exception("Byte buffer is past limit");
    }
  }
  public byte[] ReadBytes(int length, bool peek = true) {
    if (bufferUpdated) {
      readBuffer = buffer.ToArray();
      bufferUpdated = false;
    }

    byte[] returnValue = buffer.GetRange(readPosition, length).ToArray();
    if (peek)
      readPosition += length;

    return returnValue;
  }
  public string ReadString(bool peek = true) {
    int length = ReadInteger(true);
    if (bufferUpdated) {
      readBuffer = buffer.ToArray();
      bufferUpdated = false;
    }

    string returnValue = Encoding.ASCII.GetString(readBuffer, readPosition, length);
    if (peek && buffer.Count > readPosition && returnValue.Length > 0) {
      readPosition += length;
    }
    return returnValue;
  }
  public short ReadShort(bool peek = true) {
    if (buffer.Count > readPosition) {
      if (bufferUpdated) {
        readBuffer = buffer.ToArray();
        bufferUpdated = false;
      }
      short returnValue = BitConverter.ToInt16(readBuffer, readPosition);
      if (peek && buffer.Count > readPosition) {
        readPosition += 2;
      }
      return returnValue;
    } else {
      throw new Exception("Byte buffer is past limit");
    }
  }
  public float ReadFloat(bool peek = true) {
    if (buffer.Count > readPosition) {
      if (bufferUpdated) {
        readBuffer = buffer.ToArray();
        bufferUpdated = false;
      }
      float returnValue = BitConverter.ToSingle(readBuffer, readPosition);
      if (peek && buffer.Count > readPosition) {
        readPosition += 4;
      }
      return returnValue;
    } else {
      throw new Exception("Byte buffer is past limit");
    }
  }
  public long ReadLong(bool peek = true) {
    if (buffer.Count > readPosition) {
      if (bufferUpdated) {
        readBuffer = buffer.ToArray();
        bufferUpdated = false;
      }
      long returnValue = BitConverter.ToInt64(readBuffer, readPosition);
      if (peek && buffer.Count > readPosition) {
        readPosition += 8;
      }
      return returnValue;
    } else {
      throw new Exception("Byte buffer is past limit");
    }
  }

  // IDisposable methods
  protected virtual void Dispose(bool disposing) {
    if (!disposedValue) {
      if (disposing) {
        buffer.Clear();
      }
      readPosition = 0;
    }

    disposedValue = true;
  }
  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}