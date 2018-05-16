Create table Medicines
(
ID int not null identity(1,1) primary key,
Name varchar(200) not null, 
Manufacturer varchar(200) not null, 
Price money not null, 
Amount decimal(10,2) not null,
WithPrescription bit default ('false')
)

Create table Prescriptions
(
ID int not null identity(1,1) primary key,
CustomerName varchar(200) not null, 
PESEL char(11) not null, 
PrescriptionNumber varchar(20) not null
)

create table Orders
(
ID int not null identity(1,1) primary key,
PrescriptionID int,
MedicineID int not null,
Date datetime not null,
Amount decimal(10,2) not null
)

alter table Orders
add constraint FK_Orders_Prescriptions
foreign key (PrescriptionID)
references Prescriptions (ID)

alter table Orders
add constraint FK_Orders_Medicines
foreign key (MedicineID)
references Medicines (ID)

--Medicines
select ', '+COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Medicines'
for xml path('')

select ', @'+lower(COLUMN_NAME) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Medicines'
for xml path('')

select ' ,' + COLUMN_NAME + ' = @'+lower(COLUMN_NAME) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Medicines'
for xml path('')

select * from Medicines

--Orders
select ', '+COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Orders'
for xml path('')

select ', @'+lower(COLUMN_NAME) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Orders'
for xml path('')

select ' ,' + COLUMN_NAME + ' = @'+lower(COLUMN_NAME) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Orders'
for xml path('')

select * from Orders

select top 1 ID, PrescriptionID, MedicineID, Date, Amount from Orders where ID = 1


--Prescriptions
select ', '+COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Prescriptions'
for xml path('')

select ', @'+lower(COLUMN_NAME) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Prescriptions'
for xml path('')

select ' ,' + COLUMN_NAME + ' = @'+lower(COLUMN_NAME) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Prescriptions'
for xml path('')

select * from Prescriptions

select * from Orders o
left join Prescriptions p 
on o.PrescriptionID = p.ID
left join Medicines m
on o.MedicineID = m.ID