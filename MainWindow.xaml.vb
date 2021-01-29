Imports System.Threading
Public Class MainWindow

    '初始化
    Private Const WindowMargin = 60
    Private Sub Init() Handles Me.Loaded
        FrmMain = Me
        'UI 初始化
        SetText(TextBottomLine, "\DARKGRAY".PadRight(PanMain.Width + 5, "-"))
        SetText(TextInputResult, " \DARKGRAY等待玩家输入指令。")
        TextInput.Tag = ""
        '窗口自适应
        Dim Size As Integer = Math.Floor(Math.Min((ActualHeight - WindowMargin * 2) / PanMain.Height, (ActualWidth - WindowMargin * 2) / PanMain.Width))
        TransScale.ScaleX = Size
        TransScale.ScaleY = Size
        '刷新 UI
        Dim th As New Thread(Sub()
                                 Do While True
                                     Dispatcher.Invoke(Sub() RefreshUI())
                                     Thread.Sleep(10)
                                 Loop
                             End Sub)
        th.Start()
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
            SetText(TextInputResult, " \DARKGRAY" & If(Enter(TextInput.Tag), "\DARKRED指令未知或无效。"))
            TextInput.Tag = ""
        ElseIf Not DisabledKey.Contains(RealKey) AndAlso RealKey.Length = 1 Then
            TextInput.Tag = (TextInput.Tag.ToString & RealKey).Substring(0, Math.Min(TextInput.Tag.ToString.Length + 1, 47))
        End If
        RefreshInputBox()
    End Sub
    Private Sub RefreshInputBox() Handles Me.Loaded
        SetText(TextInput, ">" & TextInput.Tag)
    End Sub

End Class
