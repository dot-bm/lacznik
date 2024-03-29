CREATE SCHEMA [mgm]
GO
/****** Object:  Table [mgm].[7050_J_Rozdzielnik_Projekty]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7050_J_Rozdzielnik_Projekty](
	[ID7050] [int] NOT NULL,
	[Nazwa] [nvarchar](80) NOT NULL,
	[Opis] [nvarchar](150) NULL,
	[Uwagi] [nvarchar](max) SPARSE  NULL,
 CONSTRAINT [PK_7050_J_Rozdzielnik_Projekty] PRIMARY KEY CLUSTERED 
(
	[ID7050] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7051_J_Rozdzielnik_Warianty]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7051_J_Rozdzielnik_Warianty](
	[ID7051] [int] NOT NULL,
	[LX7050] [int] NOT NULL,
	[Symbol] [nvarchar](50) NOT NULL,
	[Opis] [nvarchar](max) NULL,
 CONSTRAINT [PK_7051_J_Rozdzielnik_Warianty] PRIMARY KEY CLUSTERED 
(
	[ID7051] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7052_J_DaneRozdzielnik]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7052_J_DaneRozdzielnik](
	[ID7052] [int] IDENTITY(1,1) NOT NULL,
	[LX7050] [int] NOT NULL,
	[LX7051] [int] NULL,
	[RokPrzekroju] [numeric](4, 0) NOT NULL,
 CONSTRAINT [PK_7052_J_DaneRozdzielnik] PRIMARY KEY CLUSTERED 
(
	[ID7052] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7101_YD_VisumKategoriaEncji]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7101_YD_VisumKategoriaEncji](
	[ID7101] [tinyint] NOT NULL,
	[Nazwa] [nvarchar](50) NOT NULL,
	[VisumName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_7101_YD_VisumKategoriaEncji] PRIMARY KEY CLUSTERED 
(
	[ID7101] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7102_YD_VisumTypyAtrybutów]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7102_YD_VisumTypyAtrybutów](
	[ID7102] [tinyint] NOT NULL,
	[Nazwa] [varchar](50) NOT NULL,
	[VisumName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_7102_YD_VisumTypyAtrybutów] PRIMARY KEY CLUSTERED 
(
	[ID7102] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7103_YD_VisumRodzajGenerycznosci]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7103_YD_VisumRodzajGenerycznosci](
	[ID7103] [tinyint] NOT NULL,
	[TypProsty] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_7103_YD_VisumRodzajGenerycznosci] PRIMARY KEY CLUSTERED 
(
	[ID7103] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7105_YD_ParametryZapytań]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7105_YD_ParametryZapytań](
	[ID7105] [tinyint] NOT NULL,
	[NazwaPola] [nvarchar](100) NOT NULL,
	[Opis] [nvarchar](150) NULL,
 CONSTRAINT [PK_7105_YD_ParametryZapytań] PRIMARY KEY CLUSTERED 
(
	[ID7105] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7110_Y_VisumAtrybuty]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7110_Y_VisumAtrybuty](
	[ID7110] [int] NOT NULL,
	[LX7101] [tinyint] NOT NULL,
	[LX7102] [tinyint] NOT NULL,
	[LX7103] [tinyint] NOT NULL,
	[GenTwórzZłożony] [bit] NOT NULL,
	[Użytkownika] [bit] NOT NULL,
	[Nazwa] [nvarchar](50) NOT NULL,
	[MinValue] [varchar](30) NULL,
	[MaxValue] [varchar](30) NULL,
	[DefaultValue] [varchar](30) NULL,
	[StringDefaultValue] [nvarchar](100) NULL,
	[StringConcatenator] [nvarchar](3) NULL,
	[MaxStrLen] [int] NULL,
	[NumDecPlaces] [int] NULL,
	[Scaled] [varchar](30) NULL,
	[CrossSectionLogic] [varchar](30) NULL,
	[CSLIgnoreClosed] [varchar](30) NULL,
	[Obligatoryjny] [bit] NOT NULL,
	[LX711G] [int] NULL,
 CONSTRAINT [PK_7110_Y_VisumAtrybuty] PRIMARY KEY CLUSTERED 
(
	[ID7110] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[711G_Y_GrupyAtrybutów]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[711G_Y_GrupyAtrybutów](
	[ID711G] [int] NOT NULL,
	[Opis] [nvarchar](60) NULL,
 CONSTRAINT [PK_711G_Y_GrupyAtrybutów] PRIMARY KEY CLUSTERED 
(
	[ID711G] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7120_Y_ŹródłaAtrybutów]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7120_Y_ŹródłaAtrybutów](
	[ID7120] [int] IDENTITY(1,1) NOT NULL,
	[LX7110] [int] NOT NULL,
	[LX7125] [int] NOT NULL,
	[NazwaPola] [nvarchar](100) NOT NULL,
	[IDSubAtrybutu_NazwaPola] [nvarchar](100) NULL,
 CONSTRAINT [PK_7120_Y_ŹródłaAtrybutów] PRIMARY KEY CLUSTERED 
(
	[ID7120] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7121_Y_SzablonyParametrów]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7121_Y_SzablonyParametrów](
	[ID7121] [int] NOT NULL,
	[Skrót] [nvarchar](20) NULL,
	[Opis] [nvarchar](150) NULL,
 CONSTRAINT [PK_7121_Y_SzablonyParametrów] PRIMARY KEY CLUSTERED 
(
	[ID7121] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7122_Y_SzablonyParametrów_Zawartość]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7122_Y_SzablonyParametrów_Zawartość](
	[LX7121] [int] NOT NULL,
	[Ord] [tinyint] NOT NULL,
	[LX7105] [tinyint] NOT NULL,
 CONSTRAINT [PK_7122_Y_SzablonyParametrów_Zawartość] PRIMARY KEY CLUSTERED 
(
	[LX7121] ASC,
	[Ord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7125_Y_FunkcjeWydająceAtrybuty]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7125_Y_FunkcjeWydająceAtrybuty](
	[ID7125] [int] NOT NULL,
	[Schema] [nvarchar](20) NULL,
	[NazwaFunkcji] [nvarchar](100) NOT NULL,
	[ObjectID_NazwaPola] [nvarchar](100) NULL,
	[LX7121] [int] NOT NULL,
 CONSTRAINT [PK_7125_Y_FunkcjeWydająceAtrybuty] PRIMARY KEY CLUSTERED 
(
	[ID7125] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7128_Y_PomostAtrybutów]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7128_Y_PomostAtrybutów](
	[LX7110] [int] NOT NULL,
	[Właściwość] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_7128_Y_PomostAtrybutów] PRIMARY KEY CLUSTERED 
(
	[LX7110] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7135_Y_FunkcjeWydająceEncje]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7135_Y_FunkcjeWydająceEncje](
	[ID7135] [int] NOT NULL,
	[LX7101] [tinyint] NOT NULL,
	[Schema] [nvarchar](20) NOT NULL,
	[NazwaFunkcji] [nvarchar](100) NOT NULL,
	[ObjectID_NazwaPola] [nvarchar](100) NOT NULL,
	[SekwencjaPól] [nvarchar](300) NULL,
	[LX7121] [int] NOT NULL,
 CONSTRAINT [PK_7135_Y_FunkcjeWydająceEncje] PRIMARY KEY CLUSTERED 
(
	[ID7135] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[7136_Y_FunkcjeWydająceEncje_OpcjonalnePola]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[7136_Y_FunkcjeWydająceEncje_OpcjonalnePola](
	[LX7135] [int] NOT NULL,
	[QuelleIndex] [tinyint] NOT NULL,
	[NazwaPola] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_7136_Y_FunkcjeWydająceEncje_OpcjonalnePola] PRIMARY KEY CLUSTERED 
(
	[LX7135] ASC,
	[QuelleIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mgm].[71M0_Y_Model]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[71M0_Y_Model](
	[ID71M0] [int] NOT NULL,
	[Opis] [nvarchar](110) NULL,
	[Uwagi] [ntext] NULL
 CONSTRAINT [PK_71M0_Y_Model] PRIMARY KEY CLUSTERED 
(
	[ID71M0] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [mgm].[71MA_Y_Model_GrupyAtrybutów]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mgm].[71MA_Y_Model_GrupyAtrybutów](
	[LX71M0] [int] NOT NULL,
	[LX711G] [int] NOT NULL,
 CONSTRAINT [PK_71MA_Y_Model_GrupyAtrybutów] PRIMARY KEY CLUSTERED 
(
	[LX71M0] ASC,
	[LX711G] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (2, N'Sieć', N'NETWORK')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (3, N'Systemy Transportu', N'TSYS')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (4, N'Mody', N'MODE')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (5, N'Segmenty Popytu', N'DEMANDSEGMENT')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (10, N'Węzły', N'NODE')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (20, N'Odcinki', N'LINK')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (21, N'Typy Odcinków', N'LINKTYPE')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (30, N'Rejony', N'ZONE')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (32, N'Konektory', N'CONNECTOR')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (40, N'Przystanek (Zatrzymanie)', N'STOPPOINT')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (41, N'Przystanek – Zespół Zatrzymań', N'STOPAREA')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (42, N'Przystanek', N'STOP')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (60, N'Linie', N'LINE')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (61, N'Marszruta Linii', N'LINEROUTE')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (62, N'Marszruta Linii – Punkty', N'LINEROUTEITEM')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (63, N'Czasówka Linii', N'TIMEPROFILE')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (64, N'Czasówka Linii – Zatrzymania', N'TIMEPROFILEITEM')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (65, N'Kurs', N'VEHJOURNEY')
INSERT [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101], [Nazwa], [VisumName]) VALUES (66, N'Kurs (Sekcja)', N'VEHJOURNEYSECTION')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (0, N'Integer', N'Int')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (5, N'Velocity (km/h)', N'Speed')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (10, N'Double', N'Double')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (20, N'Decimal', N'Currency')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (30, N'Length (km)', N'LongLength')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (31, N'Area(km2)', N'Area')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (40, N'Time period (s)', N'Duration')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (41, N'Precise Time Period (s)', N'LongDuration')
INSERT [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102], [Nazwa], [VisumName]) VALUES (250, N'String', N'Text')
INSERT [mgm].[7103_YD_VisumRodzajGenerycznosci] ([ID7103], [TypProsty]) VALUES (0, N'[Niegeneryczny]')
INSERT [mgm].[7103_YD_VisumRodzajGenerycznosci] ([ID7103], [TypProsty]) VALUES (1, N'TSys_PrT')
INSERT [mgm].[7103_YD_VisumRodzajGenerycznosci] ([ID7103], [TypProsty]) VALUES (2, N'TSys_PuT')

ALTER TABLE [mgm].[7110_Y_VisumAtrybuty] ADD  CONSTRAINT [DF_7110_Y_VisumAtrybuty_LX7103]  DEFAULT ((0)) FOR [LX7103]
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty] ADD  CONSTRAINT [DF_7110_Y_VisumAtrybuty_GenTwórzZłożony]  DEFAULT ((0)) FOR [GenTwórzZłożony]
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty] ADD  CONSTRAINT [DF_7110_Y_VisumAtrybuty_Użytkownika]  DEFAULT ((0)) FOR [Użytkownika]
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty] ADD  CONSTRAINT [DF_7110_Y_VisumAtrybuty_Obligatoryjny]  DEFAULT ((0)) FOR [Obligatoryjny]
GO
ALTER TABLE [mgm].[7051_J_Rozdzielnik_Warianty]  WITH CHECK ADD  CONSTRAINT [FK_7051_J_Rozdzielnik_Warianty_7050_J_Rozdzielnik_Projekty] FOREIGN KEY([LX7050])
REFERENCES [mgm].[7050_J_Rozdzielnik_Projekty] ([ID7050])
GO

ALTER TABLE [mgm].[7052_J_DaneRozdzielnik]  WITH CHECK ADD  CONSTRAINT [FK_7052_J_DaneRozdzielnik_7050_J_Rozdzielnik_Projekty] FOREIGN KEY([LX7050])
REFERENCES [mgm].[7050_J_Rozdzielnik_Projekty] ([ID7050])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[7052_J_DaneRozdzielnik] CHECK CONSTRAINT [FK_7052_J_DaneRozdzielnik_7050_J_Rozdzielnik_Projekty]
GO
ALTER TABLE [mgm].[7052_J_DaneRozdzielnik]  WITH CHECK ADD  CONSTRAINT [FK_7052_J_DaneRozdzielnik_7051_J_Rozdzielnik_Warianty] FOREIGN KEY([LX7051])
REFERENCES [mgm].[7051_J_Rozdzielnik_Warianty] ([ID7051])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[7052_J_DaneRozdzielnik] CHECK CONSTRAINT [FK_7052_J_DaneRozdzielnik_7051_J_Rozdzielnik_Warianty]
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty]  WITH CHECK ADD  CONSTRAINT [FK_7110_Y_VisumAtrybuty_7101_YD_VisumKategoriaEncji] FOREIGN KEY([LX7101])
REFERENCES [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101])
ON UPDATE CASCADE
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty] CHECK CONSTRAINT [FK_7110_Y_VisumAtrybuty_7101_YD_VisumKategoriaEncji]
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty]  WITH CHECK ADD  CONSTRAINT [FK_7110_Y_VisumAtrybuty_7102_YD_VisumTypyAtrybutów] FOREIGN KEY([LX7102])
REFERENCES [mgm].[7102_YD_VisumTypyAtrybutów] ([ID7102])
ON UPDATE CASCADE
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty] CHECK CONSTRAINT [FK_7110_Y_VisumAtrybuty_7102_YD_VisumTypyAtrybutów]
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty]  WITH CHECK ADD  CONSTRAINT [FK_7110_Y_VisumAtrybuty_7103_YD_VisumRodzajGenerycznosci] FOREIGN KEY([LX7103])
REFERENCES [mgm].[7103_YD_VisumRodzajGenerycznosci] ([ID7103])
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty] CHECK CONSTRAINT [FK_7110_Y_VisumAtrybuty_7103_YD_VisumRodzajGenerycznosci]
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty]  WITH CHECK ADD  CONSTRAINT [FK_7110_Y_VisumAtrybuty_711G_Y_GrupyAtrybutów] FOREIGN KEY([LX711G])
REFERENCES [mgm].[711G_Y_GrupyAtrybutów] ([ID711G])
GO
ALTER TABLE [mgm].[7110_Y_VisumAtrybuty] CHECK CONSTRAINT [FK_7110_Y_VisumAtrybuty_711G_Y_GrupyAtrybutów]
GO
ALTER TABLE [mgm].[7120_Y_ŹródłaAtrybutów]  WITH CHECK ADD  CONSTRAINT [FK_7120_Y_ŹródłaAtrybutów_7110_Y_VisumAtrybuty] FOREIGN KEY([LX7110])
REFERENCES [mgm].[7110_Y_VisumAtrybuty] ([ID7110])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[7120_Y_ŹródłaAtrybutów] CHECK CONSTRAINT [FK_7120_Y_ŹródłaAtrybutów_7110_Y_VisumAtrybuty]
GO
ALTER TABLE [mgm].[7120_Y_ŹródłaAtrybutów]  WITH CHECK ADD  CONSTRAINT [FK_7120_Y_ŹródłaAtrybutów_7125_Y_FunkcjeWydająceAtrybuty] FOREIGN KEY([LX7125])
REFERENCES [mgm].[7125_Y_FunkcjeWydająceAtrybuty] ([ID7125])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[7120_Y_ŹródłaAtrybutów] CHECK CONSTRAINT [FK_7120_Y_ŹródłaAtrybutów_7125_Y_FunkcjeWydająceAtrybuty]
GO
ALTER TABLE [mgm].[7122_Y_SzablonyParametrów_Zawartość]  WITH CHECK ADD  CONSTRAINT [FK_7122_Y_SzablonyParametrów_Zawartość_7105_YD_ParametryZapytań] FOREIGN KEY([LX7105])
REFERENCES [mgm].[7105_YD_ParametryZapytań] ([ID7105])
ON UPDATE CASCADE
GO
ALTER TABLE [mgm].[7122_Y_SzablonyParametrów_Zawartość] CHECK CONSTRAINT [FK_7122_Y_SzablonyParametrów_Zawartość_7105_YD_ParametryZapytań]
GO
ALTER TABLE [mgm].[7122_Y_SzablonyParametrów_Zawartość]  WITH CHECK ADD  CONSTRAINT [FK_7122_Y_SzablonyParametrów_Zawartość_7121_Y_SzablonyParametrów] FOREIGN KEY([LX7121])
REFERENCES [mgm].[7121_Y_SzablonyParametrów] ([ID7121])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[7122_Y_SzablonyParametrów_Zawartość] CHECK CONSTRAINT [FK_7122_Y_SzablonyParametrów_Zawartość_7121_Y_SzablonyParametrów]
GO
ALTER TABLE [mgm].[7125_Y_FunkcjeWydająceAtrybuty]  WITH CHECK ADD  CONSTRAINT [FK_7125_Y_FunkcjeWydająceAtrybuty_7121_Y_SzablonyParametrów] FOREIGN KEY([LX7121])
REFERENCES [mgm].[7121_Y_SzablonyParametrów] ([ID7121])
ON UPDATE CASCADE
GO
ALTER TABLE [mgm].[7125_Y_FunkcjeWydająceAtrybuty] CHECK CONSTRAINT [FK_7125_Y_FunkcjeWydająceAtrybuty_7121_Y_SzablonyParametrów]
GO
ALTER TABLE [mgm].[7128_Y_PomostAtrybutów]  WITH CHECK ADD  CONSTRAINT [FK_7128_Y_PomostAtrybutów_7110_Y_VisumAtrybuty] FOREIGN KEY([LX7110])
REFERENCES [mgm].[7110_Y_VisumAtrybuty] ([ID7110])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[7128_Y_PomostAtrybutów] CHECK CONSTRAINT [FK_7128_Y_PomostAtrybutów_7110_Y_VisumAtrybuty]
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje]  WITH CHECK ADD  CONSTRAINT [FK_7135_Y_FunkcjeWydająceEncje_7101_YD_VisumKategoriaEncji] FOREIGN KEY([LX7101])
REFERENCES [mgm].[7101_YD_VisumKategoriaEncji] ([ID7101])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje] CHECK CONSTRAINT [FK_7135_Y_FunkcjeWydająceEncje_7101_YD_VisumKategoriaEncji]
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje]  WITH CHECK ADD  CONSTRAINT [FK_7135_Y_FunkcjeWydająceEncje_7121_Y_SzablonyParametrów] FOREIGN KEY([LX7121])
REFERENCES [mgm].[7121_Y_SzablonyParametrów] ([ID7121])
ON UPDATE CASCADE
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje] CHECK CONSTRAINT [FK_7135_Y_FunkcjeWydająceEncje_7121_Y_SzablonyParametrów]
GO
ALTER TABLE [mgm].[7136_Y_FunkcjeWydająceEncje_OpcjonalnePola]  WITH CHECK ADD  CONSTRAINT [FK_7136_Y_FunkcjeWydająceEncje_OpcjonalnePola_7135_Y_FunkcjeWydająceEncje] FOREIGN KEY([LX7135])
REFERENCES [mgm].[7135_Y_FunkcjeWydająceEncje] ([ID7135])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[7136_Y_FunkcjeWydająceEncje_OpcjonalnePola] CHECK CONSTRAINT [FK_7136_Y_FunkcjeWydająceEncje_OpcjonalnePola_7135_Y_FunkcjeWydająceEncje]
GO

ALTER TABLE [mgm].[71MA_Y_Model_GrupyAtrybutów]  WITH CHECK ADD  CONSTRAINT [FK_71MA_Y_Model_GrupyAtrybutów_711G_Y_GrupyAtrybutów] FOREIGN KEY([LX711G])
REFERENCES [mgm].[711G_Y_GrupyAtrybutów] ([ID711G])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[71MA_Y_Model_GrupyAtrybutów] CHECK CONSTRAINT [FK_71MA_Y_Model_GrupyAtrybutów_711G_Y_GrupyAtrybutów]
GO
ALTER TABLE [mgm].[71MA_Y_Model_GrupyAtrybutów]  WITH CHECK ADD  CONSTRAINT [FK_71MA_Y_Model_GrupyAtrybutów_71M0_Y_Model] FOREIGN KEY([LX71M0])
REFERENCES [mgm].[71M0_Y_Model] ([ID71M0])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [mgm].[71MA_Y_Model_GrupyAtrybutów] CHECK CONSTRAINT [FK_71MA_Y_Model_GrupyAtrybutów_71M0_Y_Model]
GO
ALTER TABLE [mgm].[7120_Y_ŹródłaAtrybutów]  WITH CHECK ADD  CONSTRAINT [CK_7120_Y_ŹródłaAtrybutów] CHECK  ((charindex('[',[NazwaPola])=(0) AND charindex(']',[NazwaPola])=(0)))
GO
ALTER TABLE [mgm].[7120_Y_ŹródłaAtrybutów] CHECK CONSTRAINT [CK_7120_Y_ŹródłaAtrybutów]
GO
ALTER TABLE [mgm].[7120_Y_ŹródłaAtrybutów]  WITH CHECK ADD  CONSTRAINT [CK_7120_Y_ŹródłaAtrybutów_1] CHECK  ((charindex('[',[IDSubAtrybutu_NazwaPola])=(0) AND charindex(']',[IDSubAtrybutu_NazwaPola])=(0)))
GO
ALTER TABLE [mgm].[7120_Y_ŹródłaAtrybutów] CHECK CONSTRAINT [CK_7120_Y_ŹródłaAtrybutów_1]
GO
ALTER TABLE [mgm].[7125_Y_FunkcjeWydająceAtrybuty]  WITH CHECK ADD  CONSTRAINT [CK_7125_Y_FunkcjeWydająceAtrybuty] CHECK  ((charindex('[',[ObjectID_NazwaPola])=(0) AND charindex(']',[ObjectID_NazwaPola])=(0)))
GO
ALTER TABLE [mgm].[7125_Y_FunkcjeWydająceAtrybuty] CHECK CONSTRAINT [CK_7125_Y_FunkcjeWydająceAtrybuty]
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje]  WITH CHECK ADD  CONSTRAINT [CK_7135_Y_FunkcjeWydająceEncje_NF] CHECK  ((charindex('[',[NazwaFunkcji])=(0) AND charindex(']',[NazwaFunkcji])=(0)))
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje] CHECK CONSTRAINT [CK_7135_Y_FunkcjeWydająceEncje_NF]
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje]  WITH CHECK ADD  CONSTRAINT [CK_7135_Y_FunkcjeWydająceEncje_OID] CHECK  ((charindex('[',[ObjectID_NazwaPola])=(0) AND charindex(']',[ObjectID_NazwaPola])=(0)))
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje] CHECK CONSTRAINT [CK_7135_Y_FunkcjeWydająceEncje_OID]
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje]  WITH CHECK ADD  CONSTRAINT [CK_7135_Y_FunkcjeWydająceEncje_Schema] CHECK  ((charindex('[',[Schema])=(0) AND charindex(']',[Schema])=(0)))
GO
ALTER TABLE [mgm].[7135_Y_FunkcjeWydająceEncje] CHECK CONSTRAINT [CK_7135_Y_FunkcjeWydająceEncje_Schema]
GO
ALTER TABLE [mgm].[7136_Y_FunkcjeWydająceEncje_OpcjonalnePola]  WITH CHECK ADD  CONSTRAINT [CK_7136_Y_FunkcjeWydająceEncje_OpcjonalnePola] CHECK  ((charindex('[',[NazwaPola])=(0) AND charindex(']',[NazwaPola])=(0)))
GO
ALTER TABLE [mgm].[7136_Y_FunkcjeWydająceEncje_OpcjonalnePola] CHECK CONSTRAINT [CK_7136_Y_FunkcjeWydająceEncje_OpcjonalnePola]
GO
