using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FunctionV1
{
    public static class Helper
    {
        public static class XmlObj<T>
        {
            static StringBuilder sbData = new StringBuilder();
            static StringWriter swWriter;
            static XmlDocument xDoc;
            static XmlNodeReader xNodeReader;
            static XmlSerializer xmlSerializer;

            /// <summary>
            /// Object to xml string.
            /// </summary>
            public static string SerializeData(T data)
            {
                XmlSerializer employeeSerializer = new XmlSerializer(typeof(T));
                swWriter = new StringWriter(sbData);
                employeeSerializer.Serialize(swWriter, data);
                return sbData.ToString();
            }

            /// <summary>
            /// Xml to Object
            /// </summary>
            public static T DeserializeData(string dataXML)
            {
                xDoc = new XmlDocument();
                xDoc.LoadXml(dataXML);
                xNodeReader = new XmlNodeReader(xDoc.DocumentElement);
                xmlSerializer = new XmlSerializer(typeof(T));
                var employeeData = xmlSerializer.Deserialize(xNodeReader);
                T deserializedEmployee = (T)employeeData;
                return deserializedEmployee;
            }

            /// <summary>
            /// Xml string to XmlDocument
            /// </summary>
            public static XmlDocument SerializeToXmlDoc<T>(T source)
            {
                var document = new XmlDocument();
                var navigator = document.CreateNavigator();

                using (var writer = navigator.AppendChild())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, source);
                }
                return document;
            }
        }
    }
}
