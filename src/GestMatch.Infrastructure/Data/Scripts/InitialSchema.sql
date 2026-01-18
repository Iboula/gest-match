-- Script de création du schéma initial pour GestMatch
-- Généré selon les configurations Fluent API

-- Table Users
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" uuid NOT NULL,
    "ZitadelId" character varying(255) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "FullName" character varying(255) NOT NULL,
    "PhoneNumber" character varying(50),
    "City" character varying(100),
    "Role" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_ZitadelId" ON "Users" ("ZitadelId");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_Email" ON "Users" ("Email");
CREATE INDEX IF NOT EXISTS "IX_Users_Role" ON "Users" ("Role");

-- Table Matches
CREATE TABLE IF NOT EXISTS "Matches" (
    "Id" uuid NOT NULL,
    "TeamA" character varying(200) NOT NULL,
    "TeamB" character varying(200) NOT NULL,
    "MatchDateTime" timestamp with time zone NOT NULL,
    "Stadium" character varying(200) NOT NULL,
    "City" character varying(100) NOT NULL,
    "Region" character varying(100),
    "Competition" character varying(200) NOT NULL,
    "MatchType" text NOT NULL,
    "Status" text NOT NULL,
    "Description" text,
    "PosterUrl" text,
    "StandardTicketPrice" numeric(18,2) NOT NULL,
    "VipTicketPrice" numeric(18,2),
    "TotalTicketsAvailable" integer NOT NULL,
    "VipTicketsAvailable" integer,
    "TicketSaleEndDate" timestamp with time zone,
    "ScoreTeamA" integer,
    "ScoreTeamB" integer,
    "CreatedByUserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Matches" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Matches_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_Matches_MatchDateTime" ON "Matches" ("MatchDateTime");
CREATE INDEX IF NOT EXISTS "IX_Matches_Status" ON "Matches" ("Status");
CREATE INDEX IF NOT EXISTS "IX_Matches_City" ON "Matches" ("City");
CREATE INDEX IF NOT EXISTS "IX_Matches_CreatedByUserId" ON "Matches" ("CreatedByUserId");
CREATE INDEX IF NOT EXISTS "IX_Matches_MatchDateTime_Status" ON "Matches" ("MatchDateTime", "Status");

-- Table Payments
CREATE TABLE IF NOT EXISTS "Payments" (
    "Id" uuid NOT NULL,
    "PaymentReference" character varying(100) NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "PaymentMethod" text NOT NULL,
    "Status" text NOT NULL,
    "PhoneNumber" character varying(50),
    "ProviderTransactionId" character varying(255),
    "Metadata" text,
    "SucceededAt" timestamp with time zone,
    "FailedAt" timestamp with time zone,
    "FailureReason" text,
    "RefundedAt" timestamp with time zone,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Payments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Payments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Payments_PaymentReference" ON "Payments" ("PaymentReference");
CREATE INDEX IF NOT EXISTS "IX_Payments_UserId" ON "Payments" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_Payments_Status" ON "Payments" ("Status");

-- Table Tickets
CREATE TABLE IF NOT EXISTS "Tickets" (
    "Id" uuid NOT NULL,
    "TicketNumber" character varying(50) NOT NULL,
    "TicketType" text NOT NULL,
    "Status" text NOT NULL,
    "Price" numeric(18,2) NOT NULL,
    "QrCodeData" text NOT NULL,
    "QrCodeImageUrl" text,
    "UsedAt" timestamp with time zone,
    "CancelledAt" timestamp with time zone,
    "HolderName" character varying(200),
    "HolderPhone" character varying(50),
    "MatchId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "PaymentId" uuid,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Tickets" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Tickets_Matches_MatchId" FOREIGN KEY ("MatchId") REFERENCES "Matches" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Tickets_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Tickets_Payments_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES "Payments" ("Id") ON DELETE SET NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Tickets_TicketNumber" ON "Tickets" ("TicketNumber");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Tickets_QrCodeData" ON "Tickets" ("QrCodeData");
CREATE INDEX IF NOT EXISTS "IX_Tickets_MatchId" ON "Tickets" ("MatchId");
CREATE INDEX IF NOT EXISTS "IX_Tickets_UserId" ON "Tickets" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_Tickets_PaymentId" ON "Tickets" ("PaymentId");
CREATE INDEX IF NOT EXISTS "IX_Tickets_Status" ON "Tickets" ("Status");
CREATE INDEX IF NOT EXISTS "IX_Tickets_MatchId_Status" ON "Tickets" ("MatchId", "Status");
