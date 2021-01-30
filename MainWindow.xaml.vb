Imports System.Threading
Public Class MainWindow

    '初始化
    Private Const WindowMargin = 30
    Private Sub Init() Handles Me.Loaded
        FrmMain = Me
        AniStartRun()
        'UI 初始化
        SetText(TextBottomLine, "\DARKGRAY─────────────────────────────────────┴───────────────")
        SetText(TextActionLine, "\DARKGRAY││││││││││││││││┤││││││││││││││││││││")
        SetText(TextChatLine, "\DARKGRAY──────────────────────────────────────────────────────")
        SetText(TextInputResult, " \DARKGRAY等待玩家输入指令。")
        TextInputBox.Tag = ""
        '窗口自适应
        Dim Size As Integer = Math.Floor(Math.Min((ActualHeight - WindowMargin * 2) / PanMain.Height, (ActualWidth - WindowMargin * 2) / PanMain.Width))
        TransScale.ScaleX = Size
        TransScale.ScaleY = Size
        '刷新 UI
        RunInNewThread(Sub()
                           Do While True
                               Dispatcher.Invoke(Sub() RefreshUI())
                               Thread.Sleep(10)
                           Loop
                       End Sub, "UI Loop")
    End Sub

    '文本输入
    Private Sub MainWindow_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        '获取真实输入文本
        Dim RealKey As String = e.Key.ToString.ToUpper
        If RealKey.StartsWith("NUMPAD") Then RealKey = RealKey.Substring(6)
        If RealKey.StartsWith("D") AndAlso RealKey.Length = 2 Then RealKey = RealKey.Substring(1)
        '退出游戏
        If e.Key = Key.Escape Then Process.GetCurrentProcess.Kill()
        '按下任意按键
        If EnterStatus = EnterStatuses.Chat Then
            If CanContinue Then NextChat()
            Exit Sub
        End If
        '主输入状态
        If e.Key = Key.Back Then
            TextInputBox.Tag = ""
            'If TextInputBox.Tag.ToString.Length > 0 Then TextInputBox.Tag = TextInputBox.Tag.ToString.Substring(0, TextInputBox.Tag.ToString.Length - 1)
        ElseIf e.Key = Key.Enter Then
            If TextInputBox.Tag <> "" Then Enter(TextInputBox.Tag)
            TextInputBox.Tag = ""
        ElseIf Not DisabledKey.Contains(RealKey) AndAlso RealKey.Length = 1 Then
            TextInputBox.Tag = (TextInputBox.Tag.ToString & RealKey).Substring(0, Math.Min(TextInputBox.Tag.ToString.Length + 1, 47))
        End If
        RefreshInputBox()
    End Sub
    Private Sub RefreshInputBox() Handles Me.Loaded
        SetText(TextInputBox, ">" & TextInputBox.Tag)
    End Sub

End Class
