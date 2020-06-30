CREATE TABLE holiday_definitions
(
    id                 BIGSERIAL UNIQUE PRIMARY KEY NOT NULL,
    name               VARCHAR(128)                 NOT NULL,
    placement_strategy VARCHAR(128)                 NOT NULL,
    week_of_month      INT,
    month              INT,
    day                INT,
    day_of_week        INT
);

-- DROP TABLE holiday_definitions

INSERT INTO holiday_definitions (name, placement_strategy, month, day)
VALUES ('Australia Day', 'FixedDate', 1, 26);

INSERT INTO holiday_definitions (name, placement_strategy, month, day)
VALUES ('Anzac Day', 'FixedDate', 4, 25);

INSERT INTO holiday_definitions (name, placement_strategy, month, day)
VALUES ('New Years Day', 'FixedDateNonWeekend', 1, 1);

INSERT INTO holiday_definitions (name, placement_strategy, week_of_month, month, day_of_week)
VALUES ('Queen''s Birthday', 'FixedDay', 1, 6, 1);

INSERT INTO holiday_definitions (name, placement_strategy, week_of_month, month, day_of_week)
VALUES ('Labour Day', 'FixedDay', 0, 10, 1);

INSERT INTO holiday_definitions (name, placement_strategy, month, day)
VALUES ('Christmas', 'FixedDate', 12, 25);

INSERT INTO holiday_definitions (name, placement_strategy, month, day)
VALUES ('Boxing Day', 'FixedDate', 12, 26);

-- DELETE FROM holiday_definitions WHERE id != 0;