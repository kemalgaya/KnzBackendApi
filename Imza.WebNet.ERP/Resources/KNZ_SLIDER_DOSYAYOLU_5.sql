-- Veritabanı oluşturma
CREATE DATABASE MobileGameDB;
GO

-- Veritabanına bağlanma
USE MobileGameDB;
GO

-- Oyuncular tablosu
CREATE TABLE Players (
    player_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    registration_date DATETIME DEFAULT GETDATE(),
    level INT DEFAULT 1,
    experience_points INT DEFAULT 0
);
GO

-- Oyun içi öğeler tablosu
CREATE TABLE Items (
    item_id INT IDENTITY(1,1) PRIMARY KEY,
    item_name NVARCHAR(100),
    type NVARCHAR(50),
    value INT,
    player_id INT,
    FOREIGN KEY (player_id) REFERENCES Players(player_id)
);
GO

-- Skorlar tablosu
CREATE TABLE Scores (
    score_id INT IDENTITY(1,1) PRIMARY KEY,
    score INT,
    game_mode NVARCHAR(50),
    date DATETIME DEFAULT GETDATE(),
    player_id INT,
    FOREIGN KEY (player_id) REFERENCES Players(player_id)
);
GO

-- Görevler tablosu
CREATE TABLE Quests (
    quest_id INT IDENTITY(1,1) PRIMARY KEY,
    quest_name NVARCHAR(100),
    description NVARCHAR(MAX),
    reward NVARCHAR(100),
    player_id INT,
    status NVARCHAR(50),
    FOREIGN KEY (player_id) REFERENCES Players(player_id)
);
GO

-- Başarımlar tablosu
CREATE TABLE Achievements (
    achievement_id INT IDENTITY(1,1) PRIMARY KEY,
    achievement_name NVARCHAR(100),
    description NVARCHAR(MAX),
    date DATETIME DEFAULT GETDATE(),
    player_id INT,
    FOREIGN KEY (player_id) REFERENCES Players(player_id)
);
GO

-- Arkadaş listesi tablosu
CREATE TABLE Friends (
    friendship_id INT IDENTITY(1,1) PRIMARY KEY,
    player_id INT,
    friend_id INT,
    FOREIGN KEY (player_id) REFERENCES Players(player_id),
    FOREIGN KEY (friend_id) REFERENCES Players(player_id)
);
GO

-- Oyuncular tablosuna veri ekleme
INSERT INTO Players (username, email, password, registration_date, level, experience_points) VALUES
('player1', 'player1@example.com', 'password1', GETDATE(), 1, 100),
('player2', 'player2@example.com', 'password2', GETDATE(), 2, 200),
('player3', 'player3@example.com', 'password3', GETDATE(), 3, 300),
('player4', 'player4@example.com', 'password4', GETDATE(), 4, 400),
('player5', 'player5@example.com', 'password5', GETDATE(), 5, 500),
('player6', 'player6@example.com', 'password6', GETDATE(), 6, 600),
('player7', 'player7@example.com', 'password7', GETDATE(), 7, 700),
('player8', 'player8@example.com', 'password8', GETDATE(), 8, 800),
('player9', 'player9@example.com', 'password9', GETDATE(), 9, 900),
('player10', 'player10@example.com', 'password10', GETDATE(), 10, 1000);
GO

-- Oyun içi öğeler tablosuna veri ekleme
INSERT INTO Items (item_name, type, value, player_id) VALUES
('Sword', 'Weapon', 100, 1),
('Shield', 'Armor', 150, 2),
('Potion', 'Consumable', 50, 3),
('Helmet', 'Armor', 120, 4),
('Boots', 'Armor', 80, 5),
('Bow', 'Weapon', 110, 6),
('Arrow', 'Ammunition', 20, 7),
('Ring', 'Accessory', 200, 8),
('Necklace', 'Accessory', 250, 9),
('Staff', 'Weapon', 130, 10);
GO

