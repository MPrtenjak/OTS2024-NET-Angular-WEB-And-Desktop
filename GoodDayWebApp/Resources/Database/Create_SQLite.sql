CREATE TABLE IF NOT EXISTS users (
    id       INTEGER PRIMARY KEY AUTOINCREMENT,
    name     VARCHAR UNIQUE
                     NOT NULL,
    password VARCHAR NOT NULL,
    role     INTEGER NOT NULL,
    salt     VARCHAR NOT NULL
);

CREATE TABLE IF NOT EXISTS gratitude (
    id       INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id  INTEGER NOT NULL,
    date     TEXT NOT NULL,
    content  TEXT NOT NULL
);