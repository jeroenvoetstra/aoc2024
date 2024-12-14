using System.Windows;

namespace AoC2024.Visualization;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        Environment.SetEnvironmentVariable("AOC_HOME", @"C:\dev\personal\aoc2024", EnvironmentVariableTarget.Process);

        base.OnStartup(e);
    }
}

