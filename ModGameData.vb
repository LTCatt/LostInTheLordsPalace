Public Module ModGameData

    '伤害
    Public Enum DamageType
        Melee
        Distance
        Fire
        Ice
        Dark
        Light
        Absolute
    End Enum

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
        StartChat({RawText, "/TURNEND"}, True, False)
    End Sub

    '道具
    Public Function GetItemTitle(Id As Integer) As String
        Select Case Id
            Case 1
                Return "远古秘药"
            Case 2
                Return "秘药"
            Case 3
                Return "解毒药"
            Case 4
                Return "飞刀"
            Case 5
                Return "热饮"
            Case 6
                Return "回复药·G"
            Case 7
                Return "营养剂·G"
        End Select
    End Function
    Public Function GetItemDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "将HP与MP都完全回复的珍贵的药丸。"
            Case 2
                Return "将HP完全回复的珍贵的药丸。"
            Case 3
                Return "可解除中毒状态的蓝色药剂。"
            Case 4
                Return "对单个敌人造成15点物理伤害。"
            Case 5
                Return "获得5回合冷冻伤害抗性。"
            Case 6
                Return "可回复750HP的秘制草药。"
            Case 7
                Return "能让你提升到下一等级的珍贵药剂。"
        End Select
    End Function
    Public Sub UseItem(Id As Integer)
        If Id = 4 Then
            ScreenReturn = Screen
            Screen = Screens.Select
            ScreenData = "ITEM4"
            ScreenTitle = "飞刀的"
        Else
            PerformItem(Id, 0)
        End If
    End Sub
    Public Sub PerformItem(Id As Integer, Target As Integer)
        Screen = Screens.Combat
        ItemCount(Id) -= 1
        Dim RawText As String = "* 你使用了" & GetItemTitle(Id) & "！\n"
        Select Case Id
            Case 1
                Hp = HpMax : Mp = MpMax
                RawText += "  你的HP与MP完全恢复了！"
            Case 2
                Hp = HpMax
                RawText += "  你的HP完全恢复了！"
            Case 3
                RawText += "  但你目前并没有中毒。"
            Case 4
                Screen = Screens.Combat
                Dim Result = HurtMonster(Target, 15, DamageType.Melee)
                RawText = "* 你向" & MonsterName(Target) & "投出了飞刀！\n  " & Result(1) & "造成了" & Result(0) & "点伤害！"
            Case 5
                ExtraWarm = 5
                RawText += "  一股暖流浸润着你的五脏六腑。"
            Case 6
                Hp = Math.Min(Hp + 750, HpMax)
                RawText += "  你的HP恢复了750点！"
            Case 7
                RawText += "  你目前已经到达了最高等级，无法升级。"
        End Select
        StartChat({RawText, "/TURNEND"}, True, False)
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
                Return "炉锤战斧"
            Case 5
                Return "匿踪刺剑"
            Case 6
                Return "荆棘链甲"
            Case 7
                Return "冰霜护甲"
        End Select
    End Function
    Public Function GetEquipDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "精灵一族的传奇长剑，必须有强大的灵魂才可握持。ATK+2800。"
            Case 2
                Return "失落科技与魔法结合的究极护甲。ATK+300，DEF+1700。"
            Case 3
                Return "使用最坚固的金属打造而成的护甲。DEF+1200，免疫暴击。"
            Case 4
                Return "矮人族的至高杰作。ATK+2400，造成光耀伤害。"
            Case 5
                Return "用秘银精制的刺剑。ATK+1000，让你的隐匿检定带有优势。"
            Case 6
                Return "缠满倒刺的链甲。DEF+300，反弹受到的一半近战伤害。"
            Case 7
                Return "永冻不化的冰石制作的护甲。DEF+800，获得火焰伤害抗性。"
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
                Return True
            Case 5
                Return True
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
                Return 300
            Case 3
                Return 0
            Case 4
                Return 2400
            Case 5
                Return 1000
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
                Return 1700
            Case 3
                Return 1200
            Case 4
                Return 0
            Case 5
                Return 0
            Case 6
                Return 300
            Case 7
                Return 800
        End Select
    End Function

    '怪物
    Public Function GetMonsterHp(Name As String) As Integer
        Select Case Name
            Case "骷髅1"
                Return 50
            Case "骷髅2"
                Return 190
            Case "骑士1"
                Return 2000
            Case "魔王"
                Return 6666
            Case "苦力怕1"
                Return 20
            Case "苦力怕2"
                Return 10
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterAtk(Name As String) As Integer
        Select Case Name
            Case "骷髅1"
                Return 720
            Case "骷髅2"
                Return 800
            Case "骑士1"
                Return 2000
            Case "魔王"
                Return 3100
            Case "苦力怕1"
                Return 2150
            Case "苦力怕2"
                Return 2500
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterDef(Name As String) As Integer
        Select Case Name
            Case "骷髅1"
                Return 100
            Case "骷髅2"
                Return 200
            Case "骑士1"
                Return 2000
            Case "魔王"
                Return 2800
            Case "苦力怕1"
                Return 0
            Case "苦力怕2"
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
            Case "骑士1"
                Return 0
            Case "魔王"
                Return 0
            Case "苦力怕1"
                Return 3
            Case "苦力怕2"
                Return 2
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterInv(Name As String, Type As DamageType) As Double
        Select Case Name
            Case "骷髅1"
                Return If(Type = DamageType.Light, 2, If(Type = DamageType.Dark, 0.5, 1))
            Case "骷髅2"
                Return If(Type = DamageType.Light, 2, If(Type = DamageType.Dark, 0.5, 1))
            Case "骑士1"
                Return If(Type = DamageType.Light, 2, If(Type = DamageType.Dark, 0.5, 1))
            Case "魔王"
                Return If(Type = DamageType.Light, 2, If(Type = DamageType.Dark, 0.5, 1))
            Case "苦力怕1"
                Return 1
            Case "苦力怕2"
                Return 1
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
            Case "骑士1"
                Return "包裹在暗黑盔甲下的冷血骑士。"
            Case "魔王"
                Return "极致邪恶与极致黑暗的化身，万事万物的最终之敌。"
            Case "苦力怕1"
                Return "咝咝作响。将在" & Sp & "回合后爆炸。"
            Case "苦力怕2"
                Return "闪电中的毁灭化身。将在" & Sp & "回合后爆炸。"
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function PerformMonsterTurn(Id As Integer) As Boolean
        Select Case MonsterType(Id)
            Case "骷髅1"
                PerformMonsterAttack(Id, "挥剑向你砍来！", DamageType.Melee)
            Case "骷髅2"
                PerformMonsterAttack(Id, "挥起了它的巨剑！", DamageType.Melee)
            Case "骑士1"
                PerformMonsterAttack(Id, "将漆黑的战戟刺向你的胸口！", DamageType.Melee)
            Case "魔王"
                MonsterSp(Id) += 1
                Select Case MonsterSp(Id)
                    Case 1, 3
                        PerformMonsterAttack(Id, "抬起手，一道黑光闪过……", DamageType.Dark)
                    Case 2
                        Mp = 0
                        StartChat({"* " & MonsterName(Id) & "的双眼闪过摄人的紫光，\n  你的MP被抽光了！", "/TURNEND"}, True, False)
                    Case 4
                        FrmMain.PixelLevel = 2
                        StartChat({"* " & MonsterName(Id) & "似乎在酝酿着什么。\n  一阵强烈的不安在你的心中涌现。", "/TURNEND"}, True, False)
                    Case 5
                        FrmMain.PixelLevel = 3
                        MusicChange2("Prologue 2.mp3", 0, True)
                        StartChat({"* 一圈无形的波纹荡漾，席卷了你的全身。",
                           "/LOCK124579",
                           "* 在你眼里，似乎整个世界都在崩坏……",
                           "* 一切都在离你远去。",
                           "* 似乎有哪里不对。",
                           "/LOCKRBIGDWXKLA",
                           "* 波纹慢慢扩散……",
                           "* 一些你熟悉的事物似乎正在从你的身上被剥离。",
                           "* 知识，概念……",
                           "* 抽象的，难以理解的，超形上学的……",
                           "* 你到底失去了什么？",
                           "/WIN"}, True, True)
                    Case Else
                        Throw New Exception("未知的行动轮：" & MonsterSp(Id))
                End Select
            Case "苦力怕1"
                If MonsterSp(Id) = 1 Then
                    PerformMonsterAttack(Id, "爆炸了！", DamageType.Distance)
                    MonsterType.RemoveAt(Id)
                    MonsterName.RemoveAt(Id)
                    MonsterHp.RemoveAt(Id)
                    MonsterSp.RemoveAt(Id)
                    MonsterTurnPerformed.RemoveAt(Id)
                    Return False
                Else
                    MonsterSp(Id) -= 1
                    StartChat({"* " & MonsterName(Id) & "正在嘶嘶作响……", "/TURNEND"}, True, False)
                End If
            Case "苦力怕2"
                If MonsterSp(Id) = 1 Then
                    PerformMonsterAttack(Id, "引发了盛大的爆炸！", DamageType.Distance)
                    MonsterType.RemoveAt(Id)
                    MonsterName.RemoveAt(Id)
                    MonsterHp.RemoveAt(Id)
                    MonsterSp.RemoveAt(Id)
                    MonsterTurnPerformed.RemoveAt(Id)
                    Return False
                Else
                    MonsterSp(Id) -= 1
                    StartChat({"* " & MonsterName(Id) & "正与雷电起舞……", "/TURNEND"}, True, False)
                End If
        End Select
        Return True
    End Function
    Private Sub PerformMonsterAttack(Id As Integer, Desc As String, Type As DamageType)
        Dim Damage As Integer = Math.Max(1, GetMonsterAtk(MonsterType(Id)) - GetRealDef())
        Dim Result = HurtPlayer(Damage, Type)
        Dim BaseText As String = "* " & MonsterName(Id) & Desc & "\n  " & Result(1) & "你受到了" & Result(0) & "点伤害！"
        If EquipArmor = 6 AndAlso Type = DamageType.Melee Then
            '荆棘
            Dim BackDamage As Integer = Result(0) / 2
            BaseText += "\n  护甲上的荆棘反弹了" & BackDamage & "点伤害！"
            HurtMonster(Id, BackDamage, DamageType.Absolute)
        End If
        StartChat({BaseText, "/TURNEND"}, True, False)
    End Sub

    '关卡
    Public Function GetLevelName(Id As Integer) As String
        Select Case Id
            Case 100
                Return "魔宫入口"
            Case 101
                Return "魔宫大厅"
            Case 102
                Return "魔王房间"
            Case 1
                Return "魔宫入口？"
            Case 2
                Return "魔宫入口"
            Case 3
                Return "黑火药之厅1"
            Case 4
                Return "黑火药之厅2"
            Case 5
                Return "黑火药之厅3"
        End Select
    End Function
    Public Function GetLevelIntros(Id As Integer) As String()
        Select Case Id
            Case 100
                Return {"* 你早已为了今天做好了准备，\n  两只骷髅对已经到达99级的你来说毫无威胁。",
                        "* 无论发生什么，都无法阻挠你击败魔王的决心。",
                        "* 你已经站在了魔宫的入口。\n  魔王，大家的一生之敌，如今已近在眼前。",
                        "* 你早已决定不再犹豫。",
                        "* 你还在等待什么？"}
            Case 101
                MusicChange("Prologue 1.mp3", 0.2, True)
                Return {"* 所有怪物都在阻拦你奔向魔王房间的脚步。",
                        "* 离魔王的房间只差最后一步。",
                        "* 这些怪物对早已身经百战的你而言，根本不值一提。",
                        "* 你此前所经受的漫长磨练终于将走到尽头。",
                        "* 是时候为一切画下句号了。"}
            Case 102
                MusicChange("Prologue 1.mp3", 0, True)
                MusicChange2("Prologue 2.mp3", 0.2, True)
                Return {"* 你终于来到了魔王的面前。",
                        "* 宛如实质的黑暗在你的四周涌现。",
                        "* 传闻魔王可以操纵一切，魔力、能量，甚至是……",
                        "* 这不可能。\n  你不再愿去回想那些恐怖的传说。",
                        "* 魔王高举双手，大声诵念着你从未听过的咒文……"}
            Case 1
                MusicChange("Main 1.mp3", 0.1, True)
                Return {"* 你似乎回到了起点。",
                        "* 遗忘？还是剥离？这就是魔王的能力吗？"}
            Case 2
                MusicChange("Main 1.mp3", 0.2, True)
                Return {"* 三具骷髅已经将你包围。",
                        "* 骷骨的响声宛如一首嘈杂的打击乐。",
                        "* 骷髅们在用颅骨思考为什么勇者一直不进行攻击。",
                        "* 你忍耐着肉体的痛楚。",
                        "* 骨头喀拉作响。"}
            Case 3
                Return {"* 前方是火药与爆炸之厅。",
                        "* 移动的坟墓正在靠近……",
                        "* 倒计时已经数到了「1」。",
                        "* 爆炸，硝烟，艺术！",
                        "* 火药的气味在空中久久不散。"}
            Case 4
                Return {"* 毁灭来临。",
                        "* 电弧碰撞的火光在空气中迸溅。",
                        "* 雷电让爆炸的威能也得以跃升。"}
            Case 5
                Return {"* 而在出口门前，\n  是一场盛大的送别庆典。",
                        "* 但凡能穿上以太之甲，这一切就都应该不在话下……",
                        "* 它用空洞的眼窝盯着你。"}
        End Select
    End Function
    Public Function GetLevelMonsters(Id As Integer) As String()
        Select Case Id
            Case 100
                Return {"骷髅1", "骷髅1"}
            Case 101
                Return {"骷髅1", "骷髅2", "骷髅2", "骑士1"}
            Case 102
                Return {"魔王"}
            Case 1
                Return {"骷髅1", "骷髅1"}
            Case 2
                Return {"骷髅2", "骷髅2", "骷髅1"}
            Case 3
                Return {"苦力怕1", "骷髅2"}
            Case 4
                Return {"苦力怕2", "苦力怕1", "苦力怕1"}
            Case 5
                Return {"苦力怕1", "苦力怕2", "苦力怕2", "苦力怕2", "苦力怕1"}
        End Select
    End Function
    Public Function GetLevelMonstersName(Id As Integer) As String()
        Select Case Id
            Case 100
                Return {"骷髅士兵", "骷髅卫兵"}
            Case 101
                Return {"骷髅士兵", "粗骨骷髅士兵", "粗骨骷髅卫士", "暗黑骑士"}
            Case 102
                Return {"魔王"}
            Case 1
                Return {"骷髅士兵", "骷髅卫兵"}
            Case 2
                Return {"粗骨骷髅士兵", "粗骨骷髅卫士", "骷髅守卫"}
            Case 3
                Return {"爬行冢", "粗骨骷髅战士"}
            Case 4
                Return {"闪电爬行冢", "爬行墓", "爬行冢"}
            Case 5
                Return {"爬行冢", "闪电爬行冢", "雷电爬行冢", "高压爬行冢", "爬行墓"}
        End Select
    End Function
    Public Sub PerformLevelWin(Id As Integer)
        '存档
        ItemCountLast = ItemCount
        EquipWeaponLast = EquipWeapon
        EquipArmorLast = EquipArmor
        '回血
        Hp = HpMax : Mp = MpMax
        '切换关卡
        Screen = Screens.Empty
        Select Case Id
            Case 100
                StartChat({"* 恭喜获胜！你获得了620XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 你在魔宫之中飞速穿梭，一只只怪物在你的剑下飞灰烟灭。",
                           "* 魔王的房间已经近在咫尺。",
                           "* 这片大地所承受的苦难终要走向尽头。",
                           "/LEVEL101"}, True, False)
            Case 101
                StartChat({"* 恭喜获胜！你获得了3855XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 你迈步走向下一个路口，推开大门……",
                           "* 奇异的紫色光芒涌现。",
                           "* 这一刻，你知道你漫长的旅程终于走到了终点。",
                           "/LEVEL102"}, True, False)
            Case 102
                FrmMain.PixelLevel = 1
                MusicChange("Main 1.mp3", 0, False)
                StartChat({"* ……",
                           "* …………",
                           "* ………………",
                           "* 当你再次睁开眼……",
                           "/LEVEL1"}, True, True)
            Case 1
                StartChat({"* 恭喜获胜！你获得了620XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 顷刻间，那些骷髅刚才造成的伤痕已经消失不见。",
                           "* 守护……防御……",
                           "* 在沉思中，你回想起了你来到这里的初衷。",
                           "/UNLOCKD",
                           "* 「D」的内联逻辑已恢复。",
                           "* 你抬起头……",
                           "/LEVEL2"}, True, False)
            Case 2
                StartChat({"* 恭喜获胜！你获得了1205XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 你决定重新踏上魔宫之旅。",
                           "* 再次出发。新生，即是新的希望。\n  伊尔梅特，再次开始的机会。",
                           "/UNLOCKR",
                           "* 「R」的内联逻辑已恢复。",
                           "* 你迈步走入魔宫入口的台阶……",
                           "/LEVEL3"}, True, False)
            Case 3
                StartChat({"* 恭喜获胜！你获得了860XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 所幸，你的行囊还没有因为爆炸损坏。",
                           "/UNLOCKI",
                           "* 「I」的内联逻辑已恢复。",
                           "* 你刚整理好行囊，就听到了嗞啦作响的电弧声。",
                           "/LEVEL4"}, True, False)
            Case 4
                StartChat({"* 恭喜获胜！你获得了1755XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 但凡能再多一点助力，刚才的战斗也不至如此艰辛。",
                           "/UNLOCK4",
                           "* 「4」的内联逻辑已恢复。",
                           "* 看上去你走到了黑火药之厅的尽头。",
                           "/LEVEL5"}, True, False)
            Case 5
                StartChat({"* 恭喜获胜！你获得了2090XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 你离开了黑火药之厅。\n  在你的前方，是元素与魔法的领域。",
                           "* 但这并不是法术应当被使用的地方。",
                           "/UNLOCKA",
                           "* 「A」的内联逻辑已恢复。",
                           "* 没有下一关了。",
                           "* 真的没有下一关了。"}, True, False)
        End Select
    End Sub

End Module
