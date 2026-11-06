global using Xunit;

// Every GameEngine shares one BestGame.json, so tests running in parallel can read it mid-write
[assembly: Xunit.v3.Parallelization(Mode = Xunit.Sdk.ParallelMode.None)]
