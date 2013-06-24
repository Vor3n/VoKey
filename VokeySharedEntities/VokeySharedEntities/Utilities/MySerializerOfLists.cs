using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace VokeySharedEntities
{
	public static class MySerializerOfLists
	{
		public static string ToXml<T>(this List<T> items){
			var serializer = new XmlSerializer(items.GetType());
			string utf8;
	        using (StringWriter writer = new VokeyAssetBundle.Utf8StringWriter())
	        {
	            serializer.Serialize(writer, items);
	            utf8 = writer.ToString();
	        }
			return utf8;
		}
		
		public static List<T> FromXml<T>(string xml){
			/*string rootElementName = "";
			using (XmlReader reader = XmlReader.Create(new StringReader(xml))) {
			    while (reader.Read()) {
			      // first element is the root element
			      if (reader.NodeType == XmlNodeType.Element) {
			        rootElementName = reader.Name;
			        break;
			      }
			    }
			  }*/
			var serializer = new XmlSerializer(typeof(List<T>));
			List<T> deserializedList;
	        using (StringReader reader = new StringReader(xml))
	        {
	            deserializedList = (List<T>)serializer.Deserialize(reader);
	        }
			return deserializedList;
		}
	}
}

