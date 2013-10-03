using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SQLite;
using System.Collections;

namespace ceresLib
	{
	public class ceresLib
		{
		#region PInvoke
		[DllImport("user32.dll")]
		static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

		[DllImport("user32.dll")]
		static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);

		public struct RECT
			{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
			}
		#endregion

		#region Lib
		public static bool ImageCompareArray(Bitmap firstImage, Bitmap secondImage)
			{
			// Array method is pixel by pixel, should never be a misfire
			bool flag = true;
			string firstPixel;
			string secondPixel;
			if (firstImage.Width == secondImage.Width && firstImage.Height == secondImage.Height)
				{
				for (int i = 0; i < firstImage.Width; i++)
					{
					for (int j = 0; j < firstImage.Height; j++)
						{
						firstPixel = firstImage.GetPixel(i, j).ToString();
						secondPixel = secondImage.GetPixel(i, j).ToString();
						if (firstPixel != secondPixel)
							{
							flag = false;
							break;
							}

						}
					}
				return flag;
				}
			else
				return false;
			}
		public static bool ImageCompareString(Bitmap firstImage, Bitmap secondImage)
			{
			// String method is faster, though not quite as accurate
			MemoryStream ms = new MemoryStream();
			firstImage.Save(ms, ImageFormat.Png);
			firstImage.Save("c://one.png", ImageFormat.Png);
			string firstBitmap = Convert.ToBase64String(ms.ToArray());
			ms.Position = 0;

			secondImage.Save(ms, ImageFormat.Png);
			string secondBitmap = Convert.ToBase64String(ms.ToArray());
			secondImage.Save("c://two.png", ImageFormat.Png);

			if (firstBitmap.Equals(secondBitmap))
				return true;
			else
				return false;
			}
		public static void ClickOnPoint(IntPtr wndHandle, Point clientPoint)
			{
			var oldPos = Cursor.Position;

			/// get screen coordinates
			ClientToScreen(wndHandle, ref clientPoint);

			/// set cursor on coords, and press mouse
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);
			mouse_event(0x00000002, 0, 0, 0, UIntPtr.Zero); /// left mouse button down
			mouse_event(0x00000004, 0, 0, 0, UIntPtr.Zero); /// left mouse button up

			/// return mouse 
			Cursor.Position = oldPos;
			}
		public static Bitmap Capture(string processName)
			{
			Process[] aProc = Process.GetProcessesByName(processName);
			if (SetForegroundWindow(aProc[0].MainWindowHandle))
				{
				RECT srcRect;
				if (GetWindowRect(aProc[0].MainWindowHandle, out srcRect))
					{
					int width = srcRect.Right - srcRect.Left;
					int height = srcRect.Bottom - srcRect.Top;
					Bitmap bmp = new Bitmap(width, height);
					Graphics g = Graphics.FromImage(bmp);
					g.CopyFromScreen(srcRect.Left, srcRect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
					return bmp;
					}
				}
			return null;


			}
		public static Bitmap Crop(Bitmap image, Point pnt, Size sze)
			{

			Rectangle cropArea = new Rectangle(pnt, sze);
			Bitmap bmpImage = new Bitmap(image);
			Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
			return (Bitmap)(bmpCrop);
			}
		public static List<object> SqlSelect(string sql, string connString)
			{
			using (var conn = new SQLiteConnection(connString))
			using (var cmd = conn.CreateCommand())
				{
				conn.Open();
				cmd.CommandText = sql;
				List<object> ret = new List<object>();
				using (var reader = cmd.ExecuteReader())
					{
					while (reader.Read())
						{
						List<object> currObj = new List<object>();
						currObj.Add(reader.GetInt32(reader.GetOrdinal("num")));
						currObj.Add(reader.GetString(reader.GetOrdinal("type")));
						currObj.Add(reader.GetString(reader.GetOrdinal("img")));
						ret.Add(currObj);
						}
					}
				return ret;
				}
			}
		#endregion


		}
	}
