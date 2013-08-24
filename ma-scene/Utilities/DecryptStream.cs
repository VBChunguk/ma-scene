using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Vbc.MA.Scenario.Utilities
{
    /// <summary>
    /// 밀리언아서 데이터 복호화 스트림을 구현합니다.
    /// </summary>
    class DecryptStream : Stream
    {
        private CryptoStream mInternal;

        /// <summary>
        /// 기존 스트림으로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="stream">기존 스트림입니다.</param>
        public DecryptStream(Stream stream)
        {
            AesManaged managed = new AesManaged();
            managed.Mode = CipherMode.ECB;
            managed.Padding = PaddingMode.PKCS7;
            managed.IV = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            managed.Key = Encoding.ASCII.GetBytes("A1dPUcrvur2CRQyl");
            ICryptoTransform crypto = managed.CreateDecryptor();

            mInternal = new CryptoStream(stream, crypto, CryptoStreamMode.Read);
        }

        public override bool CanRead
        {
            get { return mInternal.CanRead; }
        }

        public override bool CanSeek
        {
            get { return mInternal.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return mInternal.CanWrite; }
        }

        public override void Flush()
        {
            mInternal.Flush();
        }

        public override long Length
        {
            get { return mInternal.Length; }
        }

        public override long Position
        {
            get
            {
                return mInternal.Position;
            }
            set
            {
                mInternal.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return mInternal.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return mInternal.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            mInternal.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            mInternal.Write(buffer, offset, count);
        }

        public override void Close()
        {
            mInternal.Close();
        }
    }
}
