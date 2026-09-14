namespace EnterpriseRisk

open System

type AssetPosition = {
    Ticker: string
    Allocation: double
    ExpectedAnnualReturn: double
    Volatility: double
}

type SimulationResult = {
    MeanPortfolioReturn: double
    ValueAtRisk95: double
    ValueAtRisk99: double
    ExpectedShortfall99: double
    TotalSimulations: int
}

module RiskEngine =

    let runMonteCarloSimulation (positions: AssetPosition list) (horizonDays: int) (iterations: int) : SimulationResult =
        let dt = float horizonDays / 252.0
        let rng = Random(42)

        let simulateSinglePortfolioRun () =
            positions
            |> List.sumBy (fun pos ->
                let u1 = rng.NextDouble()
                let u2 = rng.NextDouble()
                let z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2)
                let drift = (pos.ExpectedAnnualReturn - 0.5 * pos.Volatility * pos.Volatility) * dt
                let diffusion = pos.Volatility * Math.Sqrt(dt) * z
                let priceReturn = Math.Exp(drift + diffusion) - 1.0
                pos.Allocation * priceReturn
            )

        let outcomes = 
            Array.init iterations (fun _ -> simulateSinglePortfolioRun())
            |> Array.sort

        let meanReturn = Array.average outcomes
        let idx95 = int (float iterations * 0.05)
        let idx99 = int (float iterations * 0.01)

        let var95 = -outcomes.[idx95]
        let var99 = -outcomes.[idx99]

        let cvar99 = 
            let tail = outcomes.[0 .. idx99]
            -(Array.average tail)

        {
            MeanPortfolioReturn = meanReturn
            ValueAtRisk95 = var95
            ValueAtRisk99 = var99
            ExpectedShortfall99 = cvar99
            TotalSimulations = iterations
        }
