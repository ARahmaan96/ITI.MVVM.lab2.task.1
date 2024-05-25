using Autofac;
using System.Windows;
using Task1.DataService;
using Task1.ModleView;
using Task1.Views;

namespace Task1
{
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();

            // Register DbContext
            builder.RegisterType<AppDbContext>().AsSelf().SingleInstance();

            // Register ViewModel
            builder.RegisterType<HomeViewModel>().AsSelf();

            // Register Home window
            builder.RegisterType<Home>().AsSelf();

            _container = builder.Build();

            // Resolve and set the DataContext of Home
            using (var scope = _container.BeginLifetimeScope())
            {
                var homeWindow = scope.Resolve<Home>();
                homeWindow.DataContext = scope.Resolve<HomeViewModel>();
                homeWindow.Show();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }
}
