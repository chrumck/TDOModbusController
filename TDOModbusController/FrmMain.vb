Imports System.Xml
Imports FirebirdSql.Data.FirebirdClient
Imports System.IO.Ports
Imports System.IO
Imports System.ComponentModel

Public Class FrmMain

    Private MPointList As New List(Of MPoint)
    Private CSettings1 As New CSettings

    Private DBConn As New FbConnection
    Private SerialPort1 As New SerialPort
    Private LastAlarmsTable As New DataSet

    Public COMInCustom As String = vbNullString
    Public COMOutCustom As String = vbNullString

    Private FrmMainClosePending As Boolean = False

    Public Sub New()
        InitializeComponent()
        ReadMPListFromXML()
        ReadCSettingsFromXML()
        AlarmMonkey.WorkerReportsProgress = True
        AlarmMonkey.WorkerSupportsCancellation = True
        BtnStop.Enabled = False
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub


    Private Sub BtnMPList_Click(sender As Object, e As EventArgs) Handles BtnMPList.Click
        Dim FrmMPList1 As New FrmMPList
        FrmMPList1.MPointList = MPointList
        FrmMPList1.ShowDialog(Me)
        UpdateMPCodes()
        WriteMPListToXML()
        Dim WriteMPListResult As String = WriteMPListToXML() : If WriteMPListResult <> "0" Then MsgBox(String.Format("Writing MPList to XML failed: {0}", WriteMPListResult))
    End Sub

    Private Sub BtnSettings_Click(sender As Object, e As EventArgs) Handles BtnSettings.Click
        Dim FrmSettings1 As New FrmSettings
        FrmSettings1.CSettings1 = CSettings1
        FrmSettings1.ShowDialog(Me)
        WriteCSettingsToXML()
        Dim WriteSettingsResult As String = WriteCSettingsToXML() : If WriteSettingsResult <> "0" Then MsgBox(String.Format("Writing Settings to XML failed: {0}", WriteSettingsResult))
    End Sub

    Private Sub BtnDBCnn_Click(sender As Object, e As EventArgs) Handles BtnDBCnn.Click
        TxBStatus.Text = "Connecting to DB" : Me.Refresh()
        Dim DBConnResult As String = DBConnect()
        DBConn.Close() : TxBStatus.Text = "DB Connection Closed"
        If DBConnResult = "0" Then MsgBox("Connection to DB Succesful.", MsgBoxStyle.Information) Else MsgBox("Connection failed." & vbCr & DBConnResult, MsgBoxStyle.Exclamation)
    End Sub

    Private Sub BtnModbusCnn_Click(sender As Object, e As EventArgs) Handles BtnModbusCnn.Click
        Dim FrmCustomCOM1 As New FrmCustomCOM
        Dim DgResult As DialogResult = FrmCustomCOM1.ShowDialog(Me)
        If DgResult = Windows.Forms.DialogResult.Cancel Then Exit Sub

        TxBStatus.Text = "Communicating with port " & CSettings1.COMPort : Me.Refresh()
        Dim COMOpenString As String = COMOpen()
        If COMOpenString = "0" Then
            TxBStatus.Text = "Connected to port " & CSettings1.COMPort : Me.Refresh()
        Else
            TxBStatus.Text = COMOpenString : Me.Refresh()
            MsgBox(COMOpenString)
            Exit Sub
        End If
        Dim COMCommandString As String = COMCommand(COMInCustom, COMOutCustom)
        SerialPort1.Close() : TxBStatus.Text = "COM Connection Closed"
        If COMCommandString = "0" Then MsgBox("Connection to COM Succesful.", MsgBoxStyle.Information) Else MsgBox("COM Connection failed." & vbCr & COMCommandString, MsgBoxStyle.Exclamation)
    End Sub
    Private Sub BtnExt_Click(sender As Object, e As EventArgs) Handles BtnExt.Click
        Dim extCommResult As String = RunExternalCommand()
        If extCommResult = "0" Then MsgBox("Running Ext. Command Succesful.", MsgBoxStyle.Information) Else MsgBox("Runninf Ext. Command failed:" & vbCr & extCommResult, MsgBoxStyle.Exclamation)
    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        If AlarmMonkey.IsBusy = False Then
            TxBStatus.Text = "Started"
            BtnMPList.Enabled = False : BtnSettings.Enabled = False : BtnDBCnn.Enabled = False : BtnModbusCnn.Enabled = False : BtnClose.Enabled = False : BtnExt.Enabled = False
            BtnStart.Enabled = False : BtnStop.Enabled = True
            AlarmMonkey.RunWorkerAsync()
        End If
    End Sub

    Private Sub BtnStop_Click(sender As Object, e As EventArgs) Handles BtnStop.Click
        TxBStatus.Text = "Stopping"
        AlarmMonkey.CancelAsync()
        BtnStop.Enabled = False
    End Sub

    Private Sub AlarmMonkey_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles AlarmMonkey.ProgressChanged
        WriteToLog(e.UserState.ToString)
        LblErrCount.Text = e.ProgressPercentage.ToString()
        TxBStatus.Text = e.UserState.ToString

        If e.UserState.ToString.Contains("FATAL") Then
            TxBStatus.Text = "Fatal error -> running external command" : WriteToLog("Fatal error -> running external command")
            Dim ExtComResult As String = RunExternalCommand()
            If ExtComResult <> "0" Then TxBStatus.Text = "Running external command failed" : WriteToLog(String.Format("Running external command failed: {0}", ExtComResult))
        End If

        If e.UserState.ToString.Equals("Setting Alarm ON") And CSettings1.ExtCLIComOnAlarm = True Then
            WriteToLog("Setting Alarm ON -> running external command")
            Dim ExtComResult As String = RunExternalCommand()
            If ExtComResult <> "0" Then TxBStatus.Text = "Running external command failed" : WriteToLog(String.Format("Running external command failed: {0}", ExtComResult))
        End If
    End Sub
    Private Sub AlarmMonkey_Completed(sender As Object, e As RunWorkerCompletedEventArgs) Handles AlarmMonkey.RunWorkerCompleted
        BtnMPList.Enabled = True : BtnSettings.Enabled = True : BtnDBCnn.Enabled = True : BtnModbusCnn.Enabled = True : BtnClose.Enabled = True : BtnExt.Enabled = True
        BtnStart.Enabled = True : BtnStop.Enabled = False
        If e.Error IsNot Nothing Then
            TxBStatus.Text = "AlarmMonkey crashed! See Log for details" : WriteToLog("AlarmMonkey crashed! Error: " & e.Error.Message)
            Dim ExtComResult As String = RunExternalCommand()
            If ExtComResult <> "0" Then TxBStatus.Text = "Running external command failed" : WriteToLog(String.Format("Running external command failed: {0}", ExtComResult))
        End If
        If FrmMainClosePending = True Then Me.Close()
    End Sub
    Private Sub FrmMainClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If AlarmMonkey.IsBusy = True Then
            TxBStatus.Text = "Stopping"
            AlarmMonkey.CancelAsync()
            FrmMainClosePending = True
            Me.Enabled = False
            e.Cancel = True
        End If
    End Sub

    Public Function WriteMPListToXML(Optional xmlFileName As String = "MPointList.xml") As String
        If xmlFileName = vbNullString Then Return "Error: MPointList File name null"
        Dim xmlSettings As New XmlWriterSettings : xmlSettings.Indent = True
        Dim xmlWriter As XmlWriter
        Try
            xmlWriter = xmlWriter.Create(xmlFileName, xmlSettings)
            With xmlWriter
                .WriteStartDocument()
                .WriteStartElement("MPointList")
                For Each MPoint In MPointList
                    .WriteStartElement("MPoint")
                    .WriteElementString("MPName", MPoint.MPName)
                    .WriteElementString("MPCode", MPoint.MPCode.ToString)
                    .WriteElementString("AlarmHigh1", MPoint.AlarmHigh1)
                    .WriteElementString("AlarmHigh2", MPoint.AlarmHigh2)
                    .WriteElementString("AlarmHigh3", MPoint.AlarmHigh3)
                    .WriteElementString("AlarmHigh4", MPoint.AlarmHigh4)
                    .WriteElementString("AlarmLow1", MPoint.AlarmLow1)
                    .WriteElementString("AlarmLow2", MPoint.AlarmLow2)
                    .WriteElementString("AlarmLow3", MPoint.AlarmLow3)
                    .WriteElementString("AlarmLow4", MPoint.AlarmLow4)
                    .WriteElementString("LastAlarm", MPoint.LastAlarm)
                    .WriteElementString("LastAlarmDT", MPoint.LastAlarmDT.ToString("yyyy-MM-dd HH:mm:ss"))
                    .WriteEndElement()
                Next
                .WriteEndElement()
                .WriteEndDocument()
                .Close()
            End With
        Catch ex As Exception
            xmlWriter = Nothing : xmlSettings = Nothing : Return "Error: " & ex.Message
        End Try
