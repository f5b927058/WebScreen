Public Class Form1

    Private m_WebBrowserLeftTop As Point

    Private m_History As New HistoryClass
    Private m_NavigatedByHistory As Boolean = False
    Private m_AddressbarEventEnabled As Boolean = True

    ' -------------------------------------------------------------
    '   [Form] KeyDown
    ' -------------------------------------------------------------
    Private Sub Form1_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        Select Case e.KeyCode

            Case Keys.F5
                ' F5キー
                Me.UpdateProc()

        End Select

    End Sub

    ' -------------------------------------------------------------
    '   [Form] Load
    ' -------------------------------------------------------------
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        m_WebBrowserLeftTop = Me.WebBrowser1.Location

        Me.Navigate("http://www.google.com/")

        Me.numWidth.Maximum = 10000
        Me.numHeight.Maximum = 10000
        Me.numWidth.Value = 1000
        Me.numHeight.Value = 800

        Me.Form1_Resize(Nothing, Nothing)

    End Sub

    ' -------------------------------------------------------------
    '   [NumericUpDown] Width - ValueChanged
    ' -------------------------------------------------------------
    Private Sub numWidth_ValueChanged(sender As System.Object, e As System.EventArgs) Handles numWidth.ValueChanged

        Me.WebBrowser1.Size = New Size(Me.numWidth.Value, Me.numHeight.Value)
        ScrollChanged_W()

    End Sub

    ' -------------------------------------------------------------
    '   [NumericUpDown] Height - ValueChanged
    ' -------------------------------------------------------------
    Private Sub numHeight_ValueChanged(sender As System.Object, e As System.EventArgs) Handles numHeight.ValueChanged

        Me.WebBrowser1.Size = New Size(Me.numWidth.Value, Me.numHeight.Value)
        ScrollChanged_H()

    End Sub

    ' -------------------------------------------------------------
    '   [Scroll] 縦 - Scroll
    ' -------------------------------------------------------------
    Private Sub VScrollBar1_Scroll(sender As System.Object, e As System.Windows.Forms.ScrollEventArgs) Handles VScrollBar1.Scroll

        ScrollChanged_H()

    End Sub

    ' -------------------------------------------------------------
    '   [Scroll] 横 - Scroll
    ' -------------------------------------------------------------
    Private Sub HScrollBar1_Scroll(sender As System.Object, e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar1.Scroll

        Me.ScrollChanged_W()

    End Sub

    ' -------------------------------------------------------------
    '   [Form] Resize
    ' -------------------------------------------------------------
    Private Sub Form1_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize

        ' 縦
        If (Me.numHeight.Value <= Me.VScrollBar1.Height) Then
            Me.VScrollBar1.Enabled = False
        Else
            Me.VScrollBar1.Enabled = True
            Me.VScrollBar1.Maximum = Me.numHeight.Value
            Me.VScrollBar1.LargeChange = Me.VScrollBar1.Height
        End If

        ' 横
        If (Me.numWidth.Value <= Me.HScrollBar1.Width) Then
            Me.HScrollBar1.Enabled = False
        Else
            Me.HScrollBar1.Enabled = True
            Me.HScrollBar1.Maximum = Me.numWidth.Value
            Me.HScrollBar1.LargeChange = Me.HScrollBar1.Width
        End If

    End Sub

    ' -------------------------------------------------------------
    '   [ComboBox] Url - KeyDown
    ' -------------------------------------------------------------
    Private Sub cmbUrl_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cmbUrl.KeyDown

        Select Case e.KeyCode

            Case Keys.Enter
                ' Enterキー
                If m_NavigatedByHistory Then
                    Exit Sub
                End If

                If m_AddressbarEventEnabled Then
                    m_AddressbarEventEnabled = False
                    Me.Navigate(Me.cmbUrl.Text)
                End If

            Case Keys.F5
                ' F5キー
                Me.UpdateProc()

        End Select

    End Sub

    ' -------------------------------------------------------------
    '   [ComboBox] Url - SelectedIndexChanged
    ' -------------------------------------------------------------
    Private Sub cmbUrl_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbUrl.SelectedIndexChanged

        If m_NavigatedByHistory Then
            Exit Sub
        End If

        If m_AddressbarEventEnabled Then

            m_AddressbarEventEnabled = False
            Me.Navigate(Me.cmbUrl.Text)

        End If

    End Sub


    ' -------------------------------------------------------------
    '   Navigate
    ' -------------------------------------------------------------
    Private Sub Navigate(url As String)

        m_NavigatedByHistory = False
        Me.WebBrowser1.Navigate(url)
        m_History.NewUrl(url)

    End Sub

    ' -------------------------------------------------------------
    '   ScrollChanged_W
    ' -------------------------------------------------------------
    Private Sub ScrollChanged_W()

        Me.WebBrowser1.Left = m_WebBrowserLeftTop.X - Me.HScrollBar1.Value

    End Sub

    ' -------------------------------------------------------------
    '   ScrollChanged_H
    ' -------------------------------------------------------------
    Private Sub ScrollChanged_H()

        Me.WebBrowser1.Top = m_WebBrowserLeftTop.Y - Me.VScrollBar1.Value

    End Sub

    ' -------------------------------------------------------------
    '   [Button] Capture
    ' -------------------------------------------------------------
    Private Sub btnCapture_Click(sender As System.Object, e As System.EventArgs) Handles btnCapture.Click

        Dim filepath As String = ""
        Dim sfd As New SaveFileDialog()
        sfd.Title = "スクリーンショットを保存"
        sfd.Filter = "JPEG image (*.jpg; *.jpeg)|*.jpg;*.jpeg|All files(*.*)|*"
        If (sfd.ShowDialog(Me) <> Windows.Forms.DialogResult.OK) Then
            Exit Sub
        End If
        filepath = sfd.FileName

        CaptureWebpage(filepath, Me.WebBrowser1)
        MsgBox("保存しました。", MsgBoxStyle.Information)

    End Sub

    ' -------------------------------------------------------------
    '   [Button] Prev
    ' -------------------------------------------------------------
    Private Sub btnPrev_Click(sender As System.Object, e As System.EventArgs) Handles btnPrev.Click

        m_History.GoPrev()
        Dim url As String = m_History.CurrentUrl
        m_NavigatedByHistory = True
        Me.WebBrowser1.Navigate(url)

    End Sub

    ' -------------------------------------------------------------
    '   [Button] Next
    ' -------------------------------------------------------------
    Private Sub btnNext_Click(sender As System.Object, e As System.EventArgs) Handles btnNext.Click
        
        m_History.GoNext()
        Dim url As String = m_History.CurrentUrl
        m_NavigatedByHistory = True
        Me.WebBrowser1.Navigate(url)

    End Sub

    ' -------------------------------------------------------------
    '   [Button] Update
    ' -------------------------------------------------------------
    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click

        Me.UpdateProc()

    End Sub

    ' -------------------------------------------------------------
    '   [Button] Stop
    ' -------------------------------------------------------------
    Private Sub btnStop_Click(sender As System.Object, e As System.EventArgs) Handles btnStop.Click

        Me.WebBrowser1.Stop()

    End Sub

    ' -------------------------------------------------------------
    '   [WebBrowser] Navigated
    ' -------------------------------------------------------------
    Private Sub WebBrowser1_Navigated(sender As Object, e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated

        Dim doc_title As String = Me.WebBrowser1.DocumentTitle.TrimEnd()
        If (doc_title.Length > 0) Then
            Me.Text = String.Format("{0} - WebScreen", doc_title)
        Else
            Me.Text = "WebScreen"
        End If

        Dim url As String = e.Url.ToString()
        If Not m_NavigatedByHistory Then
            m_History.NewUrl(url)
        End If

        ' アドレスバー
        If Not IgnoreUrl.IsIgnoreUrl(url) Then
            m_AddressbarEventEnabled = False
            Me.cmbUrl.Items.Remove(url)
            Me.cmbUrl.Items.Insert(0, url)
            Me.cmbUrl.SelectedIndex = Me.cmbUrl.Items.Count - 1
            m_AddressbarEventEnabled = True
        End If

        Me.btnPrev.Enabled = m_History.CanPrev
        Me.btnNext.Enabled = m_History.CanNext


        m_NavigatedByHistory = False

    End Sub

    Private Sub WebBrowser1_Navigating(sender As Object, e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles WebBrowser1.Navigating


    End Sub

    ' -------------------------------------------------------------
    '   更新
    ' -------------------------------------------------------------
    Private Sub UpdateProc()

        m_NavigatedByHistory = False
        Me.WebBrowser1.Refresh(WebBrowserRefreshOption.Completely)

    End Sub

End Class
