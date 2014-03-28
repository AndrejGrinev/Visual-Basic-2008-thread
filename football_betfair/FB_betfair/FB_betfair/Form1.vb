Imports System.Threading
Imports System.Net
Imports System.Net.Sockets
Public Class Form1
    Dim oHeaderGL As New BFGlobal.APIRequestHeader
    Dim BetfairGL As New BFGlobal.BFGlobalService
    Dim WithEvents BetFairUK As New BFUK.BFExchangeService
    Const SessTokFile = "C:\Football_Betfair\SessToken.txt"
    Private Tabla As New DataTable
    Dim field As Giraffe()
    Dim operat30 As New Thread(AddressOf operation30) 'СОздаем новый поток для операции 1
    Dim operat60 As New Thread(AddressOf operation60) 'СОздаем новый поток для операции 2
    Dim operat180 As New Thread(AddressOf operation180) 'СОздаем новый поток для операции 3
    Dim operat300 As New Thread(AddressOf operation300) 'СОздаем новый поток для операции 4
    Private Declare Function ShowWindow Lib "user32" (ByVal handle As IntPtr, ByVal nCmdShow As Integer) As Integer
    Private Const SW_HIDE = 0
    Private Const SW_RESTORE = 9

    Sub CheckHeader(ByVal Header As BFGlobal.APIResponseHeader)
        With Header
            oHeaderGL.sessionToken = .sessionToken
        End With
    End Sub

    Sub CheckHeader(ByVal Header As BFUK.APIResponseHeader)
        With Header
            oHeaderGL.sessionToken = .sessionToken
        End With
    End Sub

    Function oHeaderUK() As BFUK.APIRequestHeader
        Dim Header As New BFUK.APIRequestHeader
        Header.sessionToken = oHeaderGL.sessionToken
        Return Header
    End Function

    Sub Print(ByVal Message As String)
        With TLog
            .SelectionStart = .Text.Length
            .SelectedText = vbCrLf & Message
        End With
    End Sub

    Sub MakeMyForm(ByVal ik As Integer)
        Dim zz As IntPtr
        Dim myForm = New Form2()
        With myForm
            .Name = field(ik).menupath
            .Text = field(ik).menupath
            .AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
            .AutoSize = True
            .Visible = True
        End With
        zz = myForm.Handle
        field(ik).hndl = zz
        ShowWindow(zz, SW_HIDE)
        myForm.txt1.Text = field(ik).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString
        myForm.txt2.Text = field(ik).res_back1
        myForm.txt3.Text = field(ik).res_lay1
        myForm.txt4.Text = field(ik).HT_back1
        myForm.txt5.Text = field(ik).HT_lay1
        myForm.txt6.Text = System.Math.Round(field(ik).back_lay, 2)
        myForm.txt7.Text = System.Math.Round(field(ik).lay_back, 2)
        myForm.txt8.Text = 0
        myForm.txt9.Text = 0
        myForm.txt10.Text = 0
        If myForm.txt6.Text > 0 Then
            myForm.txt6.BackColor = Color.SpringGreen
        Else
            myForm.txt6.BackColor = Color.Empty
        End If
        If myForm.txt7.Text > -2 Then
            myForm.txt7.BackColor = Color.SpringGreen
        Else
            myForm.txt7.BackColor = Color.Empty
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oHeaderGL.sessionToken = My.Computer.FileSystem.ReadAllText(SessTokFile)      '(from Step 3)
        BetfairGL.UserAgent = ""
        BetFairUK.UserAgent = ""
        BetfairGL.EnableDecompression = True
        BetFairUK.EnableDecompression = True
        ReDim field(200)
        For i = 0 To UBound(field)
            field(i) = New Giraffe
        Next
        With Tabla.Columns
            .Add("ВРЕМЯ")
            .Add("МАТЧ")
            .Add("BACK2")
            .Add("BACK1")
            .Add("LAY1")
            .Add("LAY2")
            .Add("BAKK2")
            .Add("BAKK1")
            .Add("LAYY1")
            .Add("LAYY2")
            .Add("Back-Lay")
            .Add("Lay-Back")
        End With
        gvData.DataSource = Tabla
        gvData.Columns(2).DefaultCellStyle.BackColor = Color.LightBlue
        gvData.Columns(3).DefaultCellStyle.BackColor = Color.LightBlue
        gvData.Columns(4).DefaultCellStyle.BackColor = Color.Plum
        gvData.Columns(5).DefaultCellStyle.BackColor = Color.Plum
        gvData.Columns(6).DefaultCellStyle.BackColor = Color.LightBlue
        gvData.Columns(7).DefaultCellStyle.BackColor = Color.LightBlue
        gvData.Columns(8).DefaultCellStyle.BackColor = Color.Plum
        gvData.Columns(9).DefaultCellStyle.BackColor = Color.Plum
        Control.CheckForIllegalCrossThreadCalls = False
        Print("*** Login ***")
        Dim oLoginReq As New BFGlobal.LoginReq
        Dim oLoginResp As BFGlobal.LoginResp
        With oLoginReq
            .username = "username"
            .password = "password"
            .productId = 82
        End With
        oLoginResp = BetfairGL.login(oLoginReq)
        With oLoginResp
            CheckHeader(.header)
            Print("ErrorCode = " & .errorCode.ToString)
        End With
        ButMarkets()
    End Sub

    Private Sub TestForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        My.Computer.FileSystem.WriteAllText(SessTokFile, oHeaderGL.sessionToken, False)
        operat30.Abort()
        operat60.Abort()
        operat180.Abort()
        operat300.Abort()
    End Sub

    Private Function IsOnline() As Boolean
        Try
            Dim dummy As IPHostEntry = Dns.GetHostEntry("www.google.com")
            Return True
        Catch ex As SocketException
            Return False
        End Try
    End Function

    Private Sub zapolnenie_field(ByVal i As Integer)
         If IsOnline() Then
            Dim s1, d11, kot, kht_0, kht_46, pot, z1, z2, d31, kot2, kht_01, d1, d2, d3, d4, d1q, d2q, d3q, d4q As Decimal
            Dim ffg As BFUK.GetMarketPricesCompressedResp
            Dim ffg2 As BFUK.GetCompleteMarketPricesCompressedResp
            Dim oMPCreq As New BFUK.GetMarketPricesCompressedReq
            Dim oMPC2req As New BFUK.GetCompleteMarketPricesCompressedReq
            With oMPCreq
                .header = oHeaderUK()
                .marketId = field(i).marketid_res
            End With
            Try
                ffg = BetFairUK.getMarketPricesCompressed(oMPCreq)
            Catch ex As Exception
                MsgBox(ex.ToString, MsgBoxStyle.Critical)
            End Try
            d1 = 1.0
            d1q = 0.0
            d2 = 1.0
            d2q = 0.0
            d3 = 1000.0
            d3q = 0.0
            d4 = 1000.0
            d4q = 0.0
            With ffg
                CheckHeader(.header)
                If .errorCode = BFUK.GetMarketPricesErrorEnum.OK Then
                    Dim oMarketPrices As New UnpackMarketPricesCompressed(.marketPrices)
                    With oMarketPrices
                        If .marketStatus = BFUK.MarketStatusEnum.ACTIVE Then
                            field(i).inp = .delay
                            If field(i).sel = 0 And .delay > 0 Then
                                field(i).sel = 3
                            Else
                                For ii = 0 To .runnerPrices.Length - 1
                                    With .runnerPrices(ii)
                                        If (.sortOrder = 1) Then
                                            If .bestPricesToBack.Length > 0 Then
                                                d1 = .bestPricesToBack(0).price
                                                d1q = .bestPricesToBack(0).amountAvailable
                                            Else
                                                d1 = 1.0
                                                d1q = 0.0
                                            End If
                                            If .bestPricesToBack.Length > 1 Then
                                                d2 = .bestPricesToBack(1).price
                                                d2q = .bestPricesToBack(1).amountAvailable
                                            Else
                                                d2 = 1.0
                                                d2q = 0.0
                                            End If
                                            If .bestPricesToLay.Length > 0 Then
                                                d3 = .bestPricesToLay(0).price
                                                d3q = .bestPricesToLay(0).amountAvailable
                                            Else
                                                d3 = 1000.0
                                                d3q = 0.0
                                            End If
                                            If .bestPricesToLay.Length > 1 Then
                                                d4 = .bestPricesToLay(1).price
                                                d4q = .bestPricesToLay(1).amountAvailable
                                            Else
                                                d4 = 1000.0
                                                d4q = 0.0
                                            End If
                                        End If
                                    End With
                                Next
                            End If
                        Else
                            If .marketStatus = BFUK.MarketStatusEnum.CLOSED Then
                                If field(i).sel = 1 Or field(i).sel = 11 Or field(i).sel = 111 Or field(i).sel = 1111 Then
                                    field(i).sel = 2
                                Else
                                    field(i).sel = 3
                                End If
                            End If
                        End If
                    End With
                End If
            End With
            field(i).res_back1 = d1
            field(i).res_back1am = d1q
            field(i).res_back2 = d2
            field(i).res_back2am = d2q
            field(i).res_lay1 = d3
            field(i).res_lay1am = d3q
            field(i).res_lay2 = d4
            field(i).res_lay2am = d4q
            field(i).rass = d1 / (System.Math.Exp((48 / 94) * System.Math.Log(d1)))
            d11 = d1
            d31 = d3
            d1 = 1.0
            d1q = 0.0
            d2 = 1.0
            d2q = 0.0
            d3 = 1000.0
            d3q = 0.0
            d4 = 1000.0
            d4q = 0.0
            With oMPC2req
                .header = oHeaderUK()
                .marketId = field(i).marketid_HT
            End With
            ffg2 = BetFairUK.getCompleteMarketPricesCompressed(oMPC2req)
            With ffg2
                CheckHeader(.header)
                If .errorCode = BFUK.GetCompleteMarketPricesErrorEnum.OK Then
                    Dim oMarketPrices As New UnpackCompleteMarketPricesCompressed(.completeMarketPrices)
                    With oMarketPrices
                        For ii = 0 To .runnerInfo.Length - 1
                            With .runnerInfo(ii)
                                If (.sortOrder = 1) Then
                                    For j = 0 To .prices.Length - 1
                                        With .prices(j)
                                            If (.backAmount > 0) And (.layAmount = 0) Then
                                                If .price > d1 Then
                                                    d2 = d1
                                                    d2q = d1q
                                                    d1 = .price
                                                    d1q = .backAmount
                                                End If
                                            End If
                                            If (.layAmount > 0) And (.backAmount = 0) Then
                                                If d3 > 999 Then
                                                    d3 = .price
                                                    d3q = .layAmount
                                                Else
                                                    If d4 > 999 Then
                                                        d4 = .price
                                                        d4q = .layAmount
                                                    End If
                                                End If
                                            End If
                                        End With
                                    Next
                                End If
                            End With
                        Next
                    End With
                Else
                    If .errorCode = BFUK.GetCompleteMarketPricesErrorEnum.EVENT_CLOSED Then
                        If field(i).sel = 1 Or field(i).sel = 11 Or field(i).sel = 111 Or field(i).sel = 1111 Then
                            field(i).sel = 2
                        Else
                            field(i).sel = 3
                        End If
                    End If
                End If
            End With
            field(i).HT_back1 = d1
            field(i).HT_back1am = d1q
            field(i).HT_back2 = d2
            field(i).HT_back2am = d2q
            field(i).HT_lay1 = d3
            field(i).HT_lay1am = d3q
            field(i).HT_lay2 = d4
            field(i).HT_lay2am = d4q
            s1 = (d3 - 1) / 0.95
            kht_0 = d3
            kht_01 = d1
            kht_46 = 1
            kot = System.Math.Exp((48 / 94) * System.Math.Log(d11))
            kot2 = System.Math.Exp((48 / 94) * System.Math.Log(d31))
            pot = 0.95 * (d11 / kot - 1) + s1 * ((kht_46 - kht_0) / (kht_46 * (kht_0 - 1)))
            z2 = 100 * pot / (s1 + 1)
            field(i).back_lay = z2
            s1 = (d31 - 1) / 0.95
            pot = 0.95 * (kht_01 / kht_46 - 1) + s1 * ((kot2 - d31) / (kot2 * (d31 - 1)))
            z1 = 100 * pot / (s1 + 1)
            field(i).lay_back = z1
            If field(i).sel <> 2 And field(i).sel <> 3 Then
                If field(i).back_lay < -7.5 Then
                    field(i).sel = 1111
                End If
                If field(i).back_lay < 0 And field(i).back_lay >= -7.5 Then
                    field(i).sel = 111
                End If
                If field(i).back_lay >= 0 Then
                    field(i).sel = 11
                End If
                If field(i).back_lay > 5 And field(i).inp = 0 And field(i).res_back1 < 1000 And field(i).res_lay1 < 1000 And field(i).HT_back1 < 1000 And field(i).HT_lay1 < 1000 Then
                    field(i).sel = 1
                End If
                If field(i).lay_back > -2 And field(i).inp = 0 And field(i).res_back1 < 1000 And field(i).res_lay1 < 1000 And field(i).HT_back1 < 1000 And field(i).HT_lay1 < 1000 Then
                    field(i).sel = 1
                End If
                If field(i).back_lay > Math.Abs(field(i).lay_back) And field(i).inp = 0 And field(i).res_back1 < 1000 And field(i).res_lay1 < 1000 And field(i).HT_back1 < 1000 And field(i).HT_lay1 < 1000 Then
                    field(i).sel = 1
                End If
                If field(i).lay_back > 0 And field(i).inp > 0 And field(i).res_back1 < 1000 And field(i).res_lay1 < 1000 And field(i).HT_back1 < 1000 And field(i).HT_lay1 < 1000 Then
                    field(i).sel = 1
                End If
                If field(i).back_lay > Math.Abs(2.5 * field(i).lay_back) And field(i).inp > 0 And field(i).res_back1 < 1000 And field(i).res_lay1 < 1000 And field(i).HT_back1 < 1000 And field(i).HT_lay1 < 1000 Then
                    field(i).sel = 1
                End If
            End If
        End If
    End Sub

    Private Sub ButMarkets()
        Dim i1, i2, i3, i4 As Integer
        Dim str1, str2, str3 As String
        Dim row As DataRow
        i1 = 0
        i3 = 0
        i4 = 0
        gvData.DataSource = Tabla
        Tabla.Clear()
        gvData.Refresh()
        Dim oMarketsReq As New BFUK.GetAllMarketsReq
        Dim oMarketsResp As BFUK.GetAllMarketsResp
        With oMarketsReq
            .header = oHeaderUK()
            ReDim .eventTypeIds(0) : .eventTypeIds(0) = 1
            .fromDate = Today
            .toDate = Today.AddDays(1)
        End With
        oMarketsResp = BetFairUK.getAllMarkets(oMarketsReq)
        With oMarketsResp
            CheckHeader(.header)
            Print("ErrorCode = " & .errorCode.ToString)
            If .errorCode = BFUK.GetAllMarketsErrorEnum.OK Then
                Dim AllMarkets As New UnpackAllMarkets(.marketData)
                Dim names As String(), TodaysCard As New List(Of MarketDataType)
                With AllMarkets
                    For i = 0 To .marketData.Length - 1
                        If (.marketData(i).marketName = "Результат" Or .marketData(i).marketName = "Счёт после 1-го тайма") And .marketData(i).turningInPlay Then
                            TodaysCard.Add(.marketData(i))
                        End If
                    Next
                End With
                TodaysCard.Sort(New CompareMarketTimes)
                For Each Race In TodaysCard
                    With Race
                        names = .menuPath.Split("\")
                        str1 = .menuPath
                        i2 = 0
                        For i = 0 To UBound(field)
                            str2 = field(i).menupath
                            If str2 = str1 Then
                                i2 = 1
                            End If
                        Next
                        If i2 = 0 Then
                            field(i3).menupath = str1
                            field(i3).id = i3
                            field(i3).eventDate = .eventDate
                            field(i3).inp = .betDelay
                            field(i3).sel = 0
                            field(i3).start_b1 = 0
                            field(i3).start_b2 = 0
                            field(i3).start_l1 = 0
                            field(i3).start_l2 = 0
                            field(i3).risk_bl = 100
                            field(i3).risk_lb = 100
                            i3 = i3 + 1
                        End If
                        i1 = i1 + 1
                        Print(i1 & " " & .marketId & " " & .marketStatus & "  " & .marketName & "  " & .menuPath & "  " & .eventDate.ToLocalTime.AddHours(1).ToString & "  " & .totalAmountMatched & "  " & .betDelay)
                    End With
                Next
                TextBox1.Text = i3
                For i = 0 To i3 - 1
                    str2 = field(i).menupath
                    str3 = ""
                    For Each Race In TodaysCard
                        With Race
                            str1 = .menuPath
                            str3 = .marketName
                            If (str1 = str2) Then
                                If (str3 = "Счёт после 1-го тайма") Then
                                    field(i).marketid_HT = .marketId
                                End If
                                If (str3 = "Результат") Then
                                    field(i).marketid_res = .marketId
                                End If
                            End If
                        End With
                    Next
                Next
                For i = 0 To i3 - 1
                    ' i2 = field(i).menupath.LastIndexOf("Игры")
                    'If (i2 > 0) Then
                    'str3 = field(i).menupath.Substring(i2)
                    str3 = field(i).menupath
                    i2 = str3.LastIndexOf("\")
                    str2 = str3.Substring(i2 + 1)
                    field(i).menupath = str2
                    'End If
                    zapolnenie_field(i)
                    If i = 59 Or i = 119 Or i = 179 Or i = 239 Or i = 299 Or i = 359 Then
                        System.Threading.Thread.Sleep(60000)
                    End If
                Next
                For i = 0 To i3 - 1
                    MakeMyForm(i)
                    row = Tabla.NewRow
                    If field(i).inp > 0 Then
                        gvData.Columns(1).CellTemplate.Style.BackColor = Color.Tomato
                    Else
                        gvData.Columns(1).CellTemplate.Style.BackColor = Color.Empty
                    End If
                    row("ВРЕМЯ") = field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString
                    row("МАТЧ") = field(i).menupath
                    row("BACK1") = CStr(field(i).res_back1) + Environment.NewLine + "$" + CStr(field(i).res_back1am)
                    row("BACK2") = CStr(field(i).res_back2) + Environment.NewLine + "$" + CStr(field(i).res_back2am)
                    row("LAY1") = CStr(field(i).res_lay1) + Environment.NewLine + "$" + CStr(field(i).res_lay1am)
                    row("LAY2") = CStr(field(i).res_lay2) + Environment.NewLine + "$" + CStr(field(i).res_lay2am)
                    row("BAKK1") = CStr(field(i).HT_back1) + Environment.NewLine + "$" + CStr(field(i).HT_back1am)
                    row("BAKK2") = CStr(field(i).HT_back2) + Environment.NewLine + "$" + CStr(field(i).HT_back2am)
                    row("LAYY1") = CStr(field(i).HT_lay1) + Environment.NewLine + "$" + CStr(field(i).HT_lay1am)
                    row("LAYY2") = CStr(field(i).HT_lay2) + Environment.NewLine + "$" + CStr(field(i).HT_lay2am)
                    row("Back-Lay") = System.Math.Round(field(i).back_lay, 2)
                    If field(i).back_lay > 0 Then
                        gvData.Columns(10).CellTemplate.Style.BackColor = Color.SpringGreen
                    Else
                        gvData.Columns(10).CellTemplate.Style.BackColor = Color.Empty
                    End If
                    row("Lay-Back") = System.Math.Round(field(i).lay_back, 2)
                    If field(i).lay_back > 0 Then
                        gvData.Columns(11).CellTemplate.Style.BackColor = Color.SpringGreen
                    Else
                        gvData.Columns(11).CellTemplate.Style.BackColor = Color.Empty
                    End If
                    For ii = 0 To 11
                        gvData.Columns(ii).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        gvData.Columns(ii).SortMode = DataGridViewColumnSortMode.NotSortable
                        gvData.Columns(ii).ReadOnly = True
                    Next
                    gvData.Columns(0).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(1).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(2).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(3).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(4).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(5).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(6).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(7).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(8).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(9).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(10).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    gvData.Columns(11).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Tabla.Rows.Add(row)
                    gvData.Refresh()
                Next
            End If
        End With
        operat30.IsBackground = True
        operat30.Start()
        operat60.IsBackground = True
        operat60.Start()
        operat180.IsBackground = True
        operat180.Start()
        operat300.IsBackground = True
        operat300.Start()
    End Sub

    Sub operation30()
        Dim logresfile, result, ss As String
        Dim z1, z2, bk, ly, rez, sm As Decimal
        Dim i3 As Integer
        Do While True
             i3 = 0
            For i = 0 To UBound(field)
                If (field(i).sel = 1) Then
                    i3 = i3 + 1
                    logresfile = "C:\football_Betfair\sigrano\" & field(i).menupath & ".result"
                    If My.Computer.FileSystem.FileExists(logresfile) Then
                        result = ""
                    Else
                        field(i).start_b1 = field(i).res_back1
                        field(i).start_l1 = field(i).res_lay1
                        field(i).start_b2 = field(i).HT_back1
                        field(i).start_l2 = field(i).HT_lay1
                        z1 = 10.53 * (field(i).HT_lay1 - 1) + 10
                        field(i).risk_bl = z1
                        z1 = 10.53 * (field(i).res_lay1 - 1) + 10
                        field(i).risk_lb = z1
                        z1 = System.Math.Round(100 * (10 * field(i).res_back1 / field(i).res_lay1 - 10 + 10.53 - 10.53 * field(i).HT_lay1 / field(i).HT_back1) / field(i).risk_bl, 2)
                        z2 = System.Math.Round(100 * (10 * field(i).HT_back1 / field(i).HT_lay1 - 10 + 10.53 - 10.53 * field(i).res_lay1 / field(i).res_back1) / field(i).risk_lb, 2)
                        result = "сейчас=" & Now & " начало=" & field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString & " бэк1=" & field(i).res_back1 & " лэй1=" & field(i).res_lay1 & " бэк2=" & field(i).HT_back1 & " лэй2=" & field(i).HT_lay1 & " бэк-лэй=" & System.Math.Round(field(i).back_lay, 2) & " лэй-бэк=" & System.Math.Round(field(i).lay_back, 2) & " риск бл=" & System.Math.Round(field(i).risk_bl, 2) & " риск лб=" & System.Math.Round(field(i).risk_lb, 2) & " закрыть бл=" & z1 & " закрыть лб=" & z2 & " инплэй=" & field(i).inp
                        My.Computer.FileSystem.WriteAllText(logresfile, result & vbCrLf, True)
                    End If
                    zapolnenie_field(i)
                    If i3 = 59 Or i3 = 119 Or i3 = 179 Or i3 = 239 Or i3 = 299 Or i3 = 359 Then
                        System.Threading.Thread.Sleep(60000)
                    End If
                    If field(i).start_b1 < 1 Then field(i).start_b1 = field(i).res_back1
                    If field(i).start_b2 < 1 Then field(i).start_b2 = field(i).HT_back1
                    If field(i).start_l1 < 1 Then field(i).start_l1 = field(i).res_lay1
                    If field(i).start_l2 < 1 Then field(i).start_l2 = field(i).HT_lay2
                    z1 = System.Math.Round(100 * (10 * field(i).start_b1 / field(i).res_lay1 - 10 + 10.53 - 10.53 * field(i).start_l2 / field(i).HT_back1) / field(i).risk_bl, 2)
                    z2 = System.Math.Round(100 * (10 * field(i).start_b2 / field(i).HT_lay1 - 10 + 10.53 - 10.53 * field(i).start_l1 / field(i).res_back1) / field(i).risk_lb, 2)
                    result = "сейчас=" & Now & " начало=" & field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString & " бэк1=" & field(i).res_back1 & " лэй1=" & field(i).res_lay1 & " бэк2=" & field(i).HT_back1 & " лэй2=" & field(i).HT_lay1 & " бэк-лэй=" & System.Math.Round(field(i).back_lay, 2) & " лэй-бэк=" & System.Math.Round(field(i).lay_back, 2) & " риск бл=" & System.Math.Round(field(i).risk_bl, 2) & " риск лб=" & System.Math.Round(field(i).risk_lb, 2) & " закрыть бл=" & z1 & " закрыть лб=" & z2 & " инплэй=" & field(i).inp
                    My.Computer.FileSystem.WriteAllText(logresfile, result & vbCrLf, True)
                End If
            Next
            TextBox5.Text = i3
            For i = 0 To UBound(field)
                If (field(i).sel = 1) Then
                    If (field(i).back_lay > 5) Or (field(i).back_lay > Math.Abs(field(i).lay_back)) Or (field(i).lay_back > -2) Then
                        ShowWindow(field(i).hndl, SW_RESTORE)
                    End If
                    ss = field(i).menupath
                    For ii = 0 To Application.OpenForms.Count - 1
                        If (Application.OpenForms(ii).Name = ss) Then
                            For iii = 0 To Application.OpenForms(ii).Controls.Count - 1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt1" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString
                                    If field(i).inp > 0 Then Application.OpenForms(ii).Controls(iii).BackColor = Color.Chocolate
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt2" Then Application.OpenForms(ii).Controls(iii).Text = field(i).res_back1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt3" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).res_lay1
                                    z1 = field(i).res_lay1
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt4" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).HT_back1
                                    z2 = field(i).HT_back1
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt5" Then Application.OpenForms(ii).Controls(iii).Text = field(i).HT_lay1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt6" Then
                                    Application.OpenForms(ii).Controls(iii).Text = System.Math.Round(field(i).back_lay, 2)
                                    If Application.OpenForms(ii).Controls(iii).Text > 0 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt7" Then
                                    Application.OpenForms(ii).Controls(iii).Text = System.Math.Round(field(i).lay_back, 2)
                                    If Application.OpenForms(ii).Controls(iii).Text > -2 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt8" Then
                                    bk = Application.OpenForms(ii).Controls(iii).Text
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt9" Then
                                    ly = Application.OpenForms(ii).Controls(iii).Text
                                End If
                                If bk > 0 And ly > 0 Then
                                    sm = 10.53 * (ly - 1) + 10
                                    rez = System.Math.Round(100 * (10 * bk / z1 - 10 + 10.53 - 10.53 * ly / z2) / sm, 2)
                                Else
                                    rez = 0
                                End If
                            Next
                            For iii = 0 To Application.OpenForms(ii).Controls.Count - 1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt10" Then
                                    Application.OpenForms(ii).Controls(iii).Text = rez
                                    If rez > 0 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next
            Thread.Sleep(31000)
        Loop
    End Sub

    Sub operation60()
        Dim logresfile, result, ss As String
        Dim z1, z2, bk, ly, rez, sm As Decimal
        Dim i3 As Integer
        Do While True
            i3 = 0
            For i = 0 To UBound(field)
                If (field(i).sel = 11) Then
                    i3 = i3 + 1
                    logresfile = "C:\football_Betfair\sigrano\" & field(i).menupath & ".result"
                    If My.Computer.FileSystem.FileExists(logresfile) Then
                        result = ""
                    Else
                        field(i).start_b1 = field(i).res_back1
                        field(i).start_l1 = field(i).res_lay1
                        field(i).start_b2 = field(i).HT_back1
                        field(i).start_l2 = field(i).HT_lay1
                        z1 = 10.53 * (field(i).HT_lay1 - 1) + 10
                        field(i).risk_bl = z1
                        z1 = 10.53 * (field(i).res_lay1 - 1) + 10
                        field(i).risk_lb = z1
                        z1 = System.Math.Round(100 * (10 * field(i).res_back1 / field(i).res_lay1 - 10 + 10.53 - 10.53 * field(i).HT_lay1 / field(i).HT_back1) / field(i).risk_bl, 2)
                        z2 = System.Math.Round(100 * (10 * field(i).HT_back1 / field(i).HT_lay1 - 10 + 10.53 - 10.53 * field(i).res_lay1 / field(i).res_back1) / field(i).risk_lb, 2)
                        result = "сейчас=" & Now & " начало=" & field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString & " бэк1=" & field(i).res_back1 & " лэй1=" & field(i).res_lay1 & " бэк2=" & field(i).HT_back1 & " лэй2=" & field(i).HT_lay1 & " бэк-лэй=" & System.Math.Round(field(i).back_lay, 2) & " лэй-бэк=" & System.Math.Round(field(i).lay_back, 2) & " риск бл=" & System.Math.Round(field(i).risk_bl, 2) & " риск лб=" & System.Math.Round(field(i).risk_lb, 2) & " закрыть бл=" & z1 & " закрыть лб=" & z2 & " инплэй=" & field(i).inp
                        My.Computer.FileSystem.WriteAllText(logresfile, result & vbCrLf, True)
                    End If
                    zapolnenie_field(i)
                    If i3 = 59 Or i3 = 119 Or i3 = 179 Or i3 = 239 Or i3 = 299 Or i3 = 359 Then
                        System.Threading.Thread.Sleep(60000)
                    End If
                    If field(i).start_b1 < 1 Then field(i).start_b1 = field(i).res_back1
                    If field(i).start_b2 < 1 Then field(i).start_b2 = field(i).HT_back1
                    If field(i).start_l1 < 1 Then field(i).start_l1 = field(i).res_lay1
                    If field(i).start_l2 < 1 Then field(i).start_l2 = field(i).HT_lay2
                    z1 = System.Math.Round(100 * (10 * field(i).start_b1 / field(i).res_lay1 - 10 + 10.53 - 10.53 * field(i).start_l2 / field(i).HT_back1) / field(i).risk_bl, 2)
                    z2 = System.Math.Round(100 * (10 * field(i).start_b2 / field(i).HT_lay1 - 10 + 10.53 - 10.53 * field(i).start_l1 / field(i).res_back1) / field(i).risk_lb, 2)
                    result = "сейчас=" & Now & " начало=" & field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString & " бэк1=" & field(i).res_back1 & " лэй1=" & field(i).res_lay1 & " бэк2=" & field(i).HT_back1 & " лэй2=" & field(i).HT_lay1 & " бэк-лэй=" & System.Math.Round(field(i).back_lay, 2) & " лэй-бэк=" & System.Math.Round(field(i).lay_back, 2) & " риск бл=" & System.Math.Round(field(i).risk_bl, 2) & " риск лб=" & System.Math.Round(field(i).risk_lb, 2) & " закрыть бл=" & z1 & " закрыть лб=" & z2 & " инплэй=" & field(i).inp
                    My.Computer.FileSystem.WriteAllText(logresfile, result & vbCrLf, True)
                End If
            Next
            TextBox4.Text = i3
            For i = 0 To UBound(field)
                If (field(i).sel = 11) Then
                    If (field(i).back_lay > 5) Or (field(i).back_lay > Math.Abs(field(i).lay_back)) Or (field(i).lay_back > -2) Then
                        '    ShowWindow(field(i).hndl, SW_RESTORE)
                    End If
                    ss = field(i).menupath
                    For ii = 0 To Application.OpenForms.Count - 1
                        If (Application.OpenForms(ii).Name = ss) Then
                            For iii = 0 To Application.OpenForms(ii).Controls.Count - 1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt1" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString
                                    If field(i).inp > 0 Then Application.OpenForms(ii).Controls(iii).BackColor = Color.Chocolate
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt2" Then Application.OpenForms(ii).Controls(iii).Text = field(i).res_back1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt3" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).res_lay1
                                    z1 = field(i).res_lay1
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt4" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).HT_back1
                                    z2 = field(i).HT_back1
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt5" Then Application.OpenForms(ii).Controls(iii).Text = field(i).HT_lay1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt6" Then
                                    Application.OpenForms(ii).Controls(iii).Text = System.Math.Round(field(i).back_lay, 2)
                                    If Application.OpenForms(ii).Controls(iii).Text > 0 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt7" Then
                                    Application.OpenForms(ii).Controls(iii).Text = System.Math.Round(field(i).lay_back, 2)
                                    If Application.OpenForms(ii).Controls(iii).Text > -2 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt8" Then
                                    bk = Application.OpenForms(ii).Controls(iii).Text
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt9" Then
                                    ly = Application.OpenForms(ii).Controls(iii).Text
                                End If
                                If bk > 0 And ly > 0 Then
                                    sm = 10.53 * (ly - 1) + 10
                                    rez = System.Math.Round(100 * (10 * bk / z1 - 10 + 10.53 - 10.53 * ly / z2) / sm, 2)
                                Else
                                    rez = 0
                                End If
                            Next
                            For iii = 0 To Application.OpenForms(ii).Controls.Count - 1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt10" Then
                                    Application.OpenForms(ii).Controls(iii).Text = rez
                                    If rez > 0 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next
            Thread.Sleep(67000)
        Loop
    End Sub

    Sub operation180()
        Dim logresfile, result, ss As String
        Dim z1, z2, bk, ly, rez, sm As Decimal
        Dim i3 As Integer
        Do While True
            i3 = 0
            For i = 0 To UBound(field)
                If (field(i).sel = 111) Then
                    i3 = i3 + 1
                    logresfile = "C:\football_Betfair\sigrano\" & field(i).menupath & ".result"
                    If My.Computer.FileSystem.FileExists(logresfile) Then
                        result = ""
                    Else
                        field(i).start_b1 = field(i).res_back1
                        field(i).start_l1 = field(i).res_lay1
                        field(i).start_b2 = field(i).HT_back1
                        field(i).start_l2 = field(i).HT_lay1
                        z1 = 10.53 * (field(i).HT_lay1 - 1) + 10
                        field(i).risk_bl = z1
                        z1 = 10.53 * (field(i).res_lay1 - 1) + 10
                        field(i).risk_lb = z1
                        z1 = System.Math.Round(100 * (10 * field(i).res_back1 / field(i).res_lay1 - 10 + 10.53 - 10.53 * field(i).HT_lay1 / field(i).HT_back1) / field(i).risk_bl, 2)
                        z2 = System.Math.Round(100 * (10 * field(i).HT_back1 / field(i).HT_lay1 - 10 + 10.53 - 10.53 * field(i).res_lay1 / field(i).res_back1) / field(i).risk_lb, 2)
                        result = "сейчас=" & Now & " начало=" & field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString & " бэк1=" & field(i).res_back1 & " лэй1=" & field(i).res_lay1 & " бэк2=" & field(i).HT_back1 & " лэй2=" & field(i).HT_lay1 & " бэк-лэй=" & System.Math.Round(field(i).back_lay, 2) & " лэй-бэк=" & System.Math.Round(field(i).lay_back, 2) & " риск бл=" & System.Math.Round(field(i).risk_bl, 2) & " риск лб=" & System.Math.Round(field(i).risk_lb, 2) & " закрыть бл=" & z1 & " закрыть лб=" & z2 & " инплэй=" & field(i).inp
                        My.Computer.FileSystem.WriteAllText(logresfile, result & vbCrLf, True)
                    End If
                    zapolnenie_field(i)
                    If i3 = 59 Or i3 = 119 Or i3 = 179 Or i3 = 239 Or i3 = 299 Or i3 = 359 Then
                        System.Threading.Thread.Sleep(60000)
                    End If
                    If field(i).start_b1 < 1 Then field(i).start_b1 = field(i).res_back1
                    If field(i).start_b2 < 1 Then field(i).start_b2 = field(i).HT_back1
                    If field(i).start_l1 < 1 Then field(i).start_l1 = field(i).res_lay1
                    If field(i).start_l2 < 1 Then field(i).start_l2 = field(i).HT_lay2
                    z1 = System.Math.Round(100 * (10 * field(i).start_b1 / field(i).res_lay1 - 10 + 10.53 - 10.53 * field(i).start_l2 / field(i).HT_back1) / field(i).risk_bl, 2)
                    z2 = System.Math.Round(100 * (10 * field(i).start_b2 / field(i).HT_lay1 - 10 + 10.53 - 10.53 * field(i).start_l1 / field(i).res_back1) / field(i).risk_lb, 2)
                    result = "сейчас=" & Now & " начало=" & field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString & " бэк1=" & field(i).res_back1 & " лэй1=" & field(i).res_lay1 & " бэк2=" & field(i).HT_back1 & " лэй2=" & field(i).HT_lay1 & " бэк-лэй=" & System.Math.Round(field(i).back_lay, 2) & " лэй-бэк=" & System.Math.Round(field(i).lay_back, 2) & " риск бл=" & System.Math.Round(field(i).risk_bl, 2) & " риск лб=" & System.Math.Round(field(i).risk_lb, 2) & " закрыть бл=" & z1 & " закрыть лб=" & z2 & " инплэй=" & field(i).inp
                    My.Computer.FileSystem.WriteAllText(logresfile, result & vbCrLf, True)
                End If
            Next
            TextBox3.Text = i3
            For i = 0 To UBound(field)
                If (field(i).sel = 111) Then
                    If (field(i).back_lay > 5) Or (field(i).back_lay > Math.Abs(field(i).lay_back)) Or (field(i).lay_back > -2) Then
                        '    ShowWindow(field(i).hndl, SW_RESTORE)
                    End If
                    ss = field(i).menupath
                    For ii = 0 To Application.OpenForms.Count - 1
                        If (Application.OpenForms(ii).Name = ss) Then
                            For iii = 0 To Application.OpenForms(ii).Controls.Count - 1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt1" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString
                                    If field(i).inp > 0 Then Application.OpenForms(ii).Controls(iii).BackColor = Color.Chocolate
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt2" Then Application.OpenForms(ii).Controls(iii).Text = field(i).res_back1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt3" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).res_lay1
                                    z1 = field(i).res_lay1
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt4" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).HT_back1
                                    z2 = field(i).HT_back1
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt5" Then Application.OpenForms(ii).Controls(iii).Text = field(i).HT_lay1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt6" Then
                                    Application.OpenForms(ii).Controls(iii).Text = System.Math.Round(field(i).back_lay, 2)
                                    If Application.OpenForms(ii).Controls(iii).Text > 0 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt7" Then
                                    Application.OpenForms(ii).Controls(iii).Text = System.Math.Round(field(i).lay_back, 2)
                                    If Application.OpenForms(ii).Controls(iii).Text > -2 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt8" Then
                                    bk = Application.OpenForms(ii).Controls(iii).Text
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt9" Then
                                    ly = Application.OpenForms(ii).Controls(iii).Text
                                End If
                                If bk > 0 And ly > 0 Then
                                    sm = 10.53 * (ly - 1) + 10
                                    rez = System.Math.Round(100 * (10 * bk / z1 - 10 + 10.53 - 10.53 * ly / z2) / sm, 2)
                                Else
                                    rez = 0
                                End If
                            Next
                            For iii = 0 To Application.OpenForms(ii).Controls.Count - 1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt10" Then
                                    Application.OpenForms(ii).Controls(iii).Text = rez
                                    If rez > 0 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next
            Thread.Sleep(187000)
        Loop
    End Sub

    Sub operation300()
        Dim logresfile, result, ss As String
        Dim z1, z2, bk, ly, rez, sm As Decimal
        Dim i3 As Integer
        Do While True
            i3 = 0
            For i = 0 To UBound(field)
                If (field(i).sel = 1111) Then
                    i3 = i3 + 1
                    logresfile = "C:\football_Betfair\sigrano\" & field(i).menupath & ".result"
                    If My.Computer.FileSystem.FileExists(logresfile) Then
                        result = ""
                    Else
                        field(i).start_b1 = field(i).res_back1
                        field(i).start_l1 = field(i).res_lay1
                        field(i).start_b2 = field(i).HT_back1
                        field(i).start_l2 = field(i).HT_lay1
                        z1 = 10.53 * (field(i).HT_lay1 - 1) + 10
                        field(i).risk_bl = z1
                        z1 = 10.53 * (field(i).res_lay1 - 1) + 10
                        field(i).risk_lb = z1
                        z1 = System.Math.Round(100 * (10 * field(i).res_back1 / field(i).res_lay1 - 10 + 10.53 - 10.53 * field(i).HT_lay1 / field(i).HT_back1) / field(i).risk_bl, 2)
                        z2 = System.Math.Round(100 * (10 * field(i).HT_back1 / field(i).HT_lay1 - 10 + 10.53 - 10.53 * field(i).res_lay1 / field(i).res_back1) / field(i).risk_lb, 2)
                        result = "сейчас=" & Now & " начало=" & field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString & " бэк1=" & field(i).res_back1 & " лэй1=" & field(i).res_lay1 & " бэк2=" & field(i).HT_back1 & " лэй2=" & field(i).HT_lay1 & " бэк-лэй=" & System.Math.Round(field(i).back_lay, 2) & " лэй-бэк=" & System.Math.Round(field(i).lay_back, 2) & " риск бл=" & System.Math.Round(field(i).risk_bl, 2) & " риск лб=" & System.Math.Round(field(i).risk_lb, 2) & " закрыть бл=" & z1 & " закрыть лб=" & z2 & " инплэй=" & field(i).inp
                        My.Computer.FileSystem.WriteAllText(logresfile, result & vbCrLf, True)
                    End If
                    zapolnenie_field(i)
                    If i3 = 59 Or i3 = 119 Or i3 = 179 Or i3 = 239 Or i3 = 299 Or i3 = 359 Then
                        System.Threading.Thread.Sleep(60000)
                    End If
                    If field(i).start_b1 < 1 Then field(i).start_b1 = field(i).res_back1
                    If field(i).start_b2 < 1 Then field(i).start_b2 = field(i).HT_back1
                    If field(i).start_l1 < 1 Then field(i).start_l1 = field(i).res_lay1
                    If field(i).start_l2 < 1 Then field(i).start_l2 = field(i).HT_lay2
                    z1 = System.Math.Round(100 * (10 * field(i).start_b1 / field(i).res_lay1 - 10 + 10.53 - 10.53 * field(i).start_l2 / field(i).HT_back1) / field(i).risk_bl, 2)
                    z2 = System.Math.Round(100 * (10 * field(i).start_b2 / field(i).HT_lay1 - 10 + 10.53 - 10.53 * field(i).start_l1 / field(i).res_back1) / field(i).risk_lb, 2)
                    result = "сейчас=" & Now & " начало=" & field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString & " бэк1=" & field(i).res_back1 & " лэй1=" & field(i).res_lay1 & " бэк2=" & field(i).HT_back1 & " лэй2=" & field(i).HT_lay1 & " бэк-лэй=" & System.Math.Round(field(i).back_lay, 2) & " лэй-бэк=" & System.Math.Round(field(i).lay_back, 2) & " риск бл=" & System.Math.Round(field(i).risk_bl, 2) & " риск лб=" & System.Math.Round(field(i).risk_lb, 2) & " закрыть бл=" & z1 & " закрыть лб=" & z2 & " инплэй=" & field(i).inp
                    My.Computer.FileSystem.WriteAllText(logresfile, result & vbCrLf, True)
                End If
            Next
            TextBox2.Text = i3
            For i = 0 To UBound(field)
                If (field(i).sel = 1111) Then
                    If (field(i).back_lay > 5) Or (field(i).back_lay > Math.Abs(field(i).lay_back)) Or (field(i).lay_back > -2) Then
                        '     ShowWindow(field(i).hndl, SW_RESTORE)
                    End If
                    ss = field(i).menupath
                    For ii = 0 To Application.OpenForms.Count - 1
                        If (Application.OpenForms(ii).Name = ss) Then
                            For iii = 0 To Application.OpenForms(ii).Controls.Count - 1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt1" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).eventDate.ToLocalTime.AddHours(1).TimeOfDay.ToString
                                    If field(i).inp > 0 Then Application.OpenForms(ii).Controls(iii).BackColor = Color.Chocolate
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt2" Then Application.OpenForms(ii).Controls(iii).Text = field(i).res_back1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt3" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).res_lay1
                                    z1 = field(i).res_lay1
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt4" Then
                                    Application.OpenForms(ii).Controls(iii).Text = field(i).HT_back1
                                    z2 = field(i).HT_back1
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt5" Then Application.OpenForms(ii).Controls(iii).Text = field(i).HT_lay1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt6" Then
                                    Application.OpenForms(ii).Controls(iii).Text = System.Math.Round(field(i).back_lay, 2)
                                    If Application.OpenForms(ii).Controls(iii).Text > 0 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt7" Then
                                    Application.OpenForms(ii).Controls(iii).Text = System.Math.Round(field(i).lay_back, 2)
                                    If Application.OpenForms(ii).Controls(iii).Text > -2 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt8" Then
                                    bk = Application.OpenForms(ii).Controls(iii).Text
                                End If
                                If Application.OpenForms(ii).Controls(iii).Name = "txt9" Then
                                    ly = Application.OpenForms(ii).Controls(iii).Text
                                End If
                                If bk > 0 And ly > 0 Then
                                    sm = 10.53 * (ly - 1) + 10
                                    rez = System.Math.Round(100 * (10 * bk / z1 - 10 + 10.53 - 10.53 * ly / z2) / sm, 2)
                                Else
                                    rez = 0
                                End If
                            Next
                            For iii = 0 To Application.OpenForms(ii).Controls.Count - 1
                                If Application.OpenForms(ii).Controls(iii).Name = "txt10" Then
                                    Application.OpenForms(ii).Controls(iii).Text = rez
                                    If rez > 0 Then
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.SpringGreen
                                    Else
                                        Application.OpenForms(ii).Controls(iii).BackColor = Color.Empty
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next
            Thread.Sleep(307000)
        Loop
    End Sub

End Class