TerminateSub:
        xmlWriter = Nothing : xmlSettings = Nothing
        Return "0"
    End Function

    Public Sub ReadMPListFromXML(Optional xmlFileName As String = "MPointList.xml")
        If Not My.Computer.FileSystem.FileExists(xmlFileName) Then MsgBox(xmlFileName & " not found. MPointList not loaded.", MsgBoxStyle.Exclamation) : Exit Sub
        Dim xmlMPointListFile As New XmlDocument, xmlMPointList As XmlNodeList, xmlMPoint As XmlNode
        Try
            xmlMPointListFile.Load(xmlFileName)
            xmlMPointList = xmlMPointListFile.SelectNodes("MPointList/MPoint")
            If xmlMPointList.Count = 0 Then MsgBox("XML file empty or format not correct. MPointList not Loaded.", MsgBoxStyle.Exclamation) : GoTo TerminateSub
            MPointList.Clear()
            For Each xmlMPoint In xmlMPointList
                MPointList.Add(New MPoint)
                With MPointList.ElementAt(MPointList.Count - 1)
                    .MPName = xmlMPoint.ChildNodes.Item(0).InnerText
                    .MPCode = CInt(xmlMPoint.ChildNodes.Item(1).InnerText)
                    .AlarmHigh1 = xmlMPoint.ChildNodes.Item(2).InnerText
                    .AlarmHigh2 = xmlMPoint.ChildNodes.Item(3).InnerText
                    .AlarmHigh3 = xmlMPoint.ChildNodes.Item(4).InnerText
                    .AlarmHigh4 = xmlMPoint.ChildNodes.Item(5).InnerText
                    .AlarmLow1 = xmlMPoint.ChildNodes.Item(6).InnerText
                    .AlarmLow2 = xmlMPoint.ChildNodes.Item(7).InnerText
                    .AlarmLow3 = xmlMPoint.ChildNodes.Item(8).InnerText
                    .AlarmLow4 = xmlMPoint.ChildNodes.Item(9).InnerText
                    .LastAlarm = xmlMPoint.ChildNodes.Item(10).InnerText
                    If IsDate(xmlMPoint.ChildNodes.Item(11).InnerText) = True Then .LastAlarmDT = xmlMPoint.ChildNodes.Item(11).InnerText
                End With
            Next
        Catch ex As Exception
            xmlMPointListFile = Nothing : xmlMPointList = Nothing : xmlMPoint = Nothing
            MsgBox(" MPointList loading error: " & ex.Message, MsgBoxStyle.Exclamation)
        End Try
