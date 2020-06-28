CREATE TABLE holiday_definitions 
(
    id BIGSERIAL UNIQUE PRIMARY KEY NOT NULL,
    placement_strategy VARCHAR(128) NOT NULL,
    month INT,
    day INT
)