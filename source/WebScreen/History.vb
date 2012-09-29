Public Class HistoryClass

    Private m_Urls As New Generic.List(Of String) ' 古い方から。
    Private m_Index As Integer = -1

    Public Sub New()

    End Sub

    Public ReadOnly Property CanPrev As Boolean
        Get
            Return (m_Index >= 1)
        End Get
    End Property

    Public ReadOnly Property CanNext As Boolean
        Get
            Return (m_Index <= m_Urls.Count - 2)
        End Get
    End Property

    Public ReadOnly Property CurrentUrl As String
        Get
            If (m_Urls.Count = 0) Then
                Return ""
            Else
                Return m_Urls(m_Index)
            End If
        End Get
    End Property

    Public Sub NewUrl(ByVal url As String)

        url = url.Trim()

        If (url.Length = 0) Then
            Exit Sub
        End If

        If IgnoreUrl.IsIgnoreUrl(url) Then
            Exit Sub
        End If

        If (m_Urls.Count = 0) OrElse (url <> m_Urls(m_Index)) Then
            m_Index += 1
            If (m_Index <= m_Urls.Count - 1) Then
                m_Urls.RemoveRange(m_Index, m_Urls.Count - m_Index)
            End If
            m_Urls.Add(url)
        End If

    End Sub

    Public Sub GoPrev()

        If Me.CanPrev Then
            m_Index -= 1
        End If

    End Sub

    Public Sub GoNext()

        If Me.CanNext Then
            m_Index += 1
        End If

    End Sub




End Class
