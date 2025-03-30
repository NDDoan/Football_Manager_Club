using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FootballManagerWPF.Models;

public partial class FootballBusinessManagerClubsSqlContext : DbContext
{
    public FootballBusinessManagerClubsSqlContext()
    {
    }

    public FootballBusinessManagerClubsSqlContext(DbContextOptions<FootballBusinessManagerClubsSqlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<BackupHistory> BackupHistories { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-HVER5I34\\DOAN;Initial Catalog=FootballBusinessManagerClubsSQL; Trusted_Connection=SSPI;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Activity__9E2397E0755A519B");

            entity.ToTable("ActivityLog");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.Activity)
                .HasMaxLength(255)
                .HasColumnName("activity");
            entity.Property(e => e.ActivityTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("activity_timestamp");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.RecordId).HasColumnName("record_id");
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .HasColumnName("table_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ActivityLog_Users");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__20D6A968A3BCDE44");

            entity.ToTable("Attendance");

            entity.HasIndex(e => new { e.EmployeeId, e.AttendanceDate }, "UQ_Attendance_Employee_Date").IsUnique();

            entity.Property(e => e.AttendanceId).HasColumnName("attendance_id");
            entity.Property(e => e.AttendanceDate).HasColumnName("attendance_date");
            entity.Property(e => e.CheckIn)
                .HasColumnType("datetime")
                .HasColumnName("check_in");
            entity.Property(e => e.CheckOut)
                .HasColumnType("datetime")
                .HasColumnName("check_out");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.LeaveType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("None")
                .HasColumnName("leave_type");
            entity.Property(e => e.OvertimeHours)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("overtime_hours");

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Attendance_Employee");
        });

        modelBuilder.Entity<BackupHistory>(entity =>
        {
            entity.HasKey(e => e.BackupId).HasName("PK__BackupHi__AE70C88056A5CE09");

            entity.ToTable("BackupHistory");

            entity.Property(e => e.BackupId).HasColumnName("backup_id");
            entity.Property(e => e.BackupDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("backup_date");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BackupHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_BackupHistory_Users");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__C2232422977ACB43");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C52E0BA8F5634852");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.DepartmentId, "IX_Employee_Department");

            entity.HasIndex(e => e.FullName, "IX_Employee_FullName");

            entity.HasIndex(e => e.Position, "IX_Employee_Position");

            entity.HasIndex(e => e.StartDate, "IX_Employee_StartDate");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.AnnualLeaveBalance).HasColumnName("annual_leave_balance");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.BaseSalary)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("base_salary");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasColumnName("position");
            entity.Property(e => e.SickLeaveBalance).HasColumnName("sick_leave_balance");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UnpaidLeaveBalance).HasColumnName("unpaid_leave_balance");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Employee_Department");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Employee_Users");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__LeaveReq__743350BCCE8335D9");

            entity.ToTable("LeaveRequest");

            entity.Property(e => e.LeaveId).HasColumnName("leave_id");
            entity.Property(e => e.ApprovedAt)
                .HasColumnType("datetime")
                .HasColumnName("approved_at");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.LeaveType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("leave_type");
            entity.Property(e => e.RequestedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("requested_at");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.TotalDays)
                .HasComputedColumnSql("(datediff(day,[start_date],[end_date])+(1))", true)
                .HasColumnName("total_days");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.ApprovedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_LeaveRequest_ApprovedBy");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_LeaveRequest_Employee");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842FE2B2A3F2");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.SenderUserId).HasColumnName("sender_user_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Department).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Notification_Department");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.SenderUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Notification_Sender");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Payroll__D99FC9443656B09D");

            entity.ToTable("Payroll");

            entity.Property(e => e.PayrollId).HasColumnName("payroll_id");
            entity.Property(e => e.Allowance)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("allowance");
            entity.Property(e => e.BaseSalary)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("base_salary");
            entity.Property(e => e.Bonus)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("bonus");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.PayrollDate).HasColumnName("payroll_date");
            entity.Property(e => e.Penalty)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("penalty");
            entity.Property(e => e.TotalIncome)
                .HasComputedColumnSql("((([base_salary]+[allowance])+[bonus])-[penalty])", true)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_income");

            entity.HasOne(d => d.Employee).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Payroll_Employee");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FDC146BA7");

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC5724CEB2A9B").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__UserSess__69B13FDCF1D3B7D5");

            entity.ToTable("UserSession");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_updated");
            entity.Property(e => e.SessionData).HasColumnName("session_data");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserSession_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
