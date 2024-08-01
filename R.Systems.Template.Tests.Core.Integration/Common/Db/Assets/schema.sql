CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE company (
    id uuid NOT NULL,
    name character varying(200) NOT NULL,
    CONSTRAINT "PK_company" PRIMARY KEY (id)
);

CREATE TABLE element (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    description character varying(1000),
    value integer NOT NULL,
    additional_value integer,
    big_value bigint NOT NULL,
    big_additional_value bigint,
    price numeric NOT NULL,
    discount numeric,
    creation_date date NOT NULL,
    update_date date,
    creation_date_time timestamp with time zone NOT NULL,
    update_date_time timestamp with time zone,
    is_new boolean NOT NULL,
    is_active boolean,
    CONSTRAINT "PK_element" PRIMARY KEY (id)
);

CREATE TABLE employee (
    id uuid NOT NULL,
    first_name character varying(100) NOT NULL,
    last_name character varying(100) NOT NULL,
    company_id uuid,
    CONSTRAINT "PK_employee" PRIMARY KEY (id),
    CONSTRAINT "FK_employee_company_company_id" FOREIGN KEY (company_id) REFERENCES company (id) ON DELETE CASCADE
);

INSERT INTO company (id, name)
VALUES ('31b04626-ed12-4d79-b3d6-1430a72000d5', 'Meta');
INSERT INTO company (id, name)
VALUES ('9427a96c-a0b6-461c-814c-9c3c2bb6ff80', 'Google');

INSERT INTO employee (id, company_id, first_name, last_name)
VALUES ('84b096f7-68a1-47a8-9e6a-8cfd79f0f069', '31b04626-ed12-4d79-b3d6-1430a72000d5', 'John', 'Doe');
INSERT INTO employee (id, company_id, first_name, last_name)
VALUES ('ab189e89-7007-43bf-85d1-b1cc3c69c503', '9427a96c-a0b6-461c-814c-9c3c2bb6ff80', 'Will', 'Smith');
INSERT INTO employee (id, company_id, first_name, last_name)
VALUES ('b82f922b-784c-40e1-b03b-476a0b447dca', '9427a96c-a0b6-461c-814c-9c3c2bb6ff80', 'Jack', 'Parker');

CREATE UNIQUE INDEX "IX_company_name" ON company (name);

CREATE INDEX "IX_employee_company_id" ON employee (company_id);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240801172949_InitDb', '8.0.4');

COMMIT;

