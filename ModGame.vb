Public Module ModGame
    Public FrmMain As MainWindow

    '刷新 UI
    Public Sub RefreshUI()

    End Sub

    '玩家按回车触发输入
    Public Sub TriggerEvent(Input As String)
        SetText(FrmMain.t, "\YELLOW玩家输入：\WHITE" & Input & "\n" & GetKeyText("ATK") & " 攻击")
    End Sub

    '显示禁用的键
    Public DisabledKey As New List(Of String) From {"A", "B", "C", "1", "2"}

End Module