-- Skorlar tablosuna veri ekleme
INSERT INTO Scores (score, game_mode, date, player_id) VALUES
(1000, 'Adventure', GETDATE(), 1),
(2000, 'Battle', GETDATE(), 2),
(1500, 'Adventure', GETDATE(), 3),
(1800, 'Battle', GETDATE(), 4),
(2200, 'Adventure', GETDATE(), 5),
(2500, 'Battle', GETDATE(), 6),
(3000, 'Adventure', GETDATE(), 7),
(3500, 'Battle', GETDATE(), 8),
(4000, 'Adventure', GETDATE(), 9),
(4500, 'Battle', GETDATE(), 10);
GO

-- Görevler tablosuna veri ekleme
INSERT INTO Quests (quest_name, description, reward, player_id, status) VALUES
('Quest 1', 'Defeat the dragon', 'Gold', 1, 'Completed'),
('Quest 2', 'Rescue the princess', 'Silver', 2, 'Completed'),
('Quest 3', 'Find the lost sword', 'Bronze', 3, 'Completed'),
('Quest 4', 'Protect the village', 'Gold', 4, 'In Progress'),
('Quest 5', 'Gather herbs', 'Silver', 5, 'Completed'),
('Quest 6', 'Defend the castle', 'Gold', 6, 'In Progress'),
('Quest 7', 'Explore the cave', 'Bronze', 7, 'Completed'),
('Quest 8', 'Hunt the beast', 'Silver', 8, 'In Progress'),
('Quest 9', 'Collect crystals', 'Gold', 9, 'Completed'),
('Quest 10', 'Retrieve the artifact', 'Silver', 10, 'Completed');
GO

-- Başarımlar tablosuna veri ekleme
INSERT INTO Achievements (achievement_name, description, date, player_id) VALUES
('Achievement 1', 'First Blood', GETDATE(), 1),
('Achievement 2', 'Monster Slayer', GETDATE(), 2),
('Achievement 3', 'Treasure Hunter', GETDATE(), 3),
('Achievement 4', 'Dragon Slayer', GETDATE(), 4),
('Achievement 5', 'Master Collector', GETDATE(), 5),
('Achievement 6', 'Hero of the Village', GETDATE(), 6),
('Achievement 7', 'Legendary Explorer', GETDATE(), 7),
('Achievement 8', 'Beast Hunter', GETDATE(), 8),
('Achievement 9', 'Crystal Gatherer', GETDATE(), 9),
('Achievement 10', 'Artifact Retriever', GETDATE(), 10);
GO

-- Arkadaş listesi tablosuna veri ekleme
INSERT INTO Friends (player_id, friend_id) VALUES
(1, 2),
(1, 3),
(2, 3),
(2, 4),
(3, 4),
(3, 5),
(4, 5),
(4, 6),
(5, 6),
(5, 7);
GO



-------------------------------------------------- sorgu 1

-- Belirli bir oyuncunun sahip olduğu tüm öğeleri alfabetik olarak listeleme
DECLARE @PlayerID INT = 3; -- İlgili oyuncunun ID'sini buraya yazın

SELECT item_name
FROM Items
WHERE player_id = @PlayerID
ORDER BY item_name ASC;



-------------------------------------------------- sorgu 2
-- Belirli bir oyuncunun tamamladığı tüm görevleri ve görevlerin durumlarını listeleme

-- Veritabanına bağlanma
USE MobileGameDB;
GO

-- Örnek öğeler listesi
DECLARE @Items TABLE (
    item_name NVARCHAR(100),
    type NVARCHAR(50),
    value INT
);

