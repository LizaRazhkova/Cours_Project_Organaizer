using Organaizer_v2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organaizer_v2
{
    public class ViewModelContainer
    {
        public ViewModelContainer() { }

        private MainViewModel mainViewModel;
        private HomeViewModel homeViewModel;
        private LoginViewModel loginViewModel;
        private RegisterViewModel registerViewModel;
        private SettingViewModel settingViewModel;
        private GalleryViewModel galleryViewModel;
        private TaskManagerViewModel taskManagerViewModel;
        private ManagerPassViewModel managerPassViewModel;

        public MainViewModel MainViewModel => ViewModelContainer.GetInstance().mainViewModel;
        public HomeViewModel HomeViewModel => ViewModelContainer.GetInstance().homeViewModel;
        public LoginViewModel LoginViewModel => ViewModelContainer.GetInstance().loginViewModel;
        public RegisterViewModel RegisterViewModel => ViewModelContainer.GetInstance().registerViewModel;
        public SettingViewModel SettingViewModel => ViewModelContainer.GetInstance().settingViewModel;
        public GalleryViewModel GalleryViewModel => ViewModelContainer.GetInstance().galleryViewModel;
        public TaskManagerViewModel TaskManagerViewModel => ViewModelContainer.GetInstance().taskManagerViewModel;
        public ManagerPassViewModel ManagerPassViewModel => ViewModelContainer.GetInstance().managerPassViewModel;

        private static ViewModelContainer instance = new ViewModelContainer();
        private ViewModelContainer(MainViewModel mainViewModel, HomeViewModel homeViewModel,
            LoginViewModel loginViewModel, RegisterViewModel registerViewModel, SettingViewModel settingViewModel,
            GalleryViewModel galleryViewModel, TaskManagerViewModel taskManagerViewModel, ManagerPassViewModel managerPassViewModel)
        {
            this.mainViewModel = mainViewModel;
            this.homeViewModel = homeViewModel;
            this.loginViewModel = loginViewModel;
            this.registerViewModel = registerViewModel;
            this.settingViewModel = settingViewModel;
            this.galleryViewModel = galleryViewModel;
            this.taskManagerViewModel = taskManagerViewModel;
            this.managerPassViewModel = managerPassViewModel;
        }
        public static void Init(MainViewModel mainViewModel, HomeViewModel homeViewModel,
            LoginViewModel loginViewModel, RegisterViewModel registerViewModel, SettingViewModel settingViewModel,
            GalleryViewModel galleryViewModel, TaskManagerViewModel taskManagerViewModel, ManagerPassViewModel managerPassViewModel)
        {
            instance = new ViewModelContainer(mainViewModel, homeViewModel, loginViewModel, registerViewModel, settingViewModel,
                galleryViewModel, taskManagerViewModel, managerPassViewModel);
        }

        public static ViewModelContainer GetInstance() => instance;
    }
}
