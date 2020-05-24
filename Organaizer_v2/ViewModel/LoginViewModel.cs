using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Organaizer_v2.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private FrameSourceService frameSourceService;
        private InvokerService invokerService;
        private AuthService authService;

        private string username;
        private string password;

        public ObservableCollection<Users> CollectionOfUser { get; private set; }
        public static int IdUser;
       

        public Users user { get; private set; }

        public LoginViewModel(FrameSourceService frameSourceService, InvokerService invokerService, AuthService authService)
        {
            this.frameSourceService = frameSourceService;
            this.invokerService = invokerService;
            this.authService = authService;

            Username = string.Empty;
            Password = string.Empty;

            CollectionOfUser = new ObservableCollection<Users>();

            LoadUser();


        }

        public ICommand VerificationUser
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    try
                    {
                        if (Username == "" || Password == "") { throw new ArgumentException("Заполните все поля"); }
                        bool active = this.authService.ActiveUser(Username, Password);
                        if (!active)
                        {
                            { throw new ArgumentException("Неверные данные"); }
                        }

                        await invokerService.Invoke<GalleryViewModel>(new UserJoinIntoAccount());
                        await invokerService.Invoke<ManagerPassViewModel>(new UserJoinIntoAccount());
                        this.frameSourceService.ChangeFrame(new Home(this.invokerService));
                    }

                    catch (ArgumentException e)
                    {
                        var notification = new Notification(e.Message);
                        await this.invokerService.Invoke<MainViewModel>(new InvokeNotification(notification));
                    }

                });
            }
        }

        private void LoadUser()
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                try
                {
                    var userList = db.Users.ToList();
                    CollectionOfUser = new ObservableCollection<Users>(userList);
                    OnPropertyChanged("CollectionOfUser");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public ICommand ToCreateNewAccount
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.frameSourceService.ChangeFrame(new Register());
                });
            }
        }

        public string Username
        {
            get => this.username;
            set
            {
                this.username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => this.password;
            set
            {
                this.password = value;
                OnPropertyChanged();
            }
        }
    }
}
