//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Organaizer_v2
{
    using System;
    using System.Collections.Generic;
    
    public partial class TaskManager
    {
        public int IdTaskManager { get; set; }
        public Nullable<int> Id { get; set; }
        public Nullable<bool> IsDone { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
