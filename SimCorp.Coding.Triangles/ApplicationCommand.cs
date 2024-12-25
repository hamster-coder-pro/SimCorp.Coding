using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCorp.Coding.Triangles;

internal class ApplicationCommand : Command
{
    private IOutputDataStrategy                       Output       { get; }
    private IReadOnlyDictionary<string, IApplication> Applications { get; }

    public ApplicationCommand(IEnumerable<IApplication> applications, IOutputDataStrategy output)
        : base("app-command", "Geometry application")
    {
        Output       = output;
        Applications = applications.ToDictionary(x => x.Name, x => x);

        AppNameArgument = new Argument<string>(name: "--app");
        AppNameArgument.Completions.Add(Applications.Keys.ToArray());
        AppNameArgument.SetDefaultValue("");
        AddArgument(AppNameArgument);

        this.SetHandler(context =>
        {
            var appName = context.ParseResult.GetValueForArgument(AppNameArgument);
            if (Applications.TryGetValue(appName, out var application))
            {
                application.Run();
            }
            else
            {
                Output.WriteLine($"Application '{appName}' not found.");
            }
        });
    }

    private Argument<string> AppNameArgument { get; }
}