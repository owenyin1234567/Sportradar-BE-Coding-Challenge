Instruction:
- Download Xampp at (https://www.apachefriends.org/de/index.html)
- Start Xampp
- Copy and Insert MySQL Queries from sql section into phpmyadmin and execute
- start Sportradar API sln (testing done on swagger)
- insert my Frontend files into your htdocs
- Open the Homepage.hmtml
------------------------------------------------------
- DB Design scheme
- (See Blank Diagram.jpeg)

- Backend: C# WebAPI
- Frontend: HTML, CSS, JS (JQuery),
- DB: MySQL

AddGame Method!
- (dateonly)Dateofevent + (int)SpordID required
- Starting/EndTime format HH:MM

GetGameWithSportById Method use int>=1

Check the ConnectionString incase mysql doesnt connect!
-------------------------------------------------------------------
sql section:
DROP TABLE IF EXISTS Game;
DROP TABLE IF EXISTS Result;
DROP TABLE IF EXISTS Season;
DROP TABLE IF EXISTS Championship;
DROP TABLE IF EXISTS Team;
DROP TABLE IF EXISTS Sport;
DROP TABLE IF EXISTS Country;

-- create
CREATE TABLE Country (
  Abbr VARCHAR(16) PRIMARY KEY,
  name VARCHAR(256)
);

CREATE TABLE Sport (
  ID INTEGER AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(256) NOT NULL
);

CREATE TABLE Championship (
  ID INTEGER AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(256) NOT NULL,
  Sport_FK INTEGER,
  FOREIGN KEY (Sport_FK) REFERENCES Sport(ID)
);

CREATE TABLE Team (
  Slug VARCHAR(256) PRIMARY KEY,
  name VARCHAR(256),
  OfficialName VARCHAR(256),
  Abbreviation VARCHAR(256),
  Country_FK VARCHAR(16),
  Sport_Team_FK INTEGER,
  FOREIGN KEY (Sport_Team_FK) REFERENCES Sport(ID),
  FOREIGN KEY (Country_FK) REFERENCES Country(Abbr)
);

CREATE TABLE Season (
  ID INTEGER AUTO_INCREMENT PRIMARY KEY,
  Year date NOT NULL,
  Championship_FK INTEGER,
  Season_Winner_FK VARCHAR(256),
  Sport_Season_FK INTEGER,
  FOREIGN KEY (Sport_Season_FK) REFERENCES Sport(ID),
  FOREIGN KEY (Championship_FK) REFERENCES Championship(ID),
  FOREIGN KEY (Season_Winner_FK) REFERENCES Team(Slug)

);

CREATE TABLE Result (
  ID INTEGER AUTO_INCREMENT PRIMARY KEY,
  HomeGoals INTEGER NULL,
  AwayGoals INTEGER NULL,
  Winner VARCHAR(256),
  FOREIGN KEY (Winner) REFERENCES Team(Slug)
);
 

CREATE TABLE Game (
  ID int AUTO_INCREMENT PRIMARY KEY,
  Dateofevent Date,
  StartingTime time, 
  EndTime time, 
  HomeTeam_FK VARCHAR(256),
  AwayTeam_FK VARCHAR(256),
  Season_FK int,
  Result_FK int,
  Sport_Game_FK INTEGER,
  FOREIGN KEY (Sport_Game_FK) REFERENCES Sport(ID),
  FOREIGN KEY (HomeTeam_FK) REFERENCES Team(Slug),
  FOREIGN KEY (AwayTeam_FK) REFERENCES Team(Slug),
  FOREIGN KEY (Season_FK) REFERENCES Season(ID),
  FOREIGN KEY (Result_FK) REFERENCES Result(ID)
);


-- insert
INSERT INTO Country (Abbr, Name) VALUES 
('USA', 'United States'),
('GER', 'Germany'),
('BRA', 'Brazil'),
('ESP', 'Spain'),
('AUT', 'Austria'); 


INSERT INTO Sport (name) VALUES
('Football'),
('Ice Hockey'),
('Basketball');

INSERT INTO Championship (name, Sport_FK) VALUES
('Austrian Bundesliga', 1),
('Austrian Ice Hockey League', 2),
('NBA', 3);

INSERT INTO Team (Slug, name, OfficialName, Abbreviation, Country_FK, Sport_Team_FK) VALUES
('salzburg', 'FC Salzburg', 'Fussball Club Salzburg', 'FCS', 'AUT', 1),
('sturm', 'Sturm Graz', 'Sportklub Sturm Graz', 'STG', 'AUT', 1),
('kac', 'KAC', 'Klagenfurt Athletic Club', 'KAC', 'AUT', 2),
('capitals', 'Vienna Capitals', 'Vienna Capitals Ice Hockey', 'CAP', 'AUT', 2),
('lakers', 'Los Angeles Lakers', 'Los Angeles Lakers Basketball', 'LAL', 'USA', 3),
('heat', 'Miami Heat', 'Miami Heat Basketball', 'MIA', 'USA', 3);

INSERT INTO Season (Year, Championship_FK, Season_Winner_FK, Sport_Season_FK) VALUES
('2019-01-01', 1, 'salzburg', 1),
('2020-01-01', 2, 'kac', 2),
('2021-01-01', 3, NULL, 3);

INSERT INTO Result (HomeGoals, AwayGoals, Winner) VALUES
(2, 1, 'salzburg'),
(3, 2, 'kac'),
(0, 0, NULL),
(4, 3, 'sturm'),
(5, 2, 'capitals'),
(1, 1, NULL),
(3, 0, 'salzburg'),
(6, 4, 'capitals');


INSERT INTO Game (Dateofevent, StartingTime, EndTime, HomeTeam_FK, AwayTeam_FK, Season_FK, Result_FK, Sport_Game_FK) VALUES
('2019-07-18', '18:30:00', '20:30:00', 'salzburg', 'sturm', 1, 1, 1),
('2019-10-23', '09:45:00', '11:45:00', 'kac', 'capitals', 2, 2, 2),
('2021-12-15', '19:00:00', '21:30:00', 'salzburg', 'capitals', 3, 3, 3),
('2020-01-12', '15:00:00', '17:30:00', 'sturm', 'salzburg', 1, 4, 1),
('2020-03-25', '19:00:00', '21:00:00', 'capitals', 'kac', 2, 5, 2),
('2021-04-10', '14:30:00', '16:30:00', 'sturm', 'capitals', 3, 6, 3),
('2022-05-15', '20:00:00', '22:15:00', 'salzburg', 'kac', 1, 7, 1),
('2023-07-22', '12:00:00', '14:30:00', 'capitals', 'sturm', 2, 8, 2);


-- fetch 
SELECT * FROM Country;
SELECT * FROM Team;
SELECT * FROM Sport;
SELECT * FROM Championship;
SELECT * FROM Season;
SELECT * FROM Result;
SELECT * FROM Game;
