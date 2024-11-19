\C boykisserbot

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE SCHEMA IF NOT EXISTS logs;
CREATE SCHEMA IF NOT EXISTS discord;
CREATE SCHEMA IF NOT EXISTS characters;
CREATE SCHEMA IF NOT EXISTS expeditions;
CREATE SCHEMA IF NOT EXISTS common;
CREATE SCHEMA IF NOT EXISTS config;

/* Create tables */
CREATE TABLE logs.commands
(
    id         uuid               DEFAULT uuid_generate_v4(),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    user_id    NUMERIC   NOT NULL,
    guild_id   NUMERIC, /* These can be null because the user may be sending a DM */
    channel_id NUMERIC,
    command    TEXT      NOT NULL,
    args       TEXT[]    NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE discord.users
(
    id         NUMERIC   NOT NULL, /* Discord ID */
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    username   text      NOT NULL,
    banned     boolean   NOT NULL DEFAULT FALSE,
    admin      boolean   NOT NULL DEFAULT FALSE,
    PRIMARY KEY (id)
);

CREATE TABLE discord.guilds
(
    id         NUMERIC   NOT NULL, /* Discord ID */
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    name       text      NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE discord.guilds_users
(
    id            uuid               DEFAULT uuid_generate_v4(),
    created_at    TIMESTAMP NOT NULL DEFAULT NOW(),
    user_id       NUMERIC   NOT NULL,
    guild_id      NUMERIC   NOT NULL,
    messages_sent NUMERIC   NOT NULL DEFAULT 0,
    PRIMARY KEY (id)
);

CREATE TABLE discord.channels
(
    id         NUMERIC   NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    guild_id   NUMERIC,
    name       text      NOT NULL,
    type       text      NOT NULL DEFAULT 'text',
    PRIMARY KEY (id)
);

CREATE TABLE discord.channels_users
(
    id         uuid               DEFAULT uuid_generate_v4(),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    user_id    NUMERIC   NOT NULL,
    channel_id NUMERIC   NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE characters.prototypes
(
    id          uuid               DEFAULT uuid_generate_v4(),
    created_at  TIMESTAMP NOT NULL DEFAULT NOW(),
    name        TEXT      NOT NULL,
    description TEXT      NOT NULL,
    rarity_id   uuid,
    PRIMARY KEY (id)
);
COMMENT ON COLUMN characters.prototypes.rarity_id IS 'If this is ever null, panic';

CREATE TABLE characters.instances
(
    id           uuid               DEFAULT uuid_generate_v4(),
    created_at   TIMESTAMP NOT NULL DEFAULT NOW(),
    prototype_id uuid      NOT NULL,
    owner_id     uuid      NOT NULL,
    expedition_count NUMERIC NOT NULL DEFAULT 0,
    PRIMARY KEY (id)
);

CREATE TABLE expeditions.expeditions
(
    id          uuid               DEFAULT uuid_generate_v4(),
    created_at  TIMESTAMP NOT NULL DEFAULT NOW(),
    name        TEXT      NOT NULL,
    description TEXT      NOT NULL,
    duration    NUMERIC   NOT NULL,
    characters  UUID[3]   NOT NULL,  /* See https://neon.tech/postgresql/postgresql-tutorial/postgresql-array */
    PRIMARY KEY (id)
);

CREATE TABLE common.rarities
(
    id                   uuid               DEFAULT uuid_generate_v4(),
    created_at           TIMESTAMP NOT NULL DEFAULT NOW(),
    name                 TEXT      NOT NULL,
    colour               TEXT      NOT NULL,
    emoji                TEXT      NOT NULL,
    weight               NUMERIC   NOT NULL,
    power                NUMERIC   NOT NULL,
    expedition_min_power NUMERIC   NOT NULL,
    expedition_min_time  NUMERIC   NOT NULL,
    expedition_max_time  NUMERIC   NOT NULL,
    expedition_tdb       NUMERIC   NOT NULL,
    expedition_tde       NUMERIC   NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE config.data
(
    id         uuid               DEFAULT uuid_generate_v4(),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    key        text      NOT NULL UNIQUE,
    value      text,
    PRIMARY KEY (id)
);

/* Create Indexes */
CREATE INDEX idx_commands_user_id ON logs.commands (user_id);
CREATE INDEX idx_commands_guild_id ON logs.commands (guild_id);

CREATE INDEX idx_users_banned ON discord.users (banned);
CREATE INDEX idx_users_admin ON discord.users (admin);

CREATE INDEX idx_guilds_channels_channel_id ON discord.channels (id);
CREATE INDEX idx_guilds_channels_guild_id ON discord.channels (guild_id);
CREATE INDEX idx_guilds_channels_type ON discord.channels (type);

CREATE INDEX idx_guilds_users_user_id ON discord.guilds_users (user_id);
CREATE INDEX idx_guilds_users_guild_id ON discord.guilds_users (guild_id);

CREATE INDEX idx_channels_users_user_id ON discord.channels_users (user_id);
CREATE INDEX idx_channels_users_channel_id ON discord.channels_users (channel_id);

/* Create Foreign Keys */
ALTER TABLE logs.commands
    ADD FOREIGN KEY (user_id) REFERENCES discord.users (id) ON DELETE CASCADE;
ALTER TABLE logs.commands
    ADD FOREIGN KEY (guild_id) REFERENCES discord.guilds (id) ON DELETE CASCADE;

ALTER TABLE discord.guilds_users
    ADD FOREIGN KEY (user_id) REFERENCES discord.users (id) ON DELETE CASCADE;
ALTER TABLE discord.guilds_users
    ADD FOREIGN KEY (guild_id) REFERENCES discord.guilds (id) ON DELETE CASCADE;

ALTER TABLE discord.channels
    ADD FOREIGN KEY (guild_id) REFERENCES discord.guilds (id) ON DELETE CASCADE;

ALTER TABLE discord.channels_users
    ADD FOREIGN KEY (user_id) REFERENCES discord.users (id) ON DELETE CASCADE;
ALTER TABLE discord.channels_users
    ADD FOREIGN KEY (channel_id) REFERENCES discord.channels (id) ON DELETE CASCADE;

ALTER TABLE characters.prototypes
    ADD FOREIGN KEY (rarity_id) REFERENCES common.rarities (id) ON DELETE SET NULL;

/* Grant privileges */
GRANT ALL PRIVILEGES ON DATABASE boykisserbot TO boykisserbot;

GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO boykisserbot;

GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO boykisserbot;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA characters TO boykisserbot;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA logs TO boykisserbot;