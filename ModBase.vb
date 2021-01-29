Public Module ModBase

    '修改目标 TextBlock 中的文本
    Public Sub SetText(Target As TextBlock, RawText As String)
        RawText = RawText.Replace("\n", vbCrLf)
        '将文本按颜色分段，保证每段开头均为颜色标记
        If Not RawText.StartsWith("\") Then RawText = "WHITE" & RawText
        Dim RawTexts As String() = RawText.Split("\")
        '修改目标显示
        Target.Inlines.Clear()
        For Each Inline In RawTexts
            If Inline = "" Then Continue For
            Dim TargetColor As SolidColorBrush
            Dim Delta As Integer = 0
            If Inline.StartsWith("KEY") Then
                '特殊：根据字符是否解锁自动使用黄色和暗黄色
                If DisabledKey.Contains(Inline.Substring(3, 1)) Then
                    TargetColor = New SolidColorBrush(Color.FromRgb(100, 100, 0))
                Else
                    TargetColor = New SolidColorBrush(Color.FromRgb(255, 255, 0))
                End If
                Delta = 3
            ElseIf Inline.StartsWith("WHITE") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 255, 255))
                Delta = 5
            ElseIf Inline.StartsWith("AQUA") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(0, 255, 255))
                Delta = 4
            ElseIf Inline.StartsWith("RED") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 0, 0))
                Delta = 3
            ElseIf Inline.StartsWith("DARKRED") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(100, 0, 0))
                Delta = 7
            ElseIf Inline.StartsWith("YELLOW") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 255, 0))
                Delta = 6
            ElseIf Inline.StartsWith("GREEN") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(0, 255, 0))
                Delta = 5
            ElseIf Inline.StartsWith("BLUE") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(0, 0, 255))
                Delta = 4
            ElseIf Inline.StartsWith("GRAY") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(160, 160, 160))
                Delta = 4
            ElseIf Inline.StartsWith("DARKGRAY") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(60, 60, 60))
                Delta = 8
            Else
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 255, 255))
                Inline += "未知的颜色"
            End If
            Target.Inlines.Add(New Run(StrConv(Inline.Substring(Delta), VbStrConv.Wide)) With {.Foreground = TargetColor})
        Next
    End Sub
    Public Function GetKeyText(Key As String) As String
        Dim Letters As New List(Of String)
        For Each Letter In Key
            Letters.Add("\KEY" & Letter)
        Next
        Return "\YELLOW<" & Join(Letters.ToArray, "") & "\YELLOW>\WHITE"
    End Function

End Module
