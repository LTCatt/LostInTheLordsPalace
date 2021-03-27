Imports System.Threading

Public Module ModBase

#Region "随机"

    Private ReadOnly Random As New Random
    Public Path As String = AppDomain.CurrentDomain.SetupInformation.ApplicationBase

    ''' <summary>
    ''' 随机选择其一。
    ''' </summary>
    Public Function RandomOne(objects As Array)
        Return objects.GetValue(RandomInteger(0, objects.Length - 1))
    End Function
    ''' <summary>
    ''' 随机选择其一。
    ''' </summary>
    Public Function RandomOne(objects As IList)
        Return objects(RandomInteger(0, objects.Count - 1))
    End Function

    ''' <summary>
    ''' 取随机整数。
    ''' </summary>
    Public Function RandomInteger(min As Integer, max As Integer) As Integer
        Return Math.Floor((max - min + 1) * Random.NextDouble) + min
    End Function

    ''' <summary>
    ''' 将数组随机打乱。
    ''' </summary>
    Public Function RandomChaos(Of T)(array As IList(Of T)) As IList(Of T)
        RandomChaos = New List(Of T)
        Do While array.Count > 0
            Dim i As Integer = RandomInteger(0, array.Count - 1)
            RandomChaos.Add(array(i))
            array.RemoveAt(i)
        Loop
    End Function

#End Region

