Module Unpack
    Public Class Giraffe
        Public id As Integer
        Public eventDate As DateTime
        Public menupath As String
        Public marketid_res As Integer
        Public marketid_HT As Integer
        Public res_back1 As Double
        Public res_back2 As Double
        Public res_back1am As Double
        Public res_back2am As Double
        Public res_lay1 As Double
        Public res_lay2 As Double
        Public res_lay1am As Double
        Public res_lay2am As Double
        Public rass As Decimal
        Public HT_back1 As Double
        Public HT_back2 As Double
        Public HT_back1am As Double
        Public HT_back2am As Double
        Public HT_lay1 As Double
        Public HT_lay2 As Double
        Public HT_lay1am As Double
        Public HT_lay2am As Double
        Public back_lay As Decimal
        Public lay_back As Decimal
        Public inp As Integer
        Public sel As Integer
        Public risk_bl As Decimal
        Public risk_lb As Decimal
        Public start_b1 As Decimal
        Public start_l1 As Decimal
        Public start_b2 As Decimal
        Public start_l2 As Decimal
        Public hndl As IntPtr
    End Class
    Class MarketDataType           'For getAllMarkets data
        Public marketId As Integer
        Public marketName As String
        Public marketType As String
        Public marketStatus As String
        Public eventDate As DateTime
        Public menuPath As String
        Public eventHeirachy As String
        Public betDelay As Integer
        Public exchangeId As Integer
        Public countryCode As String
        Public lastRefresh As DateTime
        Public noOfRunners As Integer
        Public noOfWinners As Integer
        Public totalAmountMatched As Double
        Public bspMarket As Boolean
        Public turningInPlay As Boolean
    End Class
    Class UnpackAllMarkets       'For getAllMArkets
        Public marketData As MarketDataType() = {}  'The returned array of market data
        Private Const BaseDate As DateTime = #1/1/1970#
        Private Const ColonCode = "&%^@"  'The substitute code for "\:"

        Sub New(ByVal MarketString As String)
            Dim n As Integer, Mdata, Field As String()

            Mdata = MarketString.Replace("\:", ColonCode).Split(":") 'Get array of Market substrings
            n = UBound(Mdata) - 1
            ReDim marketData(n)

            For i = 0 To n
                Field = Mdata(i + 1).Replace("\~", "-").Split("~") 'Get array of data fields
                marketData(i) = New MarketDataType
                With marketData(i)
                    .marketId = Field(0)   'Load the array items
                    .marketName = Field(1).Replace(ColonCode, ":")
                    .marketType = Field(2)
                    .marketStatus = Field(3)
                    .eventDate = BaseDate.AddMilliseconds(Field(4))
                    .menuPath = Field(5).Replace(ColonCode, ":")
                    .eventHeirachy = Field(6)
                    .betDelay = Field(7)
                    .exchangeId = Field(8)
                    .countryCode = Field(9)
                    .lastRefresh = BaseDate.AddMilliseconds(Field(10))
                    .noOfRunners = Field(11)
                    .noOfWinners = Field(12)
                    .totalAmountMatched = Val(Field(13))
                    .bspMarket = (Field(14) = "Y")
                    .turningInPlay = (Field(15) = "Y")

                End With
            Next
        End Sub
    End Class
    
    Class CompareMarketTimes
        Implements IComparer(Of MarketDataType)
        Public Function Compare(ByVal x As MarketDataType, ByVal y As MarketDataType) As Integer Implements System.Collections.Generic.IComparer(Of MarketDataType).Compare
             Return If(x.eventDate >= y.eventDate, 1, -1)
        End Function
    End Class
End Module
