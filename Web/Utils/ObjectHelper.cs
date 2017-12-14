using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DigitalMarketing.Web.Utils
{
    public static class ObjectHelper
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static T XmlDeserializeFromString<T>(string objectData)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StringReader textReader = new StringReader(objectData))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }
    }
}