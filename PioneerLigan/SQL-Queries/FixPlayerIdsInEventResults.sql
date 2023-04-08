
UPDATE [PioneerLigan].[dbo].[EventResult]
SET [PioneerLigan].[dbo].[EventResult].PlayerId = [PioneerLigan].[dbo].[Player].Id
FROM [PioneerLigan].[dbo].[EventResult]
INNER JOIN [PioneerLigan].[dbo].[Player] ON [PioneerLigan].[dbo].[EventResult].PlayerName = [PioneerLigan].[dbo].[Player].PlayerName;