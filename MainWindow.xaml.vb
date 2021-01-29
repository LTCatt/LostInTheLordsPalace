Class MainWindow


    Private Sub MainWindow_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        SetText(t, "white\ared\wgf\nnext")
    End Sub

    Private Sub SetText(Target As TextBlock, RawText As String)
        '处理文本
        'TODO : 将 RawText 中的字母、数字、符号转换为全角

        Target.Inlines.Clear()
        Target.Inlines.Add(New Run("test") With {.Foreground = New SolidColorBrush(Color.FromRgb(255, 255, 255))})
        Target.Inlines.Add(New Run("test2") With {.Foreground = New SolidColorBrush(Color.FromRgb(0, 255, 255))})
    End Sub

End Class
