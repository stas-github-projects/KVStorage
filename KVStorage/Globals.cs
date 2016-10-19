using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    internal static class Globals
    {
        internal static Service _service = new Service();
        internal static DataTypeSerializer _datatype = new DataTypeSerializer();
        internal static HashFNV _hash = new HashFNV();
        internal static Collections _cols = new Collections();
        internal static Tags _tags = new Tags();
        internal static IO _io = new IO();

        internal static char[] storage_version = new char[] { 'K', 'V', 'S', '1' };
        internal static int storage_document_id = 0;
        //internal static string storage_dir = "";
        internal static string storage_name = "";

        //system params
        internal static int storage_read_write_buffer = 1024 * 1024;
        internal static long storage_virtual_length = 0;
        internal static ushort storage_global_header = 70;
        
        internal static byte storage_col_max_len = 30;
        internal static ushort storage_cols_per_page = 4;

        internal static byte storage_tag_max_len = 30;
        internal static ushort storage_tags_per_page = 4;

        internal static ushort storage_indexes_per_page = 4;
        
        //page defines
        internal static class PagesParams
        {
            internal static bool bool_update_existing_page; //if 'true' lst_pages[0] is always add to the end of the last existing page
            internal static int pos_in_updating_page;

            //internal static ushort current_freecell;
            //internal static ushort max_freecells;
            internal static long current_file_length;
            //internal static long output_file_length;

            internal static long first_page_pos =0;
            internal static long last_page_pos=0;
            internal static ushort last_page_freecells = 0;

            internal static List<byte[]> lst_pages = new List<byte[]>(10);

            internal static void flush()
            {
                bool_update_existing_page = false; pos_in_updating_page = 0; current_file_length = 0; //output_file_length = 0;
                //current_freecell = 0; max_freecells = 0;
            }
        }
    }

}