-- Örnek öğe verilerini ekleme
INSERT INTO @Items (item_name, type, value) VALUES
('Sword', 'Weapon', 100),
('Shield', 'Armor', 150),
('Potion', 'Consumable', 50),
('Helmet', 'Armor', 120),
('Boots', 'Armor', 80),
('Bow', 'Weapon', 110),
('Arrow', 'Ammunition', 20),
('Ring', 'Accessory', 200),
('Necklace', 'Accessory', 250),
('Staff', 'Weapon', 130),
('Dagger', 'Weapon', 90),
('Axe', 'Weapon', 140),
('Spear', 'Weapon', 160),
('Gauntlets', 'Armor', 110),
('Amulet', 'Accessory', 220),
('Bracelet', 'Accessory', 180),
('Potion of Strength', 'Consumable', 70),
('Potion of Healing', 'Consumable', 60),
('Potion of Speed', 'Consumable', 55),
('Potion of Mana', 'Consumable', 65);

-- Rastgele dağılım için değişkenler
DECLARE @TotalItems INT = 50;
DECLARE @PlayerCount INT = 10;
DECLARE @PlayerID INT;
DECLARE @ItemName NVARCHAR(100);
DECLARE @ItemType NVARCHAR(50);
DECLARE @ItemValue INT;
DECLARE @RemainingItems INT = @TotalItems;

-- Geçici tablo oluşturma
CREATE TABLE #PlayerItems (
    player_id INT,
    item_name NVARCHAR(100),
    type NVARCHAR(50),
    value INT
);

-- Oyunculara rastgele öğe sayısı dağıtma
WHILE @RemainingItems > 0
BEGIN
    SET @PlayerID = ABS(CHECKSUM(NEWID()) % @PlayerCount) + 1;
    DECLARE @ItemsToAssign INT = ABS(CHECKSUM(NEWID()) % (@RemainingItems / 2 + 1)) + 1;
    
    IF @ItemsToAssign > @RemainingItems
    BEGIN
        SET @ItemsToAssign = @RemainingItems;
    END

    SET @RemainingItems = @RemainingItems - @ItemsToAssign;

    WHILE @ItemsToAssign > 0
    BEGIN
        SELECT TOP 1 
            @ItemName = item_name, 
            @ItemType = type, 
            @ItemValue = value
        FROM @Items
        ORDER BY NEWID();
        
        INSERT INTO #PlayerItems (player_id, item_name, type, value)
        VALUES (@PlayerID, @ItemName, @ItemType, @ItemValue);
        
        SET @ItemsToAssign = @ItemsToAssign - 1;
    END
END

-- Dağıtılan öğeleri gerçek öğeler tablosuna ekleme
INSERT INTO Items (item_name, type, value, player_id)
SELECT item_name, type, value, player_id FROM #PlayerItems;

-- Geçici tabloyu kaldırma
DROP TABLE #PlayerItems;
GO

-- Dağıtılan öğeleri kontrol etme
SELECT * FROM Items;
GO





DECLARE @PlayerID INT = 3; -- İlgili oyuncunun ID'sini buraya yazın

SELECT quest_name, status
FROM Quests
WHERE player_id = @PlayerID;


--------------------------------------------------sorgu 3
-- Yeni öğe toplama işlemi için SQL kodu
DECLARE @PlayerID INT = 3; -- Kullanıcının ID'sini buraya yazın

-- Kullanıcının mevcut öğelerini belirleme
DECLARE @CurrentItems TABLE (
    item_name NVARCHAR(100)
);

INSERT INTO @CurrentItems (item_name)
SELECT item_name
FROM Items
WHERE player_id = @PlayerID;

-- Mevcut olmayan öğeleri belirleme
DECLARE @AvailableItems TABLE (
    item_id INT IDENTITY(1,1),
    item_name NVARCHAR(100)
);

INSERT INTO @AvailableItems (item_name)
SELECT item_name
FROM Items
WHERE item_name NOT IN (SELECT item_name FROM @CurrentItems);

-- Rastgele bir mevcut olmayan öğe seçme
DECLARE @RandomItemID INT;
DECLARE @RandomItemName NVARCHAR(100);

