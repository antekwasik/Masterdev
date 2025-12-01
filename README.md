# Masterdev
Praktyki
--------------------
Baza danych
--------------------
CREATE TABLE Klienci(
    id int GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name varchar(50),
    surname varchar(50),
    pesel varchar(11),
    birthyear int,
    płeć int
)
