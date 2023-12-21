// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using AdventOfCode;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<InputFileService>();

var iSolverType = typeof(ISolver);
var solverTypes = iSolverType.Assembly.GetTypes().Where(x => x.IsAssignableTo(iSolverType) && !x.IsInterface);
foreach (var solverType in solverTypes)
    serviceCollection.AddScoped(iSolverType, solverType);

var serviceProvider = serviceCollection.BuildServiceProvider();
var rootCommand = new RootCommand("AdventOfCode Solver program");

var inputOption = new Option<FileInfo?>(
    name: "--input",
    description: "Input file to run the solver agaisnt.",
    parseArgument: result =>
    {
        if (result.Tokens.Count == 0)
        {
            result.ErrorMessage = "Please choose a file.";
            return null;
        }

        var filePath = result.Tokens.Single().Value;
        if (!File.Exists(filePath))
        {
            result.ErrorMessage = "File does not exist.";
            return null;
        }

        return new FileInfo(filePath);
    }
)
{ IsRequired = true };

var solverOption = new Option<int?>(
    name: "--solver",
    description: "Solver to use, day of the program to solve.",
    parseArgument: result =>
    {
        if (result.Tokens.Count == 0)
        {
            result.ErrorMessage = "Please choose a solver.";
            return null;
        }

        string? day = result.Tokens.Single().Value;
        if (!int.TryParse(day, out var parsedDay))
        {
            result.ErrorMessage = "Solver should be the day number.";
            return null;
        }

        var solver = serviceProvider.GetServices<ISolver>().FirstOrDefault(x => x.GetDay() == parsedDay);
        if (solver == null)
        {
            result.ErrorMessage = "Can't find solver for this day.";
            return null;
        }

        return parsedDay;
    }
)
{ IsRequired = true };

var debugOption = new Option<bool>(
    name: "--showDebug",
    description: "Show debug output of the solver.",
    getDefaultValue: () => false
);

var solveCommand = new Command("solve", "Solve and display the solution of a particular day for a specified input.")
{
    inputOption,
    solverOption,
    debugOption
};
rootCommand.Add(solveCommand);

solveCommand.SetHandler((inputFile, solverDay, showDebug) =>
{
    var inputFileService = serviceProvider.GetRequiredService<InputFileService>();
    var solver = serviceProvider.GetServices<ISolver>().First(x => x.GetDay() == solverDay);
    var inputs = inputFileService.ReadInputFile(inputFile!.ToString());
    var result = solver.Solve(inputs, showDebug);

    Console.WriteLine($"Result for day {solverDay} is {result}");
},
inputOption, solverOption, debugOption);

return rootCommand.Invoke(args);
