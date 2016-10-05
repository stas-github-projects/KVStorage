using System;
using System.Collections.Generic;
using System.Linq;
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
            if (dict_tags.ContainsKey(hash)==false)
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
    }
}
