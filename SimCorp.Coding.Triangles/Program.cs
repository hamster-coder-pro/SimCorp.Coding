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

                var triangleTypeApp = sp.GetRequiredService<TriangleTypeApp>();
                triangleTypeApp.Run();
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
                serviceCollection.AddValidatorsFromAssembly(typeof(Program).Assembly, ServiceLifetime.Transient);

                serviceCollection.AddSingleton<IDoubleHelper, DoubleHelper>();

                serviceCollection.AddTransient<TriangleTypeApp>();
                serviceCollection.AddTransient<TriangleAreaApp>();
                serviceCollection.AddTransient<TriangleLargestAngleApp>();

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

                serviceCollection.AddTransient<IInputProvider<TriangleArguments>, TriangleInputProvider>();

                return serviceCollection.BuildServiceProvider();
            }
            catch (Exception exception)
            {
                throw new Exception("Can't initialize service provider", exception);
            }
        }
    }
}