using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    
    internal class Pages
    {
        //create documentation page
        internal byte[] makepages(ref List<KVDocument> lst_docs)
        {
            int i = 0, index = 0, ilen = 0, icount = lst_docs.Count, iposinpage = 0, ipos = 0;
            long l_next_page_pos = 0;
            byte[] b_out = new byte[0];
            string s_tagname = "";

            //analyze params
            //if (Globals.PagesParams.bool_update_existing_page == true) //start from update existing
            //{ }

            //set buffer
            byte[] b_buffer = new byte[0];
            if (Globals.PagesParams.last_page_freecells < Globals.storage_cols_per_page || Globals.PagesParams.last_page_freecells == 0)
            { Globals.PagesParams.bool_update_existing_page = false; }
            else
            { Globals.PagesParams.pos_in_updating_page = 111; } //TO-DO

            //create/update page
            for (i = 0; i < icount; i++) //go thru all docs
            {
                KVDocument _kvdoc = lst_docs[i];
                b_buffer[ipos] = 1; ipos++; //active
                Globals._service.InsertBytes(ref b_buffer, BitConverter.GetBytes(_kvdoc.collection), ipos); ipos += 2; //collection
                Globals._service.InsertBytes(ref b_buffer, BitConverter.GetBytes(_kvdoc.doc_id), ipos); ipos += 4; //doc_id

                for (index = 0; i < _kvdoc.tag_hash.Count; i++) //fill up
                {
                    b_out[ipos] = 1; ipos++; //active
                    b_out[ipos] = _kvdoc.tag_data_type[i]; ipos++; //data type
                    Globals._service.InsertBytes(ref b_buffer, BitConverter.GetBytes(_kvdoc.tag_hash[i]), ipos); ipos += 8; //tag_hash
                    s_tagname = Globals._tags.getname(_kvdoc.tag_hash[i]);
                    Globals._service.InsertBytes(ref b_buffer, Encoding.ASCII.GetBytes(s_tagname), ipos); ipos += Globals.storage_tag_max_len; //tag_name
                    Globals._service.InsertBytes(ref b_buffer, BitConverter.GetBytes(_kvdoc.tag_data_len[i]), ipos); ipos +=8; //tag_len
                    Globals._service.InsertBytes(ref b_buffer, _kvdoc.tag_data[i], ipos); ipos += 8; //tag_pos OR tag_data
                }//for
            }//for

            return b_out;
        }
    }

}
