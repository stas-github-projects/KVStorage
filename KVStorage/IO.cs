using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    internal class IO
    {
        FileStream fstream_cols;

        internal long get_stream_length()//IO_PARAM param)
        {
            if (fstream_cols != null) { return fstream_cols.Length; } else { return 0; }
        }

        internal bool init()//string filename)
        {
            bool bool_ret = true;

            //create dir & files
            try
            {
                FileInfo fcols = new FileInfo(Globals.storage_name);

                if (fcols.Exists == false)
                {
                    fstream_cols = new FileStream(Globals.storage_name, FileMode.Create, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);
                    this.write(createheader()); //write default empty header
                }
                else
                {
                    fstream_cols = new FileStream(Globals.storage_name, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);
                }

            }
            catch (Exception) //on error return false
            { return false; }

            return bool_ret;
        }

        internal byte[] createheader()
        {
            int ipos = 0;
            byte[] bout = new byte[Globals.storage_global_header];

            Globals._service.InsertBytes(ref bout, Encoding.ASCII.GetBytes(Globals.storage_version), ipos); ipos += 4; //version
            Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(Globals.storage_document_id), ipos); ipos += 4; //document id
            Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(Globals.storage_col_max_len), ipos); ipos++; //max collection size
            Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(Globals.storage_tag_max_len), ipos); ipos++; //max tag size
            Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(Globals.storage_cols_per_page), ipos); ipos += 2; //collections per page
            Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(Globals.storage_tags_per_page), ipos); ipos += 2; //tags per page
            Globals._service.InsertBytes(ref bout, BitConverter.GetBytes(Globals.storage_indexes_per_page), ipos); ipos += 2; //indexes per page

            //+ 18*3 - pages headers
            return bout;
        }

        internal bool storageisopen()
        {
            if (fstream_cols != null) { return true; }
            return false;
        }

        internal void parseparams(params string[] parameters)
        {

        }

        internal bool write(byte[] barray)
        {
            return write(ref barray);
        }
        internal bool write(ref byte[] barray)
        {
            bool bool_ret = false;

            if (fstream_cols != null)
            {
                fstream_cols.Position = fstream_cols.Length;
                fstream_cols.Write(barray, 0, barray.Length);
                fstream_cols.Position = fstream_cols.Length;
                bool_ret = true;
            }

            return bool_ret;
        }

        internal void finalize()
        {
            if (fstream_cols != null) { fstream_cols.Close(); fstream_cols = null; }
        }

    }
}
