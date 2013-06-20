using System;
using System.Net;
using System.Text;

namespace VokeySharedEntities
{
	public static class HttpListenerContextExtension
	{
		public static void returnXmlStringToClient(this HttpListenerContext hlc, string xml)
		{
			byte[] returnBytes;
			try {
				returnBytes = Encoding.UTF8.GetBytes (xml);
			} catch (Exception e) {
				Console.WriteLine (e.GetBaseException());
				returnBytes = Encoding.UTF8.GetBytes ("No data in response");
				return;
			}
			hlc.Response.ContentLength64 = returnBytes.Length;
			hlc.Response.OutputStream.Write (returnBytes, 0, returnBytes.Length);
		}
	}
}

