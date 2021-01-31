Imports System.ComponentModel
Imports System.Threading
Public Class MainWindow
    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Process.GetCurrentProcess.Kill()
    End Sub

    '初始化
    Private Const WindowMargin = 40
    Public PixelLevel As Integer = 0
    Public IsHalfSec As Boolean = False
    Private Sub Init() Handles Me.Loaded
        FrmMain = Me
        AniStartRun()
        StartLevel(Level)
        'UI 初始化
        SetText(TextBottomLine, "\DARKGRAY─────────────────────────────────────┴───────────────")
        SetText(TextActionLine, "\DARKGRAY││││││││││││││││┤││││││││││││││││││││")
        SetText(TextChatLine, "\DARKGRAY──────────────────────────────────────────────────────")
        SetText(TextInputResult, "\DARKGRAY等待玩家输入指令。")
        TextInputBox.Tag = ""
        '窗口自适应
        Dim Size As Integer = Math.Floor(Math.Min((ActualHeight - WindowMargin * 2) / PanMain.Height, (ActualWidth - WindowMargin * 2) / PanMain.Width))
        TransScale.ScaleX = Size
        TransScale.ScaleY = Size
        '闪烁计数与动画
        RunInNewThread(Sub()
                           Do While True
                               IsHalfSec = Not IsHalfSec
                               If IsHalfSec Then
                                   '扫描线
                                   AniStart({AaTranslateY(ImgLine, 40, 1000), AaTranslateY(ImgLine, -40, 1, After:=True)})
                                   '选择闪烁
                                   If Screen = Screens.Select Then
                                       AniStart({AaOpacity(TextTitle, -0.6, 490, 0, New AniEaseInoutFluent(AniEasePower.Weak)),
                                                 AaOpacity(TextTitle, 0.6, 490, 490, New AniEaseInoutFluent(AniEasePower.Weak))}, "Title Opacity")
                                   End If
                               End If
                               Thread.Sleep(500)
                           Loop
                       End Sub, "Half Sec")
        '刷新 UI
        RunInNewThread(Sub()
                           Do While True
                               Dispatcher.Invoke(Sub() RefreshUI())
                               Thread.Sleep(25)
                           Loop
                       End Sub, "UI Loop")
        '抖动特效
        RunInNewThread(Sub()
                           Do While True
                               '获取偏移值
                               Dim Pixelation As Integer '最大值为 1000（对应 1）
                               Select Case PixelLevel
                                   Case 0
                                       Pixelation = 0
                                   Case 1
                                       If RandomInteger(0, 39) = 0 Then
                                           Pixelation = RandomInteger(620, 820)
                                       Else
                                           Pixelation = 0
                                       End If
                                   Case 2
                                       If RandomInteger(0, 29) = 0 Then
                                           Pixelation = RandomInteger(660, 860)
                                       Else
                                           Pixelation = RandomInteger(-200, 400)
                                       End If
                                   Case 3
                                       If RandomInteger(0, 19) < 3 Then
                                           Pixelation = RandomInteger(700, 900)
                                       Else
                                           Pixelation = RandomInteger(200, 700)
                                       End If
                               End Select
                               '改变状态
                               RunInUi(Sub()
                                           If Pixelation <= 0 Then
                                               PanBack2.Effect = Nothing
                                           ElseIf PanBack2.Effect IsNot Nothing Then
                                               CType(PanBack2.Effect, Microsoft.Expression.Media.Effects.PixelateEffect).Pixelation = Pixelation / 1000
                                           Else
                                               PanBack2.Effect = New Microsoft.Expression.Media.Effects.PixelateEffect With {.Pixelation = Pixelation / 1000}
                                           End If
                                       End Sub)
                               Thread.Sleep(30)
                           Loop
                       End Sub, "Shake")
        '音频引擎
        SoundStartRun()
        MusicStartRun()
        MusicStartRun2()
    End Sub

    '文本输入
    Private Sub MainWindow_KeyPress(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        '获取真实输入文本
        Dim RealKey As String = e.Key.ToString.ToUpper
        If RealKey.StartsWith("NUMPAD") Then RealKey = RealKey.Substring(6)
        If RealKey.StartsWith("D") AndAlso RealKey.Length = 2 Then RealKey = RealKey.Substring(1)
        If RealKey = "SPACE" Then RealKey = " "
        '按下任意按键
        If EnterStatus = EnterStatuses.Chat Then
            NextChat(True)
            Exit Sub
        End If
        '主输入状态
        If e.Key = Key.Escape AndAlso Not (Screen = Screens.Combat OrElse Screen = Screens.Empty) Then
            TextInputBox.Tag = ""
            Enter("ESC")
        ElseIf e.Key = Key.Back Then
            If TextInputBox.Tag.ToString.Length > 0 Then TextInputBox.Tag = TextInputBox.Tag.ToString.Substring(0, TextInputBox.Tag.ToString.Length - 1)
        ElseIf e.Key = Key.Enter Then
            If TextInputBox.Tag <> "" Then Enter(TextInputBox.Tag)
            TextInputBox.Tag = ""
        ElseIf RealKey.Length = 1 Then
            If DisabledKey.Contains(RealKey) AndAlso Not e.KeyboardDevice.IsKeyDown(Key.RightCtrl) Then
                SetText(FrmMain.TextInputResult, "\RED错误：内联逻辑已损坏！")
                PlaySound("Error.mp3", 0.35)
            Else
                '成功输入
                SetText(FrmMain.TextInputResult, "\DARKGRAY等待玩家输入指令。")
                TextInputBox.Tag = (TextInputBox.Tag.ToString & RealKey).Substring(0, Math.Min(TextInputBox.Tag.ToString.Length + 1, 47))
            End If
        End If
    End Sub

End Class
