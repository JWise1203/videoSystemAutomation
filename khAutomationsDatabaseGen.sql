USE [Automations]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

PRINT '
*** Television Table Generation ***'
IF OBJECT_ID('[dbo].[tblTelevision]', 'U') IS NOT NULL
DROP TABLE [dbo].[tblTelevision]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblTelevision](
	[TelevisionID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[MACAddress] [nvarchar](50) NOT NULL,
	[AutoOnOff] [bit] NOT NULL CONSTRAINT [DF_tblTelevision_AutoOnOff]  DEFAULT (1),
	[CookieData] [nvarchar](max) NULL,
	[CommandList] [nvarchar](max) NULL,
	[UpdatedBy] [varchar](36) NULL,
	[UpdatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_tblTelevision_UpdatedDateTime]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_tblTelevisionTelevisionID] PRIMARY KEY CLUSTERED 
(
	[TelevisionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--TODO: SET THE RIGHT IP AND MAC FOR THE AUDITORIUM TVS
INSERT INTO [dbo].[tblTelevision] VALUES(1,'Library TV','192.168.1.6','FC:F1:52:A0:F0:56', 1,NULL, NULL,'Russell Ford',getutcdate());
INSERT INTO [dbo].[tblTelevision] VALUES(2,'Office TV','192.168.1.7','FC:F1:52:D3:51:D8', 1,NULL,NULL, 'Russell Ford',getutcdate());
INSERT INTO [dbo].[tblTelevision] VALUES(3,'Auditorium TV Left','192.168.1.8','FC:F1:52:A0:F0:56', 1,NULL,NULL, 'Russell Ford',getutcdate());
INSERT INTO [dbo].[tblTelevision] VALUES(4,'Auditorium TV Right','192.168.1.9','FC:F1:52:D3:51:D8', 1,NULL,NULL, 'Russell Ford',getutcdate());

PRINT
'*** Projector Table Generation ***'
IF OBJECT_ID('[dbo].[tblProjector]', 'U') IS NOT NULL
DROP TABLE [dbo].[tblProjector]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblProjector](
	[ProjectorID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[AutoOnOff] [bit] NOT NULL CONSTRAINT [DF_tblProjector_AutoOnOff]  DEFAULT (1),
	[UpdatedBy] [varchar](36) NULL,
	[UpdatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_tblProjector_UpdatedDateTime]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_tblProjectorProjectorID] PRIMARY KEY CLUSTERED 
(
	[ProjectorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

INSERT INTO [dbo].[tblProjector] VALUES(1,'Auditorium Projector','192.168.1.5', 1, 'Russell Ford', getutcdate());

PRINT '
*** Projector Lift Table Generation ***'

IF OBJECT_ID('[dbo].[tblProjectorLift]', 'U') IS NOT NULL
DROP TABLE [dbo].[tblProjectorLift]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblProjectorLift](
	[ProjectorLiftID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[COMPort] [nvarchar](50) NOT NULL,
	[AutoOnOff] [bit] NOT NULL CONSTRAINT [DF_tblProjectorLift_AutoOnOff]  DEFAULT (1),
	[UpdatedBy] [varchar](36) NULL,
	[UpdatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_tblProjectorLift_UpdatedDateTime]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_tblProjectorLiftProjectorLiftID] PRIMARY KEY CLUSTERED 
(
	[ProjectorLiftID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

INSERT INTO [dbo].[tblProjectorLift] VALUES(1,'Auditorium Lift','COM1', 1, 'Russell Ford', getutcdate());

PRINT '
*** Projector And Lift Table Generation ***'

IF OBJECT_ID('[dbo].[tblReleateProjectorAndLift]', 'U') IS NOT NULL
DROP TABLE [dbo].[tblReleateProjectorAndLift]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblReleateProjectorAndLift](
	[ProjectorID] [Int] NOT NULL,
	[ProjectorLiftID] [Int] NOT NULL,
	[UpdatedBy] [varchar](36) NULL,
	[UpdatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_tblReleateProjectorAndLift_UpdatedDateTime]  DEFAULT (getdate()),
 CONSTRAINT [PK_tblReleateProjectorAndLift] PRIMARY KEY CLUSTERED 
(
	[ProjectorID] ASC,
	[ProjectorLiftID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

INSERT INTO [dbo].[tblReleateProjectorAndLift] VALUES(1, 1, 'Russell Ford', getutcdate());

PRINT '
*** Operation Status Table Generation ***'

IF OBJECT_ID('[dbo].[tblOperationStatus]', 'U') IS NOT NULL
DROP TABLE [dbo].[tblOperationStatus]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblOperationStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SessionID] [varchar](36) NOT NULL,
	[StatusID] [Int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[UpdatedBy] [varchar](36) NULL,
	[UpdatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_tblOperationStatus_UpdatedDateTime]  DEFAULT (getdate()),
 CONSTRAINT [PK_tblOperationStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

PRINT '
*** Matrix Switcher Table Generation ***'

IF OBJECT_ID('[dbo].[tblMatrixSwitcher]', 'U') IS NOT NULL
DROP TABLE [dbo].[tblMatrixSwitcher]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblMatrixSwitcher](
	[MatrixSwitcherID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[COMPort] [nvarchar](50) NOT NULL,
	[AutoOnOff] [bit] NOT NULL CONSTRAINT [DF_tblMatrixSwitcher_AutoOnOff]  DEFAULT (1),
	[UpdatedBy] [varchar](36) NULL,
	[UpdatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_tblMatrixSwitcher_UpdatedDateTime]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_tblMatrixSwitcherMatrixSwitcher] PRIMARY KEY CLUSTERED 
(
	[MatrixSwitcherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

INSERT INTO [dbo].[tblMatrixSwitcher] VALUES(1,'Main Auditorium Matrix Switcher','COM4', 1, 'Russell Ford', getutcdate());

PRINT '
*** TV Command White List ***'

IF OBJECT_ID('[dbo].[tblMatrixSwitcher]', 'U') IS NOT NULL
DROP TABLE [dbo].[tblTVCommandWhiteList]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblTVCommandWhiteList](
	[CommandID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[DisplayValue][varchar](100) NOT NULL,
	[UpdatedBy] [varchar](36) NULL,
	[UpdatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_tblTVCommandWhiteList_UpdatedDateTime]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_tblTVCommandWhiteListTVCommandWhiteList] PRIMARY KEY CLUSTERED 
(
	[CommandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--TODO FILL OUT THE REST OF THE COMMANDS we want
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(1,'PowerOff','Power Off','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(3,'Input','Input','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(4,'Home','Home','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(5,'Options','Options','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(6,'Return','Return','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(7,'Up','Up','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(8,'Down','Down','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(9,'Right','Right','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(10,'Left','Left','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(11,'Confirm','Confirm','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(12,'Num1','1','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(13,'Num2','2','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(14,'Num3','3','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(15,'Num4','4','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(16,'Num5','5','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(17,'Num6','6','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(18,'Num7','7','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(19,'Num8','8','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(20,'Num9','9','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(21,'Num0','0','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(22,'Num11','11','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(23,'Num12','12','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(24,'VolumeUp','Volume Up','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(25,'VolumeDown','Volume Down','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(26,'Mute','Mute','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(27,'ChannelUp','Channel Up','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(28,'ChannelDown','Channel Down','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(29,'Enter','Enter','Russell Ford', getutcdate());
INSERT INTO [dbo].[tblTVCommandWhiteList] VALUES(30,'Exit','Exit','Russell Ford', getutcdate());