using System;
using System.Collections.Generic;

namespace UavObjectGenerator
{
    public enum FieldDataType{
        INT8 = 0,
        INT16,
        INT32,
        UINT8,
        UINT16,
        UINT32,
        FLOAT32,
        ENUM
    };

    public class FieldData
    {   
        // Anything added here should be added as well in the CloneFrom method
        public string Name { get; set; }
        public string TypeString { get; set; }
        public string Units { get; set; }
        public string Elements { get; set; }
        public List<string> ElementNames { get; set; } 
        public List<string> Options { get; set; }
        public List<string> DefaultValues { get; set; }
        public FieldDataType Type { get; set; }

        public int NumElements
        {
            get
            {
                if (ElementNames.Count > 0) return ElementNames.Count;

                if (Elements == null || Elements == "") return 0;

                int result; 
                if (Int32.TryParse(Elements, out result)) return result;

                return 0;
            }
        }

        public bool IsEnum
        {
            get { return this.Type == FieldDataType.ENUM; }
        }

        public FieldData()
        {
            ElementNames = new List<string>();
            Options = new List<string>();
            DefaultValues = new List<string>();
        }

        public void ParseElementNamesFromAttribute(string elementNamesAttribute)
        {
            ParseItemsIntoList(elementNamesAttribute, ElementNames);
        }

        public void ParseOptionsFromAttribute(string optionsAttribute)
        {
            ParseItemsIntoList(optionsAttribute, Options);
        }

        public void ParseDefaultValuesFromAttribute(string defaultValuesAttribute)
        {
            ParseItemsIntoList(defaultValuesAttribute, DefaultValues);
        }

        public void ExpandDefaultValue()
        {
            if (DefaultValues.Count == 1 && NumElements > 1)
            {
                //Only one default value given: apply it to all the elements
                for (int i = 1; i < NumElements; ++i)
                {
                    DefaultValues.Add(DefaultValues[0]);
                }
            }

        }

        public void CloneFrom(FieldData f)
        {
            this.Type = f.Type;
            this.TypeString = f.TypeString;
            this.Units = f.Units;
            this.Elements = f.Elements;
            this.ElementNames = f.ElementNames;
            this.Options = f.Options;
            this.DefaultValues = f.DefaultValues;
        }

        private void ParseItemsIntoList(string items, List<string> target)
        {
            if (items == null || items == "")
                return;

            string[] ss = items.Split(',');

            foreach (string s in ss)
            {
                target.Add(s);
            }
        }

        public static string GetEscapedItemName(string s)
        {
            if (s == null || s == "") return "";

            //string old = s;

            if (s[0] >= '0' && s[0] <= '9') s = '_' + s;

            s = s.Trim();
            s = s.Replace(' ', '_');
            s = s.Replace('+', '_');
            s = s.Replace('.', '_');
            s = s.Replace('(', '_');
            s = s.Replace(')', '_');
            s = s.Replace('-', '_');

            //Console.WriteLine("D: initial: [{0}] processed: [{1}]", old, s);

            return s;
        }
    }
}

