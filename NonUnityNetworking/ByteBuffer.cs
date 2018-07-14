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
  public long getReadPosition() {
    return readPosition;
  }
  public byte[] toArray() {
    return buffer.ToArray();
  }
  public int count() {
    return buffer.Count;
  }
  public int length() {
    return count() - readPosition;
  }
  public void clear() {
    buffer.Clear();
    readPosition = 0;
  }

  // Write methods
  public void writeBytes(byte[] input) {
    buffer.AddRange(input);
    bufferUpdated = true;
  }
  public void writeShort(short input) {
    buffer.AddRange(BitConverter.GetBytes(input));
    bufferUpdated = true;
  }
  public void writeInteger(int input) {
    buffer.AddRange(BitConverter.GetBytes(input));
    bufferUpdated = true;
  }
  public void writeFloat(float input) {
    buffer.AddRange(BitConverter.GetBytes(input));
    bufferUpdated = true;
  }
  public void writeLong(long input) {
    buffer.AddRange(BitConverter.GetBytes(input));
    bufferUpdated = true;
  }
  public void writeString(string input) {
    buffer.AddRange(BitConverter.GetBytes(input.Length));
    buffer.AddRange(Encoding.ASCII.GetBytes(input));
    bufferUpdated = true;
  }

  // Read methods
  public int readInteger(bool peek = true) {
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
  public byte[] readBytes(int length, bool peek = true) {
    if (bufferUpdated) {
      readBuffer = buffer.ToArray();
      bufferUpdated = false;
    }

    byte[] returnValue = buffer.GetRange(readPosition, length).ToArray();
    if (peek)
      readPosition += length;

    return returnValue;
  }
  public string readString(bool peek = true) {
    int length = readInteger(true);
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
  public short readShort(bool peek = true) {
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
  public float readFloat(bool peek = true) {
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
  public long readLong(bool peek = true) {
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
  protected virtual void dispose(bool disposing) {
    if (!disposedValue) {
      if (disposing) {
        buffer.Clear();
      }
      readPosition = 0;
    }

    disposedValue = true;
  }
  public void Dispose() {
    dispose(true);
    GC.SuppressFinalize(this);
  }
}