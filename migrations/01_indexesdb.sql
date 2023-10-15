-- ALTER TABLE IF EXISTS public.cities DROP CONSTRAINT IF EXISTS cities_pkey;

ALTER TABLE IF EXISTS public.cities
    ADD CONSTRAINT cities_pkey PRIMARY KEY (id);

-- ALTER TABLE IF EXISTS public.users DROP CONSTRAINT IF EXISTS users_pkey;

ALTER TABLE IF EXISTS public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);

-- ALTER TABLE IF EXISTS public.users DROP CONSTRAINT IF EXISTS city_id_fk;

ALTER TABLE IF EXISTS public.users
    ADD CONSTRAINT city_id_fk FOREIGN KEY (city_id)
    REFERENCES public.cities (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;

-- DROP INDEX IF EXISTS public.fki_city_id_fk;

CREATE INDEX IF NOT EXISTS fki_city_id_fk
    ON public.users USING btree
    (city_id ASC NULLS LAST)
    TABLESPACE pg_default;

-- DROP INDEX IF EXISTS public.idx_name;

CREATE INDEX IF NOT EXISTS idx_names
 ON public.users USING btree
 (surname COLLATE pg_catalog."default" ASC NULLS LAST, name COLLATE 
pg_catalog."default" ASC NULLS LAST)
 WITH (deduplicate_items=True)
 TABLESPACE pg_default;

CREATE EXTENSION pg_trgm;

-- DROP INDEX IF EXISTS public.trgm_idx_names;

CREATE INDEX IF NOT EXISTS trgm_idx_names
    ON public.users USING gin
    (surname COLLATE pg_catalog."default" gin_trgm_ops, name COLLATE pg_catalog."default" gin_trgm_ops)
    WITH (fastupdate=True)
    TABLESPACE pg_default;