using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class KokazContext : DbContext
    {
        public KokazContext()
        {
        }

        public KokazContext(DbContextOptions<KokazContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AgentCountr> AgentCountrs { get; set; }
        public virtual DbSet<AgentOrderPrint> AgentOrderPrints { get; set; }
        public virtual DbSet<AgentPrint> AgentPrints { get; set; }
        public virtual DbSet<AgentPrintDetail> AgentPrintDetails { get; set; }
        public virtual DbSet<ApproveAgentEditOrderRequest> ApproveAgentEditOrderRequests { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientPhone> ClientPhones { get; set; }
        public virtual DbSet<ClientPrint> ClientPrints { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DisAcceptOrder> DisAcceptOrders { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<EditRequest> EditRequests { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupPrivilege> GroupPrivileges { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<IncomeType> IncomeTypes { get; set; }
        public virtual DbSet<Market> Markets { get; set; }
        public virtual DbSet<MoenyPlaced> MoenyPlaceds { get; set; }
        public virtual DbSet<Notfication> Notfications { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderFromExcel> OrderFromExcels { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<OrderLog> OrderLogs { get; set; }
        public virtual DbSet<OrderPlaced> OrderPlaceds { get; set; }
        public virtual DbSet<OrderPrint> OrderPrints { get; set; }
        public virtual DbSet<OrderState> OrderStates { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<OutCome> OutComes { get; set; }
        public virtual DbSet<OutComeType> OutComeTypes { get; set; }
        public virtual DbSet<PaymentRequest> PaymentRequests { get; set; }
        public virtual DbSet<PaymentWay> PaymentWays { get; set; }
        public virtual DbSet<PointsSetting> PointsSettings { get; set; }
        public virtual DbSet<Printed> Printeds { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserPhone> UserPhones { get; set; }
        public virtual DbSet<VOrderClientPrnitRepeate> VOrderClientPrnitRepeates { get; set; }
        public virtual DbSet<VOrderclientPrintReportWithOrderDeital> VOrderclientPrintReportWithOrderDeitals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=Kokaz;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AgentCountr>(entity =>
            {
                entity.HasKey(e => new { e.AgentId, e.CountryId });

                entity.ToTable("AgentCountr");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.AgentCountrs)
                    .HasForeignKey(d => d.AgentId)
                    .HasConstraintName("FK_AgentCountr_Users");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.AgentCountrs)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_AgentCountr_Country");
            });

            modelBuilder.Entity<AgentOrderPrint>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.AgentPrintId })
                    .HasName("PK__AgentOrd__CAFCEB4A1121FD0D");

                entity.ToTable("AgentOrderPrint");

                entity.HasOne(d => d.AgentPrint)
                    .WithMany(p => p.AgentOrderPrints)
                    .HasForeignKey(d => d.AgentPrintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AgentOrde__Agent__336AA144");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.AgentOrderPrints)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AgentOrde__Order__345EC57D");
            });

            modelBuilder.Entity<AgentPrint>(entity =>
            {
                entity.ToTable("AgentPrint");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DestinationName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DestinationPhone)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.PrinterName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AgentPrintDetail>(entity =>
            {
                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.AgentPrint)
                    .WithMany(p => p.AgentPrintDetails)
                    .HasForeignKey(d => d.AgentPrintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AgentPrin__Agent__308E3499");
            });

            modelBuilder.Entity<ApproveAgentEditOrderRequest>(entity =>
            {
                entity.ToTable("ApproveAgentEditOrderRequest");

                entity.Property(e => e.NewAmount).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.ApproveAgentEditOrderRequests)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ApproveAg__Agent__02FC7413");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ApproveAgentEditOrderRequests)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ApproveAg__Order__03F0984C");

                entity.HasOne(d => d.OrderPlaced)
                    .WithMany(p => p.ApproveAgentEditOrderRequests)
                    .HasForeignKey(d => d.OrderPlacedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ApproveAg__Order__04E4BC85");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.FirstDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Clients_Country");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clients_Users");
            });

            modelBuilder.Entity<ClientPhone>(entity =>
            {
                entity.ToTable("clientPhones");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientPhones)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_clientPhones_Clients");
            });

            modelBuilder.Entity<ClientPrint>(entity =>
            {
                entity.ToTable("ClientPrint");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LastTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PayForClient).HasColumnType("money");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.MoneyPlaced)
                    .WithMany(p => p.ClientPrints)
                    .HasForeignKey(d => d.MoneyPlacedId)
                    .HasConstraintName("FK__ClientPri__Money__06CD04F7");

                entity.HasOne(d => d.OrderPlaced)
                    .WithMany(p => p.ClientPrints)
                    .HasForeignKey(d => d.OrderPlacedId)
                    .HasConstraintName("FK__ClientPri__Order__07C12930");

                entity.HasOne(d => d.Print)
                    .WithMany(p => p.ClientPrints)
                    .HasForeignKey(d => d.PrintId)
                    .HasConstraintName("FK_ClientPrint_Printed");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.DeliveryCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MediatorId).HasColumnName("mediatorId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Mediator)
                    .WithMany(p => p.InverseMediator)
                    .HasForeignKey(d => d.MediatorId)
                    .HasConstraintName("FK__Country__mediato__74794A92");
            });

            modelBuilder.Entity<DisAcceptOrder>(entity =>
            {
                entity.ToTable("DisAcceptOrder");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RecipientName).HasMaxLength(50);

                entity.Property(e => e.RecipientPhones).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.DisAcceptOrders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DisAccept__Clien__0C85DE4D");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.DisAcceptOrders)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DisAccept__Count__0D7A0286");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.DisAcceptOrders)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK__DisAccept__Regio__0E6E26BF");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.Money).HasColumnType("money");

                entity.HasOne(d => d.Printed)
                    .WithMany(p => p.Discounts)
                    .HasForeignKey(d => d.PrintedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Discount_Printed");
            });

            modelBuilder.Entity<EditRequest>(entity =>
            {
                entity.ToTable("EditRequest");

                entity.Property(e => e.NewName).HasMaxLength(50);

                entity.Property(e => e.NewUserName).HasMaxLength(50);

                entity.Property(e => e.OldName).HasMaxLength(50);

                entity.Property(e => e.OldUserName).HasMaxLength(50);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.EditRequests)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EditReque__Clien__10566F31");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EditRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__EditReque__UserI__114A936A");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<GroupPrivilege>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.PrivilegId });

                entity.ToTable("GroupPrivilege");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPrivileges)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupPrivilege_Group");

                entity.HasOne(d => d.Privileg)
                    .WithMany(p => p.GroupPrivileges)
                    .HasForeignKey(d => d.PrivilegId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupPrivilege_Privilege");
            });

            modelBuilder.Entity<Income>(entity =>
            {
                entity.ToTable("Income");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Earining).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.IncomeType)
                    .WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.IncomeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Income_IncomeType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Income_Users");
            });

            modelBuilder.Entity<IncomeType>(entity =>
            {
                entity.ToTable("IncomeType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Market>(entity =>
            {
                entity.ToTable("Market");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Markets)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__Market__ClientId__160F4887");
            });

            modelBuilder.Entity<MoenyPlaced>(entity =>
            {
                entity.ToTable("MoenyPlaced");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Notfication>(entity =>
            {
                entity.ToTable("Notfication");

                entity.Property(e => e.IsSeen).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Notfications)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notficati__Clien__17036CC0");

                entity.HasOne(d => d.MoneyPlaced)
                    .WithMany(p => p.Notfications)
                    .HasForeignKey(d => d.MoneyPlacedId)
                    .HasConstraintName("FK__Notficati__Money__17F790F9");

                entity.HasOne(d => d.OrderPlaced)
                    .WithMany(p => p.Notfications)
                    .HasForeignKey(d => d.OrderPlacedId)
                    .HasConstraintName("FK__Notficati__Order__18EBB532");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.AgentCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ClientPaied).HasColumnType("money");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DeliveryCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiliveryDate).HasColumnType("date");

                entity.Property(e => e.OldCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OldDeliveryCost).HasColumnType("money");

                entity.Property(e => e.RecipientName).HasMaxLength(50);

                entity.Property(e => e.RecipientPhones).IsRequired();

                entity.Property(e => e.Seen).HasColumnName("seen");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AgentId)
                    .HasConstraintName("FK_Order_Users");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Clients");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.OrderCountries)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Country");

                entity.HasOne(d => d.CurrentCountryNavigation)
                    .WithMany(p => p.OrderCurrentCountryNavigations)
                    .HasForeignKey(d => d.CurrentCountry)
                    .HasConstraintName("FK__Order__CurrentCo__078C1F06");

                entity.HasOne(d => d.MoenyPlaced)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MoenyPlacedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_MoenyPlaced");

                entity.HasOne(d => d.OrderState)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_OrderState");

                entity.HasOne(d => d.Orderplaced)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderplacedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_OrderPlaced");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_Order_Region");
            });

            modelBuilder.Entity<OrderFromExcel>(entity =>
            {
                entity.ToTable("OrderFromExcel");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.RecipientName).HasMaxLength(50);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.OrderFromExcels)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderFrom__Clien__22401542");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.OrderTpyeId })
                    .HasName("PK_OrderOrderType");

                entity.ToTable("OrderItem");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderOrderType_Order");

                entity.HasOne(d => d.OrderTpye)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderTpyeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderOrderType_OrderType");
            });

            modelBuilder.Entity<OrderLog>(entity =>
            {
                entity.ToTable("OrderLog");

                entity.Property(e => e.AgentCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiliveryDate).HasColumnType("date");

                entity.Property(e => e.OldCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RecipientName).HasMaxLength(50);

                entity.Property(e => e.RecipientPhones).IsRequired();

                entity.Property(e => e.Seen).HasColumnName("seen");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.OrderLogs)
                    .HasForeignKey(d => d.AgentId)
                    .HasConstraintName("FK_OrderLog_Users");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.OrderLogs)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLog_Clients");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.OrderLogs)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLog_Country");

                entity.HasOne(d => d.MoenyPlaced)
                    .WithMany(p => p.OrderLogs)
                    .HasForeignKey(d => d.MoenyPlacedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLog_MoenyPlaced");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLogs)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderLog_Order");

                entity.HasOne(d => d.Orderplaced)
                    .WithMany(p => p.OrderLogs)
                    .HasForeignKey(d => d.OrderplacedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLog_OrderPlaced");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.OrderLogs)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_OrderLog_Region");
            });

            modelBuilder.Entity<OrderPlaced>(entity =>
            {
                entity.ToTable("OrderPlaced");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OrderPrint>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.PrintId });

                entity.ToTable("OrderPrint");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderPrints)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderPrint_Order");

                entity.HasOne(d => d.Print)
                    .WithMany(p => p.OrderPrints)
                    .HasForeignKey(d => d.PrintId)
                    .HasConstraintName("FK_OrderPrint_Printed1");
            });

            modelBuilder.Entity<OrderState>(entity =>
            {
                entity.ToTable("OrderState");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.State).IsRequired();
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.ToTable("OrderType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OutCome>(entity =>
            {
                entity.ToTable("OutCome");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.OutComeType)
                    .WithMany(p => p.OutComes)
                    .HasForeignKey(d => d.OutComeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutCome_OutComeType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OutComes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutCome_Users");
            });

            modelBuilder.Entity<OutComeType>(entity =>
            {
                entity.ToTable("OutComeType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentRequest>(entity =>
            {
                entity.ToTable("PaymentRequest");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.PaymentRequests)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaymentRe__Clien__5AB9788F");

                entity.HasOne(d => d.PaymentWay)
                    .WithMany(p => p.PaymentRequests)
                    .HasForeignKey(d => d.PaymentWayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PaymentRe__Payme__5BAD9CC8");
            });

            modelBuilder.Entity<PaymentWay>(entity =>
            {
                entity.ToTable("PaymentWay");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<PointsSetting>(entity =>
            {
                entity.ToTable("PointsSetting");

                entity.HasIndex(e => e.Points, "UQ__PointsSe__DA826786AAEFC670")
                    .IsUnique();

                entity.HasIndex(e => e.Money, "UQ__PointsSe__FA951B46B4297ACE")
                    .IsUnique();

                entity.Property(e => e.Money).HasColumnType("money");
            });

            modelBuilder.Entity<Printed>(entity =>
            {
                entity.ToTable("Printed");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DestinationName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DestinationPhone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PrinterName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Privilege>(entity =>
            {
                entity.ToTable("Privilege");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SysName).HasMaxLength(50);
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Manager).IsRequired();

                entity.Property(e => e.Note).IsRequired();

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Receipt_Clients");

                entity.HasOne(d => d.Print)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.PrintId)
                    .HasConstraintName("FK_Receipt_Printed");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Regions)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Reginos_Country");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GroupId });

                entity.ToTable("UserGroup");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_UserGroup_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserGroup_Users");
            });

            modelBuilder.Entity<UserPhone>(entity =>
            {
                entity.ToTable("UserPhone");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPhones)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserPhone_Users");
            });

            modelBuilder.Entity<VOrderClientPrnitRepeate>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_OrderClientPrnitRepeate");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ocount).HasColumnName("OCount");
            });

            modelBuilder.Entity<VOrderclientPrintReportWithOrderDeital>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_OrderclientPrintReportWithOrderDeitals");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DeliveryCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Ocount).HasColumnName("OCount");

                entity.Property(e => e.OldCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OldDeliveryCost).HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
