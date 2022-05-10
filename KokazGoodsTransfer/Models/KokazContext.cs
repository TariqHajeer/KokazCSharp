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
        public virtual DbSet<CashMovment> CashMovments { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientPayment> ClientPayments { get; set; }
        public virtual DbSet<ClientPaymentDetail> ClientPaymentDetails { get; set; }
        public virtual DbSet<ClientPhone> ClientPhones { get; set; }
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
        public virtual DbSet<OrderClientPaymnet> OrderClientPaymnets { get; set; }
        public virtual DbSet<OrderFromExcel> OrderFromExcels { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<OrderLog> OrderLogs { get; set; }
        public virtual DbSet<OrderPlaced> OrderPlaceds { get; set; }
        public virtual DbSet<OrderState> OrderStates { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<OutCome> OutComes { get; set; }
        public virtual DbSet<OutComeType> OutComeTypes { get; set; }
        public virtual DbSet<PaymentRequest> PaymentRequests { get; set; }
        public virtual DbSet<PaymentWay> PaymentWays { get; set; }
        public virtual DbSet<PointsSetting> PointsSettings { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<ReceiptOfTheOrderStatus> ReceiptOfTheOrderStatuses { get; set; }
        public virtual DbSet<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Treasury> Treasuries { get; set; }
        public virtual DbSet<TreasuryHistory> TreasuryHistories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserPhone> UserPhones { get; set; }

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
                    .HasName("PK__AgentOrd__CAFCEB4A3800EF29");

                entity.ToTable("AgentOrderPrint");

                entity.HasOne(d => d.AgentPrint)
                    .WithMany(p => p.AgentOrderPrints)
                    .HasForeignKey(d => d.AgentPrintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AgentOrde__Agent__07C12930");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.AgentOrderPrints)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AgentOrde__Order__08B54D69");
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
                    .HasConstraintName("FK__AgentPrin__Agent__09A971A2");
            });

            modelBuilder.Entity<ApproveAgentEditOrderRequest>(entity =>
            {
                entity.ToTable("ApproveAgentEditOrderRequest");

                entity.Property(e => e.NewAmount).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.ApproveAgentEditOrderRequests)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ApproveAg__Agent__0B91BA14");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ApproveAgentEditOrderRequests)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ApproveAg__Order__0C85DE4D");

                entity.HasOne(d => d.OrderPlaced)
                    .WithMany(p => p.ApproveAgentEditOrderRequests)
                    .HasForeignKey(d => d.OrderPlacedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ApproveAg__Order__0D7A0286");
            });

            modelBuilder.Entity<CashMovment>(entity =>
            {
                entity.ToTable("CashMovment");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedOnUtc).HasColumnType("date");

                entity.HasOne(d => d.Treasury)
                    .WithMany(p => p.CashMovments)
                    .HasForeignKey(d => d.TreasuryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CashMovme__Treas__1975C517");
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

            modelBuilder.Entity<ClientPayment>(entity =>
            {
                entity.ToTable("ClientPayment");

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

            modelBuilder.Entity<ClientPaymentDetail>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LastTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PayForClient).HasColumnType("money");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ClientPayment)
                    .WithMany(p => p.ClientPaymentDetails)
                    .HasForeignKey(d => d.ClientPaymentId)
                    .HasConstraintName("FK__ClientPay__Clien__51300E55");

                entity.HasOne(d => d.MoneyPlaced)
                    .WithMany(p => p.ClientPaymentDetails)
                    .HasForeignKey(d => d.MoneyPlacedId)
                    .HasConstraintName("FK__ClientPay__Money__5224328E");

                entity.HasOne(d => d.OrderPlaced)
                    .WithMany(p => p.ClientPaymentDetails)
                    .HasForeignKey(d => d.OrderPlacedId)
                    .HasConstraintName("FK__ClientPay__Order__531856C7");
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
                    .HasConstraintName("FK__DisAccept__Clien__151B244E");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.DisAcceptOrders)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DisAccept__Count__0D7A0286");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.DisAcceptOrders)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK__DisAccept__Regio__17036CC0");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.Money).HasColumnType("money");

                entity.HasOne(d => d.ClientPayment)
                    .WithMany(p => p.Discounts)
                    .HasForeignKey(d => d.ClientPaymentId)
                    .HasConstraintName("FK__Discount__Client__56E8E7AB");
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
                    .HasConstraintName("FK__EditReque__Clien__18EBB532");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EditRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__EditReque__UserI__19DFD96B");
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
                    .HasConstraintName("FK__Market__ClientId__1EA48E88");
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
                    .HasConstraintName("FK__Notficati__Clien__1F98B2C1");

                entity.HasOne(d => d.MoneyPlaced)
                    .WithMany(p => p.Notfications)
                    .HasForeignKey(d => d.MoneyPlacedId)
                    .HasConstraintName("FK__Notficati__Money__208CD6FA");

                entity.HasOne(d => d.OrderPlaced)
                    .WithMany(p => p.Notfications)
                    .HasForeignKey(d => d.OrderPlacedId)
                    .HasConstraintName("FK__Notficati__Order__2180FB33");
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

            modelBuilder.Entity<OrderClientPaymnet>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ClientPaymentId })
                    .HasName("Pk_OrderClientPaymnet");

                entity.ToTable("OrderClientPaymnet");

                entity.HasOne(d => d.ClientPayment)
                    .WithMany(p => p.OrderClientPaymnets)
                    .HasForeignKey(d => d.ClientPaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderClie__Clien__55F4C372");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderClientPaymnets)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderClie__Order__55009F39");
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
                    .HasConstraintName("FK__OrderFrom__Clien__2A164134");
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

                entity.HasIndex(e => e.Points, "UQ__PointsSe__DA826786C9B4659A")
                    .IsUnique();

                entity.HasIndex(e => e.Money, "UQ__PointsSe__FA951B46C519FCD7")
                    .IsUnique();

                entity.Property(e => e.Money).HasColumnType("money");
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

                entity.HasOne(d => d.ClientPayment)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.ClientPaymentId)
                    .HasConstraintName("FK__Receipt__ClientP__57DD0BE4");
            });

            modelBuilder.Entity<ReceiptOfTheOrderStatus>(entity =>
            {
                entity.ToTable("ReceiptOfTheOrderStatus");

                entity.Property(e => e.CreatedOn).HasColumnType("date");
            });

            modelBuilder.Entity<ReceiptOfTheOrderStatusDetali>(entity =>
            {
                entity.Property(e => e.AgentCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrderCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.ReceiptOfTheOrderStatusDetalis)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceiptOf__Agent__23F3538A");

                entity.HasOne(d => d.MoneyPlaced)
                    .WithMany(p => p.ReceiptOfTheOrderStatusDetalis)
                    .HasForeignKey(d => d.MoneyPlacedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceiptOf__Money__25DB9BFC");

                entity.HasOne(d => d.OrderState)
                    .WithMany(p => p.ReceiptOfTheOrderStatusDetalis)
                    .HasForeignKey(d => d.OrderStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceiptOf__Order__24E777C3");

                entity.HasOne(d => d.ReceiptOfTheOrderStatus)
                    .WithMany(p => p.ReceiptOfTheOrderStatusDetalis)
                    .HasForeignKey(d => d.ReceiptOfTheOrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceiptOf__Recei__26CFC035");
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

            modelBuilder.Entity<Treasury>(entity =>
            {
                entity.ToTable("Treasury");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateOnUtc).HasColumnType("date");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Treasury)
                    .HasForeignKey<Treasury>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Treasury__Id__1699586C");
            });

            modelBuilder.Entity<TreasuryHistory>(entity =>
            {
                entity.ToTable("TreasuryHistory");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedOnUtc).HasColumnType("date");

                entity.HasOne(d => d.CashMovment)
                    .WithMany(p => p.TreasuryHistories)
                    .HasForeignKey(d => d.CashMovmentId)
                    .HasConstraintName("FK__TreasuryH__CashM__1E3A7A34");

                entity.HasOne(d => d.ClientPayment)
                    .WithMany(p => p.TreasuryHistories)
                    .HasForeignKey(d => d.ClientPaymentId)
                    .HasConstraintName("FK__TreasuryH__Clien__1D4655FB");

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.TreasuryHistories)
                    .HasForeignKey(d => d.ReceiptId)
                    .HasConstraintName("FK__TreasuryH__Recei__1F2E9E6D");

                entity.HasOne(d => d.ReceiptOfTheOrderStatus)
                    .WithMany(p => p.TreasuryHistories)
                    .HasForeignKey(d => d.ReceiptOfTheOrderStatusId)
                    .HasConstraintName("FK__TreasuryH__Recei__27C3E46E");

                entity.HasOne(d => d.Treasury)
                    .WithMany(p => p.TreasuryHistories)
                    .HasForeignKey(d => d.TreasuryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TreasuryH__Treas__1C5231C2");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
