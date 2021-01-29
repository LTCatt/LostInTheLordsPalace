Public Module ModGame
    Public FrmMain As MainWindow

    '存档数据
    Public DisabledKey As New List(Of String) From {"A", "B", "C", "1", "2"}
    Public Hp As Integer = 58, HpMax As Integer = 100, HpScale As Double = 36.52
    Public Mp As Integer = 68, MpMax As Integer = 100, MpScale As Double = 6.38

    '刷新 UI
    Public Sub RefreshUI()
        SetText(FrmMain.TextStatus, "勇者  LV 99  \REDHP " & Math.Round(Hp * HpScale).ToString.PadLeft(4, " ") & "/" & Math.Round(HpMax * HpScale).ToString.PadLeft(4, " ") & vbCrLf &
                                    "           \BLUEMP " & Math.Round(Mp * MpScale).ToString.PadLeft(4, " ") & "/" & Math.Round(MpMax * MpScale).ToString.PadLeft(4, " "))
    End Sub

    '玩家输入指令
    Public Function Enter(Input As String) As String
        SetText(FrmMain.TextInfo, "\YELLOW玩家输入：\WHITE" & Input & "\n" & GetKeyText("ATK") & " 攻击")
        If Input = "5" Then
            Hp += 5
            Return "YES5"
        End If
        Return Nothing
    End Function

End Module
