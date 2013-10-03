using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using ceresLib;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace mmDoc
	{
	class mmDoc
		{
		#region struct
		struct guiObject
			{
			int num;
			string type;
			string img;
			}
		#endregion
		static void Main(string[] args)
			{
	    /*
			Process[] proc = Process.GetProcessesByName("calc");
			ceresLib.ceresLib.ClickOnPoint(proc[0].MainWindowHandle, new Point(0,0));

			Bitmap aImage = ceresLib.ceresLib.Capture("calc");
			Bitmap bImage = ceresLib.ceresLib.Capture("calc");
			Console.WriteLine(ceresLib.ceresLib.ImageCompareString(aImage, bImage));
	    */
			Console.WriteLine(guiFunctions.getFortune("me"));
			Console.ReadKey();
			}
		}
	}
