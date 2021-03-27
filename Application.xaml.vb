Imports System.Windows.Threading

Class Application

    ' 应用程序级事件(例如 Startup、Exit 和 DispatcherUnhandledException)
    ' 可以在此文件中进行处理。

    '异常
    Private IsCritErrored As Boolean = False
    Private Sub Application_DispatcherUnhandledException(sender As Object, e As DispatcherUnhandledExceptionEventArgs) Handles Me.DispatcherUnhandledException
        On Error Resume Next
        e.Handled = True
        If IsCritErrored Then
            '在汇报错误后继续引发错误，知道这次压不住了
            Process.GetCurrentProcess.Kill()
        Else
            IsCritErrored = True
            Dim ExceptionString As String = e.Exception.Message & vbCrLf & e.Exception.StackTrace
            If ExceptionString.Contains("System.Windows.Threading.Dispatcher.Invoke") OrElse
               ExceptionString.Contains("MS.Internal.AppModel.ITaskbarList.HrInit") Then
                If Path.Contains(IO.Path.GetTempPath()) Then
                    '如果在临时文件夹运行，且 .Net Framework 版本不足，考虑是微步的沙箱，如果打开 IE 会被莫名其妙地报毒，就不打开了
                    MsgBox("你的 .Net Framework 版本过低或损坏，请重新下载并安装 .Net Framework 4.5 后重试！", MsgBoxStyle.Information, "运行环境错误")
                Else
                    Process.Start("https://www.microsoft.com/zh-CN/download/details.aspx?id=30653")
                    MsgBox("你的 .Net Framework 版本过低或损坏，请在打开的网页中重新下载并安装 .Net Framework 4.5 后重试！", MsgBoxStyle.Information, "运行环境错误")
                End If
            Else
                MsgBox(ExceptionString, MsgBoxStyle.Critical, "锟斤拷烫烫烫")
            End If
        End If
    End Sub

End Class
