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
    Public Function Enter(Input As String) As String
        If Input = "5" Then
            FrmMain.TextChat.Text = GetRawText("<系统> 这是一段测试的聊天文本！:D 52/55")
            AniStart(AaTextAppear(FrmMain.TextChat, Time:=50))
            Return "YES5"
        End If
        Return "\RED指令未知或无效，请输入右上角指令窗口中显示的指令。"
    End Function

End Module
