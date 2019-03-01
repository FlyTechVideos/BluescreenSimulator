
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct DEVMODE1 
{
	[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)] public string dmDeviceName;
	public short  dmSpecVersion;
	public short  dmDriverVersion;
	public short  dmSize;
	public short  dmDriverExtra;
	public int    dmFields;

	public short dmOrientation;
	public short dmPaperSize;
	public short dmPaperLength;
	public short dmPaperWidth;

	public short dmScale;
	public short dmCopies;
	public short dmDefaultSource;
	public short dmPrintQuality;
	public short dmColor;
	public short dmDuplex;
	public short dmYResolution;
	public short dmTTOption;
	public short dmCollate;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmFormName;
	public short dmLogPixels;
	public short dmBitsPerPel;
	public int   dmPelsWidth;
	public int   dmPelsHeight;

	public int   dmDisplayFlags;
	public int   dmDisplayFrequency;

	public int   dmICMMethod;
	public int   dmICMIntent;
	public int   dmMediaType;
	public int   dmDitherType;
	public int   dmReserved1;
	public int   dmReserved2;

	public int   dmPanningWidth;
	public int   dmPanningHeight;
};



class User_32
{
	[DllImport("user32.dll")]
	public static extern int EnumDisplaySettings (string deviceName, int modeNum, ref DEVMODE1 devMode );         
	[DllImport("user32.dll")]
	public static extern int ChangeDisplaySettings(ref DEVMODE1 devMode, int flags);

	public const int ENUM_CURRENT_SETTINGS = -1;
	public const int CDS_UPDATEREGISTRY = 0x01;
	public const int CDS_TEST = 0x02;
	public const int DISP_CHANGE_SUCCESSFUL = 0;
	public const int DISP_CHANGE_RESTART = 1;
	public const int DISP_CHANGE_FAILED = -1;
}


namespace Resolution
{
	public static class CResolution
    {
        private static readonly Rectangle OriginalBounds = Screen.PrimaryScreen.Bounds;

        public static void ResetResolution()
        {
            ChangeResolution(OriginalBounds.Width, OriginalBounds.Height);
        }
        public static void ChangeResolution(int width, int height)
        {
            var screen = Screen.PrimaryScreen;

            var iWidth = width;
            var iHeight = height;


            var dm = new DEVMODE1 {dmDeviceName = new string(new char[32]), dmFormName = new string(new char[32])};
            dm.dmSize = (short) Marshal.SizeOf(dm);

            if (0 != User_32.EnumDisplaySettings(null, User_32.ENUM_CURRENT_SETTINGS, ref dm))
            {
                dm.dmPelsWidth = iWidth;
                dm.dmPelsHeight = iHeight;

                var iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_TEST);

                if (iRet == User_32.DISP_CHANGE_FAILED)
                {
                    MessageBox.Show("Unable to process your request");
                    MessageBox.Show("Description: Unable To Process Your Request. Sorry For This Inconvenience.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_UPDATEREGISTRY);

                    switch (iRet)
                    {
                        case User_32.DISP_CHANGE_SUCCESSFUL:
                        {
                            break;

                            //successfull change
                        }
                        case User_32.DISP_CHANGE_RESTART:
                        {
                            MessageBox.Show(
                                "Description: You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode.",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                            //windows 9x series you have to restart... in .net framework 4.5, sure.
                        }
                        default:
                        {
                            throw new SystemException("Couldn't change resolution.");
                            //failed to change
                        }
                    }
                }
            }
        }
    }
}
