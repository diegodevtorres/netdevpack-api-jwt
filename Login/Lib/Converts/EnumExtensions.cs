using System;
using System.ComponentModel;
using System.Reflection;

namespace Login.Lib.Converts
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Retorna o valor informado no atributo Description do enum
        /// </summary>
        public static string GetDescription(this Enum ennum)
        {
            string output = null;
            Type type = ennum.GetType();

            FieldInfo fi = type.GetField(ennum.ToString());
            DescriptionAttribute[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attrs.Length > 0)
                output = attrs[0].Description;

            return output;
        }

        /// <summary>
        /// Retorna o valor informado no atributo StringValue do enum
        /// </summary>
        public static string GetValue(this Enum enumm)
        {
            string output = null;
            Type type = enumm.GetType();

            FieldInfo fi = type.GetField(enumm.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];

            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        public static T ParseEnum<T>(string value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(StringValue)) as StringValue;
                if (attribute != null)
                {
                    if (attribute.Value == value)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            //return default(T);
        }


        /// <summary>
        /// Retorna o valor informado no atributo StringValue do enum
        /// </summary>
        public static Int16 GetValueInt(this Enum enumm)
        {
            Int16 output = 0;
            Type type = enumm.GetType();

            FieldInfo fi = type.GetField(enumm.ToString());

            if (fi != null)
            {
                IntValue[] attrs = fi.GetCustomAttributes(typeof(IntValue), false) as IntValue[];


                if (attrs.Length > 0)
                {
                    output = attrs[0].Value;
                }
            }

            return output;
        }

        public static short GetValueShort(this Enum enumm)
        {
            Int16 output = 0;
            Type type = enumm.GetType();

            FieldInfo fi = type.GetField(enumm.ToString());

            if (fi != null)
            {
                IntValue[] attrs = fi.GetCustomAttributes(typeof(IntValue), false) as IntValue[];


                if (attrs.Length > 0)
                {
                    output = attrs[0].Value;
                }
            }

            return output;
        }


        public static T ParseEnum<T>(Int16 value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(IntValue)) as IntValue;
                if (attribute != null)
                {
                    if (attribute.Value == value)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}
