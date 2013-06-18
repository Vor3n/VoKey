using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
namespace Thisiswhytheinternetexists {
	public static class HashStatics {
		public static String GenerateKey(object sourceObject){
	        String hashString;
	
	        //Catch unuseful parameter values
	        if (sourceObject == null)
	        {
	            UnityEngine.Debug.Log("Null as parameter is not allowed");
				return null;
	        }
	        else
	        {
	            //We determine if the passed object is really serializable.
	            try
	            {
	                //Now we begin to do the real work.
	                hashString = "";//GetMD5HashFromFile(ObjectToByteArray(sourceObject));
	                return hashString;
	            }
	            catch (AmbiguousMatchException ame)
	            {
	                UnityEngine.Debug.Log("Could not definitely decide if object is serializable. Message:" + ame.Message);
	            }
				
	        }
			return null;
	    }
			
		public static string GetMD5HashFromFile(byte[] bytes)
		{
		  MemoryStream ms = new MemoryStream(bytes);
		  MD5 md5 = new MD5CryptoServiceProvider();
		  byte[] retVal = md5.ComputeHash(ms);
		  ms.Close();
		 
		  StringBuilder sb = new StringBuilder();
		  for (int i = 0; i < retVal.Length; i++)
		  {
		    sb.Append(retVal[i].ToString("x2"));
		  }
		  return sb.ToString();
		}
	}
}
