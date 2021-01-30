Public Module ModGameData

    '法术
    Public Function GetMagicTitle(Id As Integer) As String
        Select Case Id
            Case 1
                Return "魔法1"
            Case 2
                Return "魔法2"
            Case 3
                Return "魔法3"
            Case 4
                Return "魔法4"
            Case 5
                Return "魔法5"
            Case 6
                Return "魔法6"
            Case 7
                Return "魔法7名称"
        End Select
    End Function
    Public Function GetMagicDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "魔法1描述"
            Case 2
                Return "魔法2描述"
            Case 3
                Return "魔法3描述"
            Case 4
                Return "魔法4描述"
            Case 5
                Return "魔法5描述"
            Case 6
                Return "魔法6描述"
            Case 7
                Return "魔法7描述"
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

    '道具
    Public Function GetItemTitle(Id As Integer) As String
        Select Case Id
            Case 1
                Return "魔法a1"
            Case 2
                Return "魔法2"
            Case 3
                Return "魔法3"
            Case 4
                Return "魔法4"
            Case 5
                Return "魔法5"
            Case 6
                Return "魔法6"
            Case 7
                Return "魔法7名称"
        End Select
    End Function
    Public Function GetItemDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "魔法1a描述"
            Case 2
                Return "魔法2描述"
            Case 3
                Return "魔法3描述"
            Case 4
                Return "魔法4描述"
            Case 5
                Return "魔法5描述"
            Case 6
                Return "魔法6描述"
            Case 7
                Return "魔法7描述"
        End Select
    End Function

    '装备
    Public Function GetEquipTitle(Id As Integer) As String
        Select Case Id
            Case 1
                Return "装备1a"
            Case 2
                Return "魔法2"
            Case 3
                Return "魔法3"
            Case 4
                Return "魔法4"
            Case 5
                Return "魔法5"
            Case 6
                Return "魔法6"
            Case 7
                Return "魔法7名称"
        End Select
    End Function
    Public Function GetEquipDesc(Id As Integer) As String
        Select Case Id
            Case 1
                Return "魔法1描述"
            Case 2
                Return "魔法2描述"
            Case 3
                Return "魔法3描述"
            Case 4
                Return "魔法4描述"
            Case 5
                Return "魔法5描述"
            Case 6
                Return "魔法6描述"
            Case 7
                Return "魔法7描述"
        End Select
    End Function
    Public Function GetEquipIsSword(Id As Integer) As Boolean
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

End Module
