Public Class IgnoreUrl

    Public Shared Function IsIgnoreUrl(url As String) As Boolean

        Dim ignore_urls As New Generic.List(Of String)
        ignore_urls.Add("about:blank")
        ignore_urls.Add("http://www.google.co.jp/url?")

        Return ignore_urls.Exists(Function(ignore_url As String) url.StartsWith(ignore_url))

    End Function

End Class
