USE [ToursDb]
GO
UPDATE [dbo].[Category] SET Name = 'Nature and Adventure' WHERE Id = 4;
GO
SET IDENTITY_INSERT [dbo].[Tour] ON 

GO
INSERT [dbo].[Tour] ([Id], [Name], [ShortDescription], [LongDescription], [AdultPrice], [ChildPrice], [MaxGroupSize], [AverageRatingId], [CategoryId], [DateTimeCreated]) VALUES (12, N'Dwejra Snorkeling', N'Experience the Maltese waters with our qualified instructors! An opportunity not to be missed especially if you love adventure, nature and the sea.', N'This tour will take you on a unique adventure and offers you the opportunity to experience the beautiful Maltese waters. Our qualified instructors will take you to the best sea locations where you will be able to explore the incredible life in our seas, and see several beautiful water creatures. This tour will take place in Dwejra, Gozo, where one will find one of the most magnificent natural landmark of this island, which is the Azure Window.', CAST(30.00 AS Decimal(10, 2)), CAST(25.00 AS Decimal(10, 2)), 15, 6, 4, CAST(N'2016-05-19 14:18:25.233' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Tour] OFF
GO
SET IDENTITY_INSERT [dbo].[TourDate] ON 

GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (313, 12, CAST(N'2016-05-20' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (314, 12, CAST(N'2016-05-25' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (315, 12, CAST(N'2016-05-30' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (316, 12, CAST(N'2016-06-05' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (317, 12, CAST(N'2016-06-10' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (318, 12, CAST(N'2016-06-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (319, 12, CAST(N'2016-06-16' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (320, 12, CAST(N'2016-06-17' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (321, 12, CAST(N'2016-06-23' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (322, 12, CAST(N'2016-06-28' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (323, 12, CAST(N'2016-06-30' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (324, 12, CAST(N'2016-07-05' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (325, 12, CAST(N'2016-07-10' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (326, 12, CAST(N'2016-07-15' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (327, 12, CAST(N'2016-07-20' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (328, 12, CAST(N'2016-07-25' AS Date))
GO
INSERT [dbo].[TourDate] ([Id], [TourId], [DateOfTour]) VALUES (329, 12, CAST(N'2016-07-30' AS Date))
GO
SET IDENTITY_INSERT [dbo].[TourDate] OFF
GO
SET IDENTITY_INSERT [dbo].[Image] ON 

GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (58, 12, N'~/Images/rsz_tour12_1.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (59, 12, N'~/Images/product_details/tour12_1.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (60, 12, N'~/Images/product_details/tour12_2.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (61, 12, N'~/Images/product_details/tour12_3.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (62, 12, N'~/Images/product_details/tour12_4.jpg')
GO
INSERT [dbo].[Image] ([Id], [TourId], [Link]) VALUES (63, 12, N'~/Images/product_details/tour12_5.jpg')
GO
SET IDENTITY_INSERT [dbo].[Image] OFF
GO
SET IDENTITY_INSERT [dbo].[TourTime] ON 

GO
INSERT [dbo].[TourTime] ([Id], [TourId], [StartTime], [EndTime]) VALUES (12, 12, N'09:00', N'11:00')
GO
INSERT [dbo].[TourTime] ([Id], [TourId], [StartTime], [EndTime]) VALUES (13, 12, N'15:00', N'17:00')
GO
SET IDENTITY_INSERT [dbo].[TourTime] OFF
GO
SET IDENTITY_INSERT [dbo].[Location] ON 

GO
INSERT [dbo].[Location] ([Id], [Name], [TownId], [Longitude], [Latitude]) VALUES (34, N'Dwejra Bay', 24, 14.1888459, 36.0468326)
GO
SET IDENTITY_INSERT [dbo].[Location] OFF
GO
SET IDENTITY_INSERT [dbo].[LocationAttraction] ON 

GO
INSERT [dbo].[LocationAttraction] ([Id], [LocationId], [AttractionTypeId]) VALUES (64, 34, 1)
GO
INSERT [dbo].[LocationAttraction] ([Id], [LocationId], [AttractionTypeId]) VALUES (65, 34, 13)
GO
SET IDENTITY_INSERT [dbo].[LocationAttraction] OFF
GO
SET IDENTITY_INSERT [dbo].[TourTimeTable] ON 

GO
INSERT [dbo].[TourTimeTable] ([Id], [LocationId], [StartTime], [EndTime], [TourTimeId]) VALUES (23, 34, N'09:00', N'11:00', 12)
GO
INSERT [dbo].[TourTimeTable] ([Id], [LocationId], [StartTime], [EndTime], [TourTimeId]) VALUES (24, 34, N'15:00', N'17:00', 13)
GO
SET IDENTITY_INSERT [dbo].[TourTimeTable] OFF
GO
SET IDENTITY_INSERT [dbo].[TourDateTime] ON 

GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (366, 313, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (367, 314, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (368, 315, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (369, 316, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (370, 317, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (371, 318, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (372, 319, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (373, 320, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (374, 321, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (375, 322, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (376, 323, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (377, 324, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (378, 325, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (379, 326, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (380, 327, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (381, 328, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (382, 329, 12)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (383, 313, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (384, 314, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (385, 315, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (386, 316, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (387, 317, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (388, 318, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (389, 319, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (390, 320, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (391, 321, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (392, 322, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (393, 323, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (394, 324, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (395, 325, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (396, 326, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (397, 327, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (398, 328, 13)
GO
INSERT [dbo].[TourDateTime] ([Id], [TourDateId], [TourTimeId]) VALUES (399, 329, 13)
GO
SET IDENTITY_INSERT [dbo].[TourDateTime] OFF
GO