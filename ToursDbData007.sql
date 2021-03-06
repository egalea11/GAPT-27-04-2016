USE [ToursDb]
GO
SET IDENTITY_INSERT [dbo].[Tour] ON 

GO
INSERT [dbo].[Tour] ([Id], [Name], [ShortDescription], [LongDescription], [AdultPrice], [ChildPrice], [MaxGroupSize], [AverageRatingId], [CategoryId], [DateTimeCreated]) VALUES (8, N'Half-Day Arts Tour', N'This tour is perfect for those who love art and are looking for an opportunity to see and explore both local and international artistic pieces.', N'Set in a complementing historic building, the museum presents a multifaceted overview of art and artistic expression in Malta from the Late Medieval period to the contemporary. Highlights from the collection on display include paintings by leading local and internationally acclaimed artists, precious Maltese silverware, statuary in marble bronze and wood, fine furniture items and splendid maiolica pieces.', CAST(10.00 AS Decimal(10, 2)), CAST(7.00 AS Decimal(10, 2)), 40, 6, 1, CAST(N'2016-05-04 19:59:54.923' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Tour] OFF
GO
SET IDENTITY_INSERT [dbo].[TourDate] ON 

GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (177, 8, CAST(N'2016-05-10' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (178, 8, CAST(N'2016-05-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (179, 8, CAST(N'2016-05-18' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (180, 8, CAST(N'2016-05-25' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (181, 8, CAST(N'2016-05-28' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (182, 8, CAST(N'2016-06-01' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (183, 8, CAST(N'2016-06-05' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (184, 8, CAST(N'2016-06-10' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (185, 8, CAST(N'2016-06-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (186, 8, CAST(N'2016-06-20' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (187, 8, CAST(N'2016-06-25' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (188, 8, CAST(N'2016-06-30' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (189, 8, CAST(N'2016-07-06' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (190, 8, CAST(N'2016-07-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (191, 8, CAST(N'2016-07-22' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (192, 8, CAST(N'2016-07-30' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (193, 8, CAST(N'2016-08-05' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (194, 8, CAST(N'2016-08-10' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (195, 8, CAST(N'2016-08-16' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (196, 8, CAST(N'2016-08-22' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (197, 8, CAST(N'2016-08-31' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (198, 8, CAST(N'2016-09-05' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (199, 8, CAST(N'2016-09-10' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (200, 8, CAST(N'2016-09-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (201, 8, CAST(N'2016-09-20' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (202, 8, CAST(N'2016-09-25' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (203, 8, CAST(N'2016-09-30' AS Date))
GO
SET IDENTITY_INSERT [dbo].[TourDate] OFF
GO
SET IDENTITY_INSERT [dbo].[Image] ON 

GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (34, 8, N'~/Images/rsz_tour8_1.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (35, 8, N'~/Images/product_details/tour8_1.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (36, 8, N'~/Images/product_details/tour8_2.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (37, 8, N'~/Images/product_details/tour8_3.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (38, 8, N'~/Images/product_details/tour8_4.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (39, 8, N'~/Images/product_details/tour8_5.jpg')
GO
SET IDENTITY_INSERT [dbo].[Image] OFF
GO
SET IDENTITY_INSERT [dbo].[TourTime] ON 

GO
INSERT [dbo].[TourTime] ([Id], [TourId], [StartTime], [EndTime]) VALUES (8, 8, N'09:00', N'13:00')
GO
SET IDENTITY_INSERT [dbo].[TourTime] OFF
GO
SET IDENTITY_INSERT [dbo].[TourDateTime] ON 

GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (230, 177, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (231, 178, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (232, 179, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (233, 180, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (234, 181, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (235, 182, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (236, 183, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (237, 184, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (238, 185, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (239, 186, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (240, 187, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (241, 188, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (242, 189, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (243, 190, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (244, 191, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (245, 192, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (246, 193, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (247, 194, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (248, 195, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (249, 196, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (250, 197, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (251, 198, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (252, 199, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (253, 200, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (254, 201, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (255, 202, 8)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (256, 203, 8)
GO
SET IDENTITY_INSERT [dbo].[TourDateTime] OFF
GO
SET IDENTITY_INSERT [dbo].[TourTimeTable] ON 

GO
INSERT [dbo].[TourTimeTable] ([Id], [LocationId], [StartTime], [EndTime], [TourTimeId]) VALUES (16, 3, N'09:00', N'13:00', 8)
GO
SET IDENTITY_INSERT [dbo].[TourTimeTable] OFF
GO
