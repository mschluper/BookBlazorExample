// See https://aka.ms/new-console-template for more information

using Castle.Core.Internal;
using RequirementsTracer;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using TestResult = RequirementsTracer.TestResult;

var appSettings = ConfigurationManager.AppSettings;
if (!appSettings.AllKeys.Contains("RootDir"))
{
    Console.WriteLine("INVALID App.Config - missing RootDir appSetting");
    return;
}
if (!appSettings.AllKeys.Contains("TestAssembly"))
{
    Console.WriteLine("INVALID App.Config - missing TestAssembly appSetting");
    return;
}
if (!appSettings.AllKeys.Contains("TestClasses"))
{
    Console.WriteLine("INVALID App.Config - missing TestClasses appSetting");
    return;
}
if (!appSettings.AllKeys.Contains("TestResults"))
{
    Console.WriteLine("INVALID App.Config - missing TestResults appSetting");
    return;
}

var rootDir = ConfigurationManager.AppSettings["RootDir"];

// We have to build first (to get all updates of test method attributes, and we have to run the tests (to get the latest pass/fail data).
// Build is done in background
System.Diagnostics.Process process = new();
System.Diagnostics.ProcessStartInfo startInfo = new()
{
    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
    FileName = "cmd.exe",
    Arguments = $"/C dotnet build {rootDir}\\BookBlazorExample.sln"
};

process.StartInfo = startInfo;
process.Start();
process.WaitForExit(60000); // wait at most 60 seconds

string strCmdText;
strCmdText = @$"/C ""C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"" {rootDir}\BookBlazorExample.Test\bin\Debug\net7.0\BookBlazorExample.Test.dll > {rootDir}\BookBlazorExample\test_output.txt";
System.Diagnostics.Process.Start("CMD.exe", strCmdText).WaitForExit(60000);


var testResultFileName = $"{rootDir}\\{appSettings["TestResults"]}";

var lines = File.ReadAllLines(testResultFileName!);
List<TestResult> testResults = new();
for (var i = 0; i < lines.Length; i += 1)
{
    var line = lines[i];
    // Process line
    var match = Regex.Match(line, @"  Passed ([^\(]+)\(([^\)]+)\) \[[^\]]*\]");
    if (match.Success)
    {
        if (match.Groups.Count == 3)
        {
            Console.WriteLine($"Test: {match.Groups[1].Value}    Parameters: {match.Groups[2].Value}    PASSED");
            testResults.Add(new() { Passed = true, TestName = match.Groups[1].Value, Parameters = match.Groups[2].Value, });
        }
    }
    match = Regex.Match(line, @"  Passed ([^\[]+) \[[^\]]*\]");
    if (match.Success)
    {
        if (match.Groups.Count == 2)
        {
            Console.WriteLine($"Test: {match.Groups[1].Value}    PASSED");
            testResults.Add(new() { Passed = true, TestName = match.Groups[1].Value, Parameters = null, });
        }
    }
    match = Regex.Match(line, @"  Failed ([^\(]+)\(([^\)]+)\) \[[^\]]*\]");
    if (match.Success)
    {
        if (match.Groups.Count == 3)
        {
            Console.WriteLine($"Test: {match.Groups[1].Value}    Parameters: {match.Groups[2].Value}    FAILED!!!");
            testResults.Add(new() { Passed = false, TestName = match.Groups[1].Value, Parameters = match.Groups[2].Value, });
        }
    }
    match = Regex.Match(line, @"  Failed ([^\[]+) \[[^\]]*\]");
    if (match.Success)
    {
        if (match.Groups.Count == 2)
        {
            Console.WriteLine($"Test: {match.Groups[1].Value}    FAILED!!!");
            testResults.Add(new() { Passed = false, TestName = match.Groups[1].Value, Parameters = null, });
        }
    }
}

var requirements = RequirementsSpecification.Requirements;

var testDll = $"{rootDir}\\{appSettings["TestAssembly"]}";
var testClasses = Assembly.LoadFrom(testDll!).GetTypes();

var CoversAttributeType = testClasses.First(c => c.Name == "CoversAttribute");

var selectedTestClassNames = ConfigurationManager.AppSettings["TestClasses"]!.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new();
var selectedTestClasses = testClasses.Where(t => t.FullName != null && selectedTestClassNames.Contains(t.FullName)).ToList();

foreach (var testClass in selectedTestClasses)
{
    MemberInfo[] MyMemberInfo = testClass.GetMethods();

    for (int i = 0; i < MyMemberInfo.Length; i++)
    {
        var testName = MyMemberInfo[i].Name ?? "[Unknown]";
        var testFullName = MyMemberInfo[i].DeclaringType + "." + MyMemberInfo[i].Name;
        var isFact = MyMemberInfo[i].GetAttributes<Xunit.FactAttribute>().Any();
        var isTheory = MyMemberInfo[i].GetAttributes<Xunit.TheoryAttribute>().Any();

        var factAttributes = MyMemberInfo[i].GetAttributes<Xunit.FactAttribute>().ToList();
        var theoryAttributes = MyMemberInfo[i].GetAttributes<Xunit.TheoryAttribute>().ToList();

        if (!isFact && !isTheory)
        {
            continue;
        }
        var testDisplayName = MyMemberInfo[i].GetAttributes<Xunit.FactAttribute>().FirstOrDefault()?.DisplayName; // Yeah, this is weird. The TheoryAttribute does not have the DisplayName ...
        if (testDisplayName == null)
        {
            Console.WriteLine($"Warning: Test {testName} does not have a DisplayName (Fact, Theory).");
        }

        var attributes = (Attribute.GetCustomAttributes(MyMemberInfo[i], CoversAttributeType)).ToList();
        if (attributes.Count == 0)
        {
            Console.WriteLine("Warning: Test not referring to a requirement. No attribute in member function {0}.\n", testName);
        }
        else
        {
            var attr = attributes[0];// as BookBlazorExample.Test.CoversAttribute;// as CoversAttribute;
            var requirementId = attr.GetType()?.GetRuntimeProperty("RequirementId")?.GetValue(attr)?.ToString();

            var coveredRequirement = requirements.FirstOrDefault(r => r.Id == requirementId); //  attr.RequirementId);
            if (coveredRequirement != null)
            {
                coveredRequirement.IsCoveredByTest = testName != null;

                var matchingTestResults = testResults.Where(tr => tr.TestName == testFullName).ToList();
                if (matchingTestResults.Count > 0)
                {
                    coveredRequirement.PassedTests = !matchingTestResults.Any(tr => !tr.Passed); // none failed
                }
                else if (testDisplayName != null)
                {
                    Console.WriteLine($"Warning: Test {testName} (DisplayName=\"{testDisplayName}\") is not referring to a requirement. ");
                }
            }
        }
    }
}

Console.WriteLine();
Console.WriteLine($"Found {testResults.Count()} tests; {testResults.Count(r => !r.Passed)} failed. ");
if (testResults.Any(r => !r.Passed))
{
    Console.WriteLine($"Test results can be found in file {testResultFileName}.");
}

var output = System.Text.Json.JsonSerializer.Serialize(requirements);

var originalDirectoryName = Path.GetDirectoryName(testResultFileName);

File.WriteAllText($"{originalDirectoryName}/requirementCoverage.json", output);