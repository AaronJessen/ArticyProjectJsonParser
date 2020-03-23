create table "Model"
(
	"Type" varchar(35),
	"TechnicalName" varchar(20),
	"Id" varchar(18) primary key,
	"Parent" varchar(18),
	"DisplayName" text,
	"Color" varchar(6),
	"Text" text,
	"ExternalId" text,
	"ShortId" int
);
create table "Pin"
(
	"Type" varchar(6),
	"Text" text,
	"Id" varchar(18) primary key,
	"Owner" varchar(18)
);
create table "Connection"
(
	"Id" integer primary key AUTOINCREMENT,
	"Color" varchar(6),
	"Label" text,
	"Source" varchar(18),
	"SourcePin" varchar(18),
	"TargetPin" varchar(18),
	"Target" varchar(18)
);
