using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    public class Service
    {
        public void InsertBytes(ref byte[] _src, byte[] _what, int _pos = 0, int _length = 0)
        {
            InsertBytes(ref _src, ref _what, _pos, _length);
        }
        public void InsertBytes(ref byte[] _src, byte _what, int _pos = 0, int _length = 0)
        {
            int i = _src.Length;
            if (_pos < 0) { _pos = 0; }
            if (_length == 0) { _length = 1; }// _what.Length; }
            if (_pos + _length > i) { return; }//out of dimensions
            //Buffer.BlockCopy(_what, 0, _src, _pos, _length);
            _src[_pos] = _what;
        }
        public void InsertBytes(ref byte[] _src, ref byte[] _what, int _pos = 0, int _length = 0)
        {
            int i = _src.Length;
            if (_pos < 0) { _pos = 0; }
            if (_length == 0) { _length = _what.Length; }
            if (_pos + _length > i) { return; }//out of dimensions
            Buffer.BlockCopy(_what, 0, _src, _pos, _length);
        }
        public byte[] GetBytes(byte[] _src, int _pos = 0, int _length = 0)
        {
            byte[] b_out;
            int ilen = _src.Length;
            if (_length < 1) { _length = ilen; }
            if (_pos < 0) { _pos = 0; }
            if (ilen >= (_pos + _length))
            {
                b_out = new byte[_length];
                Buffer.BlockCopy(_src, _pos, b_out, 0, _length);//copy piece
                return b_out;
            }
            else
            { return _src; }
        }
        public byte GetByte(byte[] _src, int _pos = 0)
        {
            byte b_out;
            int ilen = _src.Length, _length = 1;
            if (_length < 1) { _length = ilen; }
            if (_pos < 0) { _pos = 0; }
            if (ilen >= (_pos + _length))
            {
                b_out = _src[_pos];// Buffer.BlockCopy(_src, _pos, b_out, 0, _length);//copy piece
                return b_out;
            }
            else
            { return 0; }
        }

        public string GetStringWONulls(byte[] _in_bytes)
        {
            return GetStringWONulls(Encoding.ASCII.GetString(_in_bytes));
        }
        public string GetStringWONulls(string _in)
        {
            string _out = "";
            int istart = _in.IndexOf('\0');
            if (istart > -1)
            { _out = _in.Substring(0, istart); return _out; }
            else
            { return _in; }
        }
    }

}
