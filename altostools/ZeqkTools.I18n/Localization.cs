using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;

namespace ZeqkTools.I18n
{
    public class Localization
    {
        Hashtable resources;

        public Localization(string resourceFile)
        {
            resources = new Hashtable();
            using (XmlTextReader xtr = new XmlTextReader(resourceFile))
            {
                while (xtr.Read())
                {
                    resources.Add(xtr["key"], xtr["value"]);
                }
            }
        }

        public string getString(string key, params object[] args)
        {
            string rv = key;
            object value = resources[key];
            if (value != null)
                rv = string.Format(value.ToString(), args);

            return rv;
           
        }

    }
}
