
SELECT TOP 5
	cust_Id, memb.*
FROM 
	[V5_Vubz].[dbo].[Cust] INNER JOIN
	[V5_Vubz].[dbo].[Memb] ON Cust_AcctId = Memb_AcctId
where 
	Cust_Id = 'WSCC3MPD'
	AND
	(memb_Internal = 1 OR memb_Id like '%_SALES')


	Order by Cust_No desc
