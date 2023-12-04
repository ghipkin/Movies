USE [Movies]
GO
/****** Object:  Table [dbo].[Genre]    Script Date: 28/11/2023 16:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genre](
	[GenreId] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[GenreDescription] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED 
(
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movie]    Script Date: 28/11/2023 16:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movie](
	[MovieId] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ReleaseDate] [date] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Overview] [nvarchar](max) NOT NULL,
	[Popularity] [float] NOT NULL,
	[VoteCount] [smallint] NOT NULL,
	[VoteAverage] [float] NOT NULL,
	[OriginalLanguage] [nvarchar](50) NOT NULL,
	[PosterUrl] [nvarchar](100) NOT NULL
 CONSTRAINT [PK_Movie] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovieGenre]    Script Date: 28/11/2023 16:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieGenre](
	[MovieId] [numeric](18, 0) NOT NULL,
	[GenreId] [numeric](18, 0) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MovieGenre]  WITH CHECK ADD FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genre] ([GenreId])
GO
ALTER TABLE [dbo].[MovieGenre]  WITH CHECK ADD FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movie] ([MovieId])
GO
