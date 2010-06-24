using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltosTools.Data;

namespace AltosTools.Query
{
    public class Filter
    {
        private string _criteria;

        private ValueTypes _valueTypes;

        private object _value;

        private string _columnName;        

        public Filter() { }

        public Filter(string columnName, string criteria, object value, ValueTypes valueType)
        {
            this._columnName = columnName;
            this._criteria = criteria;
            this._value = value;
            this._valueTypes = valueType;
        }




        public string Criteria
        {
            get { return _criteria; }
            set { _criteria = value; }
        }

        
        public ValueTypes ValueTypes
        {
            get { return _valueTypes; }
            set { _valueTypes = value; }
        }

        
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
	
	
	
	
	
    }
}
