using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace Socialize.Tests.ArchitectureTests
{
    public class ArchitectureTests
    {
        private static readonly Assembly[] TestAssemblies = new[]
        {
            Assembly.Load("Socialize.Core.Domain"),
            Assembly.Load("Socialize.Core.Application"),
            Assembly.Load("Socialize.Infrastructure.Identity"),
            Assembly.Load("Socialize.Infrastructure.Shared"),
            Assembly.Load("Socialize.Presentation")
        };

        [Fact]
        public void DomainLayer()
        {
            var dependsOnApplication = Types.InAssemblies(TestAssemblies)
                .That()
                .ResideInNamespace("Socialize.Core.Domain")
                .ShouldNot()
                .HaveDependencyOn("Socialize.Core.Application")
                .GetResult();

            var dependsOnPersistence = Types.InAssemblies(TestAssemblies)
                .That()
                .ResideInNamespace("Socialize.Core.Domain")
                .ShouldNot()
                .HaveDependencyOn("Socialize.Infrastructure.Identity")
                .GetResult();

            var dependsOnShared = Types.InAssemblies(TestAssemblies)
                .That()
                .ResideInNamespace("Socialize.Core.Domain")
                .ShouldNot()
                .HaveDependencyOn("Socialize.Infrastructure.Shared")
                .GetResult();

            var dependsOnPresentation = Types.InAssemblies(TestAssemblies)
                .That()
                .ResideInNamespace("Socialize.Core.Domain")
                .ShouldNot()
                .HaveDependencyOn("Socialize.Presentation")
                .GetResult();

            Assembly domainAssembly = Assembly.Load("Socialize.Core.Domain");

            
            AssemblyName applicationAssemblyName = new AssemblyName("Socialize.Core.Application");
            AssemblyName persistenceAssemblyName = new AssemblyName("Socialize.Infrastructure.Identity");
            AssemblyName sharedAssemblyName = new AssemblyName("Socialize.Infrastructure.Shared");
            AssemblyName presentationAssemblyName = new AssemblyName("Socialize.Presentation");

            var referencedAssemblies = domainAssembly.GetReferencedAssemblies();

            
            var applicationReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == applicationAssemblyName.Name);
            var persistenceReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == persistenceAssemblyName.Name);
            var sharedReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == sharedAssemblyName.Name);
            var presentationReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == presentationAssemblyName.Name);

            Assert.Null(applicationReference);
            Assert.Null(persistenceReference);
            Assert.Null(sharedReference);
            Assert.Null(presentationReference);

            Assert.True(dependsOnApplication.IsSuccessful && dependsOnPersistence.IsSuccessful && dependsOnShared.IsSuccessful && dependsOnPresentation.IsSuccessful);
        }

        [Fact]
        public void ApplicationLayer()
        {
            var doesnotDependOnPersistence = Types.InAssemblies(TestAssemblies)
                .That()
                .ResideInNamespace("Socialize.Core.Application")
                .ShouldNot()
                .HaveDependencyOn("Socialize.Infrastructure.Identity")
                .GetResult();

            var doesnotDependOnShared = Types.InAssemblies(TestAssemblies)
                .That()
                .ResideInNamespace("Socialize.Core.Application")
                .ShouldNot()
                .HaveDependencyOn("Socialize.Infrastructure.Shared")
                .GetResult();

            var doesnotDependsOnPresentation = Types.InAssemblies(TestAssemblies)
                .That()
                .ResideInNamespace("Socialize.Core.Application")
                .ShouldNot()
                .HaveDependencyOn("Socialize.Presentation")
                .GetResult();

            Assembly applicationAssembly = Assembly.Load("Socialize.Core.Application");
            AssemblyName domainAssemblyName = new AssemblyName("Socialize.Core.Domain");
            AssemblyName persistenceAssemblyName = new AssemblyName("Socialize.Infrastructure.Identity");
            AssemblyName sharedAssemblyName = new AssemblyName("Socialize.Infrastructure.Shared");
            AssemblyName presentationAssemblyName = new AssemblyName("Socialize.Presentation");

            // Obtener las referencias de Socialize.Core.Application
            var referencedAssemblies = applicationAssembly.GetReferencedAssemblies();

            // Verificar si hay una referencia a Socialize.Core.Domain en Socialize.Core.Application
            var domainReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == domainAssemblyName.Name);
            var persistenceReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == persistenceAssemblyName.Name);
            var sharedReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == sharedAssemblyName.Name);
            var presentationReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == presentationAssemblyName.Name);

            Assert.NotNull(domainReference);
            Assert.Null(persistenceReference);
            Assert.Null(sharedReference);
            Assert.Null(presentationReference);

            Assert.True(doesnotDependOnPersistence.IsSuccessful && doesnotDependOnShared.IsSuccessful && doesnotDependsOnPresentation.IsSuccessful);
        }

        [Fact]
        public void PersistenceLayer()
        {
            Assembly persistenceAssembly = Assembly.Load("Socialize.Infrastructure.Identity");

            AssemblyName domainAssemblyName = new AssemblyName("Socialize.Core.Domain");
            AssemblyName applicationAssemblyName = new AssemblyName("Socialize.Core.Application");

            var referencedAssemblies = persistenceAssembly.GetReferencedAssemblies();

            var domainReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == domainAssemblyName.Name);
            var applicationReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == applicationAssemblyName.Name);


            Assert.NotNull(domainReference);
            Assert.NotNull(applicationReference);
        }

        [Fact]
        public void SharedLayer() {

            Assembly sharedAssembly = Assembly.Load("Socialize.Infrastructure.Identity");

            AssemblyName domainAssemblyName = new AssemblyName("Socialize.Core.Domain");
            AssemblyName applicationAssemblyName = new AssemblyName("Socialize.Core.Application");

            var referencedAssemblies = sharedAssembly.GetReferencedAssemblies();

            var domainReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == domainAssemblyName.Name);
            var applicationReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == applicationAssemblyName.Name);


            Assert.NotNull(domainReference);
            Assert.NotNull(applicationReference);
        }


        [Fact]
        public void PresentationLayer()
        {
            Assembly presentationAssembly = Assembly.Load("Socialize.Infrastructure.Identity");

            AssemblyName domainAssemblyName = new AssemblyName("Socialize.Core.Domain");
            AssemblyName applicationAssemblyName = new AssemblyName("Socialize.Core.Application");

            var referencedAssemblies = presentationAssembly.GetReferencedAssemblies();

            var domainReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == domainAssemblyName.Name);
            var applicationReference = referencedAssemblies.FirstOrDefault(assembly => assembly.Name == applicationAssemblyName.Name);


            Assert.NotNull(domainReference);
            Assert.NotNull(applicationReference);
        }
    }
}