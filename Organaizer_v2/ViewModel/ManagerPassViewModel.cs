using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Organaizer_v2.ViewModel
{
    public class ManagerPassViewModel : BaseViewModel
    {
        private FrameSourceService frameSourceService;
        private InvokerService invokerService;
        private AuthService authService;

        private string network;
        private string login;
        private string password;

        public ManagerPass SelectedPass { get; set; }

        public ObservableCollection<ManagerPass> CollectionOfPassword { get; private set; }

        public ManagerPassViewModel(FrameSourceService frameSourceService, InvokerService invokerService, AuthService authService)
        {
            this.frameSourceService = frameSourceService;
            this.invokerService = invokerService;
            this.authService = authService;
            CollectionOfPassword = new ObservableCollection<ManagerPass>();
            this.invokerService.Receive<LoadNeccessaryData>(this, async (data) => LoadPassword());

            Network = string.Empty;
            Login = string.Empty;
            Password = string.Empty;



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

        private async Task<ManagerPass> SaveIntoDB(string network, string login, string password)
        {
            Users user = this.authService.User;

            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {


                ManagerPass pass = new ManagerPass()
                {
                    SocialNetwork = network,
                    Login = login,
                    Password = password,
                    Id = user.id
                };
                db.ManagerPass.Add(pass);
                await db.SaveChangesAsync();
                LoadPassword();
                return pass;
            }
        }

        private void LoadPassword()
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                try
                {
                    Users user = this.authService.User;
                    var list = db.ManagerPass.ToList();
                    List<ManagerPass> passwordsList = new List<ManagerPass>();


                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list[i].Id == user.id)
                        {
                            passwordsList.Add(list[i]);
                        }
                    }
                    CollectionOfPassword = new ObservableCollection<ManagerPass>(passwordsList);
                    OnPropertyChanged("CollectionOfPassword");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public ICommand AddPassword
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    try
                    {
                        if (Network == "" || Login == "" || Password == "") { throw new ArgumentException("Заполните все поля"); }

                        CollectionOfPassword.Add(await SaveIntoDB(Network, Login, Password));
                        LoadPassword();

                        Network = string.Empty;
                        Login = string.Empty;
                        Password = string.Empty;
                        
                    }
                    catch(ArgumentException e)
                    {
                        var notification = new Notification(e.Message);
                        await this.invokerService.Invoke<MainViewModel>(new InvokeNotification(notification));
                    }
                });
            }
        }

        public ICommand DeleteElem
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    var item = SelectedPass as ManagerPass;

                    await DeleteIntoDB(item.IdManagerPass);
                    CollectionOfPassword.Remove(item);
                    LoadPassword();

                });
            }
        }

        private async Task<ManagerPass> DeleteIntoDB(int id)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                ManagerPass managerPass = db.ManagerPass.FirstOrDefault(item => item.IdManagerPass == id);
                db.ManagerPass.Remove(managerPass);
                await db.SaveChangesAsync();
                return managerPass;
            }
        }

        public ICommand ChangedElem
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    var item = SelectedPass as ManagerPass;
                    await ChangedIntoDB(item.IdManagerPass, item.SocialNetwork, item.Login, item.Password);
                    LoadPassword();
                });
            }
        }

        private async Task<ManagerPass> ChangedIntoDB(int id,string network, string login, string password)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                ManagerPass managerPass = db.ManagerPass.FirstOrDefault(item => item.IdManagerPass == id);

                if (managerPass.SocialNetwork != network)
                {
                    managerPass.SocialNetwork = network;
                }

                if (managerPass.Login != login)
                {
                    managerPass.Login = login;
                }

                if (managerPass.Password != password)
                {
                    managerPass.Password = password;
                }

                await db.SaveChangesAsync();
                return managerPass;
            }
        }

        public string Network
        {
            get => this.network;
            set
            {
                this.network = value;
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
        public string Login
        {
            get => this.login;
            set
            {
                this.login = value;
                OnPropertyChanged();
            }
        }

    }
}