TerminateSub:
        xmlMPointListFile = Nothing : xmlMPointList = Nothing : xmlMPoint = Nothing
    End Sub

    Public Function WriteCSettingsToXML(Optional xmlFileName As String = "CSettings.xml") As String
        If xmlFileName = vbNullString Then Return "Error: CSettings File name null"
        Dim xmlSettings As New XmlWriterSettings : xmlSettings.Indent = True
        Dim xmlWriter As XmlWriter
        Try
            xmlWriter = xmlWriter.Create(xmlFileName, xmlSettings)
            With xmlWriter
                .WriteStartDocument()
                .WriteStartElement("CSettings")
                .WriteElementString("DBIP", CSettings1.DBIP)
                .WriteElementString("DBPort", CSettings1.DBPort)
                .WriteElementString("DBPath", CSettings1.DBPath)
                .WriteElementString("DBLogin", CSettings1.DBLogin)
                .WriteElementString("DBPass", CSettings1.DBPass)
                .WriteElementString("DBCharset", CSettings1.DBCharset)
                .WriteElementString("DBServerType", CSettings1.DBServerType)
                .WriteElementString("DBScanIntSeconds", CSettings1.DBScanIntSeconds)
                .WriteElementString("COMPort", CSettings1.COMPort)
                .WriteElementString("COMBaudRate", CSettings1.COMBaudRate)
                .WriteElementString("COMDataBits", CSettings1.COMDataBits)
                .WriteElementString("COMStopBits", CSettings1.COMStopBits)
                .WriteElementString("COMParity", CSettings1.COMParity)
                .WriteElementString("COMHandshake", CSettings1.COMHandshake)
                .WriteElementString("COMInRelON", CSettings1.COMInRelON)
                .WriteElementString("COMOutRelON", CSettings1.COMOutRelON)
                .WriteElementString("COMInGetRel", CSettings1.COMInGetRel)
                .WriteElementString("COMOutGetRel", CSettings1.COMOutGetRel)
                .WriteElementString("COMInRelOFF", CSettings1.COMInRelOFF)
                .WriteElementString("COMOutRelOFF", CSettings1.COMOutRelOFF)
                .WriteElementString("COMRetries", CSettings1.COMRetries)
                .WriteElementString("COMTimeoutMiliSeconds", CSettings1.COMTimeoutMiliSeconds)
                .WriteElementString("COMPingIntSeconds", CSettings1.COMPingIntSeconds)
                .WriteElementString("COMPingReportFailed", CSettings1.COMPingReportFailed)
                .WriteElementString("COMInPing", CSettings1.COMInPing)
                .WriteElementString("COMOutPing", CSettings1.COMOutPing)
                .WriteElementString("COMAlarmMaxDelayMinutes", CSettings1.COMAlarmMaxDelayMinutes)
                .WriteElementString("ExtCLICommand", CSettings1.ExtCLICommand)
                .WriteElementString("ExtCLICommandArgs", CSettings1.ExtCLICommandArgs)
                .WriteElementString("ExtCLIComOnAlarm", CSettings1.ExtCLIComOnAlarm)
                .WriteElementString("AlarmDurSeconds", CSettings1.AlarmDurSeconds)
                .WriteEndElement()
                .WriteEndDocument()
                .Close()
            End With
        Catch ex As Exception
            xmlWriter = Nothing : xmlSettings = Nothing
            Return "Error: " & ex.Message
        End Try
