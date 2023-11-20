CREATE EXTENSION pgcrypto ;

-- Table: public.cities

-- DROP TABLE IF EXISTS public.cities;

CREATE TABLE IF NOT EXISTS public.cities
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    name character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT cities_name_key UNIQUE (name)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.cities
    OWNER to sa;

-- Table: public.users

-- DROP TABLE IF EXISTS public.users;

CREATE TABLE IF NOT EXISTS public.users
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    name character varying COLLATE pg_catalog."default" NOT NULL,
    surname character varying COLLATE pg_catalog."default" NOT NULL,
    age smallint,
    sex character(1) COLLATE pg_catalog."default",
    info character varying COLLATE pg_catalog."default",
    password_hash varchar(100) NOT NULL,
    city_id integer
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.users
    OWNER to sa;