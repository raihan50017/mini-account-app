CREATE PROCEDURE sp_ManageChartOfAccounts
    @Action NVARCHAR(10), 
    @Id INT = NULL,       
    @AccountType NVARCHAR(50) = NULL,
    @AccountName NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'Create'
    BEGIN
        INSERT INTO ChartOfAccounts (AccountType, AccountName)
        VALUES (@AccountType, @AccountName);
    END
    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE ChartOfAccounts
        SET AccountType = @AccountType,
            AccountName = @AccountName
        WHERE Id = @Id;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        DELETE FROM ChartOfAccounts
        WHERE Id = @Id;
    END
END;