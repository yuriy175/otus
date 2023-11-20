-- Table: public.friends

-- DROP TABLE IF EXISTS public.friends;

CREATE TABLE IF NOT EXISTS public.friends
(
    user_id integer NOT NULL,
    friend_id integer NOT NULL,
    "isDeleted" boolean,
    CONSTRAINT friends_pkey PRIMARY KEY (user_id, friend_id)
);

-- Index: fki_user_friends_id_fk

-- DROP INDEX IF EXISTS public.fki_user_friends_id_fk;

CREATE INDEX IF NOT EXISTS fki_user_friends_id_fk
    ON public.friends USING btree
    (friend_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: fki_user_id_fk

-- DROP INDEX IF EXISTS public.fki_user_id_fk;

CREATE INDEX IF NOT EXISTS fki_user_id_fk
    ON public.friends USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;

-- Index: friends_friend_id_idx

-- DROP INDEX IF EXISTS public.friends_friend_id_idx;

CREATE INDEX IF NOT EXISTS friends_friend_id_idx
    ON public.friends USING btree
    (friend_id ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;

-- Table: public.posts

-- DROP TABLE IF EXISTS public.posts;

CREATE TABLE IF NOT EXISTS public.posts
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    user_id integer,
    message text COLLATE pg_catalog."default",
    created timestamp without time zone DEFAULT now(),
    "isDeleted" boolean,
    CONSTRAINT posts_pkey PRIMARY KEY (id)
);
