﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace KVStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            KVStorage.Engine kvstorage = new Engine();
            KVStorage.Document _doc = new Document();
            
            //get
            /**/
            for (int i = 0; i < 1000; i++)
            {
                KVStorage.Document _document = new Document() { { "name", "test" + i.ToString() }, { "id", i }, { "time", DateTime.Now.Ticks } };//, {"fff", new List<string> { "dwer" + i } }};
                //_docum.Add("fff", new List<string> { "dwer" + i });
                kvstorage.set(_document);
            }
            /**/
            
            //set
            //var output_document = kvstorage.get();
            kvstorage.get(Fields.New("id", "2"), Fields.New("k", "j"));

            s.Stop();

            Console.WriteLine("\ntimings: {0} sec / {1} msec / {2} ticks", s.Elapsed.Seconds, s.ElapsedMilliseconds, s.ElapsedTicks);
            Console.ReadKey();
        }
    }

    /*
    class test_document
    {
        public string name;
        public long time;
        public long id;
        public List<string> fff;
    }
    */

}