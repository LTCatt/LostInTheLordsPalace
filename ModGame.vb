Public Module ModGame
    Public FrmMain As MainWindow

    '存档数据
    Public DisabledKey As String = ""
    Public Hp As Integer = 58, HpMax As Integer = 100, HpScale As Double = 36.52
    Public Mp As Integer = 68, MpMax As Integer = 100, MpScale As Double = 6.38
    Public Location As String = "玩家位置"

    '刷新 UI
    Public Sub RefreshUI()
        '状态栏
        SetText(FrmMain.TextStatus, "勇者  LV 99  \REDHP " & Math.Round(Hp * HpScale).ToString.PadLeft(4, " ") & "/" & Math.Round(HpMax * HpScale).ToString.PadLeft(4, " ") & vbCrLf &
                                    "           \BLUEMP " & Math.Round(Mp * MpScale).ToString.PadLeft(4, " ") & "/" & Math.Round(MpMax * MpScale).ToString.PadLeft(4, " "))
        SetText(FrmMain.TextStatusRight, "\GRAY" & Location)
        '主要部分
        Select Case Screen
            Case Screens.Empty
                SetText(FrmMain.TextTitle, "")
                SetText(FrmMain.TextAction, "")
            Case Screens.Combat
                SetText(FrmMain.TextTitle, "战斗")
                SetText(FrmMain.TextAction,
                        GetKeyText("ATK") & " 攻击\n" &
                        GetKeyText("DEF") & " 防御\n" &
                        GetKeyText("MAG") & " 法术\n" &
                        GetKeyText("ITM") & " 道具\n" &
                        GetKeyText("EQU") & " 装备\n")
            Case Screens.Magic
                SetText(FrmMain.TextTitle, "使用法术")
                SetText(FrmMain.TextAction,
                        GetKeyText("BAC") & " 返回")
                SetText(FrmMain.TextInfo, MagicInfo)
            Case Screens.Item
                SetText(FrmMain.TextTitle, "使用道具")
                SetText(FrmMain.TextAction,
                        GetKeyText("BAC") & " 返回")
                SetText(FrmMain.TextInfo, ItemInfo)
            Case Screens.Equip
                SetText(FrmMain.TextTitle, "更换装备")
                SetText(FrmMain.TextAction,
                        GetKeyText("BAC") & " 返回")
                SetText(FrmMain.TextInfo, EquipInfo)
        End Select

    End Sub

    '当前屏幕
    Public Screen As Screens = Screens.Magic
    Public Enum Screens
        Empty
        Combat
        Magic
        Item
        Equip
    End Enum

    '魔法
    Public Function MagicInfo() As String
        Return "mm"
    End Function

    '物品
    Public Function ItemInfo() As String
        Return "it"
    End Function

    '魔法
    Public Function EquipInfo() As String
        Return "eq"
    End Function

End Module