TerminateSub:
        xmlWriter = Nothing : xmlSettings = Nothing
        Return "0"
    End Function

    Public Sub ReadCSettingsFromXML(Optional xmlFileName As String = "CSettings.xml")
        If Not My.Computer.FileSystem.FileExists(xmlFileName) Then MsgBox(xmlFileName & " not found. Settings not loaded.", MsgBoxStyle.Exclamation) : Exit Sub
        Dim xmlCSettingsFile As New XmlDocument, xmlCSettings As XmlNode
        Try
            xmlCSettingsFile.Load(xmlFileName)
            xmlCSettings = xmlCSettingsFile.SelectSingleNode("CSettings")
            If IsNothing(xmlCSettings) = True Then MsgBox("XML file empty or format not correct. Settings not Loaded.", MsgBoxStyle.Exclamation) : GoTo TerminateSub
            With CSettings1
                .DBIP = xmlCSettings.ChildNodes.Item(0).InnerText
                .DBPort = xmlCSettings.ChildNodes.Item(1).InnerText
                .DBPath = xmlCSettings.ChildNodes.Item(2).InnerText
                .DBLogin = xmlCSettings.ChildNodes.Item(3).InnerText
                .DBPass = xmlCSettings.ChildNodes.Item(4).InnerText
                .DBCharset = xmlCSettings.ChildNodes.Item(5).InnerText
                .DBServerType = xmlCSettings.ChildNodes.Item(6).InnerText
                .DBScanIntSeconds = xmlCSettings.ChildNodes.Item(7).InnerText
                .COMPort = xmlCSettings.ChildNodes.Item(8).InnerText
                .COMBaudRate = xmlCSettings.ChildNodes.Item(9).InnerText
                .COMDataBits = xmlCSettings.ChildNodes.Item(10).InnerText
                .COMStopBits = xmlCSettings.ChildNodes.Item(11).InnerText
                .COMParity = xmlCSettings.ChildNodes.Item(12).InnerText
                .COMHandshake = xmlCSettings.ChildNodes.Item(13).InnerText
                .COMInRelON = xmlCSettings.ChildNodes.Item(14).InnerText
                .COMOutRelON = xmlCSettings.ChildNodes.Item(15).InnerText
                .COMInGetRel = xmlCSettings.ChildNodes.Item(16).InnerText
                .COMOutGetRel = xmlCSettings.ChildNodes.Item(17).InnerText
                .COMInRelOFF = xmlCSettings.ChildNodes.Item(18).InnerText
                .COMOutRelOFF = xmlCSettings.ChildNodes.Item(19).InnerText
                .COMRetries = xmlCSettings.ChildNodes.Item(20).InnerText
                .COMTimeoutMiliSeconds = xmlCSettings.ChildNodes.Item(21).InnerText
                .COMPingIntSeconds = xmlCSettings.ChildNodes.Item(22).InnerText
                .COMPingReportFailed = xmlCSettings.ChildNodes.Item(23).InnerText
                .COMInPing = xmlCSettings.ChildNodes.Item(24).InnerText
                .COMOutPing = xmlCSettings.ChildNodes.Item(25).InnerText
                .COMAlarmMaxDelayMinutes = xmlCSettings.ChildNodes.Item(26).InnerText
                .ExtCLICommand = xmlCSettings.ChildNodes.Item(27).InnerText
                .ExtCLICommandArgs = xmlCSettings.ChildNodes.Item(28).InnerText
                .ExtCLIComOnAlarm = xmlCSettings.ChildNodes.Item(29).InnerText
                .AlarmDurSeconds = xmlCSettings.ChildNodes.Item(30).InnerText
            End With
        Catch ex As Exception
            xmlCSettingsFile = Nothing : xmlCSettings = Nothing
            MsgBox("Settings loading error: " & ex.Message, MsgBoxStyle.Exclamation)
        End Try
