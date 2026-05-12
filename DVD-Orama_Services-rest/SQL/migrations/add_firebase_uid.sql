USE DvDOrama_DB;

-- Run this once to add FirebaseUid to the Users table
ALTER TABLE Users ADD FirebaseUid NVARCHAR(128) NULL;
