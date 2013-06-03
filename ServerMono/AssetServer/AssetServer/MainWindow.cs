using System;
using Gtk;
using System.ComponentModel;

namespace Vokey {
public partial class MainWindow: Gtk.Window
{	
	AssetServer ws;
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		ws = new AssetServer();
		ws.LogMessage += HandleWsLogMessage;
	}

	void HandleWsLogMessage (string obj)
	{
		logTextBox.Buffer.Text += obj + Environment.NewLine;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButton2Clicked (object sender, System.EventArgs e)
	{
		ws.Start();
		
	}

	protected void OnScanForAssetsButtonClicked (object sender, System.EventArgs e)
	{
		ws.scanForAssets();
	}
}
}