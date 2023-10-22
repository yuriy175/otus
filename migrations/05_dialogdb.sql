-- Database: dialogdb

-- DROP DATABASE IF EXISTS dialogdb;

CREATE DATABASE dialogdb
    WITH
    OWNER = sa
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

-- Table: public.dialogs

-- DROP TABLE IF EXISTS public.dialogs;

CREATE TABLE IF NOT EXISTS public.dialogs
(
    user_id_1 bigint NOT NULL,
    user_id_2 bigint NOT NULL,
    author_id bigint,
    message text COLLATE pg_catalog."default",
    created timestamp without time zone DEFAULT now()
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.dialogs
    OWNER to sa;
-- Index: idx_users

-- DROP INDEX IF EXISTS public.idx_users;

CREATE INDEX IF NOT EXISTS idx_users
    ON public.dialogs USING btree
    (user_id_1 ASC NULLS LAST, user_id_2 ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;