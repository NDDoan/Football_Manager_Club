CREATE DATABASE FootballBusinessManagerClubsSQL
GO
USE [FootballBusinessManagerClubsSQL]
GO
/****** Object:  Table [dbo].[ActivityLog]    Script Date: 3/30/2025 8:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityLog](
	[log_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[activity] [nvarchar](255) NOT NULL,
	[table_name] [nvarchar](100) NULL,
	[record_id] [int] NULL,
	[details] [nvarchar](max) NULL,
	[activity_timestamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[attendance_id] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[attendance_date] [date] NOT NULL,
	[check_in] [datetime] NULL,
	[check_out] [datetime] NULL,
	[overtime_hours] [decimal](5, 2) NOT NULL,
	[leave_type] [varchar](10) NOT NULL,
	[created_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[attendance_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BackupHistory]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BackupHistory](
	[backup_id] [int] IDENTITY(1,1) NOT NULL,
	[backup_date] [datetime] NOT NULL,
	[file_name] [nvarchar](255) NOT NULL,
	[created_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[backup_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[department_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
	[created_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[department_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[employee_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[full_name] [nvarchar](100) NOT NULL,
	[date_of_birth] [date] NOT NULL,
	[gender] [varchar](10) NOT NULL,
	[address] [nvarchar](255) NULL,
	[phone] [nvarchar](20) NULL,
	[department_id] [int] NULL,
	[position] [nvarchar](100) NULL,
	[base_salary] [decimal](15, 2) NULL,
	[start_date] [date] NULL,
	[avatar] [nvarchar](255) NULL,
	[annual_leave_balance] [int] NOT NULL,
	[sick_leave_balance] [int] NOT NULL,
	[unpaid_leave_balance] [int] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[employee_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveRequest]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveRequest](
	[leave_id] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[leave_type] [varchar](10) NOT NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NOT NULL,
	[total_days]  AS (datediff(day,[start_date],[end_date])+(1)) PERSISTED,
	[status] [varchar](10) NOT NULL,
	[requested_at] [datetime] NOT NULL,
	[approved_by] [int] NULL,
	[approved_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[leave_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[notification_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[content] [nvarchar](max) NOT NULL,
	[department_id] [int] NULL,
	[sender_user_id] [int] NULL,
	[created_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[notification_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payroll]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payroll](
	[payroll_id] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[base_salary] [decimal](15, 2) NOT NULL,
	[allowance] [decimal](15, 2) NOT NULL,
	[bonus] [decimal](15, 2) NOT NULL,
	[penalty] [decimal](15, 2) NOT NULL,
	[total_income]  AS ((([base_salary]+[allowance])+[bonus])-[penalty]) PERSISTED,
	[payroll_date] [date] NOT NULL,
	[created_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[payroll_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[password] [varchar](255) NOT NULL,
	[role] [varchar](10) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSession]    Script Date: 3/30/2025 8:27:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSession](
	[session_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[session_data] [nvarchar](max) NULL,
	[last_updated] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[session_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ActivityLog] ON 
GO
INSERT [dbo].[ActivityLog] ([log_id], [user_id], [activity], [table_name], [record_id], [details], [activity_timestamp]) VALUES (1, 1, N'INSERT', N'Employee', 2, N'Added new employee John Doe.', CAST(N'2025-03-15T13:16:51.417' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[ActivityLog] OFF
GO
SET IDENTITY_INSERT [dbo].[Attendance] ON 
GO
INSERT [dbo].[Attendance] ([attendance_id], [employee_id], [attendance_date], [check_in], [check_out], [overtime_hours], [leave_type], [created_at]) VALUES (1, 2, CAST(N'2023-09-01' AS Date), CAST(N'2023-09-01T08:00:00.000' AS DateTime), CAST(N'2023-09-01T17:00:00.000' AS DateTime), CAST(1.50 AS Decimal(5, 2)), N'None', CAST(N'2025-03-15T13:16:51.407' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Attendance] OFF
GO
SET IDENTITY_INSERT [dbo].[BackupHistory] ON 
GO
INSERT [dbo].[BackupHistory] ([backup_id], [backup_date], [file_name], [created_by]) VALUES (1, CAST(N'2025-03-15T13:16:51.420' AS DateTime), N'Backup_20230901.bak', 1)
GO
SET IDENTITY_INSERT [dbo].[BackupHistory] OFF
GO
SET IDENTITY_INSERT [dbo].[Department] ON 
GO
INSERT [dbo].[Department] ([department_id], [name], [description], [created_at]) VALUES (1, N'Human Resources', N'Handles recruitment, employee relations.', CAST(N'2025-03-15T13:16:51.363' AS DateTime))
GO
INSERT [dbo].[Department] ([department_id], [name], [description], [created_at]) VALUES (2, N'Finance', N'Manages company finances.', CAST(N'2025-03-15T13:16:51.363' AS DateTime))
GO
INSERT [dbo].[Department] ([department_id], [name], [description], [created_at]) VALUES (3, N'IT', N'Handles information technology.', CAST(N'2025-03-15T13:16:51.363' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Department] OFF
GO
SET IDENTITY_INSERT [dbo].[Employee] ON 
GO
INSERT [dbo].[Employee] ([employee_id], [user_id], [full_name], [date_of_birth], [gender], [address], [phone], [department_id], [position], [base_salary], [start_date], [avatar], [annual_leave_balance], [sick_leave_balance], [unpaid_leave_balance], [created_at], [updated_at]) VALUES (1, 1, N'Admin User', CAST(N'1980-01-01' AS Date), N'Other', N'123 Admin St', N'1234567890', 1, N'Administrator', CAST(10000.00 AS Decimal(15, 2)), CAST(N'2020-01-01' AS Date), N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQS9WDX7JlmoXx1-KXqPeJAwiS0xWGDmjBEWw&s', 15, 10, 5, CAST(N'2025-03-15T13:16:51.380' AS DateTime), CAST(N'2025-03-15T13:16:51.380' AS DateTime))
GO
INSERT [dbo].[Employee] ([employee_id], [user_id], [full_name], [date_of_birth], [gender], [address], [phone], [department_id], [position], [base_salary], [start_date], [avatar], [annual_leave_balance], [sick_leave_balance], [unpaid_leave_balance], [created_at], [updated_at]) VALUES (2, 2, N'John Doe', CAST(N'1990-05-15' AS Date), N'Male', N'456 Main St', N'0987654321', 2, N'Accountant', CAST(5000.00 AS Decimal(15, 2)), CAST(N'2021-03-10' AS Date), N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSNtLnYEqvhKKHET_JzfYOv5hZNV1cngGuY_A&s', 12, 8, 3, CAST(N'2025-03-15T13:16:51.380' AS DateTime), CAST(N'2025-03-28T06:38:24.563' AS DateTime))
GO
INSERT [dbo].[Employee] ([employee_id], [user_id], [full_name], [date_of_birth], [gender], [address], [phone], [department_id], [position], [base_salary], [start_date], [avatar], [annual_leave_balance], [sick_leave_balance], [unpaid_leave_balance], [created_at], [updated_at]) VALUES (3, 3, N'Jane Smith', CAST(N'1985-07-20' AS Date), N'Female', N'789 Side St', N'1122334455', 3, N'IT Specialist', CAST(6000.00 AS Decimal(15, 2)), CAST(N'2022-06-01' AS Date), N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRNmStyyWkFVQR89gLDqlG83jrDVFKkd5HW2Q&s', 14, 9, 4, CAST(N'2025-03-15T13:16:51.380' AS DateTime), CAST(N'2025-03-28T06:38:24.627' AS DateTime))
GO
INSERT [dbo].[Employee] ([employee_id], [user_id], [full_name], [date_of_birth], [gender], [address], [phone], [department_id], [position], [base_salary], [start_date], [avatar], [annual_leave_balance], [sick_leave_balance], [unpaid_leave_balance], [created_at], [updated_at]) VALUES (4, 4, N'Alice Wong', CAST(N'1992-11-30' AS Date), N'Female', N'321 West Ave', N'2233445566', 1, N'HR Manager', CAST(7000.00 AS Decimal(15, 2)), CAST(N'2020-10-15' AS Date), N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRpwWpJryiim85B_jnggr85kd_gJljhwqV3Tw&s', 16, 10, 5, CAST(N'2025-03-15T13:16:51.380' AS DateTime), CAST(N'2025-03-28T06:38:24.657' AS DateTime))
GO
INSERT [dbo].[Employee] ([employee_id], [user_id], [full_name], [date_of_birth], [gender], [address], [phone], [department_id], [position], [base_salary], [start_date], [avatar], [annual_leave_balance], [sick_leave_balance], [unpaid_leave_balance], [created_at], [updated_at]) VALUES (5, 5, N'Bob Lee', CAST(N'1988-09-12' AS Date), N'Male', N'654 East Rd', N'3344556678', 2, N'Finance Analyst', CAST(5500.00 AS Decimal(15, 2)), CAST(N'2021-05-20' AS Date), N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTQezCK15d0cgAULPojc6JFsTu_bgzjpTbr7g&s', 13, 7, 3, CAST(N'2025-03-15T13:16:51.380' AS DateTime), CAST(N'2025-03-28T06:38:24.677' AS DateTime))
GO
INSERT [dbo].[Employee] ([employee_id], [user_id], [full_name], [date_of_birth], [gender], [address], [phone], [department_id], [position], [base_salary], [start_date], [avatar], [annual_leave_balance], [sick_leave_balance], [unpaid_leave_balance], [created_at], [updated_at]) VALUES (6, 6, N'Son Goku', CAST(N'2005-03-01' AS Date), N'Male', N'West Capital', N'0789456129', 1, N'HR Manager', NULL, CAST(N'2025-03-24' AS Date), N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTX9xyWrmTjqFbuxWYGWNLz6FOm2oJ4OiA1yA&s', 0, 0, 0, CAST(N'2025-03-25T06:53:55.017' AS DateTime), CAST(N'2025-03-28T22:57:10.050' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Employee] OFF
GO
SET IDENTITY_INSERT [dbo].[LeaveRequest] ON 
GO
INSERT [dbo].[LeaveRequest] ([leave_id], [employee_id], [leave_type], [start_date], [end_date], [status], [requested_at], [approved_by], [approved_at]) VALUES (1, 2, N'Annual', CAST(N'2023-09-10' AS Date), CAST(N'2023-09-15' AS Date), N'Approved', CAST(N'2025-03-15T13:16:51.427' AS DateTime), 1, CAST(N'2025-03-28T08:13:35.850' AS DateTime))
GO
INSERT [dbo].[LeaveRequest] ([leave_id], [employee_id], [leave_type], [start_date], [end_date], [status], [requested_at], [approved_by], [approved_at]) VALUES (2, 2, N'Sick', CAST(N'2025-03-21' AS Date), CAST(N'2025-03-31' AS Date), N'Approved', CAST(N'2025-03-20T23:03:42.213' AS DateTime), 1, CAST(N'2025-03-21T07:27:55.940' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[LeaveRequest] OFF
GO
SET IDENTITY_INSERT [dbo].[Notification] ON 
GO
INSERT [dbo].[Notification] ([notification_id], [title], [content], [department_id], [sender_user_id], [created_at]) VALUES (1, N'Monthly Meeting', N'There will be a monthly meeting on 2023-09-05 at 10:00 AM.', 1, 1, CAST(N'2025-03-15T13:16:51.410' AS DateTime))
GO
INSERT [dbo].[Notification] ([notification_id], [title], [content], [department_id], [sender_user_id], [created_at]) VALUES (2, N'System Maintenance', N'The system will undergo maintenance on 2023-09-02 from 00:00 to 04:00 AM.', NULL, 1, CAST(N'2025-03-15T13:16:51.410' AS DateTime))
GO
INSERT [dbo].[Notification] ([notification_id], [title], [content], [department_id], [sender_user_id], [created_at]) VALUES (3, N'Player transfer', N'There is player meeting start from 2025-03-30', NULL, NULL, CAST(N'2025-03-19T07:32:09.073' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Notification] OFF
GO
SET IDENTITY_INSERT [dbo].[Payroll] ON 
GO
INSERT [dbo].[Payroll] ([payroll_id], [employee_id], [base_salary], [allowance], [bonus], [penalty], [payroll_date], [created_at]) VALUES (1, 2, CAST(5000.00 AS Decimal(15, 2)), CAST(500.00 AS Decimal(15, 2)), CAST(200.00 AS Decimal(15, 2)), CAST(0.00 AS Decimal(15, 2)), CAST(N'2023-08-31' AS Date), CAST(N'2025-03-15T13:16:51.390' AS DateTime))
GO
INSERT [dbo].[Payroll] ([payroll_id], [employee_id], [base_salary], [allowance], [bonus], [penalty], [payroll_date], [created_at]) VALUES (2, 3, CAST(6000.00 AS Decimal(15, 2)), CAST(600.00 AS Decimal(15, 2)), CAST(300.00 AS Decimal(15, 2)), CAST(50.00 AS Decimal(15, 2)), CAST(N'2023-08-31' AS Date), CAST(N'2025-03-15T13:16:51.390' AS DateTime))
GO
INSERT [dbo].[Payroll] ([payroll_id], [employee_id], [base_salary], [allowance], [bonus], [penalty], [payroll_date], [created_at]) VALUES (3, 5, CAST(5500.00 AS Decimal(15, 2)), CAST(550.00 AS Decimal(15, 2)), CAST(250.00 AS Decimal(15, 2)), CAST(0.00 AS Decimal(15, 2)), CAST(N'2023-08-31' AS Date), CAST(N'2025-03-15T13:16:51.390' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Payroll] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([user_id], [username], [password], [role], [created_at], [updated_at]) VALUES (1, N'admin', N'hashedpassword1', N'Admin', CAST(N'2025-03-15T13:16:51.343' AS DateTime), CAST(N'2025-03-15T13:16:51.343' AS DateTime))
GO
INSERT [dbo].[Users] ([user_id], [username], [password], [role], [created_at], [updated_at]) VALUES (2, N'john.doe', N'hashedpassword2', N'Employee', CAST(N'2025-03-15T13:16:51.343' AS DateTime), CAST(N'2025-03-28T06:38:24.523' AS DateTime))
GO
INSERT [dbo].[Users] ([user_id], [username], [password], [role], [created_at], [updated_at]) VALUES (3, N'jane.smith', N'hashedpassword3', N'Employee', CAST(N'2025-03-15T13:16:51.343' AS DateTime), CAST(N'2025-03-28T06:38:24.617' AS DateTime))
GO
INSERT [dbo].[Users] ([user_id], [username], [password], [role], [created_at], [updated_at]) VALUES (4, N'alice.wong', N'hashedpassword4', N'Employee', CAST(N'2025-03-15T13:16:51.343' AS DateTime), CAST(N'2025-03-28T06:38:24.640' AS DateTime))
GO
INSERT [dbo].[Users] ([user_id], [username], [password], [role], [created_at], [updated_at]) VALUES (5, N'bob.lee', N'hashedemployee5', N'Employee', CAST(N'2025-03-15T13:16:51.343' AS DateTime), CAST(N'2025-03-28T06:38:24.667' AS DateTime))
GO
INSERT [dbo].[Users] ([user_id], [username], [password], [role], [created_at], [updated_at]) VALUES (6, N'songoku', N'kakalot', N'Employee', CAST(N'2025-03-25T06:53:54.997' AS DateTime), CAST(N'2025-03-28T22:57:10.020' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[UserSession] ON 
GO
INSERT [dbo].[UserSession] ([session_id], [user_id], [session_data], [last_updated]) VALUES (1, 2, N'{"lastVisitedPage": "EmployeeList", "filters": {"department": "Finance"}}', CAST(N'2025-03-15T13:16:51.423' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[UserSession] OFF
GO
/****** Object:  Index [UQ_Attendance_Employee_Date]    Script Date: 3/30/2025 8:27:59 PM ******/
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [UQ_Attendance_Employee_Date] UNIQUE NONCLUSTERED 
(
	[employee_id] ASC,
	[attendance_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__F3DBC5724CEB2A9B]    Script Date: 3/30/2025 8:27:59 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActivityLog] ADD  DEFAULT (getdate()) FOR [activity_timestamp]
