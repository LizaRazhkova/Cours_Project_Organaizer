using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organaizer_v2.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private FrameSourceService frameSourceService;
        private InvokerService invokerService;
        private AuthService authService;

        private string username;
        private string password;
        private string confirmPassword;

        public Users User { get; private set; }

        public RegisterViewModel(FrameSourceService frameSourceService, InvokerService invokerService, AuthService authService)
        {
            this.frameSourceService = frameSourceService;
            this.invokerService = invokerService;
            this.authService = authService;

            Username = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;


        }

        public ICommand CreateNewAccount
        {
            get
            {
                return new RelayCommand(async obj =>
                {
                    try
                    {
                        if (Username == "" || Password == "" || ConfirmPassword == "") { throw new ArgumentException("Заполните все поля"); }
                        if (Password != ConfirmPassword) { throw new ArgumentException("Пароли не совпадают"); }
                        Regex regexPass = new Regex(@"(?=^.{4,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$");
                        Regex regexName = new Regex(@"^[a-zA-Z0-9А-Яа-я][a-zA-Z0-9А-Яа-я_\.]{1,20}$");
                        if (!regexPass.IsMatch(Password)) { throw new ArgumentException("Пароль не менее 4 символов,должен содержать хотя бы одну цифру, прописную и строчную латинские буквы"); }
                        if (!regexName.IsMatch(Username)) { throw new ArgumentException("Имя пользователя, не менее 2 и не более 20 символов, содержит только буквы и цифры"); }

                        string passwordHash = BCrypt.Net.BCrypt.HashPassword(Password);

                        this.authService.AddUserInDB(Username, passwordHash);
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
        public ICommand GoToLoginAccount
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    this.frameSourceService.ChangeFrame(new Login());
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
        public string ConfirmPassword
        {
            get => this.confirmPassword;
            set
            {
                this.confirmPassword = value;
                OnPropertyChanged();
            }
        }
    }
}
