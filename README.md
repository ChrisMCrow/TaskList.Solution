# TaskList Website

#### A website that allows users to record their tasks. 9/25/18

#### By **Chris Crow**

## Description

A website that allows users to record their tasks.

## Setup/Installation Requirements

1. Clone this repository.
2. Create a database using the following SQL commands:
  > CREATE DATABASE Todo;

  > USE Todo;

  > CREATE TABLE categories (id serial PRIMARY KEY, name VARCHAR(255));

  > CREATE TABLE tasks (id serial PRIMARY KEY, description VARCHAR(255), due_date VARCHAR(255));

  > CREATE TABLE categories_tasks (id serial PRIMARY KEY, category_id int(11), task_id INT(11));
  
3. Navigate to the TaskList folder in command shell and use the following commands:
  > dotnet restore

  > dotnet run

## Known Bugs
* No known bugs at this time.

## Technologies Used
* C#
* MAMP
* SQL
* Bootstrap

## Support and Contact Details

_Email chrismcrow@gmail.com._

### License

*none*

Copyright (c) 2018 **_Chris Crow_**