GO
ALTER TABLE [dbo].[Attendance] ADD  DEFAULT ((0)) FOR [overtime_hours]
GO
ALTER TABLE [dbo].[Attendance] ADD  DEFAULT ('None') FOR [leave_type]
GO
ALTER TABLE [dbo].[Attendance] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[BackupHistory] ADD  DEFAULT (getdate()) FOR [backup_date]
GO
ALTER TABLE [dbo].[Department] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((0)) FOR [annual_leave_balance]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((0)) FOR [sick_leave_balance]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((0)) FOR [unpaid_leave_balance]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[LeaveRequest] ADD  DEFAULT ('Pending') FOR [status]
GO
ALTER TABLE [dbo].[LeaveRequest] ADD  DEFAULT (getdate()) FOR [requested_at]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Payroll] ADD  DEFAULT ((0)) FOR [allowance]
GO
ALTER TABLE [dbo].[Payroll] ADD  DEFAULT ((0)) FOR [bonus]
GO
ALTER TABLE [dbo].[Payroll] ADD  DEFAULT ((0)) FOR [penalty]
GO
ALTER TABLE [dbo].[Payroll] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[UserSession] ADD  DEFAULT (getdate()) FOR [last_updated]
GO
ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [FK_ActivityLog_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_Users]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[Employee] ([employee_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Employee]
GO
ALTER TABLE [dbo].[BackupHistory]  WITH CHECK ADD  CONSTRAINT [FK_BackupHistory_Users] FOREIGN KEY([created_by])
REFERENCES [dbo].[Users] ([user_id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[BackupHistory] CHECK CONSTRAINT [FK_BackupHistory_Users]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Department] FOREIGN KEY([department_id])
REFERENCES [dbo].[Department] ([department_id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Department]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Users]
GO
ALTER TABLE [dbo].[LeaveRequest]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequest_ApprovedBy] FOREIGN KEY([approved_by])
REFERENCES [dbo].[Users] ([user_id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[LeaveRequest] CHECK CONSTRAINT [FK_LeaveRequest_ApprovedBy]
GO
ALTER TABLE [dbo].[LeaveRequest]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequest_Employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[Employee] ([employee_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LeaveRequest] CHECK CONSTRAINT [FK_LeaveRequest_Employee]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Department] FOREIGN KEY([department_id])
REFERENCES [dbo].[Department] ([department_id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Department]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Sender] FOREIGN KEY([sender_user_id])
REFERENCES [dbo].[Users] ([user_id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Sender]
GO
ALTER TABLE [dbo].[Payroll]  WITH CHECK ADD  CONSTRAINT [FK_Payroll_Employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[Employee] ([employee_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payroll] CHECK CONSTRAINT [FK_Payroll_Employee]
GO
ALTER TABLE [dbo].[UserSession]  WITH CHECK ADD  CONSTRAINT [FK_UserSession_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserSession] CHECK CONSTRAINT [FK_UserSession_Users]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD CHECK  (([leave_type]='Unpaid' OR [leave_type]='Annual' OR [leave_type]='Sick' OR [leave_type]='None'))
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD CHECK  (([gender]='Other' OR [gender]='Female' OR [gender]='Male'))
GO
ALTER TABLE [dbo].[LeaveRequest]  WITH CHECK ADD CHECK  (([leave_type]='Unpaid' OR [leave_type]='Sick' OR [leave_type]='Annual'))
GO
ALTER TABLE [dbo].[LeaveRequest]  WITH CHECK ADD CHECK  (([status]='Rejected' OR [status]='Approved' OR [status]='Pending'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([role]='Employee' OR [role]='Admin'))
GO
