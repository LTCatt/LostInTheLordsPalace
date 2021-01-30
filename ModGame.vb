Public Module ModGame
    Public FrmMain As MainWindow

    '存档数据
    Public DisabledKey As String = ""
    Public Hp As Integer = 3258, HpMax As Integer = 3652
    Public Mp As Integer = 568, MpMax As Integer = 638
    Public BaseAtk As Integer = 505, BaseDef As Integer = 276
    Public ItemCount As Integer() = {0, 999, 999, 999, 999, 999, 999, 999}
    Public EquipWeapon As Integer = 1, EquipArmor As Integer = 2
    Public Location As String = "玩家位置"

    '修改存档
    Public Function GetRealAtk() As Integer
        Return BaseAtk + GetEquipData(EquipWeapon)
    End Function
    Public Function GetRealDef() As Integer
        Return BaseDef + GetEquipData(EquipArmor)
    End Function

    '刷新 UI
    Public Sub RefreshUI()
        '状态栏
        SetText(FrmMain.TextStatus, "勇者   LV 99   \REDHP " & Hp.ToString.PadLeft(4, " ") & "/" & HpMax.ToString.PadLeft(4, " ") & "\WHITE   ATK " & GetRealAtk.ToString.PadLeft(4, " ") & vbCrLf &
                                    "             \BLUEMP " & Mp.ToString.PadLeft(4, " ") & "/" & MpMax.ToString.PadLeft(4, " ") & "\WHITE   DEF " & GetRealDef.ToString.PadLeft(4, " "))
        SetText(FrmMain.TextStatusRight, "\GRAY" & Location)
        '主要部分
        Select Case Screen
            Case Screens.Empty
                SetText(FrmMain.TextTitle, "")
                SetText(FrmMain.TextAction, "")
            Case Screens.Combat
                SetText(FrmMain.TextTitle, "\ORANGE※ 战斗 ※")
                SetText(FrmMain.TextAction,
                        GetKeyText("ATK") & " 攻击\n" &
                        GetKeyText("DEF") & " 防御\n" &
                        GetKeyText("MAG") & " 法术\n" &
                        GetKeyText("ITM") & " 道具\n" &
                        GetKeyText("EQU") & " 装备\n")
            Case Screens.Magic
                SetText(FrmMain.TextTitle, "\ORANGE※ 法术 ※")
                SetText(FrmMain.TextAction,
                        GetKeyText("BAC") & " 返回")
                SetText(FrmMain.TextInfo, MagicInfo)
            Case Screens.Item
                SetText(FrmMain.TextTitle, "\ORANGE※ 道具 ※")
                SetText(FrmMain.TextAction,
                        GetKeyText("BAC") & " 返回")
                SetText(FrmMain.TextInfo, ItemInfo)
            Case Screens.Equip
                SetText(FrmMain.TextTitle, "\ORANGE※ 装备 ※")
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
        Dim Info As New List(Of String)
        For i = 1 To 7
            Info.Add(GetItemText(i,
                                 GetMagicTitle(i).PadRight(8, " ") & "\DARKBLUE" & GetMagicCost(i).ToString.PadLeft(3, " ") & "MP",
                                 If(GetMagicCost(i) > Mp, "\REDMP不足 ", "") & "\DARKGRAY" & GetMagicDesc(i)))
        Next
        Return Join(Info.ToArray, vbCrLf)
    End Function

    '物品
    Public Function ItemInfo() As String
        Dim Info As New List(Of String)
        For i = 1 To 7
            Info.Add(GetItemText(i,
                                 GetItemTitle(i).PadRight(8, " ") & "\GRAYx" & ItemCount(i).ToString.PadLeft(3, " "),
                                 "\DARKGRAY" & GetItemDesc(i)))
        Next
        Return Join(Info.ToArray, vbCrLf)
    End Function

    '装备
    Public Function EquipInfo() As String
        Dim Info As New List(Of String)
        For i = 1 To 7
            Info.Add(GetItemText(i,
                                 GetEquipTitle(i).PadRight(8, " ") & If(EquipArmor = i OrElse EquipWeapon = i, "\DARKBLUE <已装备>", ""),
                                 "\DARKGRAY" & GetEquipDesc(i)))
        Next
        Return Join(Info.ToArray, vbCrLf)
    End Function

End Module
