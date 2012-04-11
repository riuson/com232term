using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace com232term.Classes
{
    public class EnumTypeConverter : EnumConverter
    {
        private Type mEnumType;

        public EnumTypeConverter(Type type)
            : base(type)
        {
            this.mEnumType = type;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            FieldInfo fi = this.mEnumType.GetField(Enum.GetName(this.mEnumType, value));
            DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

            if (da != null)
                return da.Description;
            else
                return value.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
        {
            return srcType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            foreach (FieldInfo fi in mEnumType.GetFields())
            {
                DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                if ((da != null) && (value.ToString() == da.Description))
                    return Enum.Parse(this.mEnumType, fi.Name);
            }

            return Enum.Parse(this.mEnumType, value.ToString());
        }

    }
}
