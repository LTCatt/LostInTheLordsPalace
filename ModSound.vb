Imports System.IO
Imports System.Threading
Imports System.Windows.Threading

Public Module ModSound

#Region "音量管理"

    ''' <summary>
    ''' 全局音量。
    ''' </summary>
    Public Property VolumeMaster As Double
        Get
            Return _VolumeMaster
        End Get
        Set(value As Double)
            If value = _VolumeMaster Then Exit Property
            _VolumeMaster = value
            RefreshVolume()
        End Set
    End Property
    Private _VolumeMaster As Double = 0.7

    Private Sub RefreshVolume()
        For Each Sound In SoundList
            Sound.Volume = Sound.Volume * VolumeMaster
        Next
    End Sub

#End Region

    Public Class SoundListEntry
        ''' <summary>
        ''' 所属的播放器。
        ''' </summary>
        Public Player As MediaPlayer
        ''' <summary>
        ''' 该音频的独立音量。
        ''' </summary>
        Public Volume As Double = 1
        ''' <summary>
        ''' 音频文件的路径。
        ''' </summary>
        Public FilePath As String
        ''' <summary>
        ''' 是否已播放完成。
        ''' </summary>
        Public ReadOnly Property IsFinished As Boolean
            Get
                Return Player.NaturalDuration = Player.Position
            End Get
        End Property
    End Class
    Public SoundList As New List(Of SoundListEntry)

    Private SoundQueueList As New Queue(Of SoundQueue)
    Private Class SoundQueue
        Public FilePath As String
        Public Volume As Double
    End Class

    Private IsSoundStartRun As Boolean = False
    Public Sub SoundStartRun()
        If IsSoundStartRun Then Exit Sub
        IsSoundStartRun = True
        RunInNewThread(Sub()
                           Do While True
                               Thread.Sleep(20)
                               SoundTick()
                           Loop
                       End Sub, "Sound")
    End Sub
    Private Sub SoundTick()
        '开始新音效的播放
        Do While SoundQueueList.Count > 0
            Dim Queue As SoundQueue = SoundQueueList.Dequeue
            Dim Entry As SoundListEntry = Nothing

            '优先获取空闲的同一播放器
            For Each ExistsEntry In SoundList
                If ExistsEntry.IsFinished AndAlso ExistsEntry.FilePath = Queue.FilePath Then
                    Entry = ExistsEntry
                    GoTo GotPlayer
                End If
            Next
            '尝试获取空闲的其他播放器
            For Each ExistsEntry In SoundList
                If ExistsEntry.IsFinished Then
                    Entry = ExistsEntry
                    Entry.Player.Open(New Uri(Queue.FilePath))
                    GoTo GotPlayer
                End If
            Next
            '没有空闲播放器，新建一个
            Entry = New SoundListEntry With {.Player = New MediaPlayer}
            Entry.Player.Open(New Uri(Queue.FilePath))
            SoundList.Add(Entry)
GotPlayer:

            '初始化并开始播放
            Entry.Player.Volume = Queue.Volume * VolumeMaster
            Entry.Player.Position = New TimeSpan(0)
            Entry.FilePath = Queue.FilePath
            Entry.Volume = Queue.Volume
            Entry.Player.Play()

        Loop
    End Sub

    ''' <summary>
    ''' 播放音效。
    ''' </summary>
    Public Sub PlaySound(FileName As String, Optional Volume As Double = 1, Optional Balance As Double = 0)
        Dim FilePath As String = Path & "Sounds\" & FileName
        If Not File.Exists(FilePath) Then Throw New FileNotFoundException("未找到音频文件（" & FilePath & "）")
        SoundQueueList.Enqueue(New SoundQueue With {.Volume = Volume, .FilePath = FilePath})
    End Sub

End Module
