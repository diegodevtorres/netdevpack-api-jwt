using System;

namespace Login.Lib.Converts
{
    public class IntValue : Attribute
    {
        public Int16 Value { get; set; }

        public IntValue(Int16 _value)
        {
            Value = _value;
        }
    }

}
