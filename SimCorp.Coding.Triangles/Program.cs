using System.CommandLine;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace SimCorp.Coding.Triangles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sp = CreateServiceProvider();

                var scope = sp.CreateScope();
                var command = scope.ServiceProvider.GetRequiredService<ApplicationCommand>();
                command.Invoke(args);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                Console.WriteLine("DONE");
            }
        }

        static IServiceProvider CreateServiceProvider()
        {
            try
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddValidatorsFromAssembly(typeof(Program).Assembly, ServiceLifetime.Transient, includeInternalTypes: true);

                serviceCollection.AddSingleton<IDoubleHelper, DoubleHelper>();

                serviceCollection.AddTransient<ApplicationCommand>();
                
                serviceCollection.AddTransient<IApplication, TriangleTypeApp>();
                serviceCollection.AddTransient<IApplication, TriangleAreaApp>();
                serviceCollection.AddTransient<IApplication, TriangleLargestAngleApp>();

                serviceCollection.AddTransient<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>, TriangleIsEquilateralResultMatcher>();
                serviceCollection.AddTransient<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>, TriangleIsIsoscelesResultMatcher>();
                serviceCollection.AddTransient<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>, TriangleIsScaleneResultMatcher>();

                serviceCollection.AddTransient<IResultMatcher<TriangleArguments, TriangleAngleMatchResult>, AAngleResultMatcher>();
                serviceCollection.AddTransient<IResultMatcher<TriangleArguments, TriangleAngleMatchResult>, BAngleResultMatcher>();
                serviceCollection.AddTransient<IResultMatcher<TriangleArguments, TriangleAngleMatchResult>, CAngleResultMatcher>();

                serviceCollection.AddSingleton<ConsoleInputOutputDataStrategy>();
                serviceCollection.AddTransient<IInputDataStrategy>(sp => sp.GetRequiredService<ConsoleInputOutputDataStrategy>());
                //serviceCollection.AddSingleton<IInputDataStrategy>(sp => new FileInputDataStrategy("triangle-input.txt"));
                serviceCollection.AddTransient<IOutputDataStrategy>(sp => sp.GetRequiredService<ConsoleInputOutputDataStrategy>());

                // register processors
                serviceCollection.AddTransient<IInputProcessor<TriangleArguments, TriangleTypeResult>, TriangleTypeProcessor>();
                serviceCollection.AddTransient<IInputProcessor<TriangleArguments, TriangleAreaResult>, TriangleAreaProcessor>();
                serviceCollection.AddTransient<IInputProcessor<TriangleArguments, TriangleLargestAngleResult>, TriangleLargestAngleProcessor>();

                // register input parsers
                serviceCollection.AddTransient<IInputProvider<TriangleArguments>, TriangleArgumentsInputProvider>();

                // register output builders
                serviceCollection.AddTransient<IOutputBuilder<TriangleTypeResult>, TriangleTypeResultOutputBuilder>();
                serviceCollection.AddTransient<IOutputBuilder<TriangleAreaResult>, TriangleAreaResultOutputBuilder>();
                serviceCollection.AddTransient<IOutputBuilder<TriangleLargestAngleResult>, TriangleLargestAngleResultOutputBuilder>();

                return serviceCollection.BuildServiceProvider();
            }
            catch (Exception exception)
            {
                throw new Exception("Can't initialize service provider", exception);
            }
        }
    }
}