-- ALTER TABLE IF EXISTS public.dialogs DROP COLUMN IF EXISTS id;

ALTER TABLE IF EXISTS public.dialogs
    ADD COLUMN id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 );

ALTER TABLE IF EXISTS public.dialogs
    ADD CONSTRAINT dialogs_pkey PRIMARY KEY (id);

-- ALTER TABLE IF EXISTS public.dialogs DROP COLUMN IF EXISTS "isRead";

ALTER TABLE IF EXISTS public.dialogs
    ADD COLUMN "isRead" boolean NOT NULL DEFAULT false;