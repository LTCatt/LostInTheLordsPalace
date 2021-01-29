Public Module ModGame
    Public FrmMain As MainWindow

    '玩家按回车触发输入
    Public Sub TriggerEvent(Input As String)
        SetText(FrmMain.t, "\YELLOW玩家输入：\WHITE" & Input)
    End Sub

End Module
