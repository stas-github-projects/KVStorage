using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    internal class DataTypeSerializer
    {
        //internal Service _service = Globals._service;

        public byte returnTypeAndRawByteArray(object _data, out byte[] out_bytes)
        {
            return returnTypeAndRawByteArray(ref _data, out out_bytes);
        }
        public byte returnTypeAndRawByteArray(ref object _data, out byte[] out_bytes)
        {
            byte _data_type = new byte();
            _data_type = 0;

            if (_data == null) { out_bytes = new byte[0]; _data_type = 0; return 0; }

            Type _type = _data.GetType();
            if (_type == typeof(bool))// = 3
            {
                out_bytes = BitConverter.GetBytes((bool)_data);
                _data_type = 3;
            }
            else if (_type == typeof(int))// = 4
            {
                out_bytes = BitConverter.GetBytes((int)_data);
                _data_type = 4;
            }
            else if (_type == typeof(long))// = 5
            {
                out_bytes = BitConverter.GetBytes((long)_data);
                _data_type = 5;
            }
            else if (_type == typeof(double))// = 6
            {
                out_bytes = BitConverter.GetBytes((double)_data);
                _data_type = 6;
            }
            else if (_type == typeof(decimal))// = 7
            {
                out_bytes = DecimalToBytes((decimal)_data);
                _data_type = 7;
            }
            else if (_type == typeof(short))// = 8
            {
                out_bytes = BitConverter.GetBytes((short)_data);
                _data_type = 8;
            }
            else if (_type == typeof(float))// = 9
            {
                out_bytes = BitConverter.GetBytes((float)_data);
                _data_type = 9;
            }
            else if (_type == typeof(char))// = 10
            {
                Encoding _enc = Encoding.Unicode;
                out_bytes = BitConverter.GetBytes((char)_data); //Encoding.Default.GetBytes((char)_data);
                _data_type = 10;
            }
            else if (_type == typeof(char[]))// = 11
            {
                //Encoding _enc = Encoding.Default;
                out_bytes = Encoding.Unicode.GetBytes((char[])_data);
                _data_type = 11;
            }
            else if (_type == typeof(string))// = 12
            {
                out_bytes = Encoding.Unicode.GetBytes((string)_data);
                _data_type = 12;
            }
            else if (_type == typeof(byte))// = 13
            {
                out_bytes = new byte[1];
                out_bytes[0] = (byte)_data;
                _data_type = 13;
            }
            else if (_type == typeof(byte[]))// = 14
            {
                out_bytes = (byte[])_data;
                _data_type = 14;
            }
            else if (_type == typeof(ushort))// = 15
            {
                out_bytes = BitConverter.GetBytes((ushort)_data);
                _data_type = 15;
            }
            else if (_type == typeof(uint))// = 16
            {
                out_bytes = BitConverter.GetBytes((uint)_data);
                _data_type = 16;
            }
            else if (_type == typeof(ulong))// = 17
            {
                out_bytes = BitConverter.GetBytes((ulong)_data);
                _data_type = 17;
            }
            //
            //LISTS of all primitives
            //
            else if (_type == typeof(List<string>))
            {
                int i = 0, ilen = 0, icount = 0, ipos = 0;
                //List<int> lst_lengths = new List<int>();
                List<byte[]> lst_bytes = new List<byte[]>();
                List<string> lst_string = (List<string>)_data;
                int[] arr_lengths = new int[lst_string.Count];

                icount = lst_string.Count;
                for (i = 0; i < lst_string.Count; i++) //get length of all elements in array
                { ilen += (4 + lst_string[i].Length); arr_lengths[i] = lst_string[i].Length;}// lst_lengths.Add(lst_string[i].Length); } //4 - length of string inside (int32)

                out_bytes = new byte[ilen];
                for (i = 0; i < lst_string.Count; i++)
                {
                    Globals._service.InsertBytes(ref out_bytes, BitConverter.GetBytes(arr_lengths[i]), ipos); ipos += 4;
                    Globals._service.InsertBytes(ref out_bytes, Encoding.ASCII.GetBytes(lst_string[i]), ipos); ipos += arr_lengths[i];// lst_string[i].Length;
                }
                //return
                _data_type = 18;
            }
            else
            {
                out_bytes = new byte[0];
                _data_type = 0;
            }
            return _data_type;
        }

        public dynamic returnObjectFromByteArray(byte[] b_output, byte _type)
        {
            return returnObjectFromByteArray(ref b_output, _type);
        }
        public dynamic returnObjectFromByteArray(ref byte[] b_output, byte _type)
        {
            switch (_type)
            {
                case 3://bool
                    bool _bool = BitConverter.ToBoolean(b_output, 0);
                    return _bool;
                case 4://int
                    int _int = BitConverter.ToInt32(b_output, 0);
                    return _int;
                case 5://long
                    long _long = BitConverter.ToInt64(b_output, 0);
                    return _long;
                case 6://double
                    double _dbl = BitConverter.ToDouble(b_output, 0);
                    return _dbl;
                case 7://decimal
                    decimal _dec = ByteArrayToDecimal(b_output, 0);//Convert.ToDecimal(BitConverter.ToDouble(b_output, 0));
                    return _dec;
                case 8://short
                    short _short = BitConverter.ToInt16(b_output, 0);
                    return _short;
                case 9://float
                    float _float = BitConverter.ToSingle(b_output, 0);
                    return _float;
                case 10://char
                    char[] _char = Encoding.Unicode.GetChars(b_output);
                    return _char[0];
                case 11://char[]
                    char[] _chararr = Encoding.Unicode.GetChars(b_output);
                    return _chararr;
                case 12://string
                    string _string = Encoding.Unicode.GetString(b_output);
                    return _string;
                case 13://byte
                    return b_output[0];
                case 14://byte[]
                    return b_output;
                case 15://ushort
                    ushort _ushort = (ushort)BitConverter.ToUInt16(b_output, 0);
                    return _ushort;
                case 16://uint
                    uint _uint = (uint)BitConverter.ToUInt32(b_output, 0);
                    return _uint;
                case 17://ulong
                    ulong _ulong = (ulong)BitConverter.ToUInt64(b_output, 0);
                    return _ulong;
                default: return false;
            }//switch
        }

        public byte[] DecimalToBytes(decimal dec)
        {
            //Load four 32 bit integers from the Decimal.GetBits function
            Int32[] bits = decimal.GetBits(dec);
            //Create a temporary list to hold the bytes
            List<byte> bytes = new List<byte>();
            //iterate each 32 bit integer
            foreach (Int32 i in bits)
            {
                //add the bytes of the current 32bit integer
                //to the bytes list
                bytes.AddRange(BitConverter.GetBytes(i));
            }
            //return the bytes list as an array
            return bytes.ToArray();
        }

        public decimal ByteArrayToDecimal(byte[] src, int offset)
        {
            int i1 = BitConverter.ToInt32(src, offset);
            int i2 = BitConverter.ToInt32(src, offset + 4);
            int i3 = BitConverter.ToInt32(src, offset + 8);
            int i4 = BitConverter.ToInt32(src, offset + 12);

            return new decimal(new int[] { i1, i2, i3, i4 });
        }
    }
}
