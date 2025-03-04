Build started...
Build succeeded.
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Products" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY AUTOINCREMENT,
    "ProductName" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "ProductImage" TEXT NULL
);

CREATE TABLE "Items" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Items" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "Price" TEXT NOT NULL,
    "ProductId" INTEGER NOT NULL,
    CONSTRAINT "FK_Items_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Items_ProductId" ON "Items" ("ProductId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250203001947_InitialCreate', '9.0.1');

ALTER TABLE "Products" ADD "Description" TEXT NOT NULL DEFAULT '';

CREATE TABLE "PurchaseHistories" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PurchaseHistories" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "PurchaseDate" TEXT NOT NULL
);

CREATE TABLE "WishlistItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_WishlistItems" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NULL,
    "ProductId" INTEGER NOT NULL,
    CONSTRAINT "FK_WishlistItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PurchaseItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PurchaseItems" PRIMARY KEY AUTOINCREMENT,
    "ProductId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "PriceAtPurchase" TEXT NOT NULL,
    "PurchaseHistoryId" INTEGER NOT NULL,
    CONSTRAINT "FK_PurchaseItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PurchaseItems_PurchaseHistories_PurchaseHistoryId" FOREIGN KEY ("PurchaseHistoryId") REFERENCES "PurchaseHistories" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_PurchaseItems_ProductId" ON "PurchaseItems" ("ProductId");

CREATE INDEX "IX_PurchaseItems_PurchaseHistoryId" ON "PurchaseItems" ("PurchaseHistoryId");

CREATE INDEX "IX_WishlistItems_ProductId" ON "WishlistItems" ("ProductId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250211015009_ActualizacionModelos', '9.0.1');

ALTER TABLE "WishlistItems" ADD "UserEmail" TEXT NOT NULL DEFAULT '';

CREATE TABLE "Customers" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY AUTOINCREMENT,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250211132105_AddCustomersTable', '9.0.1');

CREATE TABLE "ef_temp_WishlistItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_WishlistItems" PRIMARY KEY AUTOINCREMENT,
    "ProductId" INTEGER NOT NULL,
    "UserEmail" TEXT NOT NULL,
    CONSTRAINT "FK_WishlistItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_WishlistItems" ("Id", "ProductId", "UserEmail")
SELECT "Id", "ProductId", "UserEmail"
FROM "WishlistItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "WishlistItems";

ALTER TABLE "ef_temp_WishlistItems" RENAME TO "WishlistItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;
CREATE INDEX "IX_WishlistItems_ProductId" ON "WishlistItems" ("ProductId");

COMMIT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250211140021_UpdateWishlistSchema', '9.0.1');

BEGIN TRANSACTION;
ALTER TABLE "PurchaseItems" ADD "UserEmail" TEXT NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250211141937_AddUserEmailToPurchaseItem', '9.0.1');

CREATE TABLE "ef_temp_PurchaseItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PurchaseItems" PRIMARY KEY AUTOINCREMENT,
    "PriceAtPurchase" TEXT NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "PurchaseHistoryId" INTEGER NULL,
    "Quantity" INTEGER NOT NULL,
    "UserEmail" TEXT NOT NULL,
    CONSTRAINT "FK_PurchaseItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PurchaseItems_PurchaseHistories_PurchaseHistoryId" FOREIGN KEY ("PurchaseHistoryId") REFERENCES "PurchaseHistories" ("Id")
);

INSERT INTO "ef_temp_PurchaseItems" ("Id", "PriceAtPurchase", "ProductId", "PurchaseHistoryId", "Quantity", "UserEmail")
SELECT "Id", "PriceAtPurchase", "ProductId", "PurchaseHistoryId", "Quantity", "UserEmail"
FROM "PurchaseItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "PurchaseItems";

ALTER TABLE "ef_temp_PurchaseItems" RENAME TO "PurchaseItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;
CREATE INDEX "IX_PurchaseItems_ProductId" ON "PurchaseItems" ("ProductId");

CREATE INDEX "IX_PurchaseItems_PurchaseHistoryId" ON "PurchaseItems" ("PurchaseHistoryId");

COMMIT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250211142536_MakePurchaseHistoryIdNullable', '9.0.1');

BEGIN TRANSACTION;
ALTER TABLE "Items" ADD "UserEmail" TEXT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250211160232_AddUserEmailToItem', '9.0.1');

CREATE TABLE "ef_temp_PurchaseItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PurchaseItems" PRIMARY KEY AUTOINCREMENT,
    "PriceAtPurchase" TEXT NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "PurchaseHistoryId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_PurchaseItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PurchaseItems_PurchaseHistories_PurchaseHistoryId" FOREIGN KEY ("PurchaseHistoryId") REFERENCES "PurchaseHistories" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_PurchaseItems" ("Id", "PriceAtPurchase", "ProductId", "PurchaseHistoryId", "Quantity")
SELECT "Id", "PriceAtPurchase", "ProductId", IFNULL("PurchaseHistoryId", 0), "Quantity"
FROM "PurchaseItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "PurchaseItems";

ALTER TABLE "ef_temp_PurchaseItems" RENAME TO "PurchaseItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;
CREATE INDEX "IX_PurchaseItems_ProductId" ON "PurchaseItems" ("ProductId");

CREATE INDEX "IX_PurchaseItems_PurchaseHistoryId" ON "PurchaseItems" ("PurchaseHistoryId");

COMMIT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250212135555_AddItemNameField', '9.0.1');