#Region "自定义类"

    ''' <summary>
    ''' 支持小数与常见类型隐式转换的颜色。
    ''' </summary>
    Public Class MyColor

        Public A As Double = 255
        Public R As Double = 0
        Public G As Double = 0
        Public B As Double = 0

        '类型转换
        Public Shared Widening Operator CType(str As String) As MyColor
            Return New MyColor(str)
        End Operator
        Public Shared Widening Operator CType(col As Color) As MyColor
            Return New MyColor(col)
        End Operator
        Public Shared Widening Operator CType(conv As MyColor) As Color
            Return Color.FromArgb(MathByte(conv.A), MathByte(conv.R), MathByte(conv.G), MathByte(conv.B))
        End Operator
        Public Shared Widening Operator CType(bru As SolidColorBrush) As MyColor
            Return New MyColor(bru.Color)
        End Operator
        Public Shared Widening Operator CType(conv As MyColor) As SolidColorBrush
            Return New SolidColorBrush(Color.FromArgb(MathByte(conv.A), MathByte(conv.R), MathByte(conv.G), MathByte(conv.B)))
        End Operator
        Public Shared Widening Operator CType(bru As Brush) As MyColor
            Return New MyColor(bru)
        End Operator
        Public Shared Widening Operator CType(conv As MyColor) As Brush
            Return New SolidColorBrush(Color.FromArgb(MathByte(conv.A), MathByte(conv.R), MathByte(conv.G), MathByte(conv.B)))
        End Operator

        '颜色运算
        Public Shared Operator +(a As MyColor, b As MyColor) As MyColor
            Return New MyColor With {.A = a.A + b.A, .B = a.B + b.B, .G = a.G + b.G, .R = a.R + b.R}
        End Operator
        Public Shared Operator -(a As MyColor, b As MyColor) As MyColor
            Return New MyColor With {.A = a.A - b.A, .B = a.B - b.B, .G = a.G - b.G, .R = a.R - b.R}
        End Operator
        Public Shared Operator *(a As MyColor, b As Double) As MyColor
            Return New MyColor With {.A = a.A * b, .B = a.B * b, .G = a.G * b, .R = a.R * b}
        End Operator
        Public Shared Operator /(a As MyColor, b As Double) As MyColor
            Return New MyColor With {.A = a.A / b, .B = a.B / b, .G = a.G / b, .R = a.R / b}
        End Operator
        Public Shared Operator =(a As MyColor, b As MyColor) As Boolean
            If IsNothing(a) AndAlso IsNothing(b) Then Return True
            If IsNothing(a) OrElse IsNothing(b) Then Return False
            Return a.A = b.A AndAlso a.R = b.R AndAlso a.G = b.G AndAlso a.B = b.B
        End Operator
        Public Shared Operator <>(a As MyColor, b As MyColor) As Boolean
            If IsNothing(a) AndAlso IsNothing(b) Then Return False
            If IsNothing(a) OrElse IsNothing(b) Then Return True
            Return Not (a.A = b.A AndAlso a.R = b.R AndAlso a.G = b.G AndAlso a.B = b.B)
        End Operator

        '构造函数
        Public Sub New()
        End Sub
        Public Sub New(col As Color)
            Me.A = col.A
            Me.R = col.R
            Me.G = col.G
            Me.B = col.B
        End Sub
        Public Sub New(HexString As String)
            Dim StringColor As Media.Color = ColorConverter.ConvertFromString(HexString)
            A = StringColor.A
            R = StringColor.R
            G = StringColor.G
            B = StringColor.B
        End Sub
        Public Sub New(newA As Double, col As MyColor)
            Me.A = newA
            Me.R = col.R
            Me.G = col.G
            Me.B = col.B
        End Sub
        Public Sub New(newR As Double, newG As Double, newB As Double)
            Me.A = 255
            Me.R = newR
            Me.G = newG
            Me.B = newB
        End Sub
        Public Sub New(newA As Double, newR As Double, newG As Double, newB As Double)
            Me.A = newA
            Me.R = newR
            Me.G = newG
            Me.B = newB
        End Sub
        Public Sub New(brush As Brush)
            Dim Color As Color = CType(brush, SolidColorBrush).Color
            A = Color.A
            R = Color.R
            G = Color.G
            B = Color.B
        End Sub
        Public Sub New(brush As SolidColorBrush)
            Dim Color As Color = brush.Color
            A = Color.A
            R = Color.R
            G = Color.G
            B = Color.B
        End Sub
        Public Sub New(obj As Object)
            If obj Is Nothing Then
                A = 255 : R = 255 : G = 255 : B = 255
            Else
                If obj.GetType.Equals(GetType(SolidColorBrush)) Then
                    '避免反复获取 Color 对象造成性能下降
                    Dim Color As Color = obj.Color
                    A = Color.A
                    R = Color.R
                    G = Color.G
                    B = Color.B
                Else
                    A = obj.A
                    R = obj.R
                    G = obj.G
                    B = obj.B
                End If
            End If
        End Sub

        'HSL
        Public Function Hue(v1 As Double, v2 As Double, vH As Double) As Double
            If vH < 0 Then vH += 1
            If vH > 1 Then vH -= 1
            If vH < 0.16667 Then Return v1 + (v2 - v1) * 6 * vH
            If vH < 0.5 Then Return v2
            If vH < 0.66667 Then Return v1 + (v2 - v1) * (4 - vH * 6)
            Return v1
        End Function
        Public Function FromHSL(sH As Double, sS As Double, sL As Double) As MyColor
            If sS = 0 Then
                R = sL * 2.55
                G = R
                B = R
            Else
                Dim H = sH / 360
                Dim S = sS / 100
                Dim L = sL / 100
                S = If(L < 0.5, S * L + L, S * (1.0 - L) + L)
                L = 2 * L - S
                R = 255 * Hue(L, S, H + 1 / 3)
                G = 255 * Hue(L, S, H)
                B = 255 * Hue(L, S, H - 1 / 3)
            End If
            A = 255
            Return Me
        End Function
        Public Function FromHSL2(sH As Double, sS As Double, sL As Double) As MyColor
            If sS = 0 Then
                R = sL * 2.55 : G = R : B = R
            Else
                '初始化
                sH = (sH + 3600000) Mod 360
                Dim cent As Double() = {
                    +0.1, -0.06, -0.3, '0, 30, 60
                    -0.19, -0.15, -0.24, '90, 120, 150
                    -0.32, -0.09, +0.18, '180, 210, 240
                    +0.05, -0.12, -0.02, '270, 300, 330
                    +0.1, -0.06} '最后两位与前两位一致，加是变亮，减是变暗
                '计算色调对应的亮度片区
                Dim center As Double = sH / 30.0
                Dim intCenter As Integer = Math.Floor(center) '亮度片区编号
                center = 50 - (
                     (1 - center + intCenter) * cent(intCenter) + (center - intCenter) * cent(intCenter + 1)
                    ) * sS
                'center = 50 + (cent(intCenter) + (center - intCenter) * (cent(intCenter + 1) - cent(intCenter))) * sS
                sL = If(sL < center, sL / center, 1 + (sL - center) / (100 - center)) * 50
                FromHSL(sH, sS, sL)
            End If
            A = 255
            Return Me
        End Function

        Public Overrides Function ToString() As String
            Return "(" & A & "," & R & "," & G & "," & B & ")"
        End Function
        Public Overrides Function Equals(obj As Object) As Boolean
            Return Me = obj
        End Function

    End Class

    ''' <summary>
    ''' 支持负数与浮点数的矩形。
    ''' </summary>
    Public Class MyRect

        '属性
        Public Property Width As Double = 0
        Public Property Height As Double = 0
        Public Property Left As Double = 0
        Public Property Top As Double = 0

        '构造函数
        Public Sub New()
        End Sub
        Public Sub New(left As Double, top As Double, width As Double, height As Double)
            Me.Left = left
            Me.Top = top
            Me.Width = width
            Me.Height = height
        End Sub

    End Class

    ''' <summary>
    ''' 模块加载状态枚举。
    ''' </summary>
    Public Enum LoadState
        Waiting
        Loading
        Finished
        Failed
        Aborted
    End Enum

    ''' <summary>
    ''' 执行返回值。
    ''' </summary>
    Public Enum Result
        ''' <summary>
        ''' 执行成功，或进程被中断。
        ''' </summary>
        Aborted = -1
        ''' <summary>
        ''' 执行成功。
        ''' </summary>
        Success = 0
        ''' <summary>
        ''' 执行失败。
        ''' </summary>
        Fail
        ''' <summary>
        ''' 执行时出现未经处理的异常。
        ''' </summary>
        Exception
        ''' <summary>
        ''' 执行超时。
        ''' </summary>
        Timeout
        ''' <summary>
        ''' 取消执行。可能是由于不满足执行的前置条件。
        ''' </summary>
        Cancel
    End Enum

    ''' <summary>
    ''' 可以使用 Equals 和等号的 List。
    ''' </summary>
    Public Class EqualableList(Of T)
        Inherits List(Of T)
        Public Overrides Function Equals(obj As Object) As Boolean
            If TryCast(obj, List(Of T)) Is Nothing Then
                '类型不同
                Return False
            Else
                '类型相同
                Dim objList As List(Of T) = obj
                If objList.Count <> Count Then Return False
                For i = 0 To objList.Count - 1
                    If Not objList(i).Equals(Me(i)) Then Return False
                Next
                Return True
            End If
        End Function
        Public Shared Operator =(left As EqualableList(Of T), right As EqualableList(Of T)) As Boolean
            Return EqualityComparer(Of EqualableList(Of T)).Default.Equals(left, right)
        End Operator
        Public Shared Operator <>(left As EqualableList(Of T), right As EqualableList(Of T)) As Boolean
            Return Not left = right
        End Operator
    End Class

#End Region

#Region "数学"

    ''' <summary>
    ''' 计算二阶贝塞尔曲线。
    ''' </summary>
    Public Function MathBezier(x As Double, x1 As Double, y1 As Double, x2 As Double, y2 As Double, Optional acc As Double = 0.01) As Double
        If x <= 0 OrElse Double.IsNaN(x) Then Return 0
        If x >= 1 Then Return 1
        Dim a, b
        a = x
        Do
            b = 3 * a * ((0.33333333 + x1 - x2) * a * a + (x2 - 2 * x1) * a + x1)
            a += (x - b) * 0.5
        Loop Until Math.Abs(b - x) < acc '精度
        Return 3 * a * ((0.33333333 + y1 - y2) * a * a + (y2 - 2 * y1) * a + y1)
    End Function

    ''' <summary>
    ''' 将一个数字限制为 0~255 的 Byte 值。
    ''' </summary>
    Public Function MathByte(d As Double) As Byte
        If d < 0 Then d = 0
        If d > 255 Then d = 255
        Return Math.Round(d)
    End Function

    ''' <summary>
    ''' 提供 MyColor 类型支持的 Math.Round。
    ''' </summary>
    Public Function MathRound(col As MyColor, Optional w As Integer = 0) As MyColor
        Return New MyColor With {.A = Math.Round(col.A, w), .R = Math.Round(col.R, w), .G = Math.Round(col.G, w), .B = Math.Round(col.B, w)}
    End Function

    ''' <summary>
    ''' 获取两数间的百分比。小数点精确到 6 位。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function MathPercent(ValueA As Double, ValueB As Double, Percent As Double) As Double
        Return Math.Round(ValueA * (1 - Percent) + ValueB * Percent, 6) '解决 Double 计算错误
    End Function
    ''' <summary>
    ''' 获取两颜色间的百分比，根据 RGB 计算。小数点精确到 6 位。
    ''' </summary>
    Public Function MathPercent(ValueA As MyColor, ValueB As MyColor, Percent As Double) As MyColor
        Return MathRound(ValueA * (1 - Percent) + ValueB * Percent, 6) '解决Double计算错误
    End Function

    ''' <summary>
    ''' 将数值限定在某个范围内。
    ''' </summary>
    Public Function MathRange(value As Double, min As Double, max As Double) As Double
        Return Math.Max(min, Math.Min(max, value))
    End Function

    ''' <summary>
    ''' 符号函数。
    ''' </summary>
    Public Function MathSgn(Value As Double) As Integer
        If Value = 0 Then
            Return 0
        ElseIf Value > 0 Then
            Return 1
        Else
            Return -1
        End If
    End Function

#End Region

    ''' <summary>
    ''' 返回一个枚举对应的字符串。
    ''' </summary>
    ''' <param name="EnumData">一个已经实例化的枚举类型。</param>
    Public Function GetStringFromEnum(EnumData As [Enum]) As String
        Return [Enum].GetName(EnumData.GetType, EnumData)
    End Function
    ''' <summary>
    ''' 将元素与 List 的混合体拆分为元素组。
    ''' </summary>
    Public Function GetFullList(data As IList) As ArrayList
        GetFullList = New ArrayList
        For i = 0 To data.Count - 1
            If data(i).GetType.ToString.Contains("List") Then
                GetFullList.AddRange(data(i))
            Else
                GetFullList.Add(data(i))
            End If
        Next i
    End Function
    ''' <summary>
    ''' 将元素与 List 的混合体拆分为元素组。
    ''' </summary>
    Public Function GetFullList(Of T)(data As IList) As List(Of T)
        GetFullList = New List(Of T)
        For i = 0 To data.Count - 1
            If data(i).GetType.ToString.Contains("List") Then
                GetFullList.AddRange(data(i))
            Else
                GetFullList.Add(data(i))
            End If
        Next i
    End Function
    ''' <summary>
    ''' 若 Key 不存在于辞典，则加入辞典。
    ''' 若 Key 已存在于辞典，则更新 Value。
    ''' </summary>
    Public Sub DictionaryAdd(Of TKey, TValue)(ByRef Dict As Dictionary(Of TKey, TValue), Key As TKey, Value As TValue)
        If Dict.ContainsKey(Key) Then
            Dict(Key) = Value
        Else
            Dict.Add(Key, Value)
        End If
    End Sub

    ''' <summary>
    ''' 获取系统运行时间，保证为正 Long 且大于 1，但可能突变减小。
    ''' </summary>
    Public Function GetTimeTick() As Long
        Return My.Computer.Clock.TickCount + 2147483651L
    End Function

    Private Uuid As Integer = 1
    Private UuidLock As Object
    ''' <summary>
    ''' 获取一个全程序内不会重复的数字（伪 Uuid）。
    ''' </summary>
    Public Function GetUuid() As Integer
        If UuidLock Is Nothing Then UuidLock = New Object
        SyncLock UuidLock
            Uuid += 1
            Return Uuid
        End SyncLock
    End Function

    Private ReadOnly UiThreadId As Integer = Thread.CurrentThread.ManagedThreadId
    ''' <summary>
    ''' 当前线程是否为主线程。
    ''' </summary>
    Public Function RunInUi() As Boolean
        Return Thread.CurrentThread.ManagedThreadId = UiThreadId
    End Function
    ''' <summary>
    ''' 在新的工作线程中执行代码。
    ''' </summary>
    Public Function RunInNewThread(ThreadStart As ThreadStart, Name As String, Optional Priority As ThreadPriority = ThreadPriority.Normal) As Thread
        Dim th As New Thread(ThreadStart) With {.Name = Name, .Priority = Priority}
        th.Start()
        Return th
    End Function
    ''' <summary>
    ''' 确保在 UI 线程中执行代码。
    ''' </summary>
    Public Sub RunInUi(ThreadStart As [Delegate], ParamArray Param As Object())
        If RunInUi() Then
            ThreadStart.DynamicInvoke(Param)
        Else
            FrmMain.Dispatcher.Invoke(ThreadStart, Param)
        End If
    End Sub
    ''' <summary>
    ''' 确保在 UI 线程中执行代码。
    ''' 如果当前并非 UI 线程，则会阻断当前线程，直至 UI 线程执行完毕。
    ''' 为防止线程互锁，请仅在开始加载动画、从 UI 获取输入时使用！
    ''' </summary>
    Public Sub RunInUiWait(ThreadStart As ThreadStart)
        If RunInUi() Then
            ThreadStart()
        Else
            Application.Current.Dispatcher.Invoke(ThreadStart)
        End If
    End Sub
    ''' <summary>
    ''' 确保在 UI 线程中执行代码。
    ''' 如果当前并非 UI 线程，则会阻断当前线程，直至 UI 线程执行完毕。
    ''' 为防止线程互锁，请仅在开始加载动画、从 UI 获取输入时使用！
    ''' </summary>
    Public Sub RunInUiWait(ThreadStart As ParameterizedThreadStart, Param As Object)
        If RunInUi() Then
            ThreadStart(Param)
        Else
            Application.Current.Dispatcher.Invoke(ThreadStart, Param)
        End If
    End Sub
    ''' <summary>
    ''' 确保在 UI 线程中执行代码，代码按触发顺序执行。
    ''' 如果当前并非 UI 线程，也不阻断当前线程的执行。
    ''' </summary>
    Public Sub RunInUi(ThreadStart As Action, Optional ForceWaitUntilLoaded As Boolean = False)
        If ForceWaitUntilLoaded Then
            Application.Current.Dispatcher.InvokeAsync(ThreadStart, Threading.DispatcherPriority.Loaded)
        ElseIf RunInUi() Then
            ThreadStart()
        Else
            Application.Current.Dispatcher.InvokeAsync(ThreadStart)
        End If
    End Sub
    ''' <summary>
    ''' 确保在工作线程中执行代码。
    ''' </summary>
    Public Sub RunInThread(ThreadStart As ThreadStart)
        If RunInUi() Then
            RunInNewThread(ThreadStart, "Runtime Invoke " & GetUuid() & "#")
        Else
            ThreadStart()
        End If
    End Sub

    '边距改变
    ''' <summary>
    ''' 相对增减控件的左边距。
    ''' </summary>
    Public Sub DeltaLeft(control As FrameworkElement, newValue As Double)
        '安全性检查
        '根据 HorizontalAlignment 改变数值
        Select Case control.HorizontalAlignment
            Case HorizontalAlignment.Left, HorizontalAlignment.Stretch
                control.Margin = New Thickness(control.Margin.Left + newValue, control.Margin.Top, control.Margin.Right, control.Margin.Bottom)
            Case HorizontalAlignment.Right
                control.Margin = New Thickness(control.Margin.Left, control.Margin.Top, control.Margin.Right - newValue, control.Margin.Bottom)
                'control.Margin = New Thickness(control.Margin.Left, control.Margin.Top, CType(control.Parent, Object).ActualWidth - control.ActualWidth - newValue, control.Margin.Bottom)
        End Select
    End Sub
    ''' <summary>
    ''' 设置控件的左边距。（仅针对置左控件）
    ''' </summary>
    Public Sub SetLeft(control As FrameworkElement, newValue As Double)
        control.Margin = New Thickness(newValue, control.Margin.Top, control.Margin.Right, control.Margin.Bottom)
    End Sub
    ''' <summary>
    ''' 相对增减控件的上边距。
    ''' </summary>
    Public Sub DeltaTop(control As FrameworkElement, newValue As Double)
        '根据 VerticalAlignment 改变数值
        Select Case control.VerticalAlignment
            Case VerticalAlignment.Top
                control.Margin = New Thickness(control.Margin.Left, control.Margin.Top + newValue, control.Margin.Right, control.Margin.Bottom)
            Case VerticalAlignment.Bottom
                control.Margin = New Thickness(control.Margin.Left, control.Margin.Top, control.Margin.Right, control.Margin.Bottom - newValue)
                'control.Margin = New Thickness(control.Margin.Left, control.Margin.Top, CType(control.Parent, Object).ActualWidth - control.ActualWidth - newValue, control.Margin.Bottom)
        End Select

        'If Double.IsNaN(newValue) OrElse Double.IsInfinity(newValue) Then Exit Sub '安全性检查
        'Select Case control.VerticalAlignment
        '  Case VerticalAlignment.Top, VerticalAlignment.Stretch, VerticalAlignment.Center
        '      control.Margin = New Thickness(control.Margin.Left, newValue, control.Margin.Right, control.Margin.Bottom)
        '  Case VerticalAlignment.Bottom
        '      control.Margin = New Thickness(control.Margin.Left, control.Margin.Top, control.Margin.Right, -newValue)
        '      'control.Margin = New Thickness(control.Margin.Left, control.Margin.Top, control.Margin.Right, CType(control.Parent, Object).ActualHeight - control.ActualHeight - newValue)
        'End Select
    End Sub
    ''' <summary>
    ''' 设置控件的顶边距。（仅针对置上控件）
    ''' </summary>
    Public Sub SetTop(control As FrameworkElement, newValue As Double)
        control.Margin = New Thickness(control.Margin.Left, newValue, control.Margin.Right, control.Margin.Bottom)
    End Sub

End Module
