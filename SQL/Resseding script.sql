USE DVDOrama_DB;
GO

DELETE FROM MovieCollection;
DELETE FROM Users;

-- Reset identity counters so IDs start from 1 again
DBCC CHECKIDENT ('MovieCollection', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);
GO