USE DvDOrama_DB;

-- ─── Genres ───────────────────────────────────────────────────────────────────
INSERT INTO Genres (Genre) VALUES
('Action'),        -- 1
('Sci-Fi'),        -- 2
('Drama'),         -- 3
('Crime'),         -- 4
('Thriller'),      -- 5
('Animation'),     -- 6
('Adventure'),     -- 7
('Fantasy');       -- 8

-- ─── Actors ───────────────────────────────────────────────────────────────────
INSERT INTO Actors (Name) VALUES
('Keanu Reeves'),       -- 1
('Laurence Fishburne'), -- 2
('Carrie-Anne Moss'),   -- 3
('Tom Hanks'),          -- 4
('Robin Wright'),       -- 5
('Morgan Freeman'),     -- 6
('Tim Robbins'),        -- 7
('Marlon Brando'),      -- 8
('Al Pacino'),          -- 9
('Leonardo DiCaprio'),  -- 10
('Joseph Gordon-Levitt'),-- 11
('Ellen Page'),         -- 12
('Liam Neeson'),        -- 13
('Harrison Ford'),      -- 14
('Mark Hamill');        -- 15

-- ─── Movies ───────────────────────────────────────────────────────────────────
INSERT INTO Movies (Title, Description, Duration, PublicationYear, PosterURL) VALUES
('The Matrix',
 'A computer hacker learns the world is a simulation and joins a rebellion against its machine controllers.',
 136, 1999,
 'https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVlLTM5YTUtZGMyZTkzZTlkZTNlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg'),

('Forrest Gump',
 'The presidencies of Kennedy and Johnson, the Vietnam War, and other historical events unfold through the perspective of an Alabama man with a low IQ.',
 142, 1994,
 'https://m.media-amazon.com/images/M/MV5BNWIwODRlZTUtY2U3ZS00Yzg1LWJhNzYtMmZiYmEyNmU1NjMzXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_.jpg'),

('The Shawshank Redemption',
 'Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.',
 142, 1994,
 'https://m.media-amazon.com/images/M/MV5BNDE3ODcxYzMtY2YzZC00NiYyLTg3MzMtYjIyNWI5YjU3NWVhXkEyXkFqcGdeQXVyNjAwNDUxODI@._V1_.jpg'),

('The Godfather',
 'The aging patriarch of an organized crime dynasty transfers control to his reluctant son.',
 175, 1972,
 'https://m.media-amazon.com/images/M/MV5BM2MyNjYxNmUtYTAwNi00MTYxLWJmNWYtYzZlODY3ZTk3OTFlXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg'),

('Inception',
 'A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.',
 148, 2010,
 'https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg'),

('Star Wars: A New Hope',
 'Luke Skywalker joins forces with a Jedi Knight and a roguish smuggler to rescue a princess and save the galaxy.',
 121, 1977,
 'https://m.media-amazon.com/images/M/MV5BOTA5NjhiOTAtZWM0ZC00MWNhLThiMzEtZDFkOTk2OTU1ZDJkXkEyXkFqcGdeQXVyMTA4NDI1NTQx._V1_.jpg'),

('Taken',
 'A retired CIA agent travels across Europe to save his estranged daughter who has been kidnapped.',
 90, 2008,
 'https://m.media-amazon.com/images/M/MV5BMTQ4Njk4NzQ1Nl5BMl5BanBnXkFtZTcwMDkzMzQzMg@@._V1_.jpg');

-- ─── Movie ↔ Genre mappings ───────────────────────────────────────────────────
-- The Matrix (1): Action, Sci-Fi
INSERT INTO MoviesGenresMap (GenreId, MovieId) VALUES (1, 1), (2, 1);
-- Forrest Gump (2): Drama
INSERT INTO MoviesGenresMap (GenreId, MovieId) VALUES (3, 2);
-- Shawshank (3): Drama, Crime
INSERT INTO MoviesGenresMap (GenreId, MovieId) VALUES (3, 3), (4, 3);
-- Godfather (4): Crime, Drama
INSERT INTO MoviesGenresMap (GenreId, MovieId) VALUES (4, 4), (3, 4);
-- Inception (5): Action, Sci-Fi, Thriller
INSERT INTO MoviesGenresMap (GenreId, MovieId) VALUES (1, 5), (2, 5), (5, 5);
-- Star Wars (6): Action, Adventure, Fantasy
INSERT INTO MoviesGenresMap (GenreId, MovieId) VALUES (1, 6), (7, 6), (8, 6);
-- Taken (7): Action, Thriller
INSERT INTO MoviesGenresMap (GenreId, MovieId) VALUES (1, 7), (5, 7);

