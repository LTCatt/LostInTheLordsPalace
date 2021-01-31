Imports System.Threading

Public Module ModMusic

    Public MusicPlayer As MediaPlayer
    Private MusicThread As Thread

    Private IsInited As Boolean = False
    Public Sub MusicStartRun()
        If IsInited Then Exit Sub
        IsInited = True
        '初始化
        MusicThread = New Thread(Sub()
                                     '初始化 MediaPlayer
                                     MusicPlayer = New MediaPlayer
                                     MusicPlayer.Volume = 0
                                     Do While True
                                         Thread.Sleep(15)
                                         '调整音乐
                                         If MusicName <> MusicNamePlaying Then
                                             Dim RawPosition = MusicPlayer.Position
                                             MusicPlayer.Open(New Uri(Path & "\Sounds\" & MusicName))
                                             MusicPlayer.Play()
                                             MusicNamePlaying = MusicName
                                             If MusicInheritProgress Then MusicPlayer.Position = RawPosition
                                         End If
                                         '渐变音量
                                         MusicPlayer.Volume = MusicPlayer.Volume * 0.9975 + MusicVolume * 0.0025
                                         '循环播放
                                         If MusicPlayer.Position + New TimeSpan(0, 0, 0, 0, 20) >= MusicPlayer.NaturalDuration Then
                                             MusicPlayer.Position = New TimeSpan(0, 0, 0, 0)
                                         End If
                                     Loop
                                 End Sub)
        MusicThread.Start()
    End Sub

    Private MusicVolume As Double = 0.05
    Private MusicName As String = "Prologue 1.mp3"
    Private MusicInheritProgress As Boolean = False
    Private MusicNamePlaying As String = ""
    Public Sub MusicChange(Name As String, Volume As Double, InheritProgress As Boolean)
        MusicName = Name
        MusicVolume = Volume
        MusicInheritProgress = InheritProgress
    End Sub

End Module
