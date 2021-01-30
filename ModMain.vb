Public Module ModMain

    '对 TextBlock 设置富文本
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
                '特殊：根据字符是否解锁自动使用黄色和深灰色
                If DisabledKey.Contains(Inline.Substring(3, 1)) Then
                    TargetColor = New SolidColorBrush(Color.FromRgb(60, 60, 60))
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
            ElseIf Inline.StartsWith("ORANGE") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 128, 0))
                Delta = 6
            ElseIf Inline.StartsWith("DARKRED") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(100, 0, 0))
                Delta = 7
            ElseIf Inline.StartsWith("YELLOW") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(255, 255, 0))
                Delta = 6
            ElseIf Inline.StartsWith("GREEN") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(0, 255, 0))
                Delta = 5
            ElseIf Inline.StartsWith("DARKBLUE") Then
                TargetColor = New SolidColorBrush(Color.FromRgb(0, 0, 150))
                Delta = 8
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
    '获取全角处理后的纯文本
    Public Function GetRawText(Text As String) As String
        Return StrConv(Text.Replace("\n", vbCrLf), VbStrConv.Wide)
    End Function

    '获取根据按键处理后的富文本
    Public Function GetKeyText(Key As String) As String
        Dim Letters As New List(Of String)
        For Each Letter In Key
            Letters.Add("\KEY" & Letter)
        Next
        Return "\YELLOW<" & Join(Letters.ToArray, "") & "\YELLOW>\WHITE"
    End Function
    '获取单个项目的富文本
    Public Function GetItemText(Id As Integer, Title As String, Desc As String) As String
        Return GetKeyText(Id) & " " & Title & vbCrLf & "    " & Desc
    End Function

    '玩家输入指令
    Public EnterStatus As EnterStatuses = EnterStatuses.Normal
    Public Enum EnterStatuses
        Normal
        Chat
    End Enum
    Public Sub Enter(Input As String)
        Select Case EnterStatus
            Case EnterStatuses.Normal
                SetText(FrmMain.TextInputResult, " \DARKGRAY等待玩家输入指令。")
                Select Case Input
                    Case "MAG"
                        Screen = Screens.Magic
                    Case "EQU"
                        Screen = Screens.Equip
                    Case "ITM"
                        Screen = Screens.Item
                    Case "BAC"
                        Screen = Screens.Combat
                    Case "CHAT"
                        StartChat({"* Chat Content 1.", "* Chat Content Line 2, it's a little long."}, True)
                    Case Else
                        SetText(FrmMain.TextInputResult, " \RED指令未知或无效，请输入屏幕上以黄色显示的指令。")
                End Select
            Case Else
                SetText(FrmMain.TextInputResult, " \RED未知的输入状态！")
        End Select
    End Sub

    '对话框
    Private ChatContents As New List(Of String)
    Private Sub StartChat(Contents As String(), RequireEnsure As Boolean)
        ChatContents = New List(Of String)(Contents)
        If RequireEnsure Then
            EnterStatus = EnterStatuses.Chat
            FrmMain.TextInputBox.Tag = ""
            FrmMain.RefreshInputBox()
        End If
        NextChat()
    End Sub
    Public Sub NextChat()
        If FrmMain.TextChat.Text <> FrmMain.TextChat.Tag Then
            '补全当前对话
            AniStop("Chat Content")
            FrmMain.TextChat.Text = FrmMain.TextChat.Tag
        ElseIf ChatContents.Count > 0 Then
            '下一句对话
            If EnterStatus = EnterStatuses.Chat Then SetText(FrmMain.TextInputResult, " \AQUA按任意键以继续对话。")
            FrmMain.TextChat.Foreground = If(EnterStatus = EnterStatuses.Chat, New MyColor(255, 255, 255), New MyColor(160, 160, 160))
            FrmMain.TextChat.Text = GetRawText(ChatContents.First)
            FrmMain.TextChat.Tag = FrmMain.TextChat.Text
            AniStop("Chat Content")
            AniStart({
                     AaTextAppear(FrmMain.TextChat, Time:=40),
                     AaCode(Sub()
                                If Not EnterStatus = EnterStatuses.Chat Then
                                    RunInThread(Sub() RunInUi(Sub() NextChat()))
                                End If
                            End Sub, If(ChatContents.Count = 1, 3000, 1500), True)
                }, "Chat Content")
            FrmMain.TextChat.Text = "" '防止动画结束前闪现
            ChatContents.RemoveAt(0)
        Else
            '结束对话
            AniStop("Chat Content")
            If EnterStatus = EnterStatuses.Chat Then
                EnterStatus = EnterStatuses.Normal
                SetText(FrmMain.TextInputResult, " \DARKGRAY等待玩家输入指令。")
            End If
            FrmMain.TextChat.Text = ""
            FrmMain.TextChat.Tag = ""
        End If
    End Sub

End Module
