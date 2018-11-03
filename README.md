# Pharmacy

*Read this in other languages: [Polish](README.pl-PL.md)

Pharamcy is a console application that supports pharmacies in medicines management and registration of activities related to the sale of medicines.

## Installation:

In order to run the application, a MS SQL database is required. New database can be created or can be restored from provided backup with sample data.

### I. New database 

1. create new database PharmacyDb using following script ```create database PharmacyDb```
1. create table People by running script **\scripts\Create_tables.sql**
1. add sample data by running script **\scripts\Sample_data.sql**

### II. SampleDB

1. restore database backup with sample data. Backup can be found in folder **SampleDB**

## Sample data:

Przykładowe dane zawierają:
1. zestaw przykładowy leków
1. wprowadzone przykładowe zamówienia

Sample data contains:
1. medicines
1. orders
    
## Available modules:

### Medicines Management module

Module allows for creation, modification, deletion, filtering and browsing list of medicines. To enter new medicine all required data must be provided such as: name, manufacturer, price, quantity and information if medicine is a prescription.

#### Functionalities
* Creation of new medicine 
* Modification of created medicine
* Deletion of created medicine
* Displaying list of available medicines
* Browsing/Filtering available medicines

### Order/Sales medicine module

Module allows for registering orders related to sales medicines. While medicine sale user need to enter sales date, quantity of sold medicine and if medicine is a prescription also first and last name of client, his PESEL and prescription number.

#### Functionalities
* Creation of sales medicine order

## Application:

Application does not require from user any specific persmissions to create, modifiy and delete new entries. All users have access to all functionalities without any restrictions. 

##### to be continued.
