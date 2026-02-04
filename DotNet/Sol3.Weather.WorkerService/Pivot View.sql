use Weather
go

DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX)

;select @cols = STUFF((
  SELECT DISTINCT
    replace(replace(replace(
      ', max(iif([Name]=val, [Temp], null)) as coltemp, max(iif([Name] = val, [Pressure], null)) as colpres
'
      /*Substitute quoted value string*/
      , 'val', quotename([Name], '''')
      )
      /*Substitute temperature column name*/
      , 'coltemp', QUOTENAME([Name] + ' temp')
      )
      /*Substitute pressure column name*/
      , 'colpres', quotename([Name] + ' pressure')
      )
  from WeatherResponse
  group by [Name]
  FOR XML PATH(''), TYPE
).value('.', 'NVARCHAR(MAX)'), 1,1,'')

;set @query = '
SELECT Created
,' + @cols + '
from (
  select
    r.Created
    , w.Temp --((w.Temp-273.35) * 9) /5 + 32 as Temp
    , pressure
    , r.Name
  from WeatherResponse r
    inner join Mains w
      on w.WeatherResponseId = r.WeatherResponseId
 where r.Created > ''2021-11-15 23:30:03.0445388''
) as x
group by created
order by created
'

;execute( @query)
GO


use Weather
go

DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX)

;select @cols = STUFF((
  SELECT DISTINCT
    replace(replace(replace(
      ', max(iif([Name]=val, [Temp], null)) as coltemp, max(iif([Name] = val, [Pressure], null)) as colpres'
      /*Substitute quoted value string*/
      , 'val', quotename([Name], '''')
      )
      /*Substitute temperature column name*/
      , 'coltemp', QUOTENAME([Name] + ' temp')
      )
      /*Substitute pressure column name*/
      , 'colpres', quotename([Name] + ' pressure')
      )
  from WeatherResponse
  where [Name] not in ('Warsaw','Sedalia')
  group by [Name]
  FOR XML PATH(''), TYPE
).value('.', 'NVARCHAR(MAX)'), 1,1,'')

;set @query = '
SELECT Created
,' + @cols + '
from (
  select
    r.Created
    , pressure
    , r.Name
  from WeatherResponse r
    inner join Mains w
      on w.WeatherResponseId = r.WeatherResponseId
 where r.Created > ''2021-11-15 23:30:03.0445388''
) as x
group by created
order by created
'

;execute( @query)
GO