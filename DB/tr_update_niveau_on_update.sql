CREATE OR ALTER TRIGGER [dbo].[tr_update_niveau_on_update] ON [dbo].[Moyennes] AFTER UPDATE AS
BEGIN
    -- Get the updated moyenne record and check the filiere and moyenne values
    DECLARE @code_eleve varchar(255), @code_fil varchar(255), @niveau varchar(255), @moyenne float
    SELECT @code_eleve = code_eleve, @code_fil = code_fil, @niveau = niveau, @moyenne = moyenne FROM inserted

    IF @code_fil = 'GINF' OR @code_fil = 'GSTR' OR @code_fil = 'GIL'
    BEGIN
        IF @moyenne >= 12
        BEGIN
            -- Update the student's niveau based on their current niveau
            IF @niveau = 'GINF1'
                UPDATE Eleve SET niveau = 'GINF2' WHERE code = @code_eleve
            ELSE IF @niveau = 'GINF2'
                UPDATE Eleve SET niveau = 'GINF3' WHERE code = @code_eleve
			IF @niveau = 'GSTR1'
                UPDATE Eleve SET niveau = 'GSTR2' WHERE code = @code_eleve
            ELSE IF @niveau = 'GSTR2'
                UPDATE Eleve SET niveau = 'GSTR3' WHERE code = @code_eleve
			IF @niveau = 'GIL1'
                UPDATE Eleve SET niveau = 'GIL2' WHERE code = @code_eleve
            ELSE IF @niveau = 'GIL2'
                UPDATE Eleve SET niveau = 'GIL3' WHERE code = @code_eleve
        END
    END
    ELSE IF @code_fil = 'AP'
    BEGIN
        IF @moyenne >= 10
        BEGIN
            -- Update the student's niveau based on their current niveau
            IF @niveau = 'AP1'
                UPDATE Eleve SET niveau = 'AP2' WHERE code = @code_eleve
            ELSE IF @niveau = 'AP2'
                UPDATE Eleve SET niveau = 'CI' WHERE code = @code_eleve
        END
    END
END
