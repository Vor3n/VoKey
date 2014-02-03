using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace VokeySharedEntities
{
	public static class MySerializerOfLists
	{
		/// <summary>
		/// Tos the xml.
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="items">Items.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
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
		
		/// <summary>
		/// Deserialises a list of Objects of Type T.
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="xml">Xml.</param>
		/// <typeparam name="T">The deserialised item.</typeparam>
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

