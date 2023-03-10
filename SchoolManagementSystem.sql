USE [SchoolManagement]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Username] [nvarchar](500) NULL,
	[Password] [nvarchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[CategoryName] [nvarchar](500) NULL,
	[Description] [ntext] NULL,
	[IsActive] [nvarchar](4) NULL,
	[BookLoanDay] [datetime] NULL,
	[BookReturnDay] [datetime] NULL,
	[StudentName] [nvarchar](500) NULL,
	[Photo] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exam]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exam](
	[ExamID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NULL,
	[Description] [ntext] NULL,
	[Type] [nvarchar](500) NULL,
	[Score] [int] NULL,
	[TimeStart] [datetime] NULL,
	[TimeFinish] [datetime] NULL,
	[NumberQuest] [int] NULL,
	[Coef] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Library]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Library](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](500) NULL,
	[Photo] [nvarchar](500) NULL,
	[Description] [ntext] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Question]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Question] [nvarchar](500) NULL,
	[RightAnswer] [nvarchar](500) NULL,
	[WrongAnswer1] [nvarchar](500) NULL,
	[WrongAnswer2] [nvarchar](500) NULL,
	[WrongAnswer3] [nvarchar](500) NULL,
	[Type] [nvarchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Classroom] [nvarchar](500) NULL,
	[SubjectName] [nvarchar](500) NULL,
	[Letter] [nvarchar](500) NULL,
	[Point] [int] NULL,
	[StudentID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](500) NULL,
	[LastName] [nvarchar](500) NULL,
	[Class] [nvarchar](500) NULL,
	[Section] [nvarchar](500) NULL,
	[Gender] [nvarchar](500) NULL,
	[DateOfBirth] [datetime] NULL,
	[Email] [nvarchar](500) NULL,
	[Phone] [nvarchar](500) NULL,
	[Photo] [nvarchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[Credits] [int] NULL,
	[StartTime] [datetime] NULL,
	[FinishTime] [datetime] NULL,
	[SchoolDay] [nvarchar](500) NULL,
	[Teacher] [nvarchar](500) NULL,
	[Classroom] [nvarchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 2/11/2023 10:25:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[TeacherID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](500) NULL,
	[LastName] [nvarchar](500) NULL,
	[DateOfBirth] [datetime] NULL,
	[Gender] [nvarchar](500) NULL,
	[Experience] [nvarchar](500) NULL,
	[Email] [nvarchar](500) NULL,
	[Phone] [nvarchar](500) NULL,
	[Photo] [nvarchar](500) NULL
) ON [PRIMARY]
GO
