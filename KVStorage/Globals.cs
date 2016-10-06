using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    internal static class Globals
    {
        internal static Service _service = new Service();

        internal static string storage_dir = "";
        internal static string storage_cols = "cols";
        internal static string storage_tags = "tags";
        internal static string storage_data = "data";
        internal static string storage_log = "log";

        //system params
        internal static int storage_read_write_buffer = 1024 * 1024;

        //user-defined params
        internal static byte storage_cols_in_memory = 0;
        internal static byte storage_tags_in_memory = 0;
        internal static byte storage_col_max_len = 30;
        internal static byte storage_tag_max_len = 30;


    }

}