SELECT TOP 1 @RandomItemID = item_id, @RandomItemName = item_name
FROM @AvailableItems
ORDER BY NEWID();

-- Seçilen öğeyi kullanıcının envanterine ekleme
IF @RandomItemID IS NOT NULL
BEGIN
    INSERT INTO Items (item_name, player_id)
    VALUES (@RandomItemName, @PlayerID);
    
    SELECT 'Yeni öğe toplandı: ' + @RandomItemName AS Result;
END
ELSE
BEGIN
    SELECT 'Kullanıcı tüm öğelere sahip.' AS Result;
END;



----------------------------------------------------------------sorgu 4
-- Belirli bir kullanıcıya ait tüm skorları sıralama
DECLARE @PlayerID INT = 3; -- Kullanıcının ID'sini buraya yazın

SELECT score_id, score, game_mode, date
FROM Scores
WHERE player_id = @PlayerID
ORDER BY date DESC;




----------------------------------------------------------------sorgu 5
-- En yüksek skora sahip oyuncu ve en fazla tamamladığı görevleri gösterme
WITH MaxScorePlayer AS (
    SELECT TOP 1 WITH TIES player_id
    FROM Scores
    ORDER BY ROW_NUMBER() OVER (PARTITION BY player_id ORDER BY score DESC)
)
SELECT p.username AS [Player],
       s.score AS [Highest Score],
       ISNULL(q.quest_name,'----') AS [Completed Quest],
       COUNT(q.quest_id) AS [Completed Quests]
FROM MaxScorePlayer msp
JOIN Players p ON msp.player_id = p.player_id
JOIN Scores s ON p.player_id = s.player_id
LEFT JOIN Quests q ON p.player_id = q.player_id AND q.status = 'Completed'
GROUP BY p.username, s.score, q.quest_name
ORDER BY s.score DESC;



----------------------------------------------------------------sorgu 6
-- Popüler oyuncuları bulma

-- Popüler oyuncuları eklemek için INSERT sorguları
-- Player1 için Scores ve Quests tablolarına veri girişi
-- Skor verisi
INSERT INTO Scores (score, player_id) VALUES (150, (SELECT player_id FROM Players WHERE username = 'Player1'));

