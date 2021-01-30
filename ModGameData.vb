Public Module ModGameData

    '法术
    Public Function GetMagicTitle(Id As Integer) As String
        Select Case Id
            Case 1
                Return "法术1"
            Case 2
                Return "法术2"
            Case 3
                Return "法术3"
            Case 4
                Return "法术4"
            Case 5
                Return "法术5"
            Case 6
                Return "法术6"
            Case 7
                Return "法术7名称"
        End Select
    End Function
    Public Function GetMagicDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "法术1描述"
            Case 2
                Return "法术2描述"
            Case 3
                Return "法术3描述"
            Case 4
                Return "法术4描述"
            Case 5
                Return "法术5描述"
            Case 6
                Return "法术6描述"
            Case 7
                Return "法术7描述"
        End Select
    End Function
    Public Function GetMagicCost(Id As Integer) As Integer
        Select Case Id
            Case 1
                Return 10
            Case 2
                Return 20
            Case 3
                Return 30
            Case 4
                Return 50
            Case 5
                Return 100
            Case 6
                Return 200
            Case 7
                Return 900
        End Select
    End Function
    Public Sub UseMagic(Id As Integer)
        PerformMagic(Id, 0)
    End Sub
    Public Sub PerformMagic(Id As Integer, Target As Integer)
        Screen = Screens.Combat
        Mp -= GetMagicCost(Id)
        Dim RawText As String = "* 你施展了" & GetMagicTitle(Id) & "！"
        Select Case Id
            Case 1
            Case 2
            Case 3
            Case 4
            Case 5
            Case 6
            Case 7
        End Select
        StartChat({RawText, "/TURNEND"}, True)
    End Sub

    '道具
    Public Function GetItemTitle(Id As Integer) As String
        Select Case Id
            Case 1
                Return "道具a1"
            Case 2
                Return "道具2"
            Case 3
                Return "道具3"
            Case 4
                Return "道具4"
            Case 5
                Return "道具5"
            Case 6
                Return "道具6"
            Case 7
                Return "道具7名称"
        End Select
    End Function
    Public Function GetItemDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "道具1a描述"
            Case 2
                Return "道具2描述"
            Case 3
                Return "道具3描述"
            Case 4
                Return "道具4描述"
            Case 5
                Return "道具5描述"
            Case 6
                Return "道具6描述"
            Case 7
                Return "道具7描述"
        End Select
    End Function
    Public Sub UseItem(Id As Integer)
        Screen = Screens.Combat
        ItemCount(Id) -= 1
        Dim RawText As String = "* 你使用了" & GetItemTitle(Id) & "！"
        Select Case Id
            Case 1
            Case 2
            Case 3
            Case 4
            Case 5
            Case 6
            Case 7
        End Select
        StartChat({RawText, "/TURNEND"}, True)
    End Sub

    '装备
    Public Function GetEquipTitle(Id As Integer) As String
        Select Case Id
            Case 1
                Return "装备1a"
            Case 2
                Return "装备2"
            Case 3
                Return "装备3"
            Case 4
                Return "装备4"
            Case 5
                Return "装备5"
            Case 6
                Return "装备6"
            Case 7
                Return "装备7名称"
        End Select
    End Function
    Public Function GetEquipDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "装备1描述"
            Case 2
                Return "装备2描述"
            Case 3
                Return "装备3描述"
            Case 4
                Return "装备4描述"
            Case 5
                Return "装备5描述"
            Case 6
                Return "装备6描述"
            Case 7
                Return "装备7描述"
        End Select
    End Function
    Public Function GetEquipIsWeapon(Id As Integer) As Boolean
        Select Case Id
            Case 1
                Return True
            Case 2
                Return False
            Case 3
                Return True
            Case 4
                Return False
            Case 5
                Return False
            Case 6
                Return True
            Case 7
                Return False
        End Select
    End Function
    Public Function GetEquipData(Id As Integer) As Integer
        Select Case Id
            Case 1
                Return 10
            Case 2
                Return 20
            Case 3
                Return 30
            Case 4
                Return 50
            Case 5
                Return 100
            Case 6
                Return 200
            Case 7
                Return 900
        End Select
    End Function

    '怪物
    Public Function GetMonsterHp(Name As String) As Integer
        Select Case Name
            Case "史莱姆"
                Return 100
            Case "大史莱姆"
                Return 1000
            Case "苦力怕"
                Return 500
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterAtk(Name As String) As Integer
        Select Case Name
            Case "史莱姆"
                Return 500
            Case "大史莱姆"
                Return 1000
            Case "苦力怕"
                Return 10000
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterDef(Name As String) As Integer
        Select Case Name
            Case "史莱姆"
                Return 100
            Case "大史莱姆"
                Return 500
            Case "苦力怕"
                Return 100
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterSp(Name As String) As Integer
        Select Case Name
            Case "史莱姆"
                Return 0
            Case "大史莱姆"
                Return 0
            Case "苦力怕"
                Return 3
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterDesc(Name As String, Sp As Integer) As String
        Select Case Name
            Case "史莱姆"
                Return "史莱姆描述。"
            Case "大史莱姆"
                Return "大史莱姆描述。"
            Case "苦力怕"
                Return "将在" & Sp & "回合后爆炸。"
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function PerformMonsterTurn(Id As Integer) As Boolean
        Select Case MonsterType(Id)
            Case "史莱姆", "大史莱姆"
                PerformMonsterAttack(Id, "向你扑来！")
            Case "苦力怕"
                If MonsterSp(Id) = 1 Then
                    PerformMonsterAttack(Id, "爆炸了！")
                    MonsterType.RemoveAt(Id)
                    MonsterName.RemoveAt(Id)
                    MonsterHp.RemoveAt(Id)
                    MonsterSp.RemoveAt(Id)
                    MonsterTurnPerformed.RemoveAt(Id)
                    Return False
                Else
                    MonsterSp(Id) -= 1
                    StartChat({"* " & MonsterName(Id) & "正在嘶嘶作响……", "/TURNEND"}, True)
                End If
        End Select
        Return True
    End Function
    Private Sub PerformMonsterAttack(Id As Integer, Desc As String)
        Dim Damage As Integer = Math.Max(1, GetMonsterAtk(MonsterType(Id)) - GetRealDef())
        Hp = Math.Max(0, Hp - Damage)
        StartChat({"* " & MonsterName(Id) & Desc & "\n  你受到了" & Damage & "点伤害！", "/TURNEND"}, True)
    End Sub

    '关卡
    Public Levels As Integer() = {1, 2}
    Public Function GetLevelName(Id As Integer) As String
        Select Case Id
            Case 1
                Return "第1关"
            Case 2
                Return "第2关"
        End Select
    End Function
    Public Function GetLevelIntro(Id As Integer) As String()
        Select Case Id
            Case 1
                Return {"* 一场测试战斗。"}
            Case 2
                Return {"* 两场测试战斗。"}
        End Select
    End Function
    Public Function GetLevelIntro2(Id As Integer) As String()
        Select Case Id
            Case 1
                Return {"* 一场测试战斗还在进行。"}
            Case 2
                Return {"* 两场测试战斗还在进行。"}
        End Select
    End Function
    Public Function GetLevelMonsters(Id As Integer) As String()
        Select Case Id
            Case 1
                Return {"史莱姆", "史莱姆", "苦力怕"}
            Case 2
                Return {"史莱姆", "苦力怕"}
        End Select
    End Function
    Public Function GetLevelMonstersName(Id As Integer) As String()
        Select Case Id
            Case 1
                Return {"史莱姆1", "史莱姆2", "苦力怕"}
            Case 2
                Return {"史莱姆？", "苦力怕"}
        End Select
    End Function
    Public Sub PerformLevelWin(Id As Integer)
        Select Case Id
            Case 1
                Screen = Screens.Empty
                StartChat({"* 战斗结束！你获得了" & RandomInteger(1000, 5000) & "XP！", "/NEXTLEVEL"}, True)
            Case 2

        End Select
    End Sub

End Module
