Public Module ModBase

    '半角转全角
    Public Function ToSBC(input As String) As String
        Dim c As Char() = input.ToCharArray()
        For i As Integer = 0 To c.Length - 1
            If Strings.Asc(c(i)) = 32 Then
                c(i) = ChrW(12288)
                Continue For
            End If
            If Strings.Asc(c(i)) < 127 AndAlso Strings.Asc(c(i)) >= 33 Then c(i) = ChrW(Strings.Asc(c(i)) + 65248)
        Next
        Return New String(c)
    End Function

    '修改目标 TextBlock 中的文本
    Public Sub SetText(Target As TextBlock, RawText As String)
        '预处理文本
        RawText = ToSBC(RawText).Replace("＼ｎ", vbCrLf)
        '将文本按颜色分段，保证每段开头均为颜色标记
        If Not RawText.StartsWith("＼") Then RawText = "ＷＨＩＴＥ" & RawText
        Dim RawTexts As String() = RawText.Split("＼")
        '修改目标显示
        Target.Inlines.Clear()
        For Each Inline In RawTexts
            If Inline = "" Then Continue For
            Dim TargetColor As SolidColorBrush
            Dim Delta As Integer = 0
            If Inline.StartsWith("ＷＨＩＴＥ") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 255, 255))
                Delta = 5
            ElseIf Inline.StartsWith("ＲＥＤ") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 0, 0))
                Delta = 3
            ElseIf Inline.StartsWith("ＹＥＬＬＯＷ") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 255, 0))
                Delta = 6
            ElseIf Inline.StartsWith("ＧＲＥＥＮ") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(0, 255, 0))
                Delta = 5
            ElseIf Inline.StartsWith("ＢＬＵＥ") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(0, 0, 255))
                Delta = 4
            ElseIf Inline.StartsWith("ＧＲＡＹ") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(128, 128, 128))
                Delta = 4
            Else
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 255, 255))
                Inline += "未知的颜色"
            End If
            Target.Inlines.Add(New Run(Inline.Substring(Delta)) With {.Foreground = TargetColor})
        Next
    End Sub

End Module
