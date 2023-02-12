using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CidWorkerService
{
	public partial class Service1 : ServiceBase
	{
		[DllImport("cid.dll", EntryPoint = "CidData", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
		public static extern string CidData();

		[DllImport("cid.dll", EntryPoint = "CidStart")]
		public static extern string CidStart();

		public Service1(string[] args)
		{
			InitializeComponent();

		}

		protected override void OnStart(string[] args)
		{
			cid();
		}

		private void cid()
		{
			string temp = "";
			string serial = "";
			string line = "";
			string number = "";
			string dateTime = "";
			// 
			temp = CidData();
			if ((temp != ""))
			{
				string[] words = temp.Split(',');
				serial = words[0];
				line = words[1];
				number = words[2];
				dateTime = words[3];

				string html = string.Empty;
				string url = @"http://127.0.0.1:45455/api/delivery/cidC812A?phone=";
				url += number;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.AutomaticDecompression = DecompressionMethods.GZip;

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					html = reader.ReadToEnd();
				}

				Console.WriteLine(html);
			}
		}


		protected override void OnStop()
		{
		}

		internal void TestStartupAndStop(string[] args)
		{

			this.OnStart(args);
			Console.ReadLine();
			this.OnStop();
		}
	}

	public class ConstCharPtrMarshaler : ICustomMarshaler
	{
		public object MarshalNativeToManaged(IntPtr pNativeData)
		{
			return Marshal.PtrToStringAnsi(pNativeData);
		}

		public IntPtr MarshalManagedToNative(object ManagedObj)
		{
			return IntPtr.Zero;
		}

		public void CleanUpNativeData(IntPtr pNativeData)
		{
		}

		public void CleanUpManagedData(object ManagedObj)
		{
		}

		public int GetNativeDataSize()
		{
			return IntPtr.Size;
		}

		static readonly ConstCharPtrMarshaler instance = new ConstCharPtrMarshaler();

		public static ICustomMarshaler GetInstance(string cookie)
		{
			return instance;
		}
	}
}
