using System;
using System.Net;
using System.Threading;

namespace Vokey
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerContext, byte[]> _responderMethod;
		/// <summary>
		/// Occurs a message is logged.
		/// </summary>
		public event Action<string> LogMessage;

        public WebServer(string[] prefixes, Func<HttpListenerContext, byte[]> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            _listener.Start();
        }
		
		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		private void Log(string message){
			if(LogMessage != null)LogMessage(message);
		}
		

        public WebServer(Func<HttpListenerContext, byte[]> method, params string[] prefixes)
            : this(prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                string logMessage = "Webserver running with prefixes:" + Environment.NewLine;
				foreach(string s in _listener.Prefixes){
					logMessage += s + Environment.NewLine;
				}
				
				Log (logMessage);
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
								_responderMethod(ctx);
                            }
                            catch (Exception e) {
								Console.WriteLine (e.GetBaseException ());
							} // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
