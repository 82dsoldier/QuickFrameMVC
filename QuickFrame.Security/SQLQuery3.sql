INSERT INTO SiteUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, PhoneNumber, DisplayName) VALUES
('S-1-5-21-2390488714-2608659811-2265349600-4828', 'Drew.Burchett', 'DREW.BURCHETT', 'drew.burchett@ffspaducah.com', 'drew.burchett@ffspaducah.com', '270-441-6774', 'Burchett, Drew')

INSERT INTO SiteRoles (Id, Name, NormalizedName) VALUES
('31F68DBF-862A-4139-8795-ED2B08C55BF5', 'Users', 'USERS')

INSERT INTO SiteUserClaims (UserId, ClaimType, ClaimValue) VALUES
('S-1-5-21-2390488714-2608659811-2265349600-4828', 'http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid', 'Users')

INSERT INTO SiteUsers_SiteRoles (UserId, RoleId) VALUES
('S-1-5-21-2390488714-2608659811-2265349600-4828', '31F68DBF-862A-4139-8795-ED2B08C55BF5')
