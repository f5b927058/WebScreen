Public Module Module1

    <System.Runtime.InteropServices.DllImport("ole32.dll")> _
    Public Function OleDraw( _
        ByVal pUnk As IntPtr, _
        ByVal dwAspect As Integer, _
        ByVal hdcDraw As IntPtr, _
        ByRef lprcBounds As Rectangle) _
        As Integer
    End Function
    Const DVASPECT_CONTENT As Integer = 1
    Const DVASPECT_THUMBNAIL As Integer = 2
    Const DVASPECT_ICON As Integer = 4
    Const DVASPECT_DOCPRINT As Integer = 8


    Public Sub CaptureWebpage(filepath As String, web_browser As WebBrowser)

        Dim bmp As Bitmap = New Bitmap(web_browser.Width, web_browser.Height)
        Dim g As Graphics = Graphics.FromImage(bmp)
        Dim hdc As IntPtr = g.GetHdc()
        Dim web As IntPtr = System.Runtime.InteropServices.Marshal.GetIUnknownForObject(web_browser.ActiveXInstance)

        Dim rect As Rectangle = New Rectangle(0, 0, bmp.Width, bmp.Height)
        OleDraw(web, DVASPECT_CONTENT, hdc, rect)
        System.Runtime.InteropServices.Marshal.Release(web)
        g.Dispose()

        Const quality As Integer = 100

        Dim eps As New System.Drawing.Imaging.EncoderParameters(1)
        Dim ep As New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality)
        eps.Param(0) = ep

        bmp.Save(filepath, GetEncoderInfo("image/jpeg"), eps)

    End Sub

    Private Function GetEncoderInfo(ByVal mineType As String) As System.Drawing.Imaging.ImageCodecInfo

        Dim encs() As System.Drawing.Imaging.ImageCodecInfo = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
        Dim enc As System.Drawing.Imaging.ImageCodecInfo
        For Each enc In encs
            If enc.MimeType = mineType Then
                Return enc
            End If
        Next

        Return Nothing

    End Function

End Module
