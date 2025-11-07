using Spectre.Console;
using System;
using System.IO;
using System.Threading.Tasks;
using TestCaseGenerator.Core.Analyzers;
using TestCaseGenerator.Core.Generators;
using TestCaseGenerator.Core.Models;

namespace TestCaseGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ShowBanner();

            var generator = new TestGenerator();
            var coverageAnalyzer = new CoverageAnalyzer();

            while (true)
            {
                var choice = ShowMainMenu();

                switch (choice)
                {
                    case "Generate Tests":
                        await GenerateTestsFlow(generator, coverageAnalyzer);
                        break;
                    case "Analyze Code":
                        await AnalyzeCodeFlow(generator, coverageAnalyzer);
                        break;
                    case "Batch Generate":
                        await BatchGenerateFlow(generator);
                        break;
                    case "Exit":
                        AnsiConsole.MarkupLine("[green]Thank you for using Test Case Generator![/]");
                        return;
                }

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[dim]Press any key to continue...[/]");
                Console.ReadKey(true);
                Console.Clear();
            }
        }

        static void ShowBanner()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText(".NET Test Generator")
                    .Centered()
                    .Color(Color.Blue));

            AnsiConsole.MarkupLine("[dim]A comprehensive test case generation tool for .NET[/]");
            AnsiConsole.WriteLine();
        }

        static string ShowMainMenu()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]What would you like to do?[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Generate Tests",
                        "Analyze Code",
                        "Batch Generate",
                        "Exit"
                    }));
        }

        static async Task GenerateTestsFlow(TestGenerator generator, CoverageAnalyzer coverageAnalyzer)
        {
            AnsiConsole.MarkupLine("[yellow]═══ Generate Test Cases ═══[/]");
            AnsiConsole.WriteLine();

            // Get source file path
            var sourceFile = AnsiConsole.Ask<string>("[cyan]Enter the path to your C# source file:[/]");

            if (!File.Exists(sourceFile))
            {
                AnsiConsole.MarkupLine("[red]Error: File not found![/]");
                return;
            }

            // Get generation options
            var options = GetGenerationOptions();

            // Generate tests with progress
            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    var task1 = ctx.AddTask("[green]Analyzing code...[/]");
                    var classAnalysis = await generator.AnalyzeCodeAsync(sourceFile);
                    task1.Value = 100;

                    var task2 = ctx.AddTask("[green]Generating test cases...[/]");
                    var testCode = await generator.GenerateTestsAsync(sourceFile, options);
                    task2.Value = 100;

                    var task3 = ctx.AddTask("[green]Analyzing coverage...[/]");
                    var coverage = coverageAnalyzer.AnalyzeCoverage(classAnalysis);
                    task3.Value = 100;

                    AnsiConsole.WriteLine();
                    DisplayCoverageReport(coverage);
                });

            AnsiConsole.MarkupLine("[green]✓ Tests generated successfully![/]");
        }

        static async Task AnalyzeCodeFlow(TestGenerator generator, CoverageAnalyzer coverageAnalyzer)
        {
            AnsiConsole.MarkupLine("[yellow]═══ Analyze Code ═══[/]");
            AnsiConsole.WriteLine();

            var sourceFile = AnsiConsole.Ask<string>("[cyan]Enter the path to your C# source file:[/]");

            if (!File.Exists(sourceFile))
            {
                AnsiConsole.MarkupLine("[red]Error: File not found![/]");
                return;
            }

            await AnsiConsole.Status()
                .StartAsync("Analyzing code...", async ctx =>
                {
                    var classAnalysis = await generator.AnalyzeCodeAsync(sourceFile);
                    var coverage = coverageAnalyzer.AnalyzeCoverage(classAnalysis);

                    AnsiConsole.WriteLine();
                    DisplayClassAnalysis(classAnalysis);
                    AnsiConsole.WriteLine();
                    DisplayCoverageReport(coverage);
                });
        }

        static async Task BatchGenerateFlow(TestGenerator generator)
        {
            AnsiConsole.MarkupLine("[yellow]═══ Batch Generate Tests ═══[/]");
            AnsiConsole.WriteLine();

            var directory = AnsiConsole.Ask<string>("[cyan]Enter the directory path containing C# files:[/]");

            if (!Directory.Exists(directory))
            {
                AnsiConsole.MarkupLine("[red]Error: Directory not found![/]");
                return;
            }

            var options = GetGenerationOptions();
            var files = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);

            AnsiConsole.MarkupLine($"[cyan]Found {files.Length} C# file(s)[/]");
            AnsiConsole.WriteLine();

            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    var task = ctx.AddTask("[green]Generating tests...[/]", maxValue: files.Length);

                    foreach (var file in files)
                    {
                        try
                        {
                            await generator.GenerateTestsAsync(file, options);
                            task.Increment(1);
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine($"[red]Error processing {Path.GetFileName(file)}: {ex.Message}[/]");
                        }
                    }
                });

            AnsiConsole.MarkupLine("[green]✓ Batch generation completed![/]");
        }

        static GenerationOptions GetGenerationOptions()
        {
            var options = new GenerationOptions();

            // Test Framework
            var frameworkChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Select test framework:[/]")
                    .AddChoices(new[] { "NUnit", "MSTest", "XUnit" }));

            options.TestFramework = frameworkChoice switch
            {
                "NUnit" => TestFramework.NUnit,
                "MSTest" => TestFramework.MSTest,
                "XUnit" => TestFramework.XUnit,
                _ => TestFramework.NUnit
            };

            // Mocking Framework
            var mockingChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Select mocking framework:[/]")
                    .AddChoices(new[] { "Moq", "NSubstitute", "None" }));

            options.MockingFramework = mockingChoice switch
            {
                "Moq" => MockingFramework.Moq,
                "NSubstitute" => MockingFramework.NSubstitute,
                _ => MockingFramework.None
            };

            // Test Type
            var testTypeChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Select test type:[/]")
                    .AddChoices(new[] { "Unit", "Integration", "Api", "UI" }));

            options.TestType = testTypeChoice switch
            {
                "Unit" => TestType.Unit,
                "Integration" => TestType.Integration,
                "Api" => TestType.Api,
                "UI" => TestType.UI,
                _ => TestType.Unit
            };

            // Output Directory
            options.OutputDirectory = AnsiConsole.Ask<string>("[cyan]Enter output directory:[/]", "./GeneratedTests");

            // Namespace
            options.Namespace = AnsiConsole.Ask<string>("[cyan]Enter test namespace:[/]", "Tests");

            // Additional options
            options.GenerateTestData = AnsiConsole.Confirm("[cyan]Generate test data?[/]", true);
            options.IncludeCoverageAnalysis = AnsiConsole.Confirm("[cyan]Include coverage analysis?[/]", true);

            return options;
        }

        static void DisplayClassAnalysis(ClassAnalysis analysis)
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[yellow]Property[/]");
            table.AddColumn("[cyan]Value[/]");

            table.AddRow("Class Name", analysis.ClassName);
            table.AddRow("Namespace", analysis.Namespace);
            table.AddRow("Methods", analysis.Methods.Count.ToString());
            table.AddRow("Properties", analysis.Properties.Count.ToString());
            table.AddRow("Interfaces", string.Join(", ", analysis.Interfaces));

            AnsiConsole.Write(table);

            // Method details
            if (analysis.Methods.Count > 0)
            {
                AnsiConsole.WriteLine();
                var methodTable = new Table();
                methodTable.Border(TableBorder.Rounded);
                methodTable.AddColumn("[yellow]Method[/]");
                methodTable.AddColumn("[cyan]Return Type[/]");
                methodTable.AddColumn("[green]Parameters[/]");
                methodTable.AddColumn("[magenta]Complexity[/]");
                methodTable.AddColumn("[blue]Test Cases[/]");

                foreach (var method in analysis.Methods)
                {
                    methodTable.AddRow(
                        method.MethodName,
                        method.ReturnType,
                        method.Parameters.Count.ToString(),
                        method.CyclomaticComplexity.ToString(),
                        method.SuggestedTestCases.Count.ToString()
                    );
                }

                AnsiConsole.Write(methodTable);
            }
        }

        static void DisplayCoverageReport(CoverageReport report)
        {
            var panel = new Panel(
                new Markup($"[bold]Overall Coverage Score:[/] [green]{report.OverallCoverageScore:F2}%[/]\n" +
                          $"[bold]Total Methods:[/] {report.TotalMethods}\n" +
                          $"[bold]Public Methods:[/] {report.PublicMethods}"))
            {
                Header = new PanelHeader("[yellow]Coverage Analysis[/]"),
                Border = BoxBorder.Rounded
            };

            AnsiConsole.Write(panel);

            // Method coverage details
            if (report.MethodCoverages.Count > 0)
            {
                AnsiConsole.WriteLine();
                var table = new Table();
                table.Border(TableBorder.Rounded);
                table.AddColumn("[yellow]Method[/]");
                table.AddColumn("[cyan]Complexity[/]");
                table.AddColumn("[green]Test Cases[/]");
                table.AddColumn("[magenta]Coverage Score[/]");

                foreach (var method in report.MethodCoverages)
                {
                    var scoreColor = method.CoverageScore >= 80 ? "green" :
                                   method.CoverageScore >= 60 ? "yellow" : "red";

                    table.AddRow(
                        method.MethodName,
                        method.CyclomaticComplexity.ToString(),
                        method.SuggestedTestCount.ToString(),
                        $"[{scoreColor}]{method.CoverageScore:F1}%[/]"
                    );
                }

                AnsiConsole.Write(table);
            }

            // Recommendations
            if (report.Recommendations.Count > 0)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]Recommendations:[/]");
                foreach (var recommendation in report.Recommendations)
                {
                    AnsiConsole.MarkupLine($"  {recommendation}");
                }
            }
        }
    }
}
