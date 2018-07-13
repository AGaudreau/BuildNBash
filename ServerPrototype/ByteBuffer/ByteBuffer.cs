using System.Collections.Generic;
using System;
using System.Text;

public class ByteBuffer
{
  private List<byte> buffer;
  private byte[] readBuffer;
  private int readPosition;
  private bool bufferUpdated = false;
  
  public ByteBuffer() {
    buffer = new List<byte>();
    readPosition = 0;
  }

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

  public int readInteger(bool peek = true) {
    if (buffer.Count > readPosition) {
      if (bufferUpdated) {
        readBuffer = buffer.ToArray();
        bufferUpdated = false;
      }
      int ret = BitConverter.ToInt32(readBuffer, readPosition);
      if (peek && buffer.Count > readPosition) {
        readPosition += 4;
      }
      return ret;
    } else {
      throw new Exception("Byte buffer is past limit");
    }
  }

  public byte[] readBytes(int length, bool peek = true) {

  }

}