TerminateSub:
        xmlCSettingsFile = Nothing : xmlCSettings = Nothing
    End Sub

    Private Function COMOpen() As String
        If SerialPort1.IsOpen = True Then Return "0"
        Try
            With SerialPort1
                .PortName = CSettings1.COMPort
                .BaudRate = CSettings1.COMBaudRate
                .DataBits = CSettings1.COMDataBits
                .StopBits = CSettings1.COMStopBits
                .Parity = CSettings1.COMParity
                .Handshake = CSettings1.COMHandshake
            End With
            SerialPort1.Open()
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
        Return "0"
    End Function

    Private Function COMCommand(COMIn As String, COMOut As String) As String
        If COMIn = vbNullString Or COMOut = vbNullString Then Return "Error: MODBUS input or output string missing"
        If SerialPort1.IsOpen = False Then Return "Error: COM command aborted, COM port not open"
        Dim COMRead As String = vbNullString
        Dim ElapsedTime As Integer = 0
        Try
            SerialPort1.Write(COMIn & vbCr)
            Do
                System.Threading.Thread.Sleep(100)
                ElapsedTime += 100
                COMRead += SerialPort1.ReadExisting
                If COMRead.ToUpper.Replace(vbCr, "").Replace(vbLf, "") = COMOut.ToUpper Then GoTo TerminateSub
                If ElapsedTime >= CSettings1.COMTimeoutMiliSeconds Then GoTo TerminateSub
            Loop
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
TerminateSub:
        If COMRead.ToUpper.Replace(vbCr, "").Replace(vbLf, "") = COMOut.ToUpper Then
            Return "0"
        Else
            Return String.Format("Error: Received data ""{0}"" does not match COMOut string ""{1}"".", COMRead.Replace(vbCr, "").Replace(vbLf, ""), COMOut)
        End If
    End Function

    Private Function DBConnect() As String
        Try
            With CSettings1
                DBConn.ConnectionString = String.Format("User={0};Password={1};Database={2};Datasource={3};Port={4};Charset={5};ServerType={6}",
                                                        .DBLogin, .DBPass, .DBPath, .DBIP, .DBPort, .DBCharset, .DBServerType)
            End With
            DBConn.Open()
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
        Return "0"
    End Function

    Private Sub UpdateMPCodes()

        Dim DBConnResult As String = DBConnect()
        If DBConnResult <> "0" Then MsgBox("MPCodes not updated from DB. The Alarms will not work withour proper MPCodes.", MsgBoxStyle.Exclamation) : Exit Sub

        Dim queryString As String = vbNullString, fbAdapter As New FbDataAdapter : Dim MPCodesTable As New DataSet
        Try
            For Each MPoint In MPointList
                If MPoint.MPName <> vbNullString Then queryString = queryString & ",'" & MPoint.MPName & "'"
            Next
            queryString = queryString.Remove(0, 1)
            queryString = String.Format("SELECT MEASUREPOINT.MP_NAME, MEASUREPOINT.MP_CODE " & _
                                        "FROM MEASUREPOINT INNER JOIN LASTALARM ON MEASUREPOINT.MP_CODE = LASTALARM.MP_CODE " & _
                                        "WHERE MEASUREPOINT.MP_NAME in ({0})", queryString)
            fbAdapter.SelectCommand = New FbCommand(queryString, DBConn)
            fbAdapter.Fill(MPCodesTable)
        Catch ex As Exception
            queryString = Nothing : fbAdapter = Nothing : DBConn.Close()
            MsgBox("MPCodes not retrieved from DB, the Alarms will not work withour MPCodes", MsgBoxStyle.Exclamation) : Exit Sub
        End Try
        queryString = Nothing : fbAdapter = Nothing : DBConn.Close()

        Dim ErrorFlag As Boolean = False
        For i = 0 To MPointList.Count - 1
            If MPointList.Item(i).MPName <> vbNullString Then
                Dim LastAlarmsTableRows() As Data.DataRow = MPCodesTable.Tables(0).Select("MP_NAME = '" & MPointList.Item(i).MPName & "'")
                If LastAlarmsTableRows.Count = 0 Then
                    ErrorFlag = True
                    MPointList.Item(i).MPCode = 0
                Else
                    MPointList.Item(i).MPCode = LastAlarmsTableRows(0).Item("MP_CODE")
                End If

            End If
        Next
        If ErrorFlag = True Then MsgBox("Some MPCodes not retrieved from DB, check MP names if correct.", MsgBoxStyle.Exclamation)
    End Sub

    Private Function DBQuery() As String
        Dim queryString As String = vbNullString, fbAdapter As New FbDataAdapter
        Try
            If LastAlarmsTable.Tables.Count <> 0 Then LastAlarmsTable.Tables(0).Clear()
            For Each MPoint In MPointList
                If MPoint.MPCode <> 0 Then queryString = queryString & ",'" & MPoint.MPCode.ToString & "'"
            Next
            queryString = queryString.Remove(0, 1)
            queryString = String.Format("SELECT MP_CODE, LA_ALARMLEVEL, LA_ALARMDT FROM LASTALARM " & _
                                        "WHERE MP_CODE in ({0})", queryString)
            : fbAdapter.SelectCommand = New FbCommand(queryString, DBConn)
            fbAdapter.Fill(LastAlarmsTable)
        Catch ex As Exception
            queryString = Nothing : fbAdapter = Nothing
            Return "Error: " & ex.Message
        End Try
        queryString = Nothing : fbAdapter = Nothing
        Return "0"
    End Function

    Private Sub WriteToLog(Optional LogEntry As String = vbNullString)
        Dim LogWriter As StreamWriter
        Try
            LogWriter = File.AppendText("TDOModbusLog.txt")
            LogWriter.WriteLine(Now().ToString("yyyy-MM-dd HH:mm:ss") & " : " & LogEntry)
            LogWriter.Flush()
            LogWriter.Close()
        Catch ex As Exception
            LogWriter = Nothing
        End Try
        LogWriter = Nothing
    End Sub

    Private Function RunExternalCommand() As String
        If CSettings1.ExtCLICommand = vbNullString Then Return "0"
        Dim p As New Process, pi As New ProcessStartInfo
        Try
            pi.Arguments = CSettings1.ExtCLICommandArgs
            pi.FileName = CSettings1.ExtCLICommand
            pi.UseShellExecute = True
            pi.WindowStyle = ProcessWindowStyle.Minimized
            pi.CreateNoWindow = False
            p.StartInfo = pi
            p.Start()
        Catch ex As Exception
            p = Nothing : pi = Nothing
            Return "Error: " & ex.Message
        End Try
        p = Nothing : pi = Nothing
        Return "0"
    End Function

    Private Sub AlarmMonkey_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles AlarmMonkey.DoWork

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Dim dBConnResult As String = "0"
        Dim bBQueryResult As String = "0"
        Dim COMConnResult As String = "0"
        Dim COMComResult As String = "0"
        Dim writeMPListResult As String = "0"
        Dim alarmFlag As Date = New Date(1900, 1, 1)
        Dim errorsCount As Integer = 0
        Dim currentAlarm As String = ""
        Dim dTCounterMain As Integer = 0
        Dim dTCounterPing As Integer = 0
        Dim pingFails As Integer = 0

        worker.ReportProgress(errorsCount, "AlarmMonkey Started")

        COMConnResult = COMOpen()
        If COMConnResult <> "0" Then
            errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("FATAL: Connection to COM failed: {0}", COMConnResult))
            GoTo TerminateAlarmMonkeyFatal
        End If

        Do
            If worker.CancellationPending = True Then GoTo TerminateAlarmMonkey
            dTCounterMain = (Now.Hour * 3600 + Now.Minute * 60 + Now.Second) Mod CSettings1.DBScanIntSeconds
            dTCounterPing = (Now.Hour * 3600 + Now.Minute * 60 + Now.Second) Mod CSettings1.COMPingIntSeconds

            'DB Lookup and Alarm Trigger routine

            If dTCounterMain = 10 Then

                dBConnResult = DBConnect()
                If dBConnResult <> "0" Then
                    errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("FATAL: Connection to DB failed: {0}", dBConnResult))
                    GoTo AlarmTrigger
                End If

                bBQueryResult = DBQuery() : DBConn.Close()
                If bBQueryResult <> "0" Then
                    errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("FATAL: DB Query failed: {0}", bBQueryResult))
                    GoTo AlarmTrigger
                End If

                For i = 0 To MPointList.Count - 1
                    With MPointList.Item(i)
                        Dim LastAlarmsTableRows() As Data.DataRow = LastAlarmsTable.Tables(0).Select("MP_CODE = '" & .MPCode & "'")
                        If LastAlarmsTableRows.Count = 0 Then
                            errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("The MPCode {0} was not found in LastAlarms table.", .MPName))
                        ElseIf LastAlarmsTableRows.Count > 1 Then
                            errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("More than one row with the MPCode {0} found in LastAlarms table.", .MPCode))
                        Else
                            currentAlarm = LastAlarmsTableRows(0).Item("LA_ALARMLEVEL").ToString
                            If .LastAlarm = vbNullString Then .LastAlarm = currentAlarm
                            If .LastAlarm <> currentAlarm Then
                                If currentAlarm <> "ALL_NOALARM" And _
                                    ((currentAlarm = "ALL_HIGH4" And .AlarmHigh4 = "1") Or _
                                     (currentAlarm = "ALL_HIGH3" And .AlarmHigh3 = "1" And Not StrBelongs(.LastAlarm, "ALL_HIGH4")) Or _
                                     (currentAlarm = "ALL_HIGH2" And .AlarmHigh2 = "1" And Not StrBelongs(.LastAlarm, "ALL_HIGH4", "ALL_HIGH3")) Or _
                                     (currentAlarm = "ALL_HIGH1" And .AlarmHigh1 = "1" And Not StrBelongs(.LastAlarm, "ALL_HIGH4", "ALL_HIGH3", "ALL_HIGH2")) Or _
                                     (currentAlarm = "ALL_LOW1" And .AlarmLow1 = "1" And Not StrBelongs(.LastAlarm, "ALL_LOW4", "ALL_LOW3", "ALL_LOW2")) Or _
                                     (currentAlarm = "ALL_LOW2" And .AlarmLow2 = "1" And Not StrBelongs(.LastAlarm, "ALL_LOW4", "ALL_LOW3")) Or _
                                     (currentAlarm = "ALL_LOW3" And .AlarmLow3 = "1" And Not StrBelongs(.LastAlarm, "ALL_LOW4")) Or _
                                     (currentAlarm = "ALL_LOW4" And .AlarmLow4 = "1")) _
                                Then
                                    alarmFlag = Now() : worker.ReportProgress(errorsCount, String.Format("Alarm condition met for: {0} - {1}, from {2} to {3} .", .MPName, .MPCode, .LastAlarm, currentAlarm))
                                    .LastAlarmDT = Now()
                                End If
                                .LastAlarm = currentAlarm
                            End If
                        End If
                    End With
                Next

                writeMPListResult = WriteMPListToXML()
                If writeMPListResult <> "0" Then errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("Writing MPList to XML failed: {0}", writeMPListResult))
