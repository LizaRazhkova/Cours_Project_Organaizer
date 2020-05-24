using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Organaizer_v2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        //dependency injection container
        protected override void OnStartup(StartupEventArgs e)
        {
            // dependenies
            FrameSourceService frameSourceService = new FrameSourceService();
            InvokerService invokerService = new InvokerService();
            AuthService authService = new AuthService();

            // viewModels
            MainViewModel mainViewModel = new MainViewModel(frameSourceService, invokerService);
            HomeViewModel homeViewModel = new HomeViewModel(frameSourceService, invokerService);
            LoginViewModel loginViewModel = new LoginViewModel(frameSourceService, invokerService,authService);
            RegisterViewModel registerViewModel = new RegisterViewModel(frameSourceService, invokerService,authService);
            SettingViewModel settingViewModel = new SettingViewModel(frameSourceService, invokerService, authService);
            GalleryViewModel galleryViewModel = new GalleryViewModel(frameSourceService, invokerService,authService);
            TaskManagerViewModel taskManagerViewModel = new TaskManagerViewModel(frameSourceService, invokerService, authService);
            ManagerPassViewModel managerPassViewModel = new ManagerPassViewModel(frameSourceService, invokerService,authService);

            // singleton
            ViewModelContainer.Init(mainViewModel, homeViewModel, loginViewModel, registerViewModel, 
                settingViewModel, galleryViewModel, taskManagerViewModel, managerPassViewModel);

            invokerService.Invoke<MainViewModel>(new InitializationViewModel());

            base.OnStartup(e);
        }


    }
}