-- Görev verileri
INSERT INTO Quests (quest_name, status, player_id) VALUES
('Quest1', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player1')),
('Quest2', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player1')),
('Quest3', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player1')),
('Quest4', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player1')),
('Quest5', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player1')),
('Quest6', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player1')),
('Quest7', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player1'));

-- Player2 için Scores ve Quests tablolarına veri girişi
-- Skor verisi
INSERT INTO Scores (score, player_id) VALUES (120, (SELECT player_id FROM Players WHERE username = 'Player2'));

-- Görev verileri
INSERT INTO Quests (quest_name, status, player_id) VALUES
('Quest1', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player2')),
('Quest2', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player2')),
('Quest3', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player2')),
('Quest4', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player2')),
('Quest5', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player2')),
('Quest6', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player2'));

-- Player3 için Scores ve Quests tablolarına veri girişi
-- Skor verisi
INSERT INTO Scores (score, player_id) VALUES (110, (SELECT player_id FROM Players WHERE username = 'Player3'));

-- Görev verileri
INSERT INTO Quests (quest_name, status, player_id) VALUES
('Quest1', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player3')),
('Quest2', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player3')),
('Quest3', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player3')),
('Quest4', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player3')),
('Quest5', 'Completed', (SELECT player_id FROM Players WHERE username = 'Player3'));




SELECT p.username AS [Player],
       s.score AS [Score],
       COUNT(q.quest_id) AS [Completed Quests]
FROM Players p
JOIN Scores s ON p.player_id = s.player_id
LEFT JOIN Quests q ON p.player_id = q.player_id AND q.status = 'Completed'
GROUP BY p.player_id, p.username, s.score
HAVING s.score > 100 AND COUNT(q.quest_id) >= 5
ORDER BY s.score DESC;





---------------------------------------------------------------sorgu 7
WITH PlayerScores AS (
    SELECT p.username,
           s.score,
           COUNT(q.quest_id) AS completed_quest_count,
           ROW_NUMBER() OVER (ORDER BY s.score DESC, COUNT(q.quest_id) DESC) AS rank
    FROM Players p
    JOIN Scores s ON p.player_id = s.player_id
    LEFT JOIN Quests q ON p.player_id = q.player_id AND q.status = 'Completed'
    GROUP BY p.player_id, p.username, s.score
)
SELECT username, score, completed_quest_count
FROM PlayerScores




SELECT TOP 5 p.username,
             s.score,
             COUNT(q.quest_id) AS completed_quest_count
FROM Players p
JOIN Scores s ON p.player_id = s.player_id
LEFT JOIN Quests q ON p.player_id = q.player_id AND q.status = 'Completed'
GROUP BY p.player_id, p.username, s.score
ORDER BY s.score DESC, COUNT(q.quest_id) DESC;


SELECT TOP 5 p.username,
             s.score,
             COUNT(q.quest_id) AS completed_quest_count
FROM Players p
JOIN Scores s ON p.player_id = s.player_id
LEFT JOIN Quests q ON p.player_id = q.player_id AND q.status = 'Completed'
GROUP BY p.player_id, p.username, s.score
ORDER BY COUNT(q.quest_id) DESC, s.score DESC;




-----------------------------------------------------------------------------------sorgu 8 

WITH MostFriendAdder AS (
    SELECT TOP 1 player_id
    FROM Friends
    GROUP BY player_id
    ORDER BY COUNT(*) DESC
)
SELECT f.player_id, p.username AS [Friend], p.email
FROM Friends f
JOIN Players p ON f.friend_id = p.player_id
WHERE f.player_id = (SELECT player_id FROM MostFriendAdder);


----------------------------------------------------------------------------------sorgu 9
CREATE PROCEDURE AddNewItem
    @item_name NVARCHAR(100),
    @item_type NVARCHAR(50),
    @item_value DECIMAL(10, 2),
    @player_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Yeni öğe ekleme
    INSERT INTO Items (item_name, type, value, player_id)
    VALUES (@item_name, @item_type, @item_value, @player_id);
END;



EXEC AddNewItem 'Yeni Öğe', 'Silah', 50.00, 1;

select * from Items


-------------------------------------------------------------sorgu 10

CREATE PROCEDURE CompleteQuest
    @player_id INT,
    @quest_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Oyuncunun ödülünü al
    DECLARE @quest_reward NVARCHAR(100);
    SELECT @quest_reward = reward
    FROM Quests
    WHERE quest_id = @quest_id;

    -- Oyuncunun deneyim puanını güncelle
    DECLARE @experience_points INT;
    SET @experience_points = TRY_CAST(@quest_reward AS INT); -- Ödülü sayıya dönüştürme
    IF @experience_points IS NOT NULL
    BEGIN
        UPDATE Players
        SET experience_points = experience_points + @experience_points
        WHERE player_id = @player_id;
    END;

    -- Oyuncunun yeni seviyesini güncelle
    DECLARE @new_level INT;
    SELECT @new_level = level + 1
    FROM Players
    WHERE player_id = @player_id;

    UPDATE Players
    SET level = @new_level
    WHERE player_id = @player_id;

    -- Görevin durumunu güncelle
    UPDATE Quests
    SET status = 'Completed'
    WHERE quest_id = @quest_id;
END;





EXEC CompleteQuest @player_id = 1, @quest_id = 1;




---------------------------------------------------sorgu 11


-- Görev ekleme tetikleyicisi
CREATE TRIGGER UpdateExperienceOnQuestInsert
ON Quests
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @player_id INT;

    -- Yeni eklenen görevin bilgilerini al
    SELECT @player_id = player_id
    FROM inserted;

    -- Oyuncunun deneyim puanını güncelle
    UPDATE Players
    SET experience_points = experience_points + 100 -- Yeni görev eklenirken oyuncunun deneyim puanını artır
    WHERE player_id = @player_id;
END;
GO

-- Başarımlar ekleme tetikleyicisi
CREATE TRIGGER UpdateLevelOnAchievementInsert
ON Achievements
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @player_id INT;

    -- Yeni eklenen başarımın bilgilerini al
    SELECT @player_id = player_id
    FROM inserted;

    -- Oyuncunun seviyesini güncelle
    UPDATE Players
    SET level = level + 1 -- Yeni başarı eklenirken oyuncunun seviyesini artır
    WHERE player_id = @player_id;
END;
GO




------------------------------------------------------------------------sorgu 12


CREATE TRIGGER UpdatePlayerStatsOnScoreDelete
ON Scores
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @player_id INT;

    -- Silinen skorun bilgilerini al
    SELECT @player_id = player_id
    FROM deleted;

    -- Oyuncunun deneyim puanını güncelle
    UPDATE Players
    SET experience_points = experience_points - (SELECT score FROM deleted)
    WHERE player_id = @player_id;

    -- Oyuncunun seviyesini güncelle
    DECLARE @new_level INT;
    SELECT @new_level = FLOOR(LOG(experience_points + 1, 2))
    FROM Players
    WHERE player_id = @player_id;

    UPDATE Players
    SET level = @new_level
    WHERE player_id = @player_id;
END;
GO



-------------------------------------------------------------Sorgu 13

-- Beğenilen başarımlar tablosu
CREATE TABLE LikedAchievements (
    like_id INT IDENTITY(1,1) PRIMARY KEY,
    player_id INT,
    achievement_id INT,
    CONSTRAINT FK_LikedAchievements_Player FOREIGN KEY (player_id) REFERENCES Players(player_id),
    CONSTRAINT FK_LikedAchievements_Achievement FOREIGN KEY (achievement_id) REFERENCES Achievements(achievement_id)
);

-- Favori başarımlar tablosu
CREATE TABLE FavoriteAchievements (
    favorite_id INT IDENTITY(1,1) PRIMARY KEY,
    player_id INT,
    achievement_id INT,
    CONSTRAINT FK_FavoriteAchievements_Player FOREIGN KEY (player_id) REFERENCES Players(player_id),
    CONSTRAINT FK_FavoriteAchievements_Achievement FOREIGN KEY (achievement_id) REFERENCES Achievements(achievement_id)
);


-- Beğeni ekleme
INSERT INTO LikedAchievements (player_id, achievement_id) VALUES (1, 3);

-- Favorilere ekleme
INSERT INTO FavoriteAchievements (player_id, achievement_id) VALUES (1, 3);




------------------------------------------------------------------Sorgu 14

-- VIP üyelik işaretlemesi
ALTER TABLE Players
ADD is_vip BIT DEFAULT 0;
GO

-- Kullanıcı tablosuna vip_membership_expiration adında bir sütun ekleyelim
ALTER TABLE Players
ADD vip_membership_expiration DATETIME;
GO

-- Görev tamamlayan ve en yüksek skoru elde eden kullanıcıya VIP üyelik hakkı tanımlama
DECLARE @top_scorer_player_id INT;

-- En yüksek skoru elde eden kullanıcıyı bul
SELECT TOP 1 @top_scorer_player_id = player_id
FROM Scores
ORDER BY score DESC;

-- VIP üyelik işaretlemesi yapma ve üyeliğin sona ereceği tarihi belirleme (1 ay sonrası)
UPDATE Players
SET is_vip = 1,
    vip_membership_expiration = DATEADD(MONTH, 1, GETDATE()) -- 1 ay sonrası
WHERE player_id = @top_scorer_player_id;





----------------------------------------------------------Sorgu 15