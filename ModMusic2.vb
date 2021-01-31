Imports System.Threading

Public Module ModMusic2

    Public MusicPlayer2 As MediaPlayer
    Private MusicThread As Thread

    Private IsInited As Boolean = False
    Public Sub MusicStartRun2()
        If IsInited Then Exit Sub
        IsInited = True
        '初始化
        MusicThread = New Thread(Sub()
                                     '初始化 MediaPlayer
                                     MusicPlayer2 = New MediaPlayer
                                     MusicPlayer2.Volume = 0
                                     Do While True
                                         Thread.Sleep(15)
                                         '调整音乐
                                         If MusicName <> MusicNamePlaying Then
                                             Dim RawPosition = MusicPlayer2.Position
                                             MusicPlayer2.Open(New Uri(Path & "\Sounds\" & MusicName))
                                             MusicPlayer2.Play()
                                             MusicNamePlaying = MusicName
                                             If MusicInheritProgress Then MusicPlayer2.Position = RawPosition
                                         End If
                                         '渐变音量
                                         MusicPlayer2.Volume = MusicPlayer2.Volume * 0.997 + MusicVolume * 0.003
                                         '循环播放
                                         If MusicPlayer2.Position + New TimeSpan(0, 0, 0, 0, 20) >= MusicPlayer2.NaturalDuration Then
                                             MusicPlayer2.Position = New TimeSpan(0, 0, 0, 0)
                                         End If
                                     Loop
                                 End Sub)
        MusicThread.Start()
    End Sub

    Private MusicVolume As Double = 0
    Private MusicName As String = "Boss 2.mp3"
    Private MusicInheritProgress As Boolean = False
    Private MusicNamePlaying As String = ""
    Public Sub MusicChange2(Name As String, Volume As Double, InheritProgress As Boolean)
        MusicName = Name
        MusicVolume = Volume
        MusicInheritProgress = InheritProgress
    End Sub

End Module
