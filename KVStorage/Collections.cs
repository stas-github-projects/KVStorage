using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    internal class Collections
    {
        HashFNV _hash = new HashFNV();
        Dictionary<ulong, string> dict_collections = new Dictionary<ulong, string>(100);
        List<ulong> lst_cols_to_save = new List<ulong>(100);

        internal ulong add(string collection_name)
        {
            ulong hash = _hash.CreateHash64bit(Encoding.ASCII.GetBytes(collection_name));
            if (dict_collections.ContainsKey(hash) == false)
            { dict_collections.Add(hash, collection_name); lst_cols_to_save.Add(hash); return hash; }
            else
            { return hash; }
        }
        internal string getname(ulong collection_hash)
        {
            if (dict_collections.ContainsKey(collection_hash) == true)
            { return dict_collections[collection_hash]; }
            return "";
        }
        internal byte[] getbytes()
        {
            int i = 0, icount = lst_cols_to_save.Count, ipos = 0, ibuflen = icount * (Globals.storage_col_max_len + 1 + 8);
            byte[] bout = new byte[ibuflen];

            for (i = 0; i < icount; i++)
            {
                Globals._service.InsertBytes(ref bout, (byte)1, ipos); ipos++; //active
                Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(lst_cols_to_save[i]), ipos); ipos += 8; //hash
                Globals._service.InsertBytes(ref bout, Encoding.ASCII.GetBytes(dict_collections[lst_cols_to_save[i]]), ipos); ipos += Globals.storage_col_max_len; //colname
            }//for
            //result
            return bout;
        }
    }
}