-- ─── Movie ↔ Actor mappings ───────────────────────────────────────────────────
-- The Matrix (1)
INSERT INTO MoviesActorsMap (ActorId, MovieId) VALUES (1, 1), (2, 1), (3, 1);
-- Forrest Gump (2)
INSERT INTO MoviesActorsMap (ActorId, MovieId) VALUES (4, 2), (5, 2);
-- Shawshank (3)
INSERT INTO MoviesActorsMap (ActorId, MovieId) VALUES (6, 3), (7, 3);
-- Godfather (4)
INSERT INTO MoviesActorsMap (ActorId, MovieId) VALUES (8, 4), (9, 4);
-- Inception (5)
INSERT INTO MoviesActorsMap (ActorId, MovieId) VALUES (10, 5), (11, 5), (12, 5);
-- Star Wars (6)
INSERT INTO MoviesActorsMap (ActorId, MovieId) VALUES (14, 6), (15, 6);
-- Taken (7)
INSERT INTO MoviesActorsMap (ActorId, MovieId) VALUES (13, 7);

-- ─── EANs ─────────────────────────────────────────────────────────────────────
INSERT INTO MovieEAN (EAN, MovieId) VALUES
(5051892012966, 1),  -- The Matrix
(5051892013000, 2),  -- Forrest Gump
(5051892013017, 3),  -- Shawshank
(5051892013024, 4),  -- Godfather
(5051892013031, 5),  -- Inception
(5051892013048, 6),  -- Star Wars
(5051892013055, 7);  -- Taken

-- ─── Streaming Locations ──────────────────────────────────────────────────────
INSERT INTO StreamingLocations (StreamingServiceName, URL, MovieId) VALUES
('Netflix',      'https://www.netflix.com/title/20557937',   1),
('HBO Max',      'https://www.hbomax.com/forrest-gump',      2),
('Netflix',      'https://www.netflix.com/title/70005379',   3),
('Prime Video',  'https://www.amazon.com/dp/B00BFQF1VS',     4),
('Netflix',      'https://www.netflix.com/title/70131314',   5),
('Disney+',      'https://www.disneyplus.com/movies/star-wars-a-new-hope/1I4h3HFGIzXC', 6),
('Prime Video',  'https://www.amazon.com/dp/B001GJ4F02',     7);

-- ─── Users ────────────────────────────────────────────────────────────────────
INSERT INTO Users (Username, Email) VALUES
('alice',   'alice@example.com'),   -- UserId 1
('bob',     'bob@example.com'),     -- UserId 2
('charlie', 'charlie@example.com'), -- UserId 3
('diana',   'diana@example.com');   -- UserId 4

-- ─── Movie Collections ────────────────────────────────────────────────────────
INSERT INTO MovieCollections (Name, IsPublic) VALUES
('Alice''s Sci-Fi Picks',   1),  -- Id 1 - public
('Bob''s Crime Classics',   0),  -- Id 2 - private
('All-Time Favourites',     1),  -- Id 3 - public
('Charlie''s Hidden Gems',  0);  -- Id 4 - private

-- ─── User ↔ Collection role mappings ─────────────────────────────────────────
-- Collection 1 (Alice's Sci-Fi Picks)
--   Alice = Owner, Bob = Co-Owner, Charlie = Viewer
INSERT INTO UserMovieCollectionMap (UserId, MovieCollectionId, RoleId) VALUES
(1, 1, 1),  -- Alice   → Owner
(2, 1, 2),  -- Bob     → Co-Owner
(3, 1, 4);  -- Charlie → Viewer

-- Collection 2 (Bob's Crime Classics)
--   Bob = Owner, Diana = Admin
INSERT INTO UserMovieCollectionMap (UserId, MovieCollectionId, RoleId) VALUES
(2, 2, 1),  -- Bob   → Owner
(4, 2, 3);  -- Diana → Admin

-- Collection 3 (All-Time Favourites)
--   Charlie = Owner, Alice = Admin, Diana = Viewer
INSERT INTO UserMovieCollectionMap (UserId, MovieCollectionId, RoleId) VALUES
(3, 3, 1),  -- Charlie → Owner
(1, 3, 3),  -- Alice   → Admin
(4, 3, 4);  -- Diana   → Viewer

-- Collection 4 (Charlie's Hidden Gems)
--   Charlie = Owner, Bob = Viewer
INSERT INTO UserMovieCollectionMap (UserId, MovieCollectionId, RoleId) VALUES
(3, 4, 1),  -- Charlie → Owner
(2, 4, 4);  -- Bob     → Viewer

-- ─── Collection ↔ Movie mappings ─────────────────────────────────────────────
-- Collection 1: Alice's Sci-Fi Picks → The Matrix, Inception, Star Wars
INSERT INTO MovieCollectionsMoviesMap (MovieCollectionId, MovieId) VALUES
(1, 1), (1, 5), (1, 6);

-- Collection 2: Bob's Crime Classics → The Godfather, Shawshank
INSERT INTO MovieCollectionsMoviesMap (MovieCollectionId, MovieId) VALUES
(2, 4), (2, 3);

-- Collection 3: All-Time Favourites → Forrest Gump, Shawshank, The Matrix, Inception
INSERT INTO MovieCollectionsMoviesMap (MovieCollectionId, MovieId) VALUES
(3, 2), (3, 3), (3, 1), (3, 5);

-- Collection 4: Charlie's Hidden Gems → Taken, Forrest Gump
INSERT INTO MovieCollectionsMoviesMap (MovieCollectionId, MovieId) VALUES
(4, 7), (4, 2);