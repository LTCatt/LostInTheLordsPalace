Imports System.Threading

Public Module ModMusic

    Private MusicPlayer As MediaPlayer
    Private MusicThread As Thread

    Private IsInited As Boolean = False
    Public Sub MusicStartRun()
        If IsInited Then Exit Sub
        IsInited = True
        '初始化
        MusicThread = New Thread(Sub()
                                     '初始化 MediaPlayer
                                     MusicPlayer = New MediaPlayer
                                     MusicChange("BGM0.mp3", 0.5) '初始音乐
                                     Do While True
                                         Thread.Sleep(15)
                                         '循环播放
                                         If MusicPlayer.Position + New TimeSpan(0, 0, 0, 0, 20) >= MusicPlayer.NaturalDuration Then
                                             MusicPlayer.Position = New TimeSpan(0, 0, 0, 0)
                                         End If
                                     Loop
                                 End Sub)
        MusicThread.Start()
    End Sub

    Public Sub MusicChange(Name As String, Volume As Double)
        MusicPlayer.Volume = Volume
        MusicPlayer.Open(New Uri(Path & "\Sounds\" & Name))
        MusicPlayer.Play()
    End Sub

End Module
