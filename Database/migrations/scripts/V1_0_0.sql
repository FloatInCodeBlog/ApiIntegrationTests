CREATE SCHEMA weather_predictions AUTHORIZATION admin_user;

CREATE TABLE weather_predictions.predictions (
    day date not null,
    temperature numeric NOT NULL
);
