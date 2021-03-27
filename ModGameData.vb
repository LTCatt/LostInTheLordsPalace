Public Module ModGameData

    '伤害类型
    Public Enum DamageType
        物理
        爆炸
        火焰
        冷冻
        黯蚀
        光耀
        真实
    End Enum

    '法术（总 MP 638）
    Public Function GetMagicTitle(Id As Integer) As String
        Select Case Id
            Case 1
                Return "真银风暴"
            Case 2
                Return "暗夜魅影"
            Case 3
                Return "天坠之火"
            Case 4
                Return "乌萨战吼"
            Case 5
                Return "恶鬼之力"
            Case 6
                Return "教条力场"
            Case 7
                Return "绝影"
        End Select
    End Function
    Public Function GetMagicDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "刮起一阵银灰色的利刃风暴，对全体敌人造成600点物理伤害。"
            Case 2
                Return "引导黑暗能量，对单个敌人造成1200点黯蚀伤害。"
            Case 3
                Return "召唤熊熊燃烧的陨石坠落地面，对全体敌人造成250点火焰伤害。"
            Case 4
                Return "在敌人的压迫下激发斗志，恢复百分之40的MP。"
            Case 5
                Return "当前回合ATK提升300。施展此法术不会使回合结束。"
            Case 6
                Return "当前回合DEF提升500，且恢复400点HP。"
            Case 7
                Return "在魔力的推动下，以三分之一的ATK进行四次连续攻击。"
        End Select
    End Function
    Public Function GetMagicCost(Id As Integer) As Integer
        Select Case Id
            Case 1
                Return 550
            Case 2
                Return 410
            Case 3
                Return 230
            Case 4
                Return 0
            Case 5
                Return 180
            Case 6
                Return 160
            Case 7
                Return 80
        End Select
    End Function
    Public Sub UseMagic(Id As Integer)
        If Id = 2 Then
            ScreenReturn = Screen
            Screen = Screens.Select
            ScreenData = "MAGIC2"
            ScreenTitle = "暗夜魅影的"
        ElseIf Id = 7 Then
            ScreenReturn = Screen
            Screen = Screens.Select
            ScreenData = "MAGIC7"
            ScreenTitle = "绝影的"
        Else
            PerformMagic(Id, 0)
        End If
    End Sub
    Public Sub PerformMagic(Id As Integer, Target As Integer)
        Mp -= GetMagicCost(Id)
        Dim RawText As String = "* 你施展了" & GetMagicTitle(Id) & "！\n"
        Select Case Id
            Case 1
                Screen = Screens.Combat
                Dim ChatList As New List(Of String)
                '0-2
                For i = 0 To Math.Min(2, MonsterHp.Count - 1)
                    Dim Result = HurtMonster(i, 600, DamageType.物理, True)
                    RawText += "  " & Result(1) & MonsterName(i) & "受到了" & Result(0) & "点伤害！\n"
                Next
                ChatList.Add(RawText)
                '3-6
                If MonsterHp.Count > 3 Then
                    RawText = ""
                    For i = 3 To MonsterHp.Count - 1
                        Dim Result = HurtMonster(i, 600, DamageType.物理, True)
                        RawText += "  " & Result(1) & MonsterName(i) & "受到了" & Result(0) & "点伤害！\n"
                    Next
                    ChatList.Add("*" & RawText.Substring(1))
                End If
                '输出
                ChatList.Add("/TURNEND")
                StartChat(ChatList.ToArray, True, False)
                Exit Sub
            Case 2
                Screen = Screens.Combat
                Dim Result = HurtMonster(Target, 1200, DamageType.黯蚀, True)
                RawText += "  " & Result(1) & MonsterName(Target) & "受到了" & Result(0) & "点伤害！"
            Case 3
                Screen = Screens.Combat
                Dim ChatList As New List(Of String)
                '0-2
                For i = 0 To Math.Min(2, MonsterHp.Count - 1)
                    Dim Result = HurtMonster(i, 250, DamageType.火焰, True)
                    RawText += "  " & Result(1) & MonsterName(i) & "受到了" & Result(0) & "点伤害！\n"
                Next
                ChatList.Add(RawText)
                '3-6
                If MonsterHp.Count > 3 Then
                    RawText = ""
                    For i = 3 To MonsterHp.Count - 1
                        Dim Result = HurtMonster(i, 250, DamageType.火焰, True)
                        RawText += "  " & Result(1) & MonsterName(i) & "受到了" & Result(0) & "点伤害！\n"
                    Next
                    ChatList.Add("*" & RawText.Substring(1))
                End If
                '输出
                ChatList.Add("/TURNEND")
                StartChat(ChatList.ToArray, True, False)
                Exit Sub
            Case 4
                Screen = Screens.Combat
                Dim Delta As Integer = MpMax * 0.4 'MonsterName.Count * 150
                Mp = Math.Min(MpMax, Mp + Delta)
                RawText += "  你的MP恢复了" & Delta & "！"
            Case 5
                Screen = Screens.Combat
                If ExtraAtk = 0 Then
                    RawText += "  你感觉自己的肌肉中充盈着力量。"
                Else
                    RawText += "  恶鬼之力的效果不能在一回合中叠加。"
                End If
                ExtraAtk = 300
                StartChat({RawText}, False, False)
                Exit Sub
            Case 6
                Screen = Screens.Combat
                RawText += "  一道无形的屏障环绕在你的周身。"
                ExtraDef += 500
                Hp = Math.Min(HpMax, Hp + 400)
            Case 7
                Screen = Screens.Combat
                Dim Result = HurtMonster(Target, GetRealAtk() / 3, If(EquipWeapon = 4, DamageType.光耀, DamageType.物理), False)
                HurtMonster(Target, GetRealAtk() / 3, If(EquipWeapon = 4, DamageType.光耀, DamageType.物理), False)
                HurtMonster(Target, GetRealAtk() / 3, If(EquipWeapon = 4, DamageType.光耀, DamageType.物理), False)
                HurtMonster(Target, GetRealAtk() / 3, If(EquipWeapon = 4, DamageType.光耀, DamageType.物理), False)
                RawText += "  " & Result(1) & "4次攻击共使" & MonsterName(Target) & "受到了" & (Result(0) * 4) & "点伤害！"
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
                Return "冷饮"
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
                Return "对单个敌人造成50点物理伤害。"
            Case 5
                Return "获得10回合火焰伤害抗性。"
            Case 6
                Return "可回复800HP的秘制草药。"
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
                Dim Result = HurtMonster(Target, 50, DamageType.物理, True)
                RawText = "* 你向" & MonsterName(Target) & "投出了飞刀！\n  " & Result(1) & "造成了" & Result(0) & "点伤害！"
            Case 5
                ExtraCold = 10
                RawText += "  一阵凉意浸透了你的五脏六腑。"
            Case 6
                Hp = Math.Min(Hp + 800, HpMax)
                RawText += "  你的HP恢复了800点！"
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
                Return "精灵一族的传奇长剑，必须有强大的灵魂才可握持。ATK+1200。"
            Case 2
                Return "失落科技与魔法结合的至高护甲。ATK+500，DEF+1800。"
            Case 3
                Return "使用世界上最坚固的金属打造而成的护甲。DEF+1200。"
            Case 4
                Return "矮人族的至高杰作。ATK+800，将伤害属性变为光耀。"
            Case 5
                Return "用秘银精制的刺剑。ATK+600，让你的隐匿检定带有优势。"
            Case 6
                Return "缠满倒刺的魔法链甲。DEF+300，反弹受到的一半伤害。"
            Case 7
                Return "永冻不化的冰石制作的护甲。DEF+1050，获得火焰伤害抗性。"
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
                Return 1200
            Case 2
                Return 500
            Case 3
                Return 0
            Case 4
                Return 800
            Case 5
                Return 600
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
                Return 1050
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
                Return 600
            Case "骑士2"
                Return 1000
            Case "冰1"
                Return 700
            Case "火1"
                Return 400
            Case "魔王1", "魔王2"
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
                Return 1770
            Case "骑士2"
                Return 1550
            Case "冰1"
                Return 400
            Case "火1"
                Return 300
            Case "魔王1"
                Return 2400
            Case "魔王2"
                Return 2450
            Case "苦力怕1"
                Return 2150
            Case "苦力怕2"
                Return 2550
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function GetMonsterDef(Name As String) As Integer
        Select Case Name
            Case "骷髅1"
                Return 300
            Case "骷髅2"
                Return 600
            Case "骑士1"
                Return 1150
            Case "骑士2"
                Return 1100
            Case "冰1"
                Return 0
            Case "火1"
                Return 0
            Case "魔王1", "魔王2"
                Return 1700
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
            Case "苦力怕1"
                Return 3
            Case "苦力怕2"
                Return 2
            Case Else
                Return 0
        End Select
    End Function
    Public Function GetMonsterInv(Name As String, Type As DamageType) As Double
        Select Case Name
            Case "骷髅1"
                Return If(Type = DamageType.光耀, 2, If(Type = DamageType.黯蚀, 0.2, 1))
            Case "骷髅2"
                Return If(Type = DamageType.光耀, 2, If(Type = DamageType.黯蚀, 0.2, 1))
            Case "骑士1"
                Return If(Type = DamageType.光耀, 2, If(Type = DamageType.黯蚀, 0.2, 1))
            Case "骑士2"
                Return If(Type = DamageType.光耀, 2, If(Type = DamageType.黯蚀, 0.2, 1))
            Case "冰1"
                Return If(Type = DamageType.火焰, 2, If(Type = DamageType.冷冻 OrElse Type = DamageType.物理, 0, 1))
            Case "火1"
                Return If(Type = DamageType.冷冻, 2, If(Type = DamageType.火焰 OrElse Type = DamageType.物理, 0, 1))
            Case "魔王1", "魔王2"
                Return If(Type = DamageType.光耀, 2, If(Type = DamageType.黯蚀, 0, 1))
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
                Return "包裹在漆黑盔甲下的冷血剑士。"
            Case "骑士2"
                Select Case Sp
                    Case 0
                        Return "逐渐蓄力，然后一举击溃敌人的精锐骑士。"
                    Case 1
                        Return "盔甲下的无名骑士正在调整自己的状态。"
                    Case 2
                        Return "\DIMRED他与战马都紧绷起肌肉，即将发起冲锋。"
                End Select
            Case "冰1"
                Return "在至净的凝冰里孕育的魂灵。"
            Case "火1"
                Return "从炼狱的真火中成长的精魄。"
            Case "魔王1", "魔王2"
                Return "极致邪恶与极致黑暗的化身，万事万物的最终之敌。"
            Case "苦力怕1"
                If Sp = 1 Then
                    Return "\DIMRED它的身躯逐渐膨胀、发光、闪烁……"
                Else
                    Return "咝咝作响。将在" & Sp & "回合后爆炸。"
                End If
            Case "苦力怕2"
                If Sp = 1 Then
                    Return "\DIMRED它急迫地想让你闻到硝烟的气息。"
                Else
                    Return "闪电中的毁灭化身。将在" & Sp & "回合后爆炸。"
                End If
            Case Else
                Throw New Exception("未知的怪物：" & Name)
        End Select
    End Function
    Public Function PerformMonsterTurn(Id As Integer) As Boolean
        Select Case MonsterType(Id)
            Case "骷髅1"
                PerformMonsterAttack(Id, "挥剑向你砍来！", DamageType.物理)
            Case "骷髅2"
                PerformMonsterAttack(Id, "挥起了它的巨剑！", DamageType.物理)
            Case "骑士1"
                PerformMonsterAttack(Id, "将漆黑的长剑刺向你的胸口！", DamageType.物理)
            Case "骑士2"
                MonsterSp(Id) += 1
                Select Case MonsterSp(Id)
                    Case 1
                        PerformMonsterAttack(Id, "将漆黑的长戟挥砍向你！", DamageType.物理)
                    Case 2
                        StartChat({"* " & MonsterName(Id) & "正在准备冲刺……", "/TURNEND"}, True, False)
                    Case 3
                        MonsterSp(Id) = 0
                        PerformMonsterAttack(Id, "驱使起战马，全力向你发起冲锋！", DamageType.物理, GetMonsterAtk(MonsterType(Id)) + 700)
                    Case Else
                        Throw New Exception("未知的行动轮：" & MonsterSp(Id))
                End Select
            Case "冰1"
                PerformMonsterAttack(Id, "喷射出一道冰柱！", DamageType.冷冻, IgnoreDef:=True)
            Case "火1"
                PerformMonsterAttack(Id, "将四周化作火海！", DamageType.火焰, IgnoreDef:=True)
            Case "魔王2"
                MonsterSp(Id) += 1
                '-MM-FF-A- MM--SS-FF-A- MM-SS-FFA- -SSMM--FFA- MMSS-A- FFMM-SSA- MM--SSSSA- 
                Select Case MonsterSp(Id) Mod 10000
                    Case 2, 10, 22, 35, 43, 52, 59, 69
                        '准备清空 MP
                        MusicChange("Win.mp3", 0, False)
                        StartChat({"* 魔王张开双臂，蓝色的光点渐渐从你体内渗出……", "/TURNEND"}, True, False)
                    Case 3, 11, 23, 36, 44, 53, 60, 70
                        '进行清空 MP
                        Mp = 0
                        StartChat({"* 随着蓝色光点渗出，你感觉一阵头晕目眩。\n  你失去了所有MP！", "/TURNEND"}, True, False)
                    Case 5, 17, 28, 39, 50
                        '准备火焰攻击
                        StartChat({"* 耀眼的橙色光点如星光一般在整个厅堂闪耀……", "/TURNEND"}, True, False)
                    Case 6, 18, 29, 40, 51
                        '进行火焰攻击
                        PerformMonsterAttack(Id, "一声喝下，漫天光点化为了无边无际的烈焰！", DamageType.火焰, IgnoreDef:=True, CustomAttack:=1.5 * (GetMonsterAtk(MonsterType(Id)) + MonsterSp(Id) / 10000 * 140))
                    Case 8, 20, 30, 41, 48, 57, 67, 72, 81
                        '加强攻击力
                        MonsterSp(Id) += 10000
                        StartChat({"* 魔王集中精力，汇聚无边虚空中的能量补充之前的消耗。\n  魔王的攻击变得更强了！\n* 魔王已经强化了" & Math.Floor(MonsterSp(Id) / 10000) & "次……", "/TURNEND"}, True, False)
                    Case 14, 25, 33, 45, 55, 63, 65, 76, 79
                        '准备重击
                        StartChat({"* 宛如实质的黑暗牵扯成丝线涌向魔王，\n  在他的手中缓缓凝结……", "/TURNEND"}, True, False)
                    Case 15, 26, 34, 46, 56, 64, 66, 77, 80
                        '进行重击
                        PerformMonsterAttack(Id, "手中汇聚出由无尽黑暗编织的巨矛，向你闪电般掷去！", DamageType.黯蚀, CustomAttack:=GetMonsterAtk(MonsterType(Id)) + MonsterSp(Id) / 10000 * 140 + 850)
                    Case Else
                        '普通攻击
                        PerformMonsterAttack(Id, "抬起右手，一道黑光闪过……", DamageType.黯蚀, CustomAttack:=GetMonsterAtk(MonsterType(Id)) + MonsterSp(Id) / 10000 * 140)
                End Select
            Case "魔王1"
                MonsterSp(Id) += 1
                Select Case MonsterSp(Id)
                    Case 2
                        MusicChange("Boss 3.mp3", 0, True) '4
                        '准备清空 MP
                        StartChat({"* 魔王张开双臂，蓝色的光点渐渐从你体内渗出……", "/TURNEND"}, True, False)
                    Case 3
                        '进行清空 MP
                        Mp = 0
                        StartChat({"* 随着蓝色光点渗出，你感觉一阵头晕目眩。\n  你失去了所有MP！", "/TURNEND"}, True, False)
                    Case 5
                        '准备重击
                        StartChat({"* 宛如实质的黑暗牵扯成丝线涌向魔王，\n  在他的手中缓缓凝结……", "/TURNEND"}, True, False)
                    Case 6
                        '进行重击
                        PerformMonsterAttack(Id, "手中汇聚出由无尽黑暗编织的巨矛，向你闪电般掷去！", DamageType.黯蚀, CustomAttack:=GetMonsterAtk(MonsterType(Id)) + 1000)
                    Case 8
                        MusicChange2("Boss 2.mp3", 0, True) '3
                        MusicChange("Boss 3.mp3", 0.4, True) '5
                        FrmMain.PixelLevel = 2
                        StartChat({"* 魔王高举起双臂，大声吟唱着刺耳的咒文。", "* 一股无形的风在宫殿中呼啸。", "* 强烈的不安在你的心中涌现。", "/TURNEND"}, True, False)
                    Case 9
                        FrmMain.PixelLevel = 3
                        MusicChange("Boss 3.mp3", 0.05, True) '6
                        StartChat({"* 周围的空气仿佛都凝固了。",
                           "* 一圈无形的波纹荡漾，席卷了你的全身。",
                           "/LOCK0124579RBIGDWXKL",
                           "* 在你眼里，似乎整个世界都在崩坏……",
                           "* 一切都在离你远去。",
                           "* 似乎有哪里不对。",
                           "* 波纹还在慢慢扩散……",
                           "* 一些你熟悉的事物似乎正在从你的身上剥离。",
                           "* 知识，概念……",
                           Text.Encoding.UTF8.GetString(Convert.FromBase64String("KiBLe3pqbm5uO2E3SH3igKbigKY=")),
                           "* 抽象的，难以理解的，超形上学的……",
                           "* ……",
                           "* 你到底失去了什么？",
                           "/WIN"}, True, True)
                    Case Else
                        '普通攻击
                        PerformMonsterAttack(Id, "抬起右手，一道黑光闪过……", DamageType.黯蚀, CustomAttack:=GetMonsterAtk(MonsterType(Id)) + 200)
                End Select
            Case "苦力怕1"
                If MonsterSp(Id) = 1 Then
                    PerformMonsterAttack(Id, "爆炸了！", DamageType.爆炸)
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
                    PerformMonsterAttack(Id, "引发了盛大的爆炸！", DamageType.爆炸)
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
    Private Sub PerformMonsterAttack(Id As Integer, Desc As String, Type As DamageType, Optional CustomAttack As Integer = -1, Optional IgnoreDef As Boolean = False)
        Dim Damage As Integer = Math.Max(1, If(CustomAttack > 0, CustomAttack, GetMonsterAtk(MonsterType(Id))) - If(IgnoreDef, 0, GetRealDef()))
        Dim Result = HurtPlayer(Damage, Type)
        Dim BaseText As String = "* " & MonsterName(Id) & Desc & "\n  " & Result(1) & "你受到了" & Result(0) & "点伤害！"
        If EquipArmor = 6 AndAlso Type <> DamageType.爆炸 Then
            '荆棘
            Dim BackDamage As Integer = Result(0) / 2
            BaseText += "\n  护甲的荆棘造成了" & BackDamage & "点伤害！"
            HurtMonster(Id, BackDamage, DamageType.真实, True)
        End If
        StartChat({BaseText, "/TURNEND"}, True, False)
    End Sub

    '关卡
    Public Function GetLevelName(Id As Integer) As String
        Select Case Id
            Case 100
                Return "魔宫入口"
            Case 101
                Return "魔王宫殿前"
            Case 102
                Return "魔王宫殿"
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
            Case 11
                Return "魔宫走廊1"
            Case 12
                Return "魔宫走廊2"
            Case 21
                Return "元素之厅1"
            Case 22
                Return "元素之厅2"
            Case 23
                Return "元素之厅3"
            Case 31
                Return "魔王宫殿前"
            Case 32
                Return "魔王宫殿"
        End Select
    End Function
    Public Function GetLevelIntros(Id As Integer) As String()
        Select Case Id
            Case 100
                Return {"* 你早已为今天做好了准备，\n  两只骷髅对已经到达99级的你来说毫无威胁。",
                        "* 无论发生什么，都无法阻挠你击败魔王的决心。",
                        "* 你已经站在了魔宫的入口。\n  魔王，万物之恶的集合体，如今已近在眼前。",
                        "* 你早已决定不再犹豫。",
                        "* 你还在等待什么？"}
            Case 101
                MusicChange("Boss 1.mp3", 0.2, True) '2
                Return {"* 魔物们从四面八方涌来，阻拦你奔向魔王宫殿的脚步。",
                        "* 离魔王的宫殿只差最后一步。",
                        "* 这些怪物对早已身经百战的你而言，根本不值一提。",
                        "* 你此前所经受的漫长磨练终于将走到尽头。",
                        "* 是时候为一切画下句号了。"}
            Case 102
                Return {"* 你终于来到了魔王的面前。",
                        "* 只要在这最后一战中取胜……",
                        "* 传闻魔王可以操纵一切能量，魔力、火焰、黑暗，甚至是……",
                        "* 你不再回想那些恐怖的传说。",
                        "* 黑暗在你的四周不断翻腾。"}
            Case 1
                MusicChange("Main 1.mp3", 0, False)
                Return {"* 你似乎回到了起点。",
                        "* 遗忘？还是剥离？这就是魔王的能力吗？",
                        "* ………………"}
            Case 2
                MusicChange("Main 1.mp3", 0.1, True)
                Return {"* 三具骷髅已经将你包围。",
                        "* 骷骨的响声宛如一首嘈杂的打击乐。",
                        "* 骷髅们在用颅骨思考为什么勇者一直不进行攻击。",
                        "* 你忍耐着肉体的痛楚。",
                        "* 骨头喀拉作响。"}
            Case 3
                MusicChange("Main 1.mp3", 0.2, True)
                MusicChange2("Main 2.mp3", 0, False)
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
            Case 11
                Return {"* 两位剑士堵住了你的去路。",
                        "* 走廊内狭窄的空间恰好得以让他们将你阻截。",
                        "* 没有爬行冢追上来。",
                        "* 一般而言，陨石是没法穿透屋顶的，对吧？",
                        "* 剑与盔甲的碰撞声在走廊里回荡。"}
            Case 12
                Return {"* 甚至精锐的骑士里，也有人中了魔王的圈套。",
                        "* 骑士略微后撤，为冲锋留出了足够的距离。",
                        "* 他们来了。",
                        "* 剑士总是被骑士瞧不起。",
                        "* 马蹄声环绕在你的周身。"}
            Case 21
                MusicChange("Main 1.mp3", 0, True)
                MusicChange2("Main 2.mp3", 0.1, True)
                Return {"* 两团光球从空中向你飞来。",
                        "* 几乎凝为实体的魔力在四周撒下炫目的辉光。",
                        "* 烤冰淇淋的气味。",
                        "* 元素精灵并没有物理实体，\n  因此如果攻击不包含能量就很难奏效。",
                        "* 冰火交加。"}
            Case 22
                MusicChange("Boss 1.mp3", 0, False)
                MusicChange2("Main 2.mp3", 0.2, True)
                Return {"* 其他骑士也追进了元素之厅。",
                        "* 普通的护甲无法抵御纯粹的烈焰。",
                        "* 骑士在元素的光团之中闪避腾挪，朝你冲来。",
                        "* 精灵们在你的周身飞舞。",
                        "* 混乱的元素在空中飞荡。"}
            Case 23
                Return {"* 你举起武器，指向你的敌人。",
                        "* 根据此前的记忆，再往前不远就是魔王宫殿。",
                        "* 绝不能在这里倒下。",
                        "* 在这里耽误得越久，情况就会变得越危险……",
                        "* 你压抑着内心的急迫。"}
            Case 31
                MusicChange("Boss 1.mp3", 0.2, True)
                MusicChange2("Main 2.mp3", 0, True)
                Return {"* 没有怪物能够阻拦你的脚步。",
                        "* 骷骨，利剑，马蹄，烈焰。",
                        "* 剑戟划破空气，发出破空的风声。",
                        "* 魔物们正从四处前来。没有犹豫的时间了。",
                        "* 勇气与希望存于你的心间。"}
            Case 32
                MusicChange("Boss 1.mp3", 0, True)
                MusicChange2("Boss 2.mp3", 0.2, True)
                Return {"* 你终于再次站到了魔王的面前。",
                        "* 魔王看上去比此前虚弱了几分，\n  使用那样的法术果然对他造成了巨大的负担……",
                        "* 但他操纵元素的威能仍在。",
                        "* 这就是最终的一战。",
                        "* 无限的勇气在你的心头涌现。"}
        End Select
    End Function
    Public Function GetLevelMonsters(Id As Integer) As String()
        Select Case Id
            Case 100
                Return {"骷髅1", "骷髅1"}
            Case 101
                Return {"冰1", "骑士1", "骑士1", "骑士2", "骷髅2"}
            Case 102
                Return {"魔王1"}
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
            Case 11
                Return {"骑士1", "骑士1"}
            Case 12
                Return {"骑士1", "骑士2", "骑士2"}
            Case 21
                Return {"冰1", "火1"}
            Case 22
                Return {"火1", "骑士2", "火1"}
            Case 23
                Return {"骑士2", "火1", "火1", "冰1"}
            Case 31
                Return {"火1", "骑士1", "骑士1", "骑士2", "骷髅2"}
            Case 32
                Return {"魔王2"}
        End Select
    End Function
    Public Function GetLevelMonstersName(Id As Integer) As String()
        Select Case Id
            Case 100
                Return {"骷髅士兵", "骷髅卫兵"}
            Case 101
                Return {"寒冰精灵", "暗黑剑士", "漆黑剑士", "漆黑骑士", "粗骨骷髅战士"}
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
            Case 11
                Return {"暗黑剑士", "玄墨剑士"}
            Case 12
                Return {"暗黑剑士", "暗黑骑士", "漆黑骑士"}
            Case 21
                Return {"寒冰精灵", "烈火精灵"}
            Case 22
                Return {"烈火精灵", "暗黑骑士", "烈焰精灵"}
            Case 23
                Return {"暗黑骑士", "烈火精灵", "烈焰精灵", "寒冰精灵"}
            Case 31
                Return {"烈焰精灵", "暗黑剑士", "漆黑剑士", "漆黑骑士", "粗骨骷髅战士"}
            Case 32
                Return {"魔王"}
        End Select
    End Function
    Public Sub PerformLevelWin(Id As Integer)
        '存档
        ItemCountLast = ItemCount.Clone
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
                           "* 没有怪物能够阻拦你的脚步。",
                           "* 魔王的宫殿终于已近在咫尺。",
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
                MusicChange("Boss 3.mp3", 0, True) '7
                StartChat({"* ……",
                           "* …………",
                           "* ………………",
                           "* 当你再次睁开眼……",
                           "/LEVEL1"}, True, True)
            Case 1
                StartChat({"* 恭喜获胜！你获得了620XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 顷刻间，骷髅方才造成的伤痕已经消失不见。",
                           "* 守护……防御……",
                           "* 在沉思中，你回想起了你来到这里的初衷。",
                           "/UNLOCKD",
                           "* 「D」的内联逻辑已恢复。",
                           "* 你抬起头……",
                           "/LEVEL2"}, True, False)
            Case 2
                StartChat({"* 恭喜获胜！你获得了1205XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 你决定重新踏上魔宫之旅。",
                           "* 这样强大的法术必然不能连续使用，\n  现在就是打败魔王的最佳机会。",
                           "* 再次出发。新生，即是新的希望。\n  那是重新开始的机会。",
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
                           "* 离开黑火药之厅，\n  再往前走不远，就是元素与魔法的领域。",
                           "* 魔力涌动，法术也能成为你的助力。",
                           "/LOCK012579BWXKL", "/UNLOCKG",
                           "* 「G」的内联逻辑已恢复。",
                           "* 但在到达元素之厅前，似乎还得解决一些麻烦……",
                           "/LEVEL11"}, True, False)
            Case 11
                StartChat({"* 恭喜获胜！你获得了1880XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 为什么会有那么多人愿意向魔王卖命？\n  难道他们真的认为魔王能给他们什么好处？",
                           "* 那一切不过都是徒劳的。毫无价值。\n* 他们什么都没有得到——就像你一样。",
                           "/UNLOCK0",
                           "* 「0」的内联逻辑已恢复。",
                           "* 魔王所承诺的权力与财富都只是他们的一厢情愿罢了。",
                           "* 但总会有人前赴后继。",
                           "/LEVEL12"}, True, False)
            Case 12
                StartChat({"* 恭喜获胜！你获得了3915XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 随着战马与骑士一一倒下，走廊中变得一片死寂。",
                           "* 如果要纠正错误，这也是不可避免的事情。",
                           "/UNLOCKX",
                           "* 「X」的内联逻辑已恢复。",
                           "* 你继续前进，迈步走进了元素之厅。",
                           "/LEVEL21"}, True, False)
            Case 21
                StartChat({"* 恭喜获胜！你获得了720XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 你略微缓了口气。",
                           "* 强大的法术会让人沉醉在力量之中，最终迷失自我。",
                           "/LOCK1259BWKL", "/UNLOCK7",
                           "* 「7」的内联逻辑已恢复。",
                           "* 不能沉醉于力量，必须保持本心。",
                           "* 此时，马蹄声从你的身后传来……",
                           "/LEVEL22"}, True, False)
            Case 22
                StartChat({"* 恭喜获胜！你获得了2830XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 根本没有给你喘息的机会，\n  又一匹战马直冲而来。",
                           "* 元素的光团也闪耀着向你飞来。",
                           "* 你无比渴望再次挥动你的武器，\n  找回你强大的力量……",
                           "/UNLOCKK",
                           "* 「K」的内联逻辑已恢复。",
                           "/LEVEL23"}, True, False)
            Case 23
                StartChat({"* 恭喜获胜！你获得了3520XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 现在的力量还不足以打败魔王……",
                           "* 噩梦，恶鬼，力量之道途，抵御烈焰的方法……",
                           "/LOCK129BWL", "/UNLOCK5",
                           "* 「5」的内联逻辑已恢复。",
                           "* 魔王的宫殿已近在咫尺，\n  你也逐渐找回了自己的能力。",
                           "* 你坚信这片大地所承受的苦难定会走向尽头。",
                           "/LEVEL31"}, True, False)
            Case 31
                MusicChange2("Boss 2.mp3", 0, False)
                StartChat({"* 恭喜获胜！你获得了4105XP！\n  洛山达的祝福已生效，你的HP与MP已全部恢复！", "/IMPTrue",
                           "* 没有人能连续释放那样强大的法术，魔王也不例外。",
                           "* 现在是最佳的机会，也是唯一的机会。",
                           "* 你跟随记忆，奔向下一个路口，站在门前。",
                           "* 此前的经历在你的脑海浮现……",
                           "/UNLOCK2",
                           "* 「2」的内联逻辑已恢复。",
                           "* 这件护甲，承载着跨越时代的期许。",
                           "* 如今，你已别无选择。",
                           "* 你深吸一口气，推开大门。",
                           "* 奇异的紫色光芒涌现。",
                           "/LEVEL32"}, True, False)
            Case 32
                MusicChange("Win.mp3", 0.2, False)
                MusicChange2("Boss 2.mp3", 0, False)
                FrmMain.PixelLevel = 0
                DisabledKey = ""
                StartChat({"* 随着你的最后一击，魔王的身躯缓缓倒下。",
                           "* 秩序逐渐回归，内联逻辑也得以修复。",
                           "* 你终于完成了你的使命。",
                           "* 你瘫坐在魔王宫殿中，轻轻闭上了双眼……",
                           "* 仿佛一切都再也与你无关……",
                           "* ………………………………",
                           "* ………………………………",
                           Text.Encoding.UTF8.GetString(Convert.FromBase64String("KiBLe3pqbm5uO2E3SH3igKbigKY=")),
                           "* 恭喜通关！",
                           "* 本游戏制作于 GLOBAL GAME JAM 2021，\n  一个两天内从头到尾开发一个游戏的活动，\n  这一届的主题是 LOST AND FOUND。",
                           "* 说实话，完全没想到会变成一个人做完全程……\n  并且还是一个纯文本游戏……",
                           "* 两天肝完真的累死了……",
                           "* 不过如果你觉得这个游戏还不错，那就OK啦。",
                           "/THEEND"}, True, True)
        End Select
    End Sub

End Module
