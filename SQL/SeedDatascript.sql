USE DVDOrama_DB;
GO

-- GET USER ID (user must already exist via /api/auth/register)
DECLARE @UserId INT = (SELECT Id FROM Users WHERE Username = 'testuser');

IF @UserId IS NULL
BEGIN
    RAISERROR('User "testuser" not found. Register via POST /api/auth/register first.', 16, 1);
    RETURN;
END

-- MOVIES INTO MOVIECOLLECTION
IF NOT EXISTS (SELECT 1 FROM MovieCollection WHERE UserId = @UserId)
BEGIN
    INSERT INTO MovieCollection (Barcode, UserId, CreatedAt) VALUES
    ('7332431033464', @UserId, GETUTCDATE()),  -- The Matrix
    ('5051895416721', @UserId, GETUTCDATE()),  -- Inception
    ('5051892190709', @UserId, GETUTCDATE()),  -- Interstellar
    ('7321900123456', @UserId, GETUTCDATE()),  -- The Dark Knight
    ('5050582548264', @UserId, GETUTCDATE());  -- Gladiator
END
GO