using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Globalization;

namespace com232term.Classes.Options
{
    /// <summary>
    /// Workaround to serialize Color
    /// </summary>
    public class ColorSerialized : IXmlSerializable
    {
        private Color mColor;

        public ColorSerialized ()
        {
            this.mColor = Color.Black;
        }

        public ColorSerialized (Color value)
	    {
            this.mColor = value;
	    }

        public static implicit operator Color(ColorSerialized value)
        {
            return value.mColor;
        }
        public static implicit operator ColorSerialized(Color value)
        {
            return new ColorSerialized(value);
        }

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void ValidationCallback(object sender, System.Xml.Schema.ValidationEventArgs args)
        {
        }

        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            string value = String.Format("{0:X8}", this.mColor.ToArgb());
            writer.WriteString(value);
        }

        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();
            try
            {
                int a;
                if (Int32.TryParse(reader.ReadString(), System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out a))
                    this.mColor = Color.FromArgb(a);
                else
                    this.mColor = Color.Black;
            }
            catch
            {
                this.mColor = Color.Black;
            }
            reader.ReadEndElement();
        }

        #endregion // IXmlSerializable Members
    }
}
