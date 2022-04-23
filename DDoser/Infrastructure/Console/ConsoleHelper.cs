using DDoser.Infrastructure.IP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DDoser.Infrastructure.ConsoleInfrastructure
{
    public class ConsoleHelper
    {
        public static string ReadResponse(string text, ConsoleColor consoleColor = ConsoleColor.White)
        {
            WriteColored(text, consoleColor);
            return Console.ReadLine();
        }

        public static void WriteColoredLine(string text, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void WriteColored(string text, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void ConsoleInit()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            SetCurrentFont("Consolas", 20);
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("                                             ");
            Console.WriteLine("  DDDD       DDDD          OO       SSSSS    ");
            Console.WriteLine("  DD  DD     DD  DD      OO  OO    SS   SS   ");
            Console.WriteLine("  DD    DD   DD    DD   OO    OO   SS        ");
            Console.WriteLine("  DD    DD   DD    DD   OO    OO     SS      ");
            Console.WriteLine("  DD    DD   DD    DD   OO    OO        SS   ");
            Console.WriteLine("  DD  DD     DD  DD      OO  OO    SS    SS  ");
            Console.WriteLine("  DDDD       DDDD          OO       SSSSSS   ");
            Console.WriteLine("                                             ");
            Console.WriteLine("---------------------------------------------");

            var country = IPHelper.GetMyCountryByIp();
            Console.WriteLine($"\nYou are in {country}\n");
        }

        #region Implementation
        private const int FixedWidthTrueType = 54;
        private const int StandardOutputHandle = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
            public string FontName;
        }

        private static void SetCurrentFont(string font, short fontSize = 0)
        {
            FontInfo before = new()
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {

                FontInfo set = new()
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    Console.WriteLine("Set error " + ex);
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                FontInfo after = new()
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);
            }
        }
        #endregion
    }
}
