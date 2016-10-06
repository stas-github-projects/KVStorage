using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    internal class Tags
    {
        HashFNV _hash = new HashFNV();        
        Dictionary<ulong, string> dict_tags = new Dictionary<ulong, string>(100);
        List<ulong> lst_tags_to_save = new List<ulong>(100);

        internal ulong add(string tag_name)
        {
            ulong hash = _hash.CreateHash64bit(Encoding.ASCII.GetBytes(tag_name));
            if (dict_tags.ContainsKey(hash) == false)
            { dict_tags.Add(hash, tag_name); lst_tags_to_save.Add(hash); return hash; }
            else
            { return hash; }
        }
        internal string getname(ulong tag_hash)
        {
            if (dict_tags.ContainsKey(tag_hash) == true)
            { return dict_tags[tag_hash]; }
            return "";
        }
        internal byte[] getbytes()
        {
            int i = 0, icount = lst_tags_to_save.Count, ipos = 0, ibuflen = icount * (Globals.storage_tag_max_len + 1 + 8);
            byte[] bout = new byte[ibuflen];

            for (i = 0; i < icount; i++)
            {
                Globals._service.InsertBytes(ref bout, (byte)1, ipos); ipos++; //active
                Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(lst_tags_to_save[i]), ipos); ipos += 8; //hash
                Globals._service.InsertBytes(ref bout, Encoding.ASCII.GetBytes(dict_tags[lst_tags_to_save[i]]), ipos); ipos += Globals.storage_tag_max_len; //colname
            }//for
            //result
            return bout;
        }
    }
}
