using System;

namespace Login.Lib.Converts
{
    public class StringValue : Attribute
    {
        public string Value { get; set; }

        public StringValue(string _value)
        {
            Value = _value;
        }
    }
}
