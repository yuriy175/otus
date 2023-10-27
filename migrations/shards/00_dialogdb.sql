-- wrap in transaction to ensure Docker flag always visible
BEGIN;
CREATE EXTENSION citus;

-- add Docker flag to node metadata
UPDATE pg_dist_node_metadata SET metadata=jsonb_insert(metadata, '{docker}', 'true');
COMMIT;

-- Database: dialogdb

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

-- Index: idx_users

-- DROP INDEX IF EXISTS public.idx_users;

CREATE INDEX IF NOT EXISTS idx_users
    ON public.dialogs USING btree
    (user_id_1 ASC NULLS LAST, user_id_2 ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;