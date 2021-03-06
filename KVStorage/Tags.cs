﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    internal class Tags
    {
        //HashFNV _hash = new HashFNV();        
        Dictionary<ulong, string> dict_tags = new Dictionary<ulong, string>(100); //tag_hash + tag_name
        Dictionary<long, List<ulong>> dict_docs_pos = new Dictionary<long, List<ulong>>(100); //pos_of_doc + list<tag_hashes>
        List<ulong> lst_tags_to_save = new List<ulong>(100); //list of tag_hases to save
        List<long> lst_docs_to_save = new List<long>(100); //list of doc_pos to save
        int i_tag_indexes_length = 0;
        //List<ulong> lst_tag_indexes_to_save = new List<ulong>(100);


        internal ulong add(string tag_name, long pos_of_doc)
        {
            ulong hash = Globals._hash.CreateHash64bit(Encoding.ASCII.GetBytes(tag_name));
            /*if (dict_tags.ContainsKey(hash) == false)
            { dict_tags.Add(hash, tag_name); lst_tags_to_save.Add(hash); dict_tags_pos.Add(hash, new List<long> { pos_of_doc }); i_tag_indexes_length += 8; return hash; }
            else
            { dict_tags_pos[hash].Add(pos_of_doc); i_tag_indexes_length += (8 +8+ 1); return hash; }*/
            if (dict_tags.ContainsKey(hash) == false)
            { dict_tags.Add(hash, tag_name); lst_tags_to_save.Add(hash); }
            //else
            //{ dict_tags[hash].Add(tag_name); }

            //doc to save
            if (dict_docs_pos.ContainsKey(pos_of_doc) == false)
            {
                dict_docs_pos.Add(pos_of_doc, new List<ulong> { hash });
                lst_docs_to_save.Add(pos_of_doc); i_tag_indexes_length += (8 + 8 + 1); //pos_of_doc + (tag_active + tag_hash)
            }
            else
            { dict_docs_pos[pos_of_doc].Add(hash); i_tag_indexes_length += (8 + 1); } //(tag_active + tag_hash)


            return hash;
        }
        internal string getname(ulong tag_hash)
        {
            if (dict_tags.ContainsKey(tag_hash) == true)
            { return dict_tags[tag_hash]; }
            return "";
        }
        internal byte[] get_tags_bytes()
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
        /*internal byte[] get_tagindexes_bytes()
        {
            int i = 0, index = 0, icount = lst_tags_to_save.Count, ipos = 0, ibuflen = i_tag_indexes_length; //icount * (8 + 1 + 8);
            byte[] bout = new byte[ibuflen];

            for (i = 0; i < icount; i++)
            {
                for (index = 0; index < dict_tags_pos[lst_tags_to_save[i]].Count; index++)
                {
                    Globals._service.InsertBytes(ref bout, (byte)1, ipos); ipos++; //active
                    Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(lst_tags_to_save[i]), ipos); ipos += 8; //tag hash
                    Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(dict_tags_pos[lst_tags_to_save[i]][index]), ipos); ipos += 8; //pos of document
                }
            }//for
            //result
            return bout;
        }*/
        internal byte[] get_tagindexes_bytes()
        {
            int i = 0, index = 0, ilen = 0, icount = lst_docs_to_save.Count, ipos = 0, ibuflen = i_tag_indexes_length;
            long l_doc = 0;
            ulong ul_hash = 0;
            byte[] bout = new byte[ibuflen];

            try
            {
                for (i = 0; i < icount; i++)
                {
                    l_doc = lst_docs_to_save[i];//[index];
                    Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(l_doc), ipos); ipos += 8; //insert pos_of_doc
                    for (index = 0; index < dict_docs_pos[lst_docs_to_save[i]].Count; index++)
                    {
                        Globals._service.InsertBytes(ref bout, (byte)1, ipos); ipos++; //active hash
                        ul_hash = dict_docs_pos[lst_docs_to_save[i]][index];
                        Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(ul_hash), ipos); ipos += 8; //tag hash
                        //Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(dict_tags_pos[lst_docs_to_save[i]][index]), ipos); ipos += 8; //
                    }
                }//for
            }
            catch (Exception)
            { return new byte[0];}
            //result
            return bout;
        }
        internal void flush()
        {
            dict_tags.Clear();
            dict_docs_pos.Clear();
            lst_tags_to_save.Clear();
            i_tag_indexes_length = 0;
            //lst_tag_indexes_to_save.Clear();
        }
    }
}
