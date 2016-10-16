using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    
    internal class Pages
    {
        internal byte[] makepage(ref List<KVDocument> lst_docs)
        {
            int i = 0, index = 0, ilen = 0, icount = lst_docs.Count, ipos = 0;
            byte[] b_out = new byte[0];

            //analyze params
            if (Globals.PagesParams.bool_update_existing_page == true) //start from update existing
            { }

            //fill up page
            for (i = 0; i < icount; i++) //go thru all docs
            {
                for (index = Globals.PagesParams.current_freecell; index < Globals.PagesParams.max_freecells; index++) //fill up
                {
                    //lst_docs[i].
                }//for
            }//for

            return b_out;
        }
    }

}
