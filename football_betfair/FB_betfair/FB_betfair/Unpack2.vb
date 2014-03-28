Module Unpack2
    Class RunnerInfoType
        Public selectionId As Integer
        Public sortOrder As Integer
        Public totalAmountMatched As Double
        Public lastPriceMatched As Double
        Public handicap As Double
        Public reductionFactor As Double
        Public vacant As Boolean
        Public asianLineId As Integer
        Public farBSP As Double
        Public nearBSP As Double
        Public actualBSP As Double
        Public prices As PricesType()
    End Class

    Class PricesType
        Public price As Double
        Public backAmount As Double
        Public layAmount As Double
        Public totalBspBackAmount As Double
        Public totalBspLayAmount As Double
    End Class
    Class UnpackCompleteMarketPricesCompressed
        Public marketId As Integer
        Public delay As Integer
        Public removedRunners As String
        Public runnerInfo As RunnerInfoType() = {}
        Private Const ColonCode = "&%^@"     'The substitute code for "\:"

        Sub New(ByVal MarketPrices As String)       'Unpack the data string 
            Dim Mprices, Field, Part As String(), k, n, m As Integer

            Mprices = MarketPrices.Replace("\:", ColonCode).Split(":")      'Split the runner data fields
            Field = Mprices(0).Replace("\~", "-").Split("~")  'Split the market data fields
            marketId = Field(0)  'Assign the market data
            delay = Field(1)
            removedRunners = Field(2).Replace(ColonCode, ":")

            n = UBound(Mprices) - 1
            ReDim runnerInfo(n)
            For i = 0 To n      'For each runner
                Part = Mprices(i + 1).Split("|")     'Split runner string into 2 parts
                Field = Part(0).Split("~")        'Split runner info fields
                runnerInfo(i) = New RunnerInfoType
                With runnerInfo(i)        'Assign the runner parameters
                    .selectionId = Field(0)
                    .sortOrder = Field(1)
                    .totalAmountMatched = Val(Field(2))
                    .lastPriceMatched = Val(Field(3))
                    .handicap = Val(Field(4))
                    .reductionFactor = Val(Field(5))
                    .vacant = (Field(6).ToLower = "true")
                    .asianLineId = Field(7)
                    .farBSP = Val(Field(8))
                    .nearBSP = Val(Field(9))
                    .actualBSP = Val(Field(10))

                    Field = Part(1).Split("~")   'Split price info
                    m = UBound(Field) \ 5 - 1   'm = number of prices - 1 
                    ReDim .prices(m) : k = 0
                    For j = 0 To m
                        .prices(j) = New PricesType
                        With .prices(j)      'Load the price array
                            .price = Val(Field(k + 0))
                            .backAmount = Val(Field(k + 1))
                            .layAmount = Val(Field(k + 2))
                            .totalBspBackAmount = Val(Field(k + 3))
                            .totalBspLayAmount = Val(Field(k + 4))
                            k += 5
                        End With
                    Next
                End With
            Next
        End Sub
    End Class
    Class CompareRunnerInfo  'A class to provide the comparison method for the RunnerInfo array sort
        Implements IComparer(Of RunnerInfoType)
        Public Function Compare(ByVal x As RunnerInfoType, ByVal y As RunnerInfoType) As Integer Implements System.Collections.Generic.IComparer(Of RunnerInfoType).Compare
            Return x.sortOrder - y.sortOrder   'To sort according to OrderIndex
        End Function
    End Class
    Class UnpackMarketPricesCompressed
        Inherits BFUK.MarketPrices
        Private Const ColonCode = "&%^@"  'The substitute code for "\:"

        Sub New(ByVal MarketPrices As String)      'Unpack the string
            Dim Mprices, Part, Field As String(), n As Integer

            Mprices = MarketPrices.Replace("\:", ColonCode).Split(":")    'Split header and runner data
            Field = Mprices(0).Replace("\~", "-").Split("~")     'Split market data fields
            marketId = Field(0)     'Assign the market data
            currencyCode = Field(1)
            marketStatus = [Enum].Parse(GetType(BFUK.MarketStatusEnum), Field(2), True)
            delay = Field(3)
            numberOfWinners = Field(4)
            marketInfo = Field(5).Replace(ColonCode, ":")
            discountAllowed = (Field(6).ToLower = "true")
            marketBaseRate = Val(Field(7))
            lastRefresh = Field(8)
            removedRunners = Field(9).Replace(ColonCode, ":")
            bspMarket = (Field(10) = "Y")

            n = UBound(Mprices) - 1
            ReDim runnerPrices(n)
            For i = 0 To n    'For each runner
                Part = Mprices(i + 1).Split("|")    'Split runner string into 3 parts
                Field = Part(0).Split("~")         'Split runner data fields
                runnerPrices(i) = New BFUK.RunnerPrices
                With runnerPrices(i)     'Assign the runner data
                    .selectionId = Field(0)
                    .sortOrder = Field(1)
                    .totalAmountMatched = Val(Field(2))
                    .lastPriceMatched = Val(Field(3))
                    .handicap = Val(Field(4))
                    .reductionFactor = Val(Field(5))
                    .vacant = (Field(6).ToLower = "true")
                    .farBSP = Val(Field(7))
                    .nearBSP = Val(Field(8))
                    .actualBSP = Val(Field(9))
                    .bestPricesToBack = Prices(Part(1))
                    .bestPricesToLay = Prices(Part(2))
                End With
            Next
        End Sub

        Private Function Prices(ByVal PriceString As String) As BFUK.Price()
            Dim Field As String(), Price As BFUK.Price(), k, m As Integer

            Field = PriceString.Split("~")  'Split price fields
            m = UBound(Field) \ 4 - 1  'm = number of prices - 1 
            ReDim Price(m)
            For i = 0 To m
                Price(i) = New BFUK.Price
                With Price(i)
                    .price = Val(Field(k + 0))  'Assign price data
                    .amountAvailable = Val(Field(k + 1))
                    .betType = [Enum].Parse(GetType(BFUK.BetTypeEnum), Field(k + 2), True)
                    .depth = Field(k + 3)
                    k += 4
                End With
            Next
            Return Price  'Return the array of prices
        End Function

    End Class
End Module
