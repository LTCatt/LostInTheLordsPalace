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
                Return "月刃"
            Case 2
                Return "以太之甲"
            Case 3
                Return "精金板甲"
            Case 4
                Return "装备4"
            Case 5
                Return "装备5"
            Case 6
                Return "荆棘链甲"
            Case 7
                Return "装备7名称"
        End Select
    End Function
    Public Function GetEquipDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "精灵一族的传奇之剑，仅有精灵敬重之人才可握持。ATK+2800。"
            Case 2
                Return "失落科技与魔法结合的究极护甲。ATK+100，DEF+1800。"
            Case 3
                Return "使用最坚固的金属打造而成的护甲。DEF+1200，免疫暴击。"
            Case 4
                Return "装备4描述"
            Case 5
                Return "装备5描述"
            Case 6
                Return "缠满倒刺的链甲。DEF+300，反弹受到的一半近战伤害。"
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
                Return False
            Case 4
                Return False
            Case 5
                Return False
            Case 6
                Return False
            Case 7
                Return False
        End Select
    End Function
    Public Function GetEquipAtk(Id As Integer) As Integer
        Select Case Id
            Case 1
                Return 2800
            Case 2
                Return 100
            Case 3
                Return 0
            Case 4
                Return 0
            Case 5
                Return 0
            Case 6
                Return 0
            Case 7
                Return 0
        End Select
    End Function
    Public Function GetEquipDef(Id As Integer) As Integer
        Select Case Id
            Case 1
                Return 0
            Case 2
                Return 1800
            Case 3
                Return 1200
            Case 4
                Return 0
            Case 5
                Return 0
            Case 6
                Return 300
            Case 7
                Return 0
        End Select
    End Function

    '怪物
    Public Function GetMonsterHp(Name As String) As Integer
        Select Case Name
            Case "骷髅1"
                Return 50
            Case "骷髅2"
                Return 150
            Case "苦力怕"
                Return 100
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterAtk(Name As String) As Integer
        Select Case Name
            Case "骷髅1"
                Return 750
            Case "骷髅2"
                Return 800
            Case "苦力怕"
                Return 100
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterDef(Name As String) As Integer
        Select Case Name
            Case "骷髅1"
                Return 10
            Case "骷髅2"
                Return 20
            Case "苦力怕"
                Return 0
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterSp(Name As String) As Integer
        Select Case Name
            Case "骷髅1"
                Return 0
            Case "骷髅2"
                Return 0
            Case "苦力怕"
                Return 3
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterDesc(Name As String, Sp As Integer) As String
        Select Case Name
            Case "骷髅1"
                Return "攻击性强，但极度脆弱的敌人。"
            Case "骷髅2"
                Return "骨架更加粗壮的骷髅。"
            Case "苦力怕"
                Return "将在" & Sp & "回合后爆炸。"
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function PerformMonsterTurn(Id As Integer) As Boolean
        Select Case MonsterType(Id)
            Case "骷髅1"
                PerformMonsterAttack(Id, "挥剑向你砍来！", True)
            Case "骷髅2"
                PerformMonsterAttack(Id, "挥起了它的巨剑！", True)
            Case "苦力怕"
                If MonsterSp(Id) = 1 Then
                    PerformMonsterAttack(Id, "爆炸了！", True)
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
    Private Sub PerformMonsterAttack(Id As Integer, Desc As String, IsMeele As Boolean)
        Dim Damage As Integer = Math.Max(1, GetMonsterAtk(MonsterType(Id)) - GetRealDef())
        Hp = Math.Max(0, Hp - Damage)
        Dim BaseText As String = "* " & MonsterName(Id) & Desc & "\n  你受到了" & Damage & "点伤害！"
        If EquipArmor = 6 AndAlso IsMeele Then
            '荆棘
            Dim BackDamage As Integer = Damage / 2
            BaseText += "\n  护甲上的荆棘对攻击者造成了" & BackDamage & "点伤害！"
            HurtMonster(Id, BackDamage)
        End If
        StartChat({BaseText, "/TURNEND"}, True)
    End Sub

    '关卡
    Public Levels As Integer() = {1, 2}
    Public Function GetLevelName(Id As Integer) As String
        Select Case Id
            Case 1
                Return "测试关卡1"
            Case 2
                Return "测试关卡2"
        End Select
    End Function
    Public Function GetLevelIntro(Id As Integer) As String()
        Select Case Id
            Case 1
                Return {"* 两只对99级的勇者而言毫无威胁的骷髅袭来。"}
            Case 2
                Return {"* 竟然还有更多的骷髅……"}
        End Select
    End Function
    Public Function GetLevelIntro2(Id As Integer) As String()
        Select Case Id
            Case 1
                Return {"* 骷髅的骨头在喀拉作响。"}
            Case 2
                Return {"* 骷髅们在用颅骨思考为什么勇者一直不进行攻击。"}
        End Select
    End Function
    Public Function GetLevelMonsters(Id As Integer) As String()
        Select Case Id
            Case 1
                Return {"骷髅1", "骷髅1"}
            Case 2
                Return {"骷髅2", "骷髅2", "骷髅1"}
        End Select
    End Function
    Public Function GetLevelMonstersName(Id As Integer) As String()
        Select Case Id
            Case 1
                Return {"骷髅士兵", "骷髅卫兵"}
            Case 2
                Return {"精英骷髅士兵", "精英骷髅卫兵", "骷髅守卫"}
        End Select
    End Function
    Public Sub PerformLevelWin(Id As Integer)
        Select Case Id
            Case 1
                Screen = Screens.Empty
                StartChat({"* 恭喜获胜！你获得了620XP！", "/UNLOCKW", "* 你找回了W按键！", "/NEXTLEVEL"}, True)
            Case 2
                Screen = Screens.Empty
                StartChat({"* 恭喜获胜！你获得了1205XP！", "/UNLOCKR", "* 你找回了R按键！", "/NEXTLEVEL"}, True)
        End Select
    End Sub

End Module
