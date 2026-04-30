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

Create Table StreamingAvailability(
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


Create Table UserMovieCollectionMap(
    UserId              INT NOT NULL,
    MovieCollectionId   INT NOT NULL,
    Role                NVARCHAR(255) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (MovieCollectionId) REFERENCES MovieCollections(MovieCollectionId),
);