using CidWorkerService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		[DllImport("cid.dll", EntryPoint = "CidData", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
		public static extern string CidData();

		[DllImport("cid.dll", EntryPoint = "CidStart")]
		public static extern string CidStart();
		public Form1()
		{
			InitializeComponent();
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

		private void Form1_Load(object sender, EventArgs e)
		{
			CidStart();
		}
	}
}
