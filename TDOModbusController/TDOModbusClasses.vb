Public Class MPoint

    Public Property MPName As String = vbNullString
    Public Property MPCode As Integer = 0
    Public Property AlarmHigh1 As String = vbNullString
    Public Property AlarmHigh2 As String = vbNullString
    Public Property AlarmHigh3 As String = vbNullString
    Public Property AlarmHigh4 As String = vbNullString
    Public Property AlarmLow1 As String = vbNullString
    Public Property AlarmLow2 As String = vbNullString
    Public Property AlarmLow3 As String = vbNullString
    Public Property AlarmLow4 As String = vbNullString
    Public Property LastAlarm As String = vbNullString
    Public Property LastAlarmDT As Date = vbNullString

End Class


Public Class CSettings
    Public Property DBIP As String
    Public Property DBPort As String
    Public Property DBPath As String
    Public Property DBLogin As String
    Public Property DBPass As String
    Public Property DBCharset As String
    Public Property DBServerType As String

    Private _DBScanIntSecondsV As Integer
    Public Property DBScanIntSeconds() As Integer
        Get
            Return _DBScanIntSecondsV
        End Get
        Set(ByVal value As Integer)
            If value < 5 Then
                _DBScanIntSecondsV = 5
            Else
                _DBScanIntSecondsV = value
            End If
        End Set
    End Property

    Public Property COMPort As String
    Public Property COMBaudRate As Integer
    Public Property COMDataBits As Integer
    Public Property COMStopBits As IO.Ports.StopBits
    Public Property COMParity As IO.Ports.Parity
    Public Property COMHandshake As IO.Ports.Handshake
    Public Property COMInRelON As String
    Public Property COMOutRelON As String
    Public Property COMInGetRel As String
    Public Property COMOutGetRel As String
    Public Property COMInRelOFF As String
    Public Property COMOutRelOFF As String

    Private _COMRetriesV As Integer
    Public Property COMRetries() As Integer
        Get
            Return _COMRetriesV
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then
                _COMRetriesV = 1
            ElseIf value > 20 Then
                _COMRetriesV = 20
            Else
                _COMRetriesV = value
            End If
        End Set
    End Property

    Private _COMTimeoutMiliSecondsV As Integer
    Public Property COMTimeoutMiliSeconds() As Integer
        Get
            Return _COMTimeoutMiliSecondsV
        End Get
        Set(ByVal value As Integer)
            If value < 100 Then
                _COMTimeoutMiliSecondsV = 100
            ElseIf value > 10000 Then
                _COMTimeoutMiliSecondsV = 10000
            Else
                _COMTimeoutMiliSecondsV = value
            End If
        End Set
    End Property

    Private _COMPingIntSecondsV As Integer
    Public Property COMPingIntSeconds() As Integer
        Get
            Return _COMPingIntSecondsV
        End Get
        Set(ByVal value As Integer)
            If value < 5 Then
                _COMPingIntSecondsV = 5
            Else
                _COMPingIntSecondsV = value
            End If
        End Set
    End Property

    Private _COMPingReportFailed As Integer
    Public Property COMPingReportFailed() As Integer
        Get
            Return _COMPingReportFailed
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                _COMPingReportFailed = 0
            ElseIf value > 999 Then
                _COMPingReportFailed = 0
            Else
                _COMPingReportFailed = value
            End If
        End Set
    End Property

    Public Property COMInPing As String
    Public Property COMOutPing As String

    Private _COMAlarmMaxDelayMinutes As Integer
    Public Property COMAlarmMaxDelayMinutes() As Integer
        Get
            Return _COMAlarmMaxDelayMinutes
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then
                _COMAlarmMaxDelayMinutes = 1
            ElseIf value > 1440 Then
                _COMAlarmMaxDelayMinutes = 1440
            Else
                _COMAlarmMaxDelayMinutes = value
            End If
        End Set
    End Property


    Public Property ExtCLICommand As String
    Public Property ExtCLICommandArgs As String
    Public Property ExtCLIComOnAlarm As Boolean

    Private _AlarmDurSecondsV As Integer
    Public Property AlarmDurSeconds() As Integer
        Get
            Return _AlarmDurSecondsV
        End Get
        Set(ByVal value As Integer)
            If value < 5 Then
                _AlarmDurSecondsV = 5
            Else
                _AlarmDurSecondsV = value
            End If
        End Set
    End Property

End Class