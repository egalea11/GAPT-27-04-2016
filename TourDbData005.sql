USE [ToursDb]
GO
SET IDENTITY_INSERT [dbo].[Tour] ON 

GO
INSERT [dbo].[Tour] ([Id], [Name], [ShortDescription], [LongDescription], [AdultPrice], [ChildPrice], [MaxGroupSize], [AverageRatingId], [CategoryId], [DateTimeCreated]) VALUES (6, N'Prehistorical Cul-De-Sac', N'A dark water-worn cave with numerous geological features. If you love historical places with an element of nature this tour is just right for you!', N'In this cave evidence of human presence on Malta is found, with artefacts dating back 7,400 years to the Neolithic Period.This attraction is divided into two sections: the cave(Ghar Dalam) and a museum where one can see  remains ranging from animal bones to human works of art, with the most important discovery being two teeth that belonged to Neanderthal man.', CAST(5.00 AS Decimal(10, 2)), CAST(2.50 AS Decimal(10, 2)), 30, 6, 2, CAST(N'2016-05-04 11:47:23.677' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Tour] OFF
GO
SET IDENTITY_INSERT [dbo].[TourDate] ON 

GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (113, 6, CAST(N'2016-05-19' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (114, 6, CAST(N'2016-05-22' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (115, 6, CAST(N'2016-05-26' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (116, 6, CAST(N'2016-06-02' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (117, 6, CAST(N'2016-06-07' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (118, 6, CAST(N'2016-06-11' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (119, 6, CAST(N'2016-06-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (120, 6, CAST(N'2016-06-22' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (121, 6, CAST(N'2016-06-27' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (122, 6, CAST(N'2016-06-30' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (123, 6, CAST(N'2016-07-05' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (124, 6, CAST(N'2016-07-10' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (125, 6, CAST(N'2016-07-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (126, 6, CAST(N'2016-07-19' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (127, 6, CAST(N'2016-07-25' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (128, 6, CAST(N'2016-07-31' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (129, 6, CAST(N'2016-08-02' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (130, 6, CAST(N'2016-08-07' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (131, 6, CAST(N'2016-08-10' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (132, 6, CAST(N'2016-08-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (133, 6, CAST(N'2016-08-20' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (134, 6, CAST(N'2016-08-25' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (135, 6, CAST(N'2016-08-31' AS Date))
GO
SET IDENTITY_INSERT [dbo].[TourDate] OFF
GO
SET IDENTITY_INSERT [dbo].[Image] ON 

GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (22, 6, N'~/Images/rsz_tour6_1.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (23, 6, N'~/Images/product_details/tour6_1.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (24, 6, N'~/Images/product_details/tour6_2.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (25, 6, N'~/Images/product_details/tour6_3.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (26, 6, N'~/Images/product_details/tour6_4.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (27, 6, N'~/Images/product_details/tour6_5.jpg')
GO
SET IDENTITY_INSERT [dbo].[Image] OFF
GO
SET IDENTITY_INSERT [dbo].[TourTime] ON 

GO
INSERT [dbo].[TourTime] ([Id], [TourId], [StartTime], [EndTime]) VALUES (5, 6, N'08:00', N'11:00')
GO
INSERT [dbo].[TourTime] ([Id], [TourId], [StartTime], [EndTime]) VALUES (6, 6, N'13:00', N'16:00')
GO
SET IDENTITY_INSERT [dbo].[TourTime] OFF
GO
SET IDENTITY_INSERT [dbo].[TourDateTime] ON 

GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (152, 113, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (153, 114, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (154, 115, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (155, 116, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (156, 117, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (157, 118, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (158, 119, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (159, 120, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (160, 121, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (161, 122, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (162, 123, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (163, 124, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (164, 125, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (165, 126, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (166, 127, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (167, 128, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (168, 129, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (169, 130, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (170, 131, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (171, 132, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (172, 133, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (173, 134, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (174, 135, 5)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (175, 113, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (176, 115, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (177, 116, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (178, 118, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (179, 120, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (180, 121, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (181, 122, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (182, 124, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (183, 126, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (184, 128, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (185, 129, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (186, 130, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (187, 132, 6)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (188, 134, 6)
GO
SET IDENTITY_INSERT [dbo].[TourDateTime] OFF
GO
SET IDENTITY_INSERT [dbo].[TourTimeTable] ON 

GO
INSERT [dbo].[TourTimeTable] ([Id], [LocationId], [StartTime], [EndTime], [TourTimeId]) VALUES (7, 6, N'08:00', N'11:00', 5)
GO
INSERT [dbo].[TourTimeTable] ([Id], [LocationId], [StartTime], [EndTime], [TourTimeId]) VALUES (8, 6, N'13:00', N'16:00', 6)
GO
SET IDENTITY_INSERT [dbo].[TourTimeTable] OFF
GO
