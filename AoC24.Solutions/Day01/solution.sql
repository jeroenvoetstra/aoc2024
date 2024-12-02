if exists (select 1 from sys.tables where name = 'aoc_day0101')
	drop table aoc_day0101
go

create table aoc_day0101 (
	num1 int,
	num2 int,
	index nc_num1 nonclustered (
		num1 asc
	),
	index nc_num2 nonclustered (
		num2 asc
	)
)
go

bulk insert aoc_day0101 
from N'<file_path, string, >'
with (
    rowterminator = '\n',
	fieldterminator = '   '
)
go

-- DAY 1 PART 1
select	sum(abs(t2.num2 - t1.num1))
from	(
	select	num1, row_number() over (order by num1) as rn
	from	aoc_day0101
) t1
inner join (
	select	num2, row_number() over (order by num2) as rn
	from	aoc_day0101
) t2 on t1.rn = t2.rn

-- DAY 1 PART 2
select	sum(aggr)
from	(
	select	num1 * (select count(1) from aoc_day0101 where num2 = t1.num1) as aggr
	from	aoc_day0101 t1
) t2
go

drop table aoc_day0101
go
