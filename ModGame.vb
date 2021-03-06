﻿Public Module ModGame
    Public FrmMain As MainWindow

    '存档数据
    Public DisabledKey As String = ""
    Public Hp As Integer = 1, HpMax As Integer = 1652
    Public Mp As Integer = 1, MpMax As Integer = 638
    Public BaseAtk As Integer = 505, ExtraAtk As Integer = 0
    Public BaseDef As Integer = 276, ExtraDef As Integer = 0
    Public ItemCount As Integer() = {0, 1, 2, 5, 95, 43, 83, 4}
    Public EquipWeapon As Integer = 1, EquipArmor As Integer = 2
    Public Level As Integer = 100
    Public Turn As Integer = 0
    Public ExtraCold As Integer = 0
    '怪物数据
    Public MonsterType As New List(Of String), MonsterName As New List(Of String), MonsterHp As New List(Of Integer), MonsterSp As New List(Of Integer)

    '受伤
    Public Function HurtMonster(Id As Integer, Damage As Integer, Type As DamageType, IgnoreDefence As Boolean)
        Dim Mul = GetMonsterInv(MonsterType(Id), Type)
        Damage = Math.Max(If(Mul = 0, 0, 1), Mul * (Damage - If(IgnoreDefence, 0, GetMonsterDef(MonsterType(Id)))))
        Dim ExtraDisc As String = If(Mul > 1 AndAlso Damage > 1, "效果拔群，", If(Mul = 0.2, Type.ToString & "抗性！", If(Mul = 0, Type.ToString & "免疫！", "")))
        If ExtraDisc = "" AndAlso Damage = 1 Then ExtraDisc = "未突破防御！"
        '实际扣血
        MonsterHp(Id) = Math.Max(0, MonsterHp(Id) - Damage)
        '返回结果
        Return {Damage, ExtraDisc}
    End Function
    Public Function HurtPlayer(Damage As Integer, Type As DamageType)
        '获取抗性
        Dim Mul = If(EquipArmor = 7 AndAlso Type = DamageType.火焰, 0.2, 1) * If(ExtraCold > 0 AndAlso Type = DamageType.火焰, 0.2, 1)
        Damage = Math.Max(If(Mul = 0, 0, 1), Damage * Mul)
        If Damage = 2 Then Damage = 1
        Dim ExtraDisc As String = If(Mul > 1 AndAlso Damage > 1, "效果拔群，", If(Mul < 1, Type.ToString & "抗性！", ""))
        '实际扣血
        Hp = Math.Max(0, Hp - Damage)
        '受伤动画
        If Damage > 1 Then
            Dim DeltaOpacity As Double = Math.Min(1, Damage / HpMax * 2)
            FrmMain.RectHurt.Opacity = DeltaOpacity
            AniStart(AaOpacity(FrmMain.RectHurt, -DeltaOpacity, Damage / HpMax * 6000, Damage / HpMax * 1500, Ease:=New AniEaseInFluent(AniEasePower.Weak)), "Hurt Player")
        End If
        '返回结果
        Return {Damage, ExtraDisc}
    End Function

    '原始存档
    Public ItemCountLast As Integer() = ItemCount.Clone
    Public EquipWeaponLast As Integer = EquipWeapon, EquipArmorLast As Integer = EquipArmor

    '修改存档
    Public Function GetRealAtk(Optional IgnoreExtra As Boolean = False) As Integer
        Return BaseAtk + GetEquipAtk(EquipWeapon) + GetEquipAtk(EquipArmor) + If(IgnoreExtra, 0, ExtraAtk)
    End Function
    Public Function GetRealDef(Optional IgnoreExtra As Boolean = False) As Integer
        Return BaseDef + GetEquipDef(EquipWeapon) + GetEquipDef(EquipArmor) + If(IgnoreExtra, 0, ExtraDef)
    End Function
    Public Function StartLevel(Id As Integer) As Integer
        Screen = Screens.Combat
        If Id = 102 Then
            MusicChange("Boss 1.mp3", 0, True) '3
            MusicChange2("Boss 2.mp3", 0.2, True) '2
        End If
        StartChat({GetLevelIntros(Id)(0)}, False, False)
        FrmMain.TextTitle.Opacity = 1 : AniStop("Title Opacity")
        '重置存档
        Turn = 0
        ItemCount = ItemCountLast.Clone
        EquipWeapon = EquipWeaponLast
        EquipArmor = EquipArmorLast
        ExtraAtk = 0 : ExtraDef = 0 : ExtraCold = 0
        Hp = HpMax : Mp = MpMax
        '初始化怪物数据
        MonsterTurnPerformed = Nothing
        MonsterType.Clear()
        MonsterName.Clear()
        MonsterHp.Clear()
        MonsterSp.Clear()
        MonsterType.AddRange(GetLevelMonsters(Id))
        MonsterName.AddRange(GetLevelMonstersName(Id))
        For Each Monster In MonsterType
            MonsterHp.Add(GetMonsterHp(Monster))
            MonsterSp.Add(GetMonsterSp(Monster))
        Next
    End Function

    '刷新 UI
    Public Sub RefreshUI()
        If AutoContinueChat Then NextChat(False)
        SetText(FrmMain.TextInputBox, If(EnterStatus = EnterStatuses.Chat, "", ">" & FrmMain.TextInputBox.Tag & If(FrmMain.IsHalfSec, "_", "")))
        SetText(FrmMain.TextActionButtom, If(Screen <> Screens.Empty, GetKeyText("RST") & " 重置\n\n", "") & GetKeyText("ALT+F4") & "\n    退出游戏")
        '状态栏
        SetText(FrmMain.TextStatus, "勇者   LV 99   \REDHP " & Hp.ToString.PadLeft(4, " ") & "/" & HpMax.ToString.PadLeft(4, " ") & "\WHITE   ATK " & GetRealAtk(True).ToString.PadLeft(4, " ") & If(ExtraAtk > 0, "\GREEN+" & ExtraAtk.ToString.PadLeft(3, ""), "") & vbCrLf &
                                    "     " & If(ExtraCold > 0, "\AQUA冷饮 " & ExtraCold.ToString.PadLeft(2, " "), "     ") & "   \BLUEMP " & Mp.ToString.PadLeft(4, " ") & "/" & MpMax.ToString.PadLeft(4, " ") & "\WHITE   DEF " & GetRealDef(True).ToString.PadLeft(4, " ") & If(ExtraDef > 0, "\GREEN+" & ExtraDef.ToString.PadLeft(3, ""), ""))
        SetText(FrmMain.TextStatusRight, "\GRAY" & GetLevelName(Level))
        '主要部分
        Select Case Screen
            Case Screens.Empty
                SetText(FrmMain.TextTitle, "")
                SetText(FrmMain.TextAction, "")
                SetText(FrmMain.TextInfo, "")
            Case Screens.Combat
                SetText(FrmMain.TextTitle, "\ORANGE※ 战斗 ※")
                SetText(FrmMain.TextAction,
                        GetKeyText("ATK") & " 攻击\n\n" &
                        GetKeyText("DEF") & " 防御\n\n" &
                        GetKeyText("MAG") & " 法术\n\n" &
                        GetKeyText("ITM") & " 道具\n\n" &
                        GetKeyText("EQU") & " 装备\n")
                SetText(FrmMain.TextInfo, CombatInfo)
            Case Screens.Select
                SetText(FrmMain.TextTitle, "\ORANGE※ 选择" & ScreenTitle & "目标 ※")
                SetText(FrmMain.TextAction,
                        GetKeyText("ESC") & " 返回")
                SetText(FrmMain.TextInfo, SelectInfo)
            Case Screens.Magic
                SetText(FrmMain.TextTitle, "\ORANGE※ 法术 ※")
                SetText(FrmMain.TextAction,
                        GetKeyText("ESC") & " 返回")
                SetText(FrmMain.TextInfo, MagicInfo)
            Case Screens.Item
                SetText(FrmMain.TextTitle, "\ORANGE※ 道具 ※")
                SetText(FrmMain.TextAction,
                        GetKeyText("ESC") & " 返回")
                SetText(FrmMain.TextInfo, ItemInfo)
            Case Screens.Equip
                SetText(FrmMain.TextTitle, "\ORANGE※ 装备 ※")
                SetText(FrmMain.TextAction,
                        GetKeyText("ESC") & " 返回")
                SetText(FrmMain.TextInfo, EquipInfo)
        End Select

    End Sub

    '玩家的回合结束
    Public MonsterTurnPerformed As List(Of Boolean)
    Public Sub TurnEnd()
        If MonsterTurnPerformed Is Nothing Then MonsterTurnPerformed = New List(Of Boolean) From {False, False, False, False, False, False, False}
        Screen = Screens.Combat
        '检查怪物死亡
        For i = 0 To MonsterType.Count - 1
            If MonsterHp(i) = 0 Then
                StartChat({"* " & MonsterName(i) & "倒下了！", "/TURNEND"}, True, False)
                MonsterType.RemoveAt(i)
                MonsterName.RemoveAt(i)
                MonsterTurnPerformed.RemoveAt(i)
                MonsterHp.RemoveAt(i)
                MonsterSp.RemoveAt(i)
                Exit Sub
            End If
        Next
        '检查玩家死亡
        If Hp = 0 Then
            Screen = Screens.Empty
            StartChat({"* 你死了！", "* 但你的灵魂还不愿放弃……", "* 光明再次在你的眼前浮现。", "/RESET"}, True, True)
            Exit Sub
        End If
        '检查玩家获胜
        If MonsterType.Count = 0 Then
            PerformLevelWin(Level)
            Exit Sub
        End If
        '怪物的回合
        For i = 0 To MonsterType.Count - 1
            If MonsterTurnPerformed(i) Then Continue For
            If PerformMonsterTurn(i) Then MonsterTurnPerformed(i) = True
            Exit Sub
        Next
        '下一回合
        MonsterTurnPerformed = Nothing
        ExtraAtk = 0 : ExtraDef = 0 : If ExtraCold > 0 Then ExtraCold -= 1
        Turn += 1
        StartChat({GetLevelIntros(Level)(Math.Min(4, Turn))}, False, False)
    End Sub

    '当前屏幕
    Public Screen As Screens = Screens.Empty
    Public ScreenReturn As Screens = Screens.Magic '用于选取对象
    Public ScreenData As String = "" '用于选取对象
    Public ScreenTitle As String = "" '用于选取对象
    Public Enum Screens
        Empty
        Combat
        Magic
        Item
        Equip
        [Select]
    End Enum

    '战斗
    Public Function CombatInfo() As String
        Dim Info As New List(Of String)
        For i = 0 To MonsterType.Count - 1
            Info.Add(GetItemText("ABCDEFG".ToCharArray()(i),
                                 MonsterName(i).PadRight(9, " ") & "\REDHP " & MonsterHp(i).ToString.PadLeft(4, " "),
                                 "\DARKGRAY" & GetMonsterDesc(MonsterType(i), MonsterSp(i))).Replace("KEY", "WHITE").Replace("YELLOW", "WHITE"))
            '& "        \DARKGRAYDEF " & GetMonsterDef(MonsterType(i)).ToString.PadLeft(4, " ")
        Next
        Return If(Join(Info.ToArray, vbCrLf), "")
    End Function

    '选取对象
    Public Function SelectInfo() As String
        Dim Info As New List(Of String)
        For i = 0 To MonsterType.Count - 1
            Info.Add(GetItemText("ABCDEFG".ToCharArray()(i),
                                 MonsterName(i).PadRight(9, " ") & "\REDHP " & MonsterHp(i).ToString.PadLeft(4, " "),
                                 "\DARKGRAY" & GetMonsterDesc(MonsterType(i), MonsterSp(i))))
        Next
        Return If(Join(Info.ToArray, vbCrLf), "")
    End Function
    Public Sub PerformSelect(Id As Integer)
        Select Case ScreenData
            Case "ATK"
                '攻击怪物
                Screen = Screens.Combat
                Dim Result = HurtMonster(Id, GetRealAtk(), If(EquipWeapon = 4, DamageType.光耀, DamageType.物理), False)
                StartChat({"* 你用" & GetEquipTitle(EquipWeapon) & "砍向了" & MonsterName(Id) & "！\n  " & Result(1) & "造成了" & Result(0) & "点" & If(EquipWeapon = 4, "光耀", "物理") & "伤害！", "/TURNEND"}, True, False)
            Case "ITEM4"
                '飞刀
                PerformItem(4, Id)
            Case "MAGIC2"
                '晦暗之触
                PerformMagic(2, Id)
            Case "MAGIC7"
                '绝影
                PerformMagic(7, Id)
        End Select
    End Sub

    '法术
    Public Function MagicInfo() As String
        Dim Info As New List(Of String)
        For i = 1 To 7
            Info.Add(GetItemText(i,
                                 GetMagicTitle(i).PadRight(9, " ") & If(GetMagicCost(i) > Mp, "\DARKRED", "\BLUE") & "MP " & GetMagicCost(i).ToString.PadLeft(4, " "),
                                 "\DARKGRAY" & GetMagicDesc(i)))
        Next
        Return Join(Info.ToArray, vbCrLf)
    End Function

    '物品
    Public Function ItemInfo() As String
        Dim Info As New List(Of String)
        For i = 1 To 7
            If ItemCount(i) = 0 Then
                Info.Add(GetItemText(i, "\GRAY无物品", ""))
            Else
                Info.Add(GetItemText(i,
                                     GetItemTitle(i).PadRight(6, " ") & "\GRAYx" & ItemCount(i),
                                     "\DARKGRAY" & GetItemDesc(i)))
            End If
        Next
        Return Join(Info.ToArray, vbCrLf)
    End Function

    '装备
    Public Function EquipInfo() As String
        Dim Info As New List(Of String)
        For i = 1 To 7
            Info.Add(GetItemText(i,
                                 GetEquipTitle(i).PadRight(6, " ") & If(EquipArmor = i OrElse EquipWeapon = i, "\BLUE <已装备>", ""),
                                 "\DARKGRAY" & GetEquipDesc(i)))
        Next
        Return Join(Info.ToArray, vbCrLf)
    End Function

End Module
