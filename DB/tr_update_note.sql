CREATE OR ALTER TRIGGER [dbo].[tr_update_note] 
ON [dbo].[Notes] AFTER UPDATE AS
BEGIN
    -- Get the student and their level and filiere
    DECLARE @code_eleve varchar(255), @niveau varchar(255), @code_fil varchar(255)
    SELECT @code_eleve = code_eleve FROM inserted
    SELECT @niveau = niveau, @code_fil = code_fil FROM Eleve WHERE code = @code_eleve

    -- Check if the number of notes matches the number of Matiere records for the student's level and filiere
    DECLARE @num_notes int, @num_matieres int
    SELECT @num_notes = COUNT(*) FROM Notes WHERE code_eleve = @code_eleve and code_mat IN (
        SELECT code FROM Matiere WHERE code_module IN(SELECT code FROM Module WHERE niveau = @niveau AND code_fil = @code_fil))
    SELECT @num_matieres = COUNT(*) FROM Matiere WHERE code_module IN (
        SELECT code FROM Module WHERE niveau = @niveau AND code_fil = @code_fil
    )

    -- Delete the corresponding row from the "Moyennes" table if the number of notes doesn't match the number of matieres
    IF @num_notes <> @num_matieres
    BEGIN
        DELETE FROM Moyennes WHERE code_eleve = @code_eleve AND niveau = @niveau
        RETURN;
    END

    -- Calculate the average and update the "Moyennes" table
    DECLARE @total_notes float
    SELECT @total_notes = SUM(note) FROM Notes WHERE code_eleve = @code_eleve
    IF @num_notes > 0
    BEGIN
        DECLARE @moyenne float
        SET @moyenne = @total_notes / @num_notes
        
        -- Update the corresponding row in the "Moyennes" table if it exists
        IF EXISTS (SELECT * FROM Moyennes WHERE code_eleve = @code_eleve AND niveau = @niveau)
        BEGIN
            UPDATE Moyennes SET moyenne = @moyenne WHERE code_eleve = @code_eleve AND niveau = @niveau
        END
        -- Otherwise, insert a new row into the "Moyennes" table
        ELSE
        BEGIN
            INSERT INTO Moyennes (code_eleve, code_fil, niveau, moyenne)
            SELECT @code_eleve, @code_fil, @niveau, @moyenne
        END
    END
    -- If there are no notes for the student, delete the corresponding row from the "Moyennes" table
    ELSE
    BEGIN
        DELETE FROM Moyennes WHERE code_eleve = @code_eleve AND niveau = @niveau
    END
END
