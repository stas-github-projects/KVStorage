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
        FileStream fstream_tags;
        FileStream fstream_data;
        FileStream fstream_log;


        internal bool init()//string filename)
        {
            bool bool_ret = true, bool_new_dir = false;
            
            if (Globals.storage_dir.Length == 0) { return false; }

            //DirectoryInfo dirinfo = new DirectoryInfo(Globals.storage_dir);

            //create dir & files
            try
            {
                //check whether dir is exists
                DirectoryInfo dirinfo = new DirectoryInfo(Globals.storage_dir);
                if (dirinfo.Exists == false)
                { dirinfo.Create(); bool_new_dir = true; }

                //make new path
                if (Globals.storage_cols.Length <= Globals.storage_dir.Length + 1 || (Globals.storage_cols.Substring(0, Globals.storage_dir.Length + 1) != (Globals.storage_dir) + @"\"))
                {
                    Globals.storage_cols = Globals.storage_dir + @"\" + Globals.storage_cols;
                    Globals.storage_tags = Globals.storage_dir + @"\" + Globals.storage_tags;
                    Globals.storage_data = Globals.storage_dir + @"\" + Globals.storage_data;
                    Globals.storage_log = Globals.storage_dir + @"\" + Globals.storage_log;
                }

                if (bool_new_dir == true) //if dir has just created
                {
                    fstream_cols = new FileStream(Globals.storage_cols, FileMode.Create, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);
                    fstream_tags = new FileStream(Globals.storage_tags, FileMode.Create, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);
                    fstream_data = new FileStream(Globals.storage_data, FileMode.Create, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);
                    fstream_log = new FileStream(Globals.storage_log, FileMode.Create, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);

                }
                else //open
                {
                    FileInfo fcols = new FileInfo(Globals.storage_cols);
                    FileInfo ftags = new FileInfo(Globals.storage_tags);
                    FileInfo fdata = new FileInfo(Globals.storage_data);
                    FileInfo flog = new FileInfo(Globals.storage_log);

                    fstream_cols = new FileStream(Globals.storage_cols, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);
                    fstream_tags = new FileStream(Globals.storage_tags, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);
                    fstream_data = new FileStream(Globals.storage_data, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);
                    fstream_log = new FileStream(Globals.storage_log, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Globals.storage_read_write_buffer);

                }
            }
            catch (Exception) //on error return false
            { return false; }

            return bool_ret;
        }

        internal bool storageisopen()
        {
            if (fstream_cols != null && fstream_tags != null && fstream_data != null && fstream_log != null)
            { return true; }
            return false;
        }

        internal void parseparams(params string[] parameters)
        {

        }

        internal bool write(ref byte[] barray, IO_PARAM param)
        {
            bool bool_ret = false;

            if (param == IO_PARAM.COLS_STREAM) //save cols
            {
                if(fstream_cols!=null)
                {
                    fstream_cols.Position = fstream_cols.Length;
                    fstream_cols.Write(barray, 0, barray.Length);
                    fstream_cols.Position = fstream_cols.Length;
                    bool_ret = true;
                }
            }
            else if (param == IO_PARAM.TAGS_STREAM) //save tags
            {
                if (fstream_tags != null)
                {
                    fstream_tags.Position = fstream_tags.Length;
                    fstream_tags.Write(barray, 0, barray.Length);
                    fstream_tags.Position = fstream_tags.Length;
                    bool_ret = true;
                }
            }
            else if (param == IO_PARAM.DATA_STREAM) //save data
            { }
            else if (param == IO_PARAM.LOG_STREAM) //save log
            { }

            return bool_ret;
        }

        internal void finalize()
        {
            if (fstream_cols != null) { fstream_cols.Close(); fstream_cols = null; }
            if (fstream_tags != null) { fstream_tags.Close(); fstream_tags = null; }
            if (fstream_data != null) { fstream_data.Close(); fstream_data = null; }
            if (fstream_log != null) { fstream_log.Close(); fstream_log = null; }
        }

        internal enum IO_PARAM
        { COLS_STREAM = 0, TAGS_STREAM = 1, DATA_STREAM = 2, LOG_STREAM = 4 };

    }
}
