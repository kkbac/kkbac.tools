using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kkbac.Tools.Web.MVC.Filters
{
    public abstract class DefaultStream : Stream
    {
        #region properties
        Stream responseStream;
        long position;
        StringBuilder html = new StringBuilder();
        #endregion
        #region constructor
        public DefaultStream(Stream inputStream)
        {
            responseStream = inputStream;
        }
        #endregion
        #region implemented abstract members
        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanSeek
        {
            get { return true; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override void Close()
        {
            responseStream.Close();
        }
        public override void Flush()
        {
            responseStream.Flush();
        }
        public override long Length
        {
            get { return 0; }
        }
        public override long Position
        {
            get { return position; }
            set { position = value; }
        }
        public override long Seek(long offset, System.IO.SeekOrigin direction)
        {
            return responseStream.Seek(offset, direction);
        }
        public override void SetLength(long length)
        {
            responseStream.SetLength(length);
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return responseStream.Read(buffer, offset, count);
        }
        #endregion

        #region write method

        //public virtual override void Write(byte[] buffer, int offset, int count)
        //{
        //    string sBuffer = System.Text.UTF8Encoding.UTF8.GetString(buffer, offset, count);

        //    sBuffer = sBuffer.Replace("用户", "<span style=\"color: red;\">用户过滤</span>");

        //    byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(sBuffer);
        //    responseStream.Write(data, 0, data.Length);
        //}
        #endregion
    }

}



//public class MyStream : DefaultStream
//{
//    public MyStream(Stream inputStream)
//        : base(inputStream)
//    {

//    }

//    public override void Write(byte[] buffer, int offset, int count)
//    {
//        throw new NotImplementedException();
//    }
//}