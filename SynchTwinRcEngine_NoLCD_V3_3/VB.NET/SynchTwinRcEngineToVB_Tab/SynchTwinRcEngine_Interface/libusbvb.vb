Imports LibUsbDotNet
Imports LibUsbDotNet.Main
Imports System.Runtime.InteropServices

Class usbasp
    Const vid_usbasp = &H16C0 '5824
    Const pid_usbasp = &H5DC '1500
    Const pid_avrboot = &H5A1 '1441
    Dim usbon As Integer = 0
    Dim usbbooton As Integer = 0

    Public usbhandle As UsbDevice
    Public usbaspfinder As New UsbDeviceFinder(vid_usbasp, pid_usbasp)
    Public avrusbbootfinder As New UsbDeviceFinder(vid_usbasp, pid_avrboot)
    private whole_usb As IUsbDevice
    Public Function _check_if_dev_available(ByVal devfinder As UsbDeviceFinder) As Boolean
        usbhandle = UsbDevice.OpenUsbDevice(devfinder)
        If (usbhandle Is Nothing) Then
            Try
                usbhandle.Close()
                UsbDevice.Exit()
            Catch
            End Try
            Return False
        Else
            Try
                usbhandle.Close()
                UsbDevice.Exit()
            Catch
            End Try
            Return True
        End If
    End Function

    Public Function _check_whole_device(ByVal devfinder As UsbDeviceFinder, ByRef whusb As IUsbDevice) As Integer
        usbhandle = UsbDevice.OpenUsbDevice(devfinder)
        If (usbhandle Is Nothing) Then
            Try
                usbhandle.Close()
                UsbDevice.Exit()
            Catch
            End Try
            If ReferenceEquals(devfinder, usbaspfinder) Then
                usbon = -1
            ElseIf ReferenceEquals(devfinder, avrusbbootfinder) Then
                usbbooton = -1
            End If
            Return -1
        Else
            whusb = TryCast(usbhandle, IUsbDevice)
            If Not ReferenceEquals(whusb, Nothing) Then
                If ReferenceEquals(devfinder, usbaspfinder) Then
                    usbon = 1
                ElseIf ReferenceEquals(devfinder, avrusbbootfinder) Then
                    usbbooton = 1
                End If
                Return 1
            Else
                If ReferenceEquals(devfinder, usbaspfinder) Then
                    usbon = 0
                ElseIf ReferenceEquals(devfinder, avrusbbootfinder) Then
                    usbbooton = 0
                End If
                Return 0
            End If
        End If
    End Function
    Public Sub _set_usb_settings(ByVal devfinder As UsbDeviceFinder)
        If _check_whole_device(devfinder, whole_usb) = 1 Then
            whole_usb.SetConfiguration(1)
            whole_usb.ClaimInterface(0)
        End If

    End Sub
    Public Sub _usb_release()
        Try
            If usbhandle.IsOpen Then
                whole_usb.ReleaseInterface(0)
                Try
                    usbhandle.Close()
                    UsbDevice.Exit()
                Catch
                End Try
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function _check_avrboot_pagesize() As String
        Dim strint As String = ""
        Dim tr As Integer
        Dim usb_pck As UsbSetupPacket
        Dim usb_dt As Byte() = New Byte() {0, 0}
        Dim ret_Dtx As Byte() = New Byte() {0, 0}
        Dim ptr As IntPtr = Marshal.AllocHGlobal(1)
        Marshal.Copy(usb_dt, 0, ptr, 2)
        _set_usb_settings(avrusbbootfinder)
        If usbbooton = 1 Then
            usb_pck = New UsbSetupPacket(&HC0, 3, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)

            Marshal.Copy(ptr, ret_Dtx, 0, 2)
            strint = ret_Dtx(0) & " " & ret_Dtx(1)
            _usb_release()
        End If
        Return strint
    End Function

    Public Sub _check_HVPP(ByVal cmd As Byte)
        Dim strint As String = ""
        Dim tr As Integer = 1
        Dim usb_pck As UsbSetupPacket
        Dim usb_dt As Byte() = New Byte() {0, 0}
        Dim ret_Dtx As Byte() = New Byte() {0, 0}
        Dim ptr As IntPtr = Marshal.AllocHGlobal(1)
        Marshal.Copy(usb_dt, 0, ptr, 1)
        _set_usb_settings(usbaspfinder)
        If usbon = 1 Then
            Try
                Threading.Thread.Sleep(10)
                usb_pck = New UsbSetupPacket(&HC0, 90, 1, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Marshal.Copy(ptr, ret_Dtx, 0, 2)
                strint = ret_Dtx(0) & " " & ret_Dtx(1)
                Threading.Thread.Sleep(10)
                'usb_pck = New UsbSetupPacket(&HC0, 80, 1, 0, 1)
                'usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                'Marshal.Copy(ptr, ret_Dtx, 0, 1)
                'strint &= " " & ret_Dtx(0)
                MsgBox(strint)
                _usb_release()
            Catch ex As Exception
            End Try
        End If
    End Sub
    Public Sub _check_IO()
        Dim strint As String = ""
        Dim tr As Integer = 1
        Dim usb_pck As UsbSetupPacket
        Dim usb_dt As Byte() = New Byte() {0}
        Dim ret_Dtx As Byte() = New Byte() {0}
        Dim ptr As IntPtr = Marshal.AllocHGlobal(1)
        Marshal.Copy(usb_dt, 0, ptr, 1)
        _set_usb_settings(usbaspfinder)
        If usbon = 1 Then
            usb_pck = New UsbSetupPacket(&HC0, 90, &HAAAA, &HAAAA, 4)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Marshal.Copy(ptr, ret_Dtx, 0, 1)
            Threading.Thread.Sleep(10)
            _usb_release()
            MsgBox(ret_Dtx(0))
        End If

    End Sub
    Public Function _check_usbasp_ext() As String
        Dim strint As String = ""
        Dim tr As Integer
        Dim usb_pck As UsbSetupPacket
        Dim usb_dt As Byte() = New Byte() {1}
        Dim ret_Dtx As Byte() = New Byte() {1}
        Dim ptr As IntPtr = Marshal.AllocHGlobal(1)
        Marshal.Copy(usb_dt, 0, ptr, 1)
        _set_usb_settings(usbaspfinder)
        If usbon = 1 Then
            usb_pck = New UsbSetupPacket(&HC0, 10, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Marshal.Copy(ptr, ret_Dtx, 0, 1)
            strint = ret_Dtx(0)
            _usb_release()
        End If
        Marshal.FreeHGlobal(ptr)
        Return strint
    End Function
  
    Public Sub check_multibyte()
        Dim strint As String = ""
        Dim tr As Integer
        Dim usb_pck As UsbSetupPacket

        Dim usb_dt As Byte() = New Byte() {0, 0, 0, 0}
        Dim ret_Dtx As Byte() = New Byte() {0, 0, 0, 0}
        Dim ptr As IntPtr = Marshal.AllocHGlobal(1)
        Marshal.Copy(usb_dt, 0, ptr, 4)
        _set_usb_settings(usbaspfinder)
        If usbon = 1 Then
            usb_pck = New UsbSetupPacket(&HC0, 40, &HAABB, &HCC, 4)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Marshal.Copy(ptr, ret_Dtx, 0, 4)

            Try
                _usb_release()
            Catch ex As Exception
            End Try
            MsgBox(ret_Dtx(0) & " " & ret_Dtx(1) & " " & ret_Dtx(2) & " " & ret_Dtx(3))
            Marshal.FreeHGlobal(ptr)
        End If
    End Sub
    Public Function check_sig_rd() As String
        Threading.Thread.Sleep(100)
        Dim strint As String = ""
        Dim tr As Integer
        Dim usb_pck As UsbSetupPacket
        Dim sig_str As String = ""
        Dim usb_dt As Byte() = New Byte() {0, 0, 0, 0}
        Dim ret_Dtx As Byte() = New Byte() {0, 0, 0, 0}
        Dim ptr As IntPtr = Marshal.AllocHGlobal(1)
        Marshal.Copy(usb_dt, 0, ptr, 4)
        Try
            _usb_release()
        Catch ex As Exception
        End Try
        _set_usb_settings(usbaspfinder)
        If usbon = 1 Then
            Try
                'read byte 0
                Dim avrdude As New libavrdude
                usb_pck = New UsbSetupPacket(&H40 + &H0 + &H80, 1, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                usb_pck = New UsbSetupPacket(&HC0, 5, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                usb_pck = New UsbSetupPacket(&HC0, 3, &H2A30, &H2A00, 4)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                Marshal.Copy(ptr, ret_Dtx, 0, 4)
                sig_str = "0x" & avrdude.dectohex(ret_Dtx(3), 1)
                usb_pck = New UsbSetupPacket(&H40 + &H0 + &H80, 2, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                'read byte 1
                usb_pck = New UsbSetupPacket(&H40 + &H0 + &H80, 1, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                usb_pck = New UsbSetupPacket(&HC0, 5, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                usb_pck = New UsbSetupPacket(&HC0, 3, &H2A30, &H2A01, 4)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                Marshal.Copy(ptr, ret_Dtx, 0, 4)
                sig_str &= avrdude.dectohex(ret_Dtx(3), 1)
                usb_pck = New UsbSetupPacket(&H40 + &H0 + &H80, 2, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                'read byte 2
                usb_pck = New UsbSetupPacket(&H40 + &H0 + &H80, 1, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                usb_pck = New UsbSetupPacket(&HC0, 5, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                usb_pck = New UsbSetupPacket(&HC0, 3, &H2A30, &H2A02, 4)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                Marshal.Copy(ptr, ret_Dtx, 0, 4)
                sig_str &= avrdude.dectohex(ret_Dtx(3), 1)
                usb_pck = New UsbSetupPacket(&H40 + &H0 + &H80, 2, 0, 0, 1)
                usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
                Threading.Thread.Sleep(1)
                Try
                    _usb_release()
                Catch ex As Exception
                End Try
                Return sig_str
                Marshal.FreeHGlobal(ptr)
            Catch ex As Exception
                Return "0"
            End Try
        End If
        Return "1"
    End Function
    Public Sub check_i2c_rw()
        Dim strint As String = ""
        Dim tr As Integer = 0
        Dim usb_pck As UsbSetupPacket
        Dim usb_dt As Byte() = New Byte() {0}
        Dim ret_Dtx As Byte() = New Byte() {0}
        Dim ptr As IntPtr = Marshal.AllocHGlobal(1)
        Marshal.Copy(usb_dt, 0, ptr, 1)
        _set_usb_settings(usbaspfinder)
        If usbon = 1 Then
            usb_pck = New UsbSetupPacket(&H40 + &H0 + &H80, 23, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(10)
            usb_pck = New UsbSetupPacket(&HC0, 25, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(10)
            usb_pck = New UsbSetupPacket(&HC0, 28, 160, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 28, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 28, 50, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 28, 456, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 28, 456, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 26, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            Marshal.Copy(ptr, ret_Dtx, 0, 1)
            strint = ret_Dtx(0)
            Try
                _usb_release()
            Catch ex As Exception
            End Try
        End If
        MsgBox(strint)
        Marshal.FreeHGlobal(ptr)
    End Sub
    Public Sub check_i2c_rd()
        Dim strint As String = ""
        Dim tr As Integer
        Dim usb_pck As UsbSetupPacket
        Dim usb_dt As Byte() = New Byte() {0}
        Dim ret_Dtx As Byte() = New Byte() {0}
        Dim ptr As IntPtr = Marshal.AllocHGlobal(1)
        Marshal.Copy(usb_dt, 0, ptr, 1)
        _set_usb_settings(usbaspfinder)
        If usbon = 1 Then
            usb_pck = New UsbSetupPacket(&H40 + &H0 + &H80, 23, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(10)
            usb_pck = New UsbSetupPacket(&HC0, 25, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(10)
            usb_pck = New UsbSetupPacket(&HC0, 28, 160, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 28, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 28, 50, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 25, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 28, 161, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            usb_pck = New UsbSetupPacket(&HC0, 27, 1, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            Marshal.Copy(ptr, ret_Dtx, 0, 1)
            strint = strint & " " & ret_Dtx(0)
            usb_pck = New UsbSetupPacket(&HC0, 27, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            Marshal.Copy(ptr, ret_Dtx, 0, 1)
            strint = strint & " " & ret_Dtx(0)
            usb_pck = New UsbSetupPacket(&HC0, 26, 0, 0, 0)
            usbhandle.ControlTransfer(usb_pck, ptr, usb_dt.Length, tr)
            Threading.Thread.Sleep(100)
            Try
                _usb_release()
            Catch ex As Exception
            End Try
        End If
        MsgBox(strint)
        Marshal.FreeHGlobal(ptr)
    End Sub
End Class
