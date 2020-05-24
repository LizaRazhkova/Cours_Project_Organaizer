using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Organaizer_v2.ViewModel
{
    public class TaskManagerViewModel : BaseViewModel
    {
        private FrameSourceService frameSourceService;
        private InvokerService invokerService;
        private AuthService authService;

        public ObservableCollection<TaskManager> CollectionOfTaskManager { get; private set; }
        private TaskManager selectedTask;

        private bool isDone;
        private bool isEnableComplete;
        private bool isEnableDelete;
        private bool isShowCompleted;
        private bool isEditMode;
        private string title;
        private string description;
        private TextBlock selectedCategory = new TextBlock();
        private TextBlock selectedPriority = new TextBlock();
        private DateTime selectedDate;

        private bool isShowAddPanel;
        public TaskManagerViewModel(FrameSourceService frameSourceService, InvokerService invokerService, AuthService authService) 
        {
            this.frameSourceService = frameSourceService;
            this.invokerService = invokerService;
            this.authService = authService;

            CollectionOfTaskManager = new ObservableCollection<TaskManager>();

            IsDone = false;
            Title = string.Empty;
            Description = string.Empty;
            selectedDate = DateTime.Now;

            IsShowAddPanel = false;
            IsEnableComplete = false;
            IsEnableDelete = false;
            IsShowCompleted = false;
            isEditMode = false;


            this.invokerService.Receive<LoadNeccessaryData>(this, async (data) => LoadTasks());
        }
        public ICommand GoToHome
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.frameSourceService.ChangeFrame(new Home(this.invokerService));
                });
            }
        }
        public ICommand ShowAddPanel
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    IsShowAddPanel = true;
                });
            }
        }
        public ICommand HideAddPanel
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    IsShowAddPanel = false;
                });
            }
        }  
        public ICommand CompleteTask
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    using (Organaizer_v2Entities db = new Organaizer_v2Entities())
                    {
                        TaskManager taskManager = db.TaskManager.FirstOrDefault(item => item.IdTaskManager == SelectedTask.IdTaskManager);

                        taskManager.IsDone = true;

                        await db.SaveChangesAsync();
                        LoadTasks();
                    }
                });
            }
        }

        public ICommand EditSelectedTask
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    string strinPriority = "";
                    switch (SelectedTask.Priority)
                    {
                        case 3:
                            strinPriority = "Высокий";
                            break;
                        case 2:
                            strinPriority = "Средний";
                            break;
                        case 1:
                            strinPriority = "Низкий";
                            break;
                    }

                    Title = SelectedTask.Title;
                    Description = SelectedTask.Description;
                    selectedDate = SelectedTask.Date.Value;
                    SelectedCategory.Text = SelectedTask.Category;
                    SelectedPriority.Text = strinPriority;

                    isEditMode = true;
                    IsShowAddPanel = true;
                    
                });
            }
        }
        public ICommand SaveTask
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    try
                    {
                        if (Title == "" || Description == "" || SelectedCategory.Text == "" || SelectedPriority.Text == "") { throw new ArgumentException("Заполните все поля"); }
                            int intPriority = 1;
                            switch (selectedPriority.Text)
                            {
                                case "Высокий":
                                    intPriority = 3;
                                    break;
                                case "Средний":
                                    intPriority = 2;
                                    break;
                                case "Низкий":
                                    intPriority = 1;
                                    break;
                            }

                            if (SelectedDate < DateTime.Today)
                            {
                                throw new ArgumentException("Неверная дата");
                            }
                        if (isEditMode)
                        {
                            var editingTask = CollectionOfTaskManager.FirstOrDefault(item => item.IdTaskManager == selectedTask.IdTaskManager);
                            editingTask = await ChangedIntoDB(this.title, this.description, this.selectedCategory.Text, intPriority, selectedDate);
                            isEditMode = false;
                            IsShowAddPanel = false;
                            LoadTasks();

                            OnPropertyChanged("CollectionOfTaskManager");
                        }

                        else
                        {
                            CollectionOfTaskManager.Add(await SaveIntoDB(this.title, this.description, this.selectedCategory.Text, intPriority, selectedDate));
                            LoadTasks();
                        }
                        


                            IsDone = false;
                            Title = string.Empty;
                            Description = string.Empty;
                            selectedDate = DateTime.Now;
                        
                    }
                    catch (ArgumentException e)
                    {
                        var notification = new Notification(e.Message);
                        await this.invokerService.Invoke<MainViewModel>(new InvokeNotification(notification));
                    }

                    catch(Exception e)
                    {

                        Debug.WriteLine(e.Message);
                    }
                });
            }
        }

        private void LoadTasks()
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                try
                {
                    Users user = this.authService.User;
                    var tasks = db.TaskManager.ToList();
                    List<TaskManager> taskList = new List<TaskManager>();


                    for (int i = 0; i < tasks.Count(); i++)
                    {
                        if (tasks[i].Id == user.id)
                        {
                            taskList.Add(tasks[i]);
                        }
                    }
                    CollectionOfTaskManager = new ObservableCollection<TaskManager>(taskList);
                    OnPropertyChanged("CollectionOfTaskManager");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private async Task<TaskManager> SaveIntoDB(string title, string description, string category, int priority, DateTime date)
        {
            int userID = this.authService.User.id;

            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                TaskManager task = new TaskManager()
                {
                    Id = userID,
                    IsDone = false,
                    Title = title,
                    Description = description,
                    Category = category,
                    Priority = priority,
                    Date = date
                };
                db.TaskManager.Add(task);
                await db.SaveChangesAsync();
                return task;
            }
        }

        private async Task<TaskManager> ChangedIntoDB(string title, string description, string category, int priority, DateTime date)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                TaskManager taskManager = db.TaskManager.FirstOrDefault(item => item.IdTaskManager == SelectedTask.IdTaskManager);

                if (taskManager.Title != title)
                {
                    taskManager.Title = title;
                }
                if (taskManager.Description != description)
                {
                    taskManager.Description = description;
                }
                if (taskManager.Category != category)
                {
                    taskManager.Category = category;
                }
                if (taskManager.Priority != priority)
                {
                    taskManager.Priority = priority;
                }
                if (taskManager.Date != date)
                {
                    taskManager.Date = date;
                }
                await db.SaveChangesAsync();
                return taskManager;
            }
        }

        public ICommand DeleteElem
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    var item = SelectedTask as TaskManager;

                    await DeleteIntoDB(item.IdTaskManager);
                    CollectionOfTaskManager.Remove(item);
                    LoadTasks();

                });
            }
        }

        private async Task<TaskManager> DeleteIntoDB(int id)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                TaskManager tasks =db.TaskManager.FirstOrDefault(item => item.IdTaskManager == id);
                db.TaskManager.Remove(tasks);
                await db.SaveChangesAsync();
                return tasks;
            }
        }

        public TaskManager SelectedTask
        {
            get => this.selectedTask;
            set
            {
                this.selectedTask = value as TaskManager;
                IsEnableComplete = this.selectedTask != null && this.selectedTask.IsDone == false;
                IsEnableDelete = this.selectedTask != null;
                OnPropertyChanged();
            }
        }

        public bool IsShowAddPanel
        {
            get => this.isShowAddPanel;
            set
            {
                this.isShowAddPanel = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnableComplete
        {
            get => this.isEnableComplete;
            set
            {
                this.isEnableComplete = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnableDelete
        {
            get => this.isEnableDelete;
            set
            {
                this.isEnableDelete = value;
                OnPropertyChanged();
            }
        }
        public bool IsShowCompleted
        {
            get => this.isShowCompleted;
            set
            {
                SelectedTask = null;
                this.isShowCompleted = value;
                OnPropertyChanged();
            }
        }

        public bool IsDone
        {
            get => this.isDone;
            set
            {
                this.isDone = value;
                OnPropertyChanged();
            }
        }
        public string Title
        {
            get => this.title;
            set
            {
                this.title = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get => this.description;
            set
            {
                this.description = value;
                OnPropertyChanged();
            }
        }
        public DateTime SelectedDate
        {
            get => this.selectedDate;
            set
            {
                this.selectedDate = value;
                OnPropertyChanged();
            }
        }
        public TextBlock SelectedCategory
        {
            get => this.selectedCategory;
            set
            {
                this.selectedCategory = value;
                OnPropertyChanged();
            }
        }
        public TextBlock SelectedPriority
        {
            get => this.selectedPriority;
            set
            {
                this.selectedPriority = value;
                OnPropertyChanged();
            }
        }
    }
}
