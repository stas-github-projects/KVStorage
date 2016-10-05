﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

//using System.Data;
using System.Reflection.Emit;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections;



namespace KVStorage
{
    public class Engine
    {
        //globals
        static DataTypeSerializer _datatype = new DataTypeSerializer();
        HashFNV _hash = new HashFNV();
        Tags _tags = new Tags();
        List<KVDocument> lst_docs_to_save = new List<KVDocument>();

        //
        //C.R.U.D.
        //

        public bool set(Document document)
        {
            bool bool_ret = false;
            Task<KVDocument> task_doc = _add_async(document);
            task_doc.Wait();
            //get result
            if (task_doc.Result != null)
            { lst_docs_to_save.Add(task_doc.Result); bool_ret = true; }
            //result
            return bool_ret;
        }
        //async creation
        private async Task<KVDocument> _add_async(Document document)
        {
            int i = 0;
            bool bool_has_dict = false;
            //Dictionary<string, object> document_dictionary;

            if (document != null)
            {
                Type _type = document.GetType();
                FieldInfo[] fields = _type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                byte[] temp_bytes;
                FieldInfo fieldInfo;
                for (i = 0; i < fields.Length; i++) //go thru all fields
                {
                    fieldInfo = fields[i];
                    string dict_name = fieldInfo.Name;
                    if (dict_name == "dict") { bool_has_dict = true; break; }
                }

                //if it's not a true Document class - exit
                if (bool_has_dict == false) { return null; }

                //FieldInfo f = fields[i];
                Dictionary<string, object> document_dictionary = (Dictionary<string, object>)fields[i].GetValue(document);

                KVDocument _doc = new KVDocument();
                foreach (var _fieldInfo in document_dictionary)//document.dict) //go thru all fields
                {
                    //if (fieldInfo.Name.Length < _globals.storage_tags_name_length) //get info if field's name not more than it's possible max value
                    {
                        if (_fieldInfo.Key.Length > 0)
                        {
                            _doc.tag_hash.Add(_tags.add(_fieldInfo.Key)); //get/set tags
                            _doc.tag_data_pos.Add(0);
                            _doc.tag_data_type.Add(_datatype.returnTypeAndRawByteArray(_fieldInfo.Value, out temp_bytes));
                            _doc.tag_data_len.Add(temp_bytes.Length);
                            _doc.tag_data.Add(temp_bytes);//data byte array
                        }
                    }
                }//foreach
                return await Task.FromResult(_doc);
            }
            return null;
        }

        //retrieve new document
        public List<Document> get(params KeyValuePair<string, string>[] conditions)
        {
            List<Document> lst_out = new List<Document>(10);


            for (int i = 0; i < lst_docs_to_save.Count; i++)
            {
                Document _doc = new Document();
                KVDocument _kvdoc = lst_docs_to_save[i]; //create new KVDocument
                List<ulong> lst_hashes = _kvdoc.tag_hash.GetRange(0, lst_docs_to_save[i].tag_hash.Count); //gt all hashes out of document
                for (int j = 0; j < lst_hashes.Count; j++) //fill up output document with information
                {
                    dynamic _value = _datatype.returnObjectFromByteArray(_kvdoc.tag_data[j], _kvdoc.tag_data_type[j]);
                    _doc.Add(_tags.getname(lst_hashes[j]), _value);
                }//for hashes
                lst_out.Add(_doc); //add document to output list

            }//for documents

            return lst_out;
        }

        public bool commit()
        {
            bool bool_ret = false;

            return bool_ret;
        }

    }


    //SERVICE CLASS

    public class Document : IEnumerable
    {
        private Dictionary<string, object> dict = new Dictionary<string, object>(10);
        public Document() { }
        public Document(params KeyValuePair<string, object>[] field)// string field,object value)
        {
            for (int i = 0; i < field.Length; i++)
            {
                if (dict.ContainsKey(field[i].Key) == false)
                { dict.Add(field[i].Key, field[i].Value); }
            }
        }
        public void Add(string key, object value)
        {
            dict.Add(key, value);
        }
        public string GetKey(string key)
        {
            if (dict.ContainsKey(key)) { return key; }
            else { return ""; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dict).GetEnumerator();
            // or throw a NotImplementedException if you prefer
        }
    }

    internal class KVDocument
    {
        internal List<ulong> tag_hash = new List<ulong>();
        internal List<byte> tag_data_type = new List<byte>();
        internal List<long> tag_data_pos = new List<long>();
        internal List<int> tag_data_len = new List<int>();
        internal List<byte[]> tag_data = new List<byte[]>();
    }


    public static class Fields
    {
        public static KeyValuePair<string, string> New(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }
    }


