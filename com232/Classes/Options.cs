using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.Xml;
using System.IO.Ports;

namespace com232term.Classes
{
    /// <summary>
    /// Класс для сериализации и десериализации классов настроек, и обращения к ним в виде синглетона.
    /// </summary>
    /// <typeparam name="T">Класс для управления</typeparam>
    public class Options
    {
        private static Options mInstance;
        private static object mLock = new object();

        public PortSettings PortOptions { get; set; }

        private Options ()
	    {
            this.PortOptions = new PortSettings();
	    }

        public static string FileName
        {
            get
            {
                return Path.Combine(
                    Path.GetDirectoryName(Application.ExecutablePath),
                    "options.xml");
            }
        }
        
        public static Options Instance
        {
            get
            {
                lock (mLock)
                {
                    if (mInstance == null)
                    {
                        mInstance = Load(FileName);
                    }
                }
                return mInstance;
            }
        }
        
        public static Options Load(string fileName)
        {
            Options opts = new Options();
            try
            {
                if (File.Exists(fileName))
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open))
                    {
                        using (XmlReader xr = new XmlTextReader(fs))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(Options));
                            if (ser.CanDeserialize(xr))
                            {
                                opts = (Options)ser.Deserialize(xr);
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
                opts = new Options();
            return opts;
        }
        
        public static void Save(string fileName)
        {
            lock (mLock)
            {
                Save(mInstance, fileName);
            }
        }
        
        public static void Save()
        {
            Save(FileName);
        }
        
        public static void Save(Options instance, string fileName)
        {
            if (instance != null)
            {
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                
                string backup = Path.ChangeExtension(fileName, "back");
                FileInfo fi = new FileInfo(fileName);
                if (fi.Exists)
                {
                    if (File.Exists(backup))
                        File.Delete(backup);
                    fi.MoveTo(backup);
                }
                TextWriter fs = null;
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Options));
                    fs = new StreamWriter(fileName);
                    ser.Serialize(fs, instance);
                    fs.Close();
                    fs = null;
                }
                catch// restore previous file
                {
                    if (fi != null && fi.Exists)
                        fi.MoveTo(fileName);
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
