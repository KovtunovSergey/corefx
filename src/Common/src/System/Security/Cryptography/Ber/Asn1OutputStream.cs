﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace System.Security.Cryptography
{
	abstract class Asn1OutputStream : Stream
	{
		protected readonly Stream OutputStream;

		public Asn1OutputStream(Stream outputStream)
		{
			OutputStream = outputStream;
		}

		public override bool CanRead
		{
			get { return false; }
		}

		public override bool CanSeek
		{
			get { return OutputStream.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return OutputStream.CanWrite; }
		}

		public override long Length
		{
			get { return OutputStream.Length; }
		}

		public override long Position
		{
			get { return OutputStream.Position; }
			set { OutputStream.Position = value; }
		}

		public override void Close()
		{
			OutputStream.Close();
		}

		public override void Flush()
		{
			OutputStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new Exception("Asn1ReadOutputStreamNotSupported");

        }

		public override long Seek(long offset, SeekOrigin origin)
		{
			return OutputStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			OutputStream.SetLength(value);
		}

		public virtual void Write(byte[] b)
		{
			OutputStream.Write(b, 0, b.Length);
		}

		public override void Write(byte[] b, int off, int len)
		{
			OutputStream.Write(b, off, len);
		}

		public override void WriteByte(byte b)
		{
			OutputStream.WriteByte(b);
		}

		public virtual void WriteByte(int b)
		{
			OutputStream.WriteByte((byte)b);
		}
	}
}
