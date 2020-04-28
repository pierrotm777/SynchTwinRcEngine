Imports LibUsbDotNet
Imports LibUsbDotNet.Main
Imports LibUsbDotNet.Info
Imports System.Collections.ObjectModel

Class ShowInfo
    Public Shared MyUsbDevice As UsbDevice

    Public Shared Function CheckUsb() As Boolean
        ' Dump all devices and descriptor information to console output.
        Dim allDevices As UsbRegDeviceList = UsbDevice.AllDevices
        For Each usbRegistry As UsbRegistry In allDevices
            If usbRegistry.Open(MyUsbDevice) Then
                'MessageBox.Show(MyUsbDevice.Info.ToString()) 'Console.WriteLine(MyUsbDevice.Info.ToString())
                'MessageBox.Show(MyUsbDevice.Info.ProductString.ToString()) 'return USBasp
                'MessageBox.Show(MyUsbDevice.IsOpen) 'return true or false
                'check the PID and VID
                If MyUsbDevice.Info.Descriptor.ProductID = 1500 And MyUsbDevice.Info.Descriptor.VendorID = 5824 Then
                    Return True
                    Exit For
                Else
                    Continue For
                End If
                'check Name and status
                'If MyUsbDevice.Info.ProductString.ToString().ToLower = "usbasp" And MyUsbDevice.IsOpen = True Then
                '    Return True
                '    Exit For
                'Else
                '    Continue For
                'End If

                'If MyUsbDevice.Info.Descriptor.ProductID = 1500 And MyUsbDevice.Info.Descriptor.VendorID = 5824 Then
                '    MessageBox.Show(MyUsbDevice.Info.ToString()) 'Console.WriteLine(MyUsbDevice.Info.ToString())
                '    For iConfig As Integer = 0 To MyUsbDevice.Configs.Count - 1
                '        Dim configInfo As UsbConfigInfo = MyUsbDevice.Configs(iConfig)
                '        MessageBox.Show(configInfo.ToString()) 'Console.WriteLine(configInfo.ToString())
                '        Dim interfaceList As ReadOnlyCollection(Of UsbInterfaceInfo) = configInfo.InterfaceInfoList
                '        For iInterface As Integer = 0 To interfaceList.Count - 1
                '            Dim interfaceInfo As UsbInterfaceInfo = interfaceList(iInterface)
                '            MessageBox.Show(interfaceInfo.ToString()) 'Console.WriteLine(interfaceInfo.ToString())

                '            Dim endpointList As ReadOnlyCollection(Of UsbEndpointInfo) = interfaceInfo.EndpointInfoList
                '            For iEndpoint As Integer = 0 To endpointList.Count - 1
                '                MessageBox.Show(endpointList(iEndpoint).ToString()) 'Console.WriteLine(endpointList(iEndpoint).ToString())
                '            Next
                '        Next
                '    Next
                'End If

  

            End If
        Next


        ' Free usb resources.
        ' This is necessary for libusb-1.0 and Linux compatibility.
        UsbDevice.Exit()

        Return False
        ' Wait for user input..
        'Console.ReadKey()
    End Function

End Class
