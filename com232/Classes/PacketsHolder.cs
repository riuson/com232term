using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace com232term.Classes
{
    public class PacketsHolder
    {
        public BindingList<String> Packets { get; set; }
        public BindingList<String> PacketsStatic { get; set; }

        private PacketsHolder()
        {
            this.Packets = new BindingList<string>();
            this.PacketsStatic = new BindingList<string>();
        }

        public static string FileName
        {
            get
            {
                return Path.Combine(
                    Path.GetDirectoryName(Application.ExecutablePath),
                    "packets.xml");
            }
        }

        public static PacketsHolder Load()
        {
            PacketsHolder opts = new PacketsHolder();
            try
            {
                if (File.Exists(PacketsHolder.FileName))
                {
                    using (FileStream fs = new FileStream(PacketsHolder.FileName, FileMode.Open))
                    {
                        using (XmlReader xr = new XmlTextReader(fs))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(PacketsHolder));
                            if (ser.CanDeserialize(xr))
                            {
                                opts = (PacketsHolder)ser.Deserialize(xr);
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (XmlException)
            {
            }
            if (opts == null)
                opts = new PacketsHolder();
            return opts;
        }

        public void Save()
        {
            if (this != null)
            {
                if (!Directory.Exists(Path.GetDirectoryName(PacketsHolder.FileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(PacketsHolder.FileName));
                }

                string backup = Path.ChangeExtension(PacketsHolder.FileName, "back");
                FileInfo fi = new FileInfo(PacketsHolder.FileName);
                if (fi.Exists)
                {
                    if (File.Exists(backup))
                        File.Delete(backup);
                    fi.MoveTo(backup);
                }
                TextWriter fs = null;
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(PacketsHolder));
                    fs = new StreamWriter(PacketsHolder.FileName);
                    ser.Serialize(fs, this);
                    fs.Close();
                    fs = null;
                }
                catch// restore previous file
                {
                    if (fi != null && fi.Exists)
                        fi.MoveTo(PacketsHolder.FileName);
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
        }
    }
}
