using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organaizer_v2.ViewModel
{
    public class SettingViewModel : BaseViewModel
    {
        private FrameSourceService frameSourceService;
        private InvokerService invokerService;
        private AuthService authService;

        private string username;
        private string password;
        private string oldPassword;

        public SettingViewModel(FrameSourceService frameSourceService, InvokerService invokerService, AuthService authService)
        {
            this.frameSourceService = frameSourceService;
            this.invokerService = invokerService;
            this.authService = authService;

            Username = string.Empty;
            Password = string.Empty;
            oldPassword = string.Empty;
        }

        public ICommand ComeBack
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.frameSourceService.ChangeFrame(new Home(this.invokerService));
                });
            }
        }

        public ICommand ChangedName
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    try
                    {
                        if (Username == "") { throw new ArgumentException("Заполните все поля"); }

                        Regex regexName = new Regex(@"^[a-zA-Z0-9А-Яа-я][a-zA-Z0-9А-Яа-я_\.]{1,20}$");

                        if (!regexName.IsMatch(Username)) { throw new ArgumentException("Имя пользователя, не менее 2 и не более 20 символов, содержит только буквы и цифры"); }
                        this.authService.SettingChangedName(this.authService.User.id, Username);
                        var notification = new Notification("Имя изменено изменен");
                        await this.invokerService.Invoke<MainViewModel>(new InvokeNotification(notification));

                        Username = string.Empty;
                    }
                    catch(ArgumentException e)
                    {
                        var notification = new Notification(e.Message);
                        await this.invokerService.Invoke<MainViewModel>(new InvokeNotification(notification));
                    }
                });
            }
        }

        public ICommand ChangedPassword
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    try
                    {
                        if (Password == "" && OldPassword == "") { throw new ArgumentException("Заполните все поля"); }
                            Regex regexPass = new Regex(@"(?=^.{4,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$");
                            if (!regexPass.IsMatch(Password)) { throw new ArgumentException("Пароль не менее 4 символов,должен содержать хотя бы одну цифру, прописную и строчную латинские буквы"); }
                            var user = await this.authService.SettingChangedPassword(this.authService.User.id, OldPassword, Password);
                            if (user == null) { throw new ArgumentException("Неверно введен старый пароль!"); }
                            
                                var notification = new Notification("Пароль изменен");
                                await this.invokerService.Invoke<MainViewModel>(new InvokeNotification(notification));
                                Password = string.Empty;
                                OldPassword = string.Empty;                        
                    }
                    catch(ArgumentException e)
                    {
                        var notification = new Notification(e.Message);
                        await this.invokerService.Invoke<MainViewModel>(new InvokeNotification(notification));
                    }
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
        public string OldPassword
        {
            get => this.oldPassword;
            set
            {
                this.oldPassword = value;
                OnPropertyChanged();
            }
        }
    }
}
