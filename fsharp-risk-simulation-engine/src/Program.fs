namespace EnterpriseRisk

open System

module Program =

    [<EntryPoint>]
    let main argv =
        printfn "===================================================================="
        printfn "  Enterprise Portfolio Risk Simulation & Value-at-Risk Engine (F#)"
        printfn "====================================================================\n"

        let portfolio = [
            { Ticker = "US_EQUITY_SP500"; Allocation = 0.50; ExpectedAnnualReturn = 0.085; Volatility = 0.16 }
            { Ticker = "GLOBAL_TECH_SEMIS"; Allocation = 0.25; ExpectedAnnualReturn = 0.140; Volatility = 0.28 }
            { Ticker = "US_TREASURY_10Y";  Allocation = 0.25; ExpectedAnnualReturn = 0.042; Volatility = 0.06 }
        ]

        printfn "[1/2] Simulating 100,000 Monte Carlo Portfolio Paths (10-day Horizon)..."
        let result = RiskEngine.runMonteCarloSimulation portfolio 10 100000

        printfn "[2/2] Quantitative Risk Metrics:"
        printfn "      Expected Mean Return   : %.4f%%" (result.MeanPortfolioReturn * 100.0)
        printfn "      95%% Value-at-Risk (VaR) : %.4f%%" (result.ValueAtRisk95 * 100.0)
        printfn "      99%% Value-at-Risk (VaR) : %.4f%%" (result.ValueAtRisk99 * 100.0)
        printfn "      99%% Expected Shortfall (CVaR): %.4f%%\n" (result.ExpectedShortfall99 * 100.0)

        printfn "[SUCCESS] F# Monte Carlo risk simulation completed successfully."
        0
