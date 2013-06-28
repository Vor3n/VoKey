using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using GuiTest;

namespace VokeySharedEntities
{
	public static class MySerializerOfItems
	{
		private static string GetXml<T>(T item){
			var serializer = new XmlSerializer(item.GetType());
			string utf8;
	        using (StringWriter writer = new VokeyAssetBundle.Utf8StringWriter())
	        {
	            serializer.Serialize(writer, item);
	            utf8 = writer.ToString();
	        }
			return utf8;
		}
		
		/// <summary>
		/// Forms an item of the specified type form it's serialized form.
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="arg1">Arg1.</param>
		/// <param name="xml">Xml.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T FromXml<T>(string xml)
		{
			return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml));
		}
		
		/// <summary>
		/// Serializes a Room to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="roomToSerialize">Room to serialize.</param>
		public static string ToXml(this Room roomToSerialize){
			return GetXml (roomToSerialize);
		}
		
		/// <summary>
		/// Serializes a house to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="houseToSerialize">House to serialize.</param>
		public static string ToXml(this House houseToSerialize){
			return GetXml (houseToSerialize);
		}
		
		/// <summary>
		/// Serializes a VoKey Asset Bundle to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="vokeyAssetBundleToSerialize">Vokey asset bundle to serialize.</param>
		public static string ToXml(this VokeyAssetBundle vokeyAssetBundleToSerialize){
			return GetXml (vokeyAssetBundleToSerialize);
		}
		
		/// <summary>
		/// Serializes a Street to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="streetToSerialize">Street to serialize.</param>
		public static string ToXml(this Street streetToSerialize){
			return GetXml (streetToSerialize);
		}
		
		/// <summary>
		/// Serializes a Town to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="townToSerialize">Town to serialize.</param>
		public static string ToXml(this Town townToSerialize){
			return GetXml (townToSerialize);
		}
		
		/// <summary>
		/// Serializes a User to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="userToSerialize">User to serialize.</param>
		public static string ToXml(this User userToSerialize){
			return GetXml (userToSerialize);
		}
		
		/// <summary>
		/// Serializes a findable object to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="findableObjectToSerialize">Findable object to serialize.</param>
		public static string ToXml(this FindableObject findableObjectToSerialize)
		{
			return GetXml (findableObjectToSerialize);
		}
		
		/// <summary>
		/// Serializes an Assignment to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="assignmentToSerialize">Assignment to serialize.</param>
		public static string ToXml(this Assignment assignmentToSerialize)
		{
			return GetXml (assignmentToSerialize);
		}
		
		/// <summary>
		/// Serializes an Assignment List to XML
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="assignmentListToSerialize">Assignment list to serialize.</param>
		public static string ToXml(this AssignmentList assignmentListToSerialize){
			return GetXml (assignmentListToSerialize);
		}
	}
}

