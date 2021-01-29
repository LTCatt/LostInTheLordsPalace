Public Module ModGame
    Public FrmMain As MainWindow

    '刷新 UI
    Public Sub RefreshUI()
        SetText(FrmMain.TextStatus, "\AQUA勇者 HP")
    End Sub

    '玩家按回车触发输入
    Public Function Enter(Input As String) As String
        SetText(FrmMain.TextInfo, "\YELLOW玩家输入：\WHITE" & Input & "\n" & GetKeyText("ATK") & " 攻击")
        If Input = "5" Then
            Return "YES5"
        End If
        Return Nothing
    End Function

    '显示禁用的键
    Public DisabledKey As New List(Of String) From {"A", "B", "C", "1", "2"}

End Module
