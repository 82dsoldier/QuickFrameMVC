BEGIN TRAN

	CREATE TABLE SiteUsers (
		Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		UserId NVARCHAR(128) NOT NULL UNIQUE,
		DisplayName NVARCHAR(256) NOT NULL,
		FirstName NVARCHAR(64) NOT NULL,
		LastName NVARCHAR(64) NOT NULL,
		Email NVARCHAR(256),
		Phone NVARCHAR(32),
		Comments NVARCHAR(1024)
		)

	CREATE TABLE SiteRoles (
		Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(256) NOT NULL,
		Description NVARCHAR(2048)
		)

	CREATE TABLE SiteGroups (
		Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		GroupId NVARCHAR(128) NOT NULL UNIQUE,
		DisplayName NVARCHAR(256)
		)

	CREATE TABLE SiteUsers_SiteRoles (
		UserId INT NOT NULL REFERENCES SiteUsers(Id),
		RoleId INT NOT NULL REFERENCES SiteRoles(Id),
		PRIMARY KEY (UserId, RoleId)
		)

	CREATE TABLE SiteGroups_SiteRoles (
		GroupId INT NOT NULL REFERENCES SiteGroups(Id),
		RoleId INT NOT NULL REFERENCES SiteRoles(Id),
		PRIMARY KEY (GroupId, RoleId)
		)

	CREATE TABLE SiteUsers_SiteGroups (
		UserId INT NOT NULL REFERENCES SiteUsers(Id),
		GroupId INT NOT NULL REFERENCES SiteGroups(Id)
		PRIMARY KEY (UserId, GroupId)
		)

	CREATE TABLE SiteGroups_SiteGroups (
		MemberGroupId INT NOT NULL REFERENCES SiteGroups(Id),
		GroupId INT NOT NULL REFERENCES SiteGroups(Id)
		PRIMARY KEY (MemberGroupId, GroupId)
		)
	GO

	INSERT INTO SiteRoles(Name, Description) VALUES ('Administrators', 'Site administrators.  Have full control over all site functionality.');
	INSERT INTO SiteRoles(Name, Description) VALUES ('Supervisors', 'Site supervisors.  Limited administration functionality.');
	INSERT INTO SiteRoles(Name, Description) VALUES ('Users', 'Site users.');

	INSERT INTO SiteGroups(GroupId, DisplayName) VALUES ('S-1-5-21-2390488714-2608659811-2265349600-12636', 'FPDPIT');
	INSERT INTO SiteGroups(GroupId, DisplayName) VALUES ('Administrators', 'Administrators');
	INSERT INTO SiteGroups(GroupId, DisplayName) VALUES ('Supervisors', 'Supervisors');
	INSERT INTO SiteGroups(GroupId, DisplayName) VALUES ('Users', 'Users');

	INSERT INTO SiteUsers(UserId, DisplayName, FirstName, LastName, Email) VALUES ('S-1-5-21-2390488714-2608659811-2265349600-4828', 'Burchett, Drew', 'Drew', 'Burchett', 'Drew.Burchett@ffspaducah.com');
	INSERT INTO SiteUsers(UserId, DisplayName, FirstName, LastName, Email) VALUES ('S-1-5-21-2390488714-2608659811-2265349600-11587', 'Petway, Jacob', 'Jacob', 'Petway', 'Jacob.Petway@ffspaducah.com');
	INSERT INTO SiteUsers(UserId, DisplayName, FirstName, LastName, Email) VALUES ('S-1-5-21-2390488714-2608659811-2265349600-4827', 'Barks, Robbie', 'Robbie', 'Barks', 'Robbie.Barks@ffspaducah.com');
	INSERT INTO SiteUsers(UserId, DisplayName, FirstName, LastName, Email) VALUES ('S-1-5-21-2390488714-2608659811-2265349600-2801', 'Reason.Jim', 'Jim', 'Reason', 'Jim.Reason@ffspaducah.com');

	GO

	DECLARE @UserId INT
	DECLARE @GroupId INT
	DECLARE @RoleId INT

	SELECT @UserId = Id FROM SiteUsers WHERE UserId='S-1-5-21-2390488714-2608659811-2265349600-4828'
	SELECT @GroupId = Id FROM SiteGroups WHERE GroupId='Administrators'

	INSERT INTO SiteUsers_SiteGroups (UserId, GroupId) VALUES (@UserId, @GroupId)

	SELECT @UserId = Id FROM SiteUsers WHERE UserId='S-1-5-21-2390488714-2608659811-2265349600-11587'

	INSERT INTO SiteUsers_SiteGroups (UserId, GroupId) VALUES (@UserId, @GroupId)

	SELECT @UserId = Id FROM SiteUsers WHERE UserId='S-1-5-21-2390488714-2608659811-2265349600-4827'
	SELECT @GroupId = Id FROM SiteGroups WHERE GroupId='Supervisors'

	INSERT INTO SiteUsers_SiteGroups (UserId, GroupId) VALUES (@UserId, @GroupId)

	SELECT @UserId = Id FROM SiteGroups WHERE GroupId='S-1-5-21-2390488714-2608659811-2265349600-12636'
	SELECT @GroupId = Id FROM SiteGroups WHERE GroupId='Users'

	INSERT INTO SiteGroups_SiteGroups (MemberGroupId, GroupId) VALUES (@UserId, @GroupId)

	SELECT @GroupId = Id FROM SiteGroups WHERE GroupId='Adminstrators'
	SELECT @RoleId = Id FROM SiteRoles WHERE Name='Administrators'

	INSERT INTO SiteGroups_SiteRoles (GroupId, RoleId) VALUES (@GroupId, @RoleId)

	SELECT @GroupId = Id FROM SiteGroups WHERE GroupId='Supervisors'
	SELECT @RoleId = Id FROM SiteRoles WHERE Name='Supervisors'

	INSERT INTO SiteGroups_SiteRoles (GroupId, RoleId) VALUES (@GroupId, @RoleId)
	
	SELECT @GroupId = Id FROM SiteGroups WHERE GroupId='Users'
	SELECT @RoleId = Id FROM SiteRoles WHERE Name='Users'

	INSERT INTO SiteGroups_SiteRoles (GroupId, RoleId) VALUES (@GroupId, @RoleId)

	SELECT @UserId = Id FROM SiteUsers WHERE UserId='S-1-5-21-2390488714-2608659811-2265349600-2801'
	SELECT @RoleId = Id FROM SiteRoles WHERE Name='Supervisors'

	INSERT INTO SiteUsers_SiteRoles (UserId, RoleId) VALUES (@UserId, @RoleId)

COMMIT TRAN