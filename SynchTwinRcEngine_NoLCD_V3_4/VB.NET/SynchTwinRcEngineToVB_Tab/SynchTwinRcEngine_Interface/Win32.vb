Imports System.Runtime.InteropServices

Friend Class Win32

#Region " Raster constants "

    Public Const SRCCOPY As Int32 = &HCC0020

#End Region

#Region " GDI32 "

    <DllImport("gdi32.dll", CharSet:=CharSet.Auto, SetLastError:=True, ExactSpelling:=True)> _
    Public Shared Function BitBlt( _
        ByVal hdcDest As IntPtr, ByVal nXDest As Integer, ByVal nYDest As Integer, _
        ByVal nWidth As Integer, ByVal nHeight As Integer, _
        ByVal hdcSrc As IntPtr, ByVal nXSrc As Integer, ByVal nYSrc As Integer, _
        ByVal dwRop As System.Int32) As Boolean
    End Function

#End Region

End Class
