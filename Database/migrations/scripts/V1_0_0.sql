CREATE SCHEMA weather_predictions AUTHORIZATION admin_user;

CREATE TABLE weather_predictions.predictions (
    day date not null,
    temperature numeric NOT NULL,

    CONSTRAINT pred_un UNIQUE (day),
    CONSTRAINT pred_pk PRIMARY KEY (day)
);
