using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
	}
}

