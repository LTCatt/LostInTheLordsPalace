﻿Public Class MainWindow

    '初始化
    Private WidthText = 16 * 3
    Private HeightText = 9 * 3
    Private Sub Init() Handles Me.Loaded
        FrmMain = Me
        '窗口自适应
        Dim Size As Integer = Math.Floor(Math.Min((ActualHeight - 40) / HeightText, (ActualWidth - 40) / WidthText)) - 1
        PanMain.Width = Size * WidthText + 40
        PanMain.Height = Size * HeightText + 40
    End Sub

    '文本输入
    Private Sub MainWindow_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        '获取真实输入文本
        Dim RealKey As String = e.Key.ToString.ToUpper
        If RealKey.StartsWith("NUMPAD") Then RealKey = RealKey.Substring(6)
        If RealKey.StartsWith("D") AndAlso RealKey.Length = 2 Then RealKey = RealKey.Substring(1)
        '根据按键判断
        If e.Key = Key.Escape Then
            Process.GetCurrentProcess.Kill()
        ElseIf e.Key = Key.Back Then
            If TextInput.Tag.ToString.Length > 0 Then TextInput.Tag = TextInput.Tag.ToString.Substring(0, TextInput.Tag.ToString.Length - 1)
        ElseIf e.Key = Key.Enter Then
            TriggerEvent(TextInput.Tag)
            TextInput.Tag = ""
        ElseIf Not DisabledKey.Contains(RealKey) AndAlso RealKey.Length = 1 Then
            TextInput.Tag = (TextInput.Tag.ToString & RealKey).Substring(0, Math.Min(TextInput.Tag.ToString.Length + 1, 50))
        End If
        RefreshInputBox()
    End Sub
    Private Sub RefreshInputBox() Handles Me.Loaded
        SetText(TextInput, ">" & TextInput.Tag)
    End Sub

End Class
