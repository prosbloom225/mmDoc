using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ceresLib;
using System.Xml;

using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace mmDoc
	{
	class guiFunctions
		{
		#region struct
		public struct guiObject
			{
			public int num;
			public string type;
			public string img;
			}
		#endregion
		public static int getFortune(string player)
			{

	    // temp
			Process[] proc = Process.GetProcessesByName("calc");
			ceresLib.ceresLib.ClickOnPoint(proc[0].MainWindowHandle, new Point(0, 0));
			Thread.Sleep(500);

			// Screen scrape
			Bitmap currScrape = ceresLib.ceresLib.Capture(Properties.Settings.Default.processName);
			// crop screenScrape
			Bitmap croppedImage = ceresLib.ceresLib.Crop(currScrape, Properties.Settings.Default.incomePoint, Properties.Settings.Default.incomeSize);
			// Populate comparisons
			List<object> comparisonList = ceresLib.ceresLib.SqlSelect("SELECT * from gui WHERE type='income'", Properties.Settings.Default.connString);
			foreach (List<object> s in comparisonList)
				{
				// Match
				if (ceresLib.ceresLib.ImageCompareArray(croppedImage, new Bitmap(Directory.GetCurrentDirectory() + "\\img\\" + Convert.ToString(s[2]))))
					{
					return Convert.ToInt32(s[0]);
					}
				}
			// We couldnt get a match, returning -1
			return -1;
			}

		}
	}
