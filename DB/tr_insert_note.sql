CREATE OR ALTER TRIGGER  tr_insert_note ON Notes AFTER INSERT AS
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
    IF @num_notes <> @num_matieres
    RETURN;

    -- Calculate the average and insert it into the Moyennes table
    DECLARE @total_notes float
    SELECT @total_notes = SUM(note) FROM Notes WHERE code_eleve = @code_eleve
    IF @num_notes > 0
    BEGIN
        DECLARE @moyenne float
        SET @moyenne = @total_notes / @num_notes
        INSERT INTO Moyennes (code_eleve, code_fil, niveau, moyenne)
        SELECT @code_eleve, @code_fil, @niveau, @moyenne
    END
END