AlarmTrigger:
                If Now.AddMinutes(-CSettings1.COMAlarmMaxDelayMinutes) <= alarmFlag Then

                    worker.ReportProgress(errorsCount, "Setting Alarm ON")

                    For i = 0 To CSettings1.COMRetries
                        COMComResult = COMCommand(CSettings1.COMInRelON, CSettings1.COMOutRelON)
                        If COMComResult = "0" Then Exit For
                    Next i
                    If COMComResult <> "0" Then
                        errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("Setting Alarm ON failed: {0}  -> Trying to reconnect to COM and resend Alarm ON command.", COMComResult))
                        If SerialPort1.IsOpen = True Then SerialPort1.Close()
                        System.Threading.Thread.Sleep(1000)
                        COMConnResult = COMOpen()
                        If COMConnResult <> "0" Then
                            errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("FATAL: Reconnecting to COM failed: {0}", COMConnResult))
                            GoTo MainLoopEnd
                        End If
                        worker.ReportProgress(errorsCount, "Reconnected to COM. Resending Alarm ON command.")
                        For i = 0 To CSettings1.COMRetries
                            COMComResult = COMCommand(CSettings1.COMInRelON, CSettings1.COMOutRelON)
                            If COMComResult = "0" Then Exit For
                        Next i
                        If COMComResult <> "0" Then
                            errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("FATAL: Resending Alarm ON failed: {0}", COMComResult))
                            GoTo MainLoopEnd
                        End If
                    End If

                    worker.ReportProgress(errorsCount, "Confirming Alarm State")
                    For i = 0 To CSettings1.COMRetries
                        COMComResult = COMCommand(CSettings1.COMInGetRel, CSettings1.COMOutGetRel)
                        If COMComResult = "0" Then alarmFlag = New Date(1900, 1, 1) : Exit For
                    Next i
                    If COMComResult <> "0" Then
                        errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("FATAL: Confirming Alarm state failed: {0}", COMComResult))
                        GoTo MainLoopEnd
                    End If

                    worker.ReportProgress(errorsCount, String.Format("Alarm running: Waiting {0} seconds before setting Alarm OFF", CSettings1.AlarmDurSeconds))
                    For i = CSettings1.AlarmDurSeconds To 0 Step -1
                        System.Threading.Thread.Sleep(1000)
                        If i = 5 Then worker.ReportProgress(errorsCount, String.Format("Alarm running: {0} seconds Left", i))
                        If worker.CancellationPending = True Then GoTo TerminateAlarmMonkey
                    Next i

                    worker.ReportProgress(errorsCount, "Setting Alarm OFF")
                    For i = 0 To CSettings1.COMRetries
                        COMComResult = COMCommand(CSettings1.COMInRelOFF, CSettings1.COMOutRelOFF)
                        If COMComResult = "0" Then Exit For
                    Next i
                    If COMComResult <> "0" Then
                        errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("FATAL: Setting Alarm OFF failed: {0}", COMComResult))
                    End If

                    GoTo MainLoopEnd
                End If
            End If

            'COM ping routine

            If dTCounterPing = 10 Then

                For i = 0 To CSettings1.COMRetries
                    COMComResult = COMCommand(CSettings1.COMInPing, CSettings1.COMOutPing)
                    If COMComResult = "0" Then
                        If pingFails <> 0 Then worker.ReportProgress(errorsCount, String.Format("Ping OK after {0} retries.", pingFails)) : pingFails = 0
                        Exit For
                    End If
                Next i
                If COMComResult <> "0" Then
                    errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("COM Ping failed: {0}  -> Trying to reconnect to COM and Ping again.", COMComResult))
                    If SerialPort1.IsOpen = True Then SerialPort1.Close()
                    System.Threading.Thread.Sleep(1000)
                    COMConnResult = COMOpen()
                    If COMConnResult <> "0" Then
                        errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("FATAL: Reconnecting to COM failed: {0}", COMConnResult))
                        GoTo MainLoopEnd
                    End If
                    worker.ReportProgress(errorsCount, "Reconnected to COM. Retrying Ping")
                    For i = 0 To CSettings1.COMRetries
                        COMComResult = COMCommand(CSettings1.COMInPing, CSettings1.COMOutPing)
                        If COMComResult = "0" Then Exit For
                    Next i
                    If COMComResult = "0" Then
                        worker.ReportProgress(errorsCount, "Ping retry OK.")
                        If pingFails <> 0 Then worker.ReportProgress(errorsCount, String.Format("Ping OK after {0} retries.", pingFails)) : pingFails = 0
                    Else
                        errorsCount += 1 : worker.ReportProgress(errorsCount, String.Format("COM Ping retry failed: {0}", COMComResult))
                        pingFails += 1
                        If CSettings1.COMPingReportFailed > 0 And (pingFails = CSettings1.COMPingReportFailed Or pingFails = CSettings1.COMPingReportFailed + 5 Or pingFails = CSettings1.COMPingReportFailed + 10) Then
                            worker.ReportProgress(errorsCount, String.Format("FATAL: Ping retry {0} failed", pingFails))
                        End If
                    End If
                End If

            End If
MainLoopEnd:
            System.Threading.Thread.Sleep(600)
        Loop


TerminateAlarmMonkey:
        SerialPort1.Close()
        worker.ReportProgress(errorsCount, "AlarmMonkey Stopped")
        e.Cancel = True
        Exit Sub
TerminateAlarmMonkeyFatal:
        worker.ReportProgress(errorsCount, "AlarmMonkey stopped due to errors")
    End Sub


    Public Function StrBelongs(StrIn As String, StrC1 As String, Optional StrC2 As String = "", Optional StrC3 As String = "", Optional StrC4 As String = "", _
                             Optional StrC5 As String = "", Optional StrC6 As String = "", Optional StrC7 As String = "", Optional StrC8 As String = "") As Boolean
        Try
            Dim Strings() As String = {StrC1, StrC2, StrC3, StrC4, StrC5, StrC6, StrC7, StrC8}

            If StrIn <> vbNullString And Strings.Contains(StrIn) Then Return True Else Return False
        Catch
            Return False
        End Try
    End Function

End Class
