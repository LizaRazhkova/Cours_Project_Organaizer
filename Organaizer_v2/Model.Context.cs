﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Organaizer_v2Entities : DbContext
    {
        public Organaizer_v2Entities()
            : base("name=Organaizer_v2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Gallery> Gallery { get; set; }
        public virtual DbSet<ManagerPass> ManagerPass { get; set; }
        public virtual DbSet<TaskManager> TaskManager { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
