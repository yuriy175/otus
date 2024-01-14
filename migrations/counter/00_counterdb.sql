-- Table: public.counters

-- DROP TABLE IF EXISTS public.counters;

CREATE TABLE IF NOT EXISTS public.counters
(
    user_id bigint NOT NULL,
    unread_count integer NOT NULL DEFAULT 0,
    CONSTRAINT counters_pkey PRIMARY KEY (user_id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.counters
    OWNER to sa;