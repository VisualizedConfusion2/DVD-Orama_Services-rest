USE master;
IF DB_ID('DvDOrama_DB') IS NOT NULL
BEGIN
    -- Force DB into single-user mode, rolling back any active connections
    ALTER DATABASE [DvDOrama_DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [DvDOrama_DB];
END

CREATE DATABASE DvDOrama_DB;
USE DvDOrama_DB;

Create Table Movies(
    MovieId     INT IDENTITY PRIMARY KEY NOT NULL,
    Title       NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Duration    INT NOT NULL,
    PublicationYear INT NOT NULL,
    PosterURL   NVARCHAR(500) NOT NULL
);

Create Table Actors(
    ActorId INT IDENTITY PRIMARY KEY NOT NULL,
    Name    NVARCHAR(255) NOT NULL
);

Create Table MoviesActorMap(
    ActorId INT NOT NULL,
    MovieId INT NOT NULL,
    FOREIGN KEY (ActorId) REFERENCES Actors(ActorId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    PRIMARY KEY (MovieId, ActorId)
);

Create Table Genres(
    GenreId INT IDENTITY PRIMARY KEY NOT NULL,
    Genre   NVARCHAR(100) NOT NULL
);

Create Table MoviesGenresMap(
    GenreId INT NOT NULL,
    MovieId INT NOT NULL,
    FOREIGN KEY (GenreId) REFERENCES Genres(GenreId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    PRIMARY KEY (MovieId, GenreId)
);

Create Table MovieEAN(
    EAN     BIGINT PRIMARY KEY NOT NULL,
    MovieId INT NOT NULL,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);

Create Table StreamingLocations(
    StreamingServiceId   INT PRIMARY KEY IDENTITY NOT NULL,
    StreamingServiceName NVARCHAR(255) NOT NULL,
    URL                  NVARCHAR(500) NOT NULL,
    MovieId              INT NOT NULL,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);

Create Table MovieCollections(
    MovieCollectionId INT PRIMARY KEY IDENTITY NOT NULL,
    Name     NVARCHAR(255) NOT NULL,
    IsPublic BIT NOT NULL DEFAULT 0
);

Create Table MovieCollectionsMoviesMap(
    MovieCollectionId INT NOT NULL,
    MovieId           INT NOT NULL,
    FOREIGN KEY (MovieCollectionId) REFERENCES MovieCollections(MovieCollectionId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    PRIMARY KEY (MovieId, MovieCollectionId)
);

Create Table Users(
    UserId      INT PRIMARY KEY IDENTITY NOT NULL,
    Username    NVARCHAR(MAX) NOT NULL,
    Email       NVARCHAR(MAX) NOT NULL
);

Create Table Roles(
    RoleId INT PRIMARY KEY IDENTITY NOT NULL,
    RoleName NVARCHAR(255) NOT NULL
);


Create Table UserMovieCollectionMap(
    UserId              INT NOT NULL,
    MovieCollectionId   INT NOT NULL,
    RoleId                INT NOT NULL,
    FOREIGN KEY (MovieCollectionId) REFERENCES MovieCollections(MovieCollectionId),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
);

-- Seed roles
INSERT INTO Roles (RoleName) VALUES ('Owner');    -- RoleId 1: owns collection, full control including delete
INSERT INTO Roles (RoleName) VALUES ('Co-Owner'); -- RoleId 2: invite, add/remove/edit movies, cannot delete collection
INSERT INTO Roles (RoleName) VALUES ('Admin');    -- RoleId 3: add/remove/edit movies only
INSERT INTO Roles (RoleName) VALUES ('Viewer');   -- RoleId 4: read-only access