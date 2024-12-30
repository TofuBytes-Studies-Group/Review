CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE restaurant (
    "Id" UUID primary key DEFAULT uuid_generate_v4() NOT NULL UNIQUE, 
    "Name" VARCHAR(255) NOT NULL,
    "AddressId" UUID NOT NULL
);

CREATE TABLE menu (
    "Id" UUID primary key default uuid_generate_v4() NOT NULL UNIQUE,
    "RestaurantId" UUID NOT NULL
);

CREATE TABLE dish (
    "Id" UUID primary key default uuid_generate_v4() NOT NULL UNIQUE,
    "Name" VARCHAR(255) NOT NULL,
    "Price" INT NOT NULL,
    "MenuId" UUID NOT NULL
);

CREATE TABLE address (
    "Id" UUID primary key default uuid_generate_v4() NOT NULL UNIQUE,
    "Street" VARCHAR(255) NOT NULL,
    "City" VARCHAR(255) NOT NULL,
    "PostalCode" int NOT NULL
);

ALTER TABLE restaurant ADD CONSTRAINT fk_address FOREIGN KEY ("AddressId") REFERENCES address("Id");
ALTER TABLE menu ADD CONSTRAINT fk_restaurant FOREIGN KEY ("RestaurantId") REFERENCES restaurant("Id");
ALTER TABLE dish ADD CONSTRAINT fk_Menu FOREIGN KEY ("MenuId") REFERENCES menu("Id");

-- Add 21 restaurants with addresses, menus, and dishes
DO $$
DECLARE
    address_id UUID;
    restaurant_id UUID;
    menu_id UUID;
BEGIN
    FOR i IN 1..21 LOOP
        -- Insert an address
        INSERT INTO address ("Street", "City", "PostalCode")
        VALUES 
            ('Street ' || i, 'City ' || i, 10000 + i)
        RETURNING "Id" INTO address_id;

        -- Insert a restaurant with the created address
        INSERT INTO restaurant ("Name", "AddressId")
        VALUES 
            ('Restaurant ' || i, address_id)
        RETURNING "Id" INTO restaurant_id;

        -- Insert a menu for the restaurant
        INSERT INTO menu ("RestaurantId")
        VALUES (restaurant_id)
        RETURNING "Id" INTO menu_id;

        -- Insert dishes for the menu
        INSERT INTO dish ("Name", "Price", "MenuId")
        VALUES 
            ('Dish ' || i || 'A', 100 + i, menu_id),
            ('Dish ' || i || 'B', 120 + i, menu_id),
            ('Dish ' || i || 'C', 150 + i, menu_id);
    END LOOP;
END $$;