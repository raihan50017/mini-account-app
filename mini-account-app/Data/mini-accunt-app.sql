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


CREATE PROCEDURE sp_InsertVoucherEntry
    @VoucherNo NVARCHAR(50),
    @VoucherSerial INT,
    @VoucherType NVARCHAR(50),
    @VoucherDate DATE,
    @ReferenceNo NVARCHAR(100),
    @NewId INT OUTPUT
AS
BEGIN
    INSERT INTO VoucherEntry (VoucherNo, VoucherSerial, VoucherType, VoucherDate, ReferenceNo)
    VALUES (@VoucherNo, @VoucherSerial, @VoucherType, @VoucherDate, @ReferenceNo);

    SET @NewId = SCOPE_IDENTITY();
END;


CREATE PROCEDURE sp_InsertVoucherEntryDetail
    @MasterId INT,
    @AccountTypeId INT,
    @Debit FLOAT,
    @Credit FLOAT
AS
BEGIN
    INSERT INTO VoucherEntryDetails (MasterId, AccountTypeId, Debit, Credit)
    VALUES (@MasterId, @AccountTypeId, @Debit, @Credit);
END;

CREATE PROCEDURE sp_UpdateVoucherEntry
    @Id INT,
    @VoucherType NVARCHAR(50),
    @VoucherDate DATE,
    @ReferenceNo NVARCHAR(100)
AS
BEGIN
    UPDATE VoucherEntry
    SET VoucherType = @VoucherType,
        VoucherDate = @VoucherDate,
        ReferenceNo = @ReferenceNo
    WHERE Id = @Id;
END;


CREATE PROCEDURE sp_DeleteVoucherEntryDetailsByMasterId
    @MasterId INT
AS
BEGIN
    DELETE FROM VoucherEntryDetails WHERE MasterId = @MasterId;
END;


CREATE PROCEDURE sp_DeleteVoucherEntryWithDetails
    @Id INT
AS
BEGIN
    DELETE FROM VoucherEntryDetails WHERE MasterId = @Id;
    DELETE FROM VoucherEntry WHERE Id = @Id;
END;
