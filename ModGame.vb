Public Module ModGame
    Public FrmMain As MainWindow

    '存档数据
    Public DisabledKey As New List(Of String) From {"K", "D", "M", "G"}
    Public Hp As Integer = 58, HpMax As Integer = 100, HpScale As Double = 36.52
    Public Mp As Integer = 68, MpMax As Integer = 100, MpScale As Double = 6.38

    '刷新 UI
    Public Sub RefreshUI()
        SetText(FrmMain.TextStatus, "勇者  LV 99  \REDHP " & Math.Round(Hp * HpScale).ToString.PadLeft(4, " ") & "/" & Math.Round(HpMax * HpScale).ToString.PadLeft(4, " ") & vbCrLf &
                                    "           \BLUEMP " & Math.Round(Mp * MpScale).ToString.PadLeft(4, " ") & "/" & Math.Round(MpMax * MpScale).ToString.PadLeft(4, " "))
        SetText(FrmMain.TextAction,
                GetKeyText("ATK") & " 攻击\n" &
                GetKeyText("DEF") & " 防御\n" &
                GetKeyText("MAG") & " 法术\n" &
                GetKeyText("ITM") & " 道具\n" &
                GetKeyText("EQU") & " 装备\n")
    End Sub

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
                    Case "1"
                        StartChat({"aaaaaa", "line2dgergedg"}, False)
                    Case "2"
                        StartChat({"aaaafsdf", "line2"}, True)
                    Case Else
                        SetText(FrmMain.TextInputResult, " \RED指令未知或无效，请输入右上角指令窗口中显示的指令。")
                End Select
            Case Else
                SetText(FrmMain.TextInputResult, " \RED未知的输入状态！")
        End Select
    End Sub

    '对话框
    Private ChatContents As New List(Of String)
    Public CanContinue As Boolean = False
    Private Sub StartChat(Contents As String(), RequireEnsure As Boolean)
        ChatContents = New List(Of String)(Contents)
        If RequireEnsure Then
            EnterStatus = EnterStatuses.Chat
        End If
        NextChat()
    End Sub
    Public Sub NextChat()
        AniStop("Chat Content")
        If ChatContents.Count > 0 Then
            CanContinue = False
            If EnterStatus = EnterStatuses.Chat Then SetText(FrmMain.TextInputResult, " \AQUA按任意键以继续。")
            FrmMain.TextChat.Foreground = If(EnterStatus = EnterStatuses.Chat, New MyColor(0, 255, 255), New MyColor(160, 160, 160))
            FrmMain.TextChat.Text = GetRawText(ChatContents.First)
            AniStart({
                     AaTextAppear(FrmMain.TextChat, Time:=50),
                     AaCode(Sub() CanContinue = True, 500),
                     AaCode(Sub()
                                If Not EnterStatus = EnterStatuses.Chat Then
                                    RunInThread(Sub() RunInUi(Sub() NextChat()))
                                End If
                            End Sub, If(ChatContents.Count = 1, 3000, 1500), True)
                }, "Chat Content")
            FrmMain.TextChat.Text = "" '防止动画结束前闪现
            ChatContents.RemoveAt(0)
        Else
            If EnterStatus = EnterStatuses.Chat Then
                EnterStatus = EnterStatuses.Normal
                SetText(FrmMain.TextInputResult, " \DARKGRAY等待玩家输入指令。")
            End If
            FrmMain.TextChat.Text = ""
        End If
    End Sub

End Module