    /* async creation ~ user custom class
     //USAGE 
     //get
            for (int i = 0; i < 100000; i++)
            {
                //kvstorage.set(new test_document { name = "test" + i.ToString(), id = i, time = DateTime.Now.Ticks, fff = new List<string>() { "dwer" + i } });
                kvstorage.set(new test_document { fff = new List<string> { "uq", "w" }, id = i, name = "test" + i.ToString(), time = DateTime.Now.Ticks });
            }

       //create new document
        public bool set(object document)
        {
            bool bool_ret = false;
            Task<KVDocument> task_doc = _add_async2(document);
            task_doc.Wait();
            //get result
            if (task_doc.Result != null)
            { lst_docs_to_save.Add(task_doc.Result); bool_ret = true; }
            //result
            return bool_ret;
        }

    private async Task<KVDocument> _add_async2(object document)//, ref Tags _tags, ref DataTypeSerializer _datatype)
    {
        if (document != null)
        {
            Type _type = document.GetType();
            FieldInfo[] fields = _type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            byte[] temp_bytes;

            KVDocument _doc = new KVDocument();
            foreach (var fieldInfo in fields) //go thru all fields
            {
                //if (fieldInfo.Name.Length < _globals.storage_tags_name_length) //get info if field's name not more than it's possible max value
                {
                    if (fieldInfo.Name.Length > 0)
                    {
                        _doc.tag_hash.Add(_tags.add(fieldInfo.Name)); //get/set tags
                        _doc.tag_data_pos.Add(0);
                        _doc.tag_data_type.Add(_datatype.returnTypeAndRawByteArray(fieldInfo.GetValue(document), out temp_bytes));
                        _doc.tag_data_len.Add(temp_bytes.Length);
                        _doc.tag_data.Add(temp_bytes);//data byte array
                    }
                }
            }//foreach
            return await Task.FromResult(_doc);
        }
        return null;
    }
    */

    /*DYNAMIC CLASS CREATION
     var fields = new List<Field>() { 
    new Field("name", typeof(string)),
    new Field("time", typeof(long)),
    new Field("id", typeof(long)) 
};
            //Field nfiled = new Field( "eee", typeof(int));
            //var fields = new List<Field>() { new Field() { { "eee", typeof(int) } } };
            dynamic obj = new DynamicClass(fields);

            //MyTypeBuilder.CreateNewObject("test_document2", fields);

            //auto-generate c# code on the fly
            //DataTable dt = new DataTable();
            //var v = dt.Compute("3 * (2+4)", ""); 
    /**/

    /*custom class creation at runtime
    public static class Fields
    {
        public static KeyValuePair<string, object> New(string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }
    }

    class test_document2
    {
        public string name;
        public long time;
        public long id;
    }

    public class Field
    {
        public string FieldName;
        public Type FieldType;
        public Field(string FieldName, Type FieldType)
        { this.FieldName = FieldName; this.FieldType = FieldType; }
    }
    public class DynamicClass : DynamicObject
    {
        private Dictionary<string, KeyValuePair<Type, object>> _fields;
        
        public DynamicClass(List<Field> fields)
        {
            _fields = new Dictionary<string, KeyValuePair<Type, object>>();
            fields.ForEach(x => _fields.Add(x.FieldName,
                new KeyValuePair<Type, object>(x.FieldType, null)));

        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_fields.ContainsKey(binder.Name))
            {
                var type = _fields[binder.Name].Key;
                if (value.GetType() == type)
                {
                    _fields[binder.Name] = new KeyValuePair<Type, object>(type, value);
                    return true;
                }
                else throw new Exception(string.Format("Value {0} is not of type {1}",
                    value, type.Name));
            }

            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _fields[binder.Name].Value;

            return true;
        }
    }

    public static class MyTypeBuilder
    {
        static string typeSignature = "";
        public static void CreateNewObject(string class_name, List<Field> yourListOfFields)
        {
            typeSignature = class_name;
            var myType = CompileResultType(yourListOfFields);
            var myObject = Activator.CreateInstance(myType);

            object instance = Activator.CreateInstance(myType);

            string property = "time";
            object value = 123;
            PropertyInfo prop = myType.GetProperty(property);
            // Set the value of the given property on the given instance
            prop.SetValue(instance, value, null);
            object ggg = (test_document2)instance;

            int i = 0;
        }
        public static Type CompileResultType(List<Field> yourListOfFields)
        {
            TypeBuilder tb = GetTypeBuilder();
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
            foreach (var field in yourListOfFields)
                CreateProperty(tb, field.FieldName, field.FieldType);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder()
        {
            //var typeSignature = "MyDynamicType";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
    /**/
}