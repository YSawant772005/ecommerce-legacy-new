using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        var existingProducts = await context.Products.AsNoTracking().ToListAsync();
        if (existingProducts.Count >= 2000)
        {
            return;
        }

        var rng = new Random(20261007);
        var now = DateTime.UtcNow;

        // Complete an earlier smaller seed without duplicating existing records.
        if (existingProducts.Count > 0)
        {
            var existingCategories = await context.Categories.AsNoTracking().ToListAsync();
            var existingBrands = await context.Brands.AsNoTracking().ToListAsync();
            var additions = new List<Product>();

            for (var index = existingProducts.Count; index < 2000; index++)
            {
                var source = existingProducts[index % existingProducts.Count];
                var category = existingCategories.FirstOrDefault(c => c.Id == source.CategoryId);
                var brand = existingBrands.FirstOrDefault(b => b.Id == source.BrandId);
                var suffix = $" Edition {index + 1}";
                additions.Add(new Product
                {
                    Name = source.Name + suffix,
                    Slug = $"{source.Slug}-{index + 1}",
                    Sku = $"{source.Sku}-{index + 1}",
                    ShortDescription = source.ShortDescription,
                    Description = source.Description,
                    Price = source.Price,
                    ComparePrice = source.ComparePrice,
                    Stock = rng.Next(5, 150),
                    Rating = source.Rating,
                    RatingCount = source.RatingCount,
                    ReviewCount = source.ReviewCount,
                    SoldCount = rng.Next(20, 2000),
                    CategoryId = category?.Id ?? source.CategoryId,
                    BrandId = brand?.Id ?? source.BrandId,
                    ImageUrl = source.ImageUrl,
                    ImageUrl2 = source.ImageUrl2,
                    ImageUrl3 = source.ImageUrl3,
                    Colors = source.Colors,
                    Sizes = source.Sizes,
                    IsFeatured = false,
                    IsDeal = index % 17 == 0,
                    IsNew = true,
                    CreatedOn = now.AddDays(-rng.Next(1, 365))
                });
            }

            context.Products.AddRange(additions);
            await context.SaveChangesAsync();
            return;
        }

        // ── Categories ────────────────────────────────────────────────────────
        var categories = new List<Category>
        {
            new() { Name = "Electronics",    Slug = "electronics",    Tagline = "Latest gadgets and devices",        Description = "Shop the latest smartphones, laptops, and electronics.",           DisplayOrder = 1, IsActive = true },
            new() { Name = "Fashion",         Slug = "fashion",        Tagline = "Trendy clothing and accessories",   Description = "Discover the latest fashion trends for men and women.",            DisplayOrder = 2, IsActive = true },
            new() { Name = "Home & Kitchen",  Slug = "home-kitchen",   Tagline = "Essentials for your home",          Description = "Transform your home with our kitchen and home essentials.",         DisplayOrder = 3, IsActive = true },
            new() { Name = "Books",           Slug = "books",          Tagline = "Bestsellers and classics",          Description = "Explore our vast collection of books across all genres.",           DisplayOrder = 4, IsActive = true },
            new() { Name = "Sports",          Slug = "sports",         Tagline = "Gear up for your game",             Description = "Find the best sports equipment and accessories.",                   DisplayOrder = 5, IsActive = true },
            new() { Name = "Beauty",          Slug = "beauty",         Tagline = "Skincare and cosmetics",            Description = "Premium beauty products for your daily routine.",                   DisplayOrder = 6, IsActive = true },
            new() { Name = "Toys",            Slug = "toys",           Tagline = "Fun for all ages",                  Description = "Discover a wide range of toys and games for children and adults.", DisplayOrder = 7, IsActive = true },
            new() { Name = "Automotive",      Slug = "automotive",     Tagline = "Car accessories and parts",        Description = "Keep your vehicle running perfectly with our auto products.",        DisplayOrder = 8, IsActive = true },
            new() { Name = "Garden",          Slug = "garden",         Tagline = "Grow your green space",             Description = "Everything you need for a beautiful garden.",                       DisplayOrder = 9, IsActive = true },
            new() { Name = "Health",          Slug = "health",         Tagline = "Wellness and nutrition",            Description = "Health supplements, fitness gear, and wellness products.",          DisplayOrder = 10, IsActive = true },
            new() { Name = "Stationery",      Slug = "stationery",     Tagline = "Office and art supplies",           Description = "Stationery, art supplies, and office essentials.",                  DisplayOrder = 11, IsActive = true },
            new() { Name = "Pet Supplies",    Slug = "pet-supplies",   Tagline = "Everything for your pet",           Description = "Food, toys, and accessories for your furry friends.",               DisplayOrder = 12, IsActive = true },
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        // ── Brands ────────────────────────────────────────────────────────────
        var brands = new List<Brand>
        {
            new() { Name = "TechPro",         Slug = "techpro",         Code = "TP" },
            new() { Name = "StyleHub",        Slug = "stylehub",        Code = "SH" },
            new() { Name = "HomeEssentials",  Slug = "homeessentials",  Code = "HE" },
            new() { Name = "BookWorld",       Slug = "bookworld",       Code = "BW" },
            new() { Name = "SportMax",        Slug = "sportmax",        Code = "SM" },
            new() { Name = "GlowBeauty",      Slug = "glowbeauty",      Code = "GB" },
            new() { Name = "FunZone",         Slug = "funzone",         Code = "FZ" },
            new() { Name = "DriveRight",      Slug = "driveright",      Code = "DR" },
            new() { Name = "GreenThumb",      Slug = "greenthumb",      Code = "GT" },
            new() { Name = "VitaPlus",        Slug = "vitaplus",        Code = "VP" },
            new() { Name = "ScriptMate",      Slug = "scriptmate",      Code = "SCM" },
            new() { Name = "PawsNClaws",      Slug = "pawsnclaws",      Code = "PNC" },
        };

        context.Brands.AddRange(brands);
        await context.SaveChangesAsync();

        // ── Products — 30 per category = 360 total ────────────────────────────
        // (A realistic seed that is comprehensive without requiring the original DB dump)
        var products = new List<Product>();

        var electronicsProducts = new[]
        {
            ("Wireless Bluetooth Headphones", 2499m, 3299m), ("Smart Watch Series X", 8999m, 12999m),
            ("4K Ultra HD Monitor 27\"", 21999m, 28999m), ("Mechanical Gaming Keyboard", 3499m, 4999m),
            ("Wireless Gaming Mouse", 1899m, 2699m), ("USB-C Hub 7-in-1", 1299m, 1799m),
            ("Laptop Stand Adjustable", 899m, 1299m), ("Noise Cancelling Earbuds", 5999m, 7999m),
            ("Portable Power Bank 20000mAh", 1499m, 2199m), ("Webcam Full HD 1080p", 2199m, 2999m),
            ("RGB Gaming Headset", 1799m, 2499m), ("External SSD 1TB", 7499m, 9999m),
            ("Smart Home Hub", 3999m, 5499m), ("Wireless Charging Pad", 799m, 1199m),
            ("Bluetooth Speaker Portable", 1599m, 2299m), ("Action Camera 4K", 6999m, 9499m),
            ("Tablet 10-inch", 12999m, 17999m), ("Smart Bulb Pack (4)", 1199m, 1699m),
            ("Mini Projector", 8499m, 11999m), ("Digital Photo Frame", 2999m, 3999m),
            ("WiFi Range Extender", 1299m, 1799m), ("Smart Doorbell Camera", 4999m, 6999m),
            ("Dash Cam Full HD", 2799m, 3999m), ("E-Reader 6-inch", 7499m, 9999m),
            ("Gaming Controller Wireless", 2999m, 3999m), ("Smart Scale Body Fat", 1999m, 2799m),
            ("LED Desk Lamp Smart", 1499m, 1999m), ("Robot Vacuum Cleaner", 13999m, 18999m),
            ("Fitness Tracker Band", 2499m, 3499m), ("HDMI Cable 4K 2m", 499m, 699m),
        };

        var fashionProducts = new[]
        {
            ("Men's Slim Fit Chinos", 799m, 1199m), ("Women's Floral Sundress", 899m, 1399m),
            ("Men's Oxford Formal Shirt", 699m, 999m), ("Women's High Waist Jeans", 1099m, 1599m),
            ("Unisex Hooded Sweatshirt", 999m, 1499m), ("Women's Blazer Set", 2499m, 3499m),
            ("Men's Running Joggers", 699m, 999m), ("Women's Kurta Ethnic Set", 1299m, 1799m),
            ("Men's Polo T-Shirt", 499m, 749m), ("Women's Flats Leather", 1299m, 1799m),
            ("Men's Sneakers White", 1599m, 2299m), ("Women's Handbag Tote", 1999m, 2999m),
            ("Men's Leather Belt", 499m, 749m), ("Women's Sunglasses UV400", 699m, 999m),
            ("Men's Formal Trousers", 899m, 1299m), ("Women's Casual Shorts", 599m, 899m),
            ("Unisex Sports Socks (3 Pack)", 299m, 449m), ("Women's Sports Bra", 499m, 749m),
            ("Men's Winter Jacket", 2499m, 3499m), ("Women's Woolen Cardigan", 1499m, 2199m),
            ("Kids Cartoon Print T-Shirt", 299m, 449m), ("Men's Swim Shorts", 499m, 749m),
            ("Women's Yoga Pants", 799m, 1199m), ("Unisex Baseball Cap", 399m, 599m),
            ("Men's Ethnic Kurta", 799m, 1199m), ("Women's Saree Cotton", 1299m, 1799m),
            ("Men's Sandals Brown", 799m, 1199m), ("Women's Ankle Boots", 1999m, 2799m),
            ("Men's Slim Jeans Dark", 1199m, 1799m), ("Women's Wrap Dress", 1099m, 1599m),
        };

        var homeKitchenProducts = new[]
        {
            ("Stainless Steel Cookware Set", 2499m, 3499m), ("Non-Stick Fry Pan 28cm", 699m, 999m),
            ("Electric Kettle 1.5L", 799m, 1199m), ("Air Fryer 4L Digital", 3499m, 4999m),
            ("Pressure Cooker 3L", 1299m, 1799m), ("Blender 500W Jar", 1499m, 2199m),
            ("Coffee Maker Drip", 2199m, 2999m), ("Toaster 2-Slice", 899m, 1299m),
            ("Rice Cooker 1.8L", 1299m, 1799m), ("Hand Mixer 250W", 799m, 1199m),
            ("Knife Set 5-Piece", 1499m, 2199m), ("Cutting Board Bamboo", 399m, 599m),
            ("Storage Containers Set", 799m, 1199m), ("Table Clock Wooden", 499m, 699m),
            ("Bed Sheet Set King", 1199m, 1799m), ("Pillow Microfibre 2-Pack", 799m, 1199m),
            ("Curtains Blackout (2 Panels)", 1299m, 1799m), ("Bathroom Mat Soft", 399m, 599m),
            ("Wall Art Canvas Print", 999m, 1499m), ("Decorative Vase Ceramic", 599m, 899m),
            ("Scented Candle Set", 699m, 999m), ("Laundry Basket Foldable", 499m, 749m),
            ("Ironing Board Slim", 1499m, 2199m), ("Steam Iron 1600W", 1299m, 1799m),
            ("Vacuum Cleaner Handheld", 1999m, 2799m), ("Dish Drying Rack", 599m, 849m),
            ("Fruit Bowl Metal", 349m, 499m), ("Spice Rack Wall Mount", 699m, 999m),
            ("Water Purifier Tabletop", 4999m, 6999m), ("Electric Chimney 60cm", 7999m, 10999m),
        };

        var booksProducts = new[]
        {
            ("Atomic Habits - James Clear", 399m, 599m), ("The Psychology of Money", 349m, 499m),
            ("Rich Dad Poor Dad", 299m, 449m), ("Think and Grow Rich", 249m, 349m),
            ("The Alchemist - Paulo Coelho", 199m, 299m), ("Ikigai - Japanese Secret", 299m, 449m),
            ("Deep Work - Cal Newport", 349m, 499m), ("The 48 Laws of Power", 449m, 649m),
            ("Zero to One - Peter Thiel", 499m, 699m), ("Sapiens - Yuval Harari", 499m, 699m),
            ("The Art of War", 199m, 299m), ("How to Win Friends", 299m, 449m),
            ("Mindset - Carol Dweck", 349m, 499m), ("Dune - Frank Herbert", 499m, 699m),
            ("1984 - George Orwell", 199m, 299m), ("To Kill a Mockingbird", 249m, 349m),
            ("The Lean Startup", 449m, 649m), ("Start With Why - Sinek", 349m, 499m),
            ("Python Crash Course 3ed", 699m, 999m), ("Clean Code - Robert Martin", 799m, 1099m),
            ("Designing Data-Intensive Apps", 1299m, 1799m), ("The Pragmatic Programmer", 899m, 1299m),
            ("A Brief History of Time", 299m, 449m), ("The Grand Design - Hawking", 399m, 599m),
            ("Originals - Adam Grant", 349m, 499m), ("Essentialism - Greg McKeown", 349m, 499m),
            ("Can't Hurt Me - David Goggins", 399m, 599m), ("Autobiography - APJ Kalam", 249m, 349m),
            ("Discovery of India - Nehru", 349m, 499m), ("Wings of Fire - APJ Kalam", 199m, 299m),
        };

        var sportsProducts = new[]
        {
            ("Yoga Mat Anti-Slip 6mm", 699m, 999m), ("Resistance Bands Set 5-Pack", 499m, 699m),
            ("Dumbbells 5kg Pair", 1299m, 1799m), ("Skipping Rope Speed", 299m, 449m),
            ("Badminton Set 4 Rackets", 999m, 1499m), ("Cricket Bat Kashmir Willow", 1299m, 1799m),
            ("Football Training Ball", 599m, 849m), ("Table Tennis Set", 799m, 1199m),
            ("Basketball Grip Control", 699m, 999m), ("Pull-Up Bar Doorway", 1299m, 1799m),
            ("Boxing Gloves 12oz", 1499m, 2199m), ("Cycling Helmet Adult", 1299m, 1799m),
            ("Knee Support Sleeve Pair", 399m, 599m), ("Gym Bag Duffel 35L", 999m, 1499m),
            ("Running Shoes Men", 2499m, 3499m), ("Sport Bottle 750ml", 299m, 449m),
            ("Foam Roller 60cm", 699m, 999m), ("Ab Roller Wheel", 499m, 699m),
            ("Chess Board Wooden", 699m, 999m), ("Carom Board Full Size", 1999m, 2799m),
            ("Swimming Goggles UV", 399m, 599m), ("Tennis Racket Professional", 2999m, 4199m),
            ("Volleyball Standard", 699m, 999m), ("Exercise Bike Foldable", 8999m, 12499m),
            ("Treadmill Manual Fold", 14999m, 19999m), ("Weight Plates 10kg Set", 1799m, 2499m),
            ("Push-Up Bars Pair", 399m, 599m), ("Soccer Shin Guards Pair", 499m, 699m),
            ("Badminton Shuttlecocks 12pk", 299m, 449m), ("Cricket Batting Gloves", 699m, 999m),
        };

        var beautyProducts = new[]
        {
            ("Vitamin C Face Serum 30ml", 599m, 849m), ("Hyaluronic Acid Moisturizer", 799m, 1099m),
            ("Sunscreen SPF50 PA+++ 50g", 449m, 649m), ("Niacinamide Serum 10%", 499m, 699m),
            ("Face Wash Gel Salicylic", 299m, 449m), ("Rose Water Toner 200ml", 199m, 299m),
            ("Retinol Night Cream 50g", 899m, 1299m), ("Under Eye Cream Peptide", 699m, 999m),
            ("Lip Balm SPF15 (Pack of 3)", 199m, 299m), ("Face Sheet Mask Set (10)", 399m, 599m),
            ("Biotin Hair Serum 50ml", 499m, 699m), ("Argan Oil Hair Mask 200g", 599m, 849m),
            ("Dry Shampoo Spray 200ml", 299m, 449m), ("Hair Growth Oil Blend", 349m, 499m),
            ("Shampoo Anti-Dandruff 400ml", 299m, 449m), ("Conditioner Deep Nourish", 299m, 449m),
            ("Foundation Matte SPF15", 799m, 1099m), ("Mascara Volumizing Black", 499m, 699m),
            ("Kajal Kohl Liner", 149m, 229m), ("Lip Tint Glossy Pack (3)", 349m, 499m),
            ("Face Scrub Walnut 100g", 199m, 299m), ("Micellar Water 400ml", 299m, 449m),
            ("Body Lotion SPF15 250ml", 249m, 349m), ("Talcum Powder Fresh 100g", 149m, 229m),
            ("Perfume Floral 50ml EDP", 1299m, 1799m), ("Deodorant Roll-On 50ml", 149m, 229m),
            ("Nail Polish Set (10 Colors)", 349m, 499m), ("Makeup Brush Set 12-Piece", 799m, 1099m),
            ("Eyebrow Pencil 2-in-1", 199m, 299m), ("CC Cream BB SPF30 40ml", 699m, 999m),
        };

        var toysProducts = new[]
        {
            ("LEGO Classic Creative Bricks", 1299m, 1799m), ("Remote Control Car Offroad", 1499m, 2199m),
            ("Barbie Dream Playset", 1999m, 2799m), ("Hot Wheels 20-Car Gift Pack", 799m, 1099m),
            ("Building Blocks 100-Piece", 599m, 849m), ("Puzzle 500-Piece Landscape", 499m, 699m),
            ("Art & Craft Kit Kids", 699m, 999m), ("Play-Doh Modeling Clay Set", 499m, 699m),
            ("Board Game Monopoly Classic", 999m, 1399m), ("Jenga Stacking Game", 699m, 999m),
            ("Rubik's Cube 3x3", 299m, 449m), ("Fidget Spinner 3-Pack", 199m, 299m),
            ("Superhero Action Figure Set", 899m, 1299m), ("Soft Stuffed Teddy 50cm", 699m, 999m),
            ("Musical Drum Set Kids", 1299m, 1799m), ("Keyboard Piano 32-Key", 1799m, 2499m),
            ("Doctor Kit Pretend Play", 499m, 699m), ("Kitchen Set Play Toy", 799m, 1199m),
            ("Race Track Slot Car Set", 1999m, 2799m), ("Walkie Talkie Set 2-Pack", 799m, 1099m),
            ("Science Experiment Kit", 999m, 1399m), ("Telescope Kids 70mm", 1999m, 2799m),
            ("Magnetic Drawing Board", 499m, 699m), ("Jigsaw Puzzle 1000-Piece", 699m, 999m),
            ("Kite String Reel Large", 399m, 599m), ("Frisbee Ultimate 175g", 299m, 449m),
            ("Nerf Blaster Dart Gun", 999m, 1399m), ("Archery Set Foam", 699m, 999m),
            ("Doll House Wooden", 3999m, 5499m), ("Train Set Electric Track", 2999m, 3999m),
        };

        var automotiveProducts = new[]
        {
            ("Car Vacuum Cleaner DC12V", 1299m, 1799m), ("Tyre Inflator Portable", 1499m, 2199m),
            ("Car Seat Cover Universal", 1799m, 2499m), ("Steering Wheel Cover", 399m, 599m),
            ("Car Phone Holder Magnetic", 499m, 699m), ("Jump Starter 2000A", 4999m, 6999m),
            ("Dashboard Camera 1080p", 2799m, 3999m), ("Car Freshener Set (3)", 299m, 449m),
            ("Floor Mat Set 5-Piece", 899m, 1299m), ("Wiper Blade Flat 22-inch", 399m, 599m),
            ("Car Cover Waterproof", 1499m, 2199m), ("Reverse Camera 170° Wide", 1299m, 1799m),
            ("LED Headlight Bulb H4", 799m, 1199m), ("Car Polishing Wax 500ml", 499m, 699m),
            ("Microfiber Cleaning Cloth 10pk", 299m, 449m), ("OBD2 Scanner Bluetooth", 2499m, 3499m),
            ("Sunshade Windshield Fold", 499m, 699m), ("Roof Rack Cargo Basket", 3999m, 5499m),
            ("Car Battery Charger 12V", 1499m, 1999m), ("Tyre Pressure Monitor", 1799m, 2499m),
            ("GPS Navigator 5-inch", 3499m, 4999m), ("Car Audio System", 4999m, 6999m),
            ("Fuel Injector Cleaner", 399m, 599m), ("Engine Oil 5W30 1L", 499m, 699m),
            ("Coolant Antifreeze 1L", 349m, 499m), ("Car First Aid Kit", 699m, 999m),
            ("Bike Chain Lubricant", 199m, 299m), ("Motorcycle Helmet Full", 2999m, 4199m),
            ("Safety Reflective Jacket", 499m, 699m), ("Parking Sensor Kit 4-Sensor", 1999m, 2799m),
        };

        var gardenProducts = new[]
        {
            ("Garden Hose Expandable 30m", 899m, 1299m), ("Pruning Shears Steel", 599m, 849m),
            ("Potting Mix Soil 5kg", 349m, 499m), ("Garden Gloves Anti-Thorn", 199m, 299m),
            ("Watering Can 5L Plastic", 299m, 449m), ("Trowel Transplanting Tool", 199m, 299m),
            ("Plant Fertilizer NPK 1kg", 299m, 449m), ("Succulent Plant Set (6)", 699m, 999m),
            ("Grow Bag Set 12-inch (5pk)", 499m, 699m), ("Drip Irrigation Kit", 1299m, 1799m),
            ("Garden Fork 4-Tine", 399m, 599m), ("Rake Adjustable Handle", 499m, 699m),
            ("Compost Bin 120L", 1999m, 2799m), ("Bird Feeder Hanging", 399m, 599m),
            ("Windmill Garden Ornament", 499m, 699m), ("Solar Garden Lights (4pk)", 799m, 1199m),
            ("Seedling Starter Tray 50", 299m, 449m), ("Grafting Tape Green", 149m, 229m),
            ("Neem Oil Spray Organic", 249m, 349m), ("Hanging Planter Basket", 349m, 499m),
            ("Terracotta Pot 10-inch", 199m, 299m), ("Metal Raised Bed Garden", 3999m, 5499m),
            ("Leaf Blower 550W", 2999m, 4199m), ("Lawn Mower Electric", 6999m, 9499m),
            ("Hedge Trimmer Cordless", 3499m, 4999m), ("Soil pH Meter Digital", 499m, 699m),
            ("Plant Labels Bamboo 50pk", 149m, 229m), ("Moss Pole 90cm", 199m, 299m),
            ("Cactus Soil Mix 2kg", 249m, 349m), ("Garden Kneeler Foldable", 799m, 1099m),
        };

        var healthProducts = new[]
        {
            ("Whey Protein Isolate 1kg", 2499m, 3499m), ("Multivitamin Gummies 60ct", 699m, 999m),
            ("Fish Oil Omega-3 60 Caps", 499m, 699m), ("Vitamin D3 1000IU 90 Tabs", 399m, 599m),
            ("Turmeric Curcumin 500mg", 499m, 699m), ("Probiotic 10 Billion CFU", 799m, 1099m),
            ("Collagen Peptides Powder", 1499m, 1999m), ("Ashwagandha Extract 500mg", 599m, 849m),
            ("Green Tea Extract 500mg", 399m, 599m), ("Iron Supplement 27mg", 299m, 449m),
            ("B12 Sublingual 1000mcg", 399m, 599m), ("Zinc Supplement 50mg", 299m, 449m),
            ("Blood Pressure Monitor", 1799m, 2499m), ("Pulse Oximeter Fingertip", 699m, 999m),
            ("Digital Thermometer Oral", 299m, 449m), ("Glucose Monitor Kit", 1499m, 1999m),
            ("Heating Pad Electric", 899m, 1299m), ("First Aid Kit 85-Piece", 699m, 999m),
            ("Orthopedic Cervical Pillow", 1499m, 1999m), ("Lumbar Support Belt", 799m, 1099m),
            ("TENS Muscle Massager", 2499m, 3499m), ("Foot Massager Electric", 3499m, 4999m),
            ("Meditation Cushion Set", 1299m, 1799m), ("Foam Knee Brace Adjustable", 599m, 849m),
            ("Hand Grip Strengthener", 299m, 449m), ("Resistance Tube Set", 499m, 699m),
            ("Pedometer Wrist Band", 799m, 1099m), ("Air Purifier HEPA Mini", 2999m, 3999m),
            ("Humidifier Ultrasonic 2L", 1499m, 1999m), ("Massage Gun Deep Tissue", 3499m, 4999m),
        };

        var stationeryProducts = new[]
        {
            ("Ballpoint Pen Set 12-Pack", 199m, 299m), ("Gel Pen Set 20-Color", 299m, 449m),
            ("Notebook A5 Hardcover", 199m, 299m), ("Spiral Notebook B5 80pg", 149m, 229m),
            ("Highlighter Pastel 6-Pack", 199m, 299m), ("Sticky Notes Set 5-Color", 199m, 299m),
            ("Stapler Heavy-Duty 25 Sheet", 399m, 599m), ("Scissor Set 3-Piece", 299m, 449m),
            ("Tape Dispenser + 3 Rolls", 249m, 349m), ("Permanent Marker Set 10", 199m, 299m),
            ("Whiteboard 60x45cm", 699m, 999m), ("Whiteboard Marker Set 4", 199m, 299m),
            ("Pencil Set HB 2B 4B (12pk)", 149m, 229m), ("Eraser Kneaded Art", 99m, 149m),
            ("Ruler 30cm Transparent", 49m, 79m), ("Geometry Box Set", 199m, 299m),
            ("Binder Ring Files A4 2\" (3pk)", 299m, 449m), ("Document Folder A4 (10pk)", 199m, 299m),
            ("Desk Organizer Bamboo", 699m, 999m), ("Pen Holder Cup Metal", 299m, 449m),
            ("Drawing Pad A4 120gsm", 349m, 499m), ("Watercolor Set 36 Colors", 699m, 999m),
            ("Acrylic Painting Set", 999m, 1399m), ("Sketch Book A4 Hard", 299m, 449m),
            ("Calligraphy Pen Set", 599m, 849m), ("Correction Tape (3-Pack)", 199m, 299m),
            ("Paper Clips Box 100-Count", 99m, 149m), ("Push Pins Thumbtacks 100pk", 99m, 149m),
            ("Label Maker Manual", 699m, 999m), ("File Box A4 Stackable", 499m, 699m),
        };

        var petProducts = new[]
        {
            ("Dog Dry Food Premium 5kg", 1299m, 1799m), ("Cat Wet Food Pouches (12pk)", 599m, 849m),
            ("Dog Collar Adjustable L", 299m, 449m), ("Cat Scratching Post 60cm", 799m, 1099m),
            ("Dog Leash Retractable 5m", 599m, 849m), ("Cat Toy Feather Wand Set", 299m, 449m),
            ("Dog Harness No-Pull M", 699m, 999m), ("Stainless Pet Bowl Set 2", 299m, 449m),
            ("Cat Litter Silica Gel 5L", 599m, 849m), ("Dog Puppy Training Pads 30pk", 399m, 599m),
            ("Pet Grooming Brush Self-Clean", 499m, 699m), ("Dog Dental Chews 30-Count", 399m, 599m),
            ("Cat Carrier Backpack", 1499m, 1999m), ("Dog Crate Foldable Medium", 2999m, 3999m),
            ("Pet Shampoo Anti-Tick", 299m, 449m), ("Dog Raincoat S/M/L", 399m, 599m),
            ("Cat Window Perch Hammock", 699m, 999m), ("Bird Cage Powder-Coated", 1499m, 1999m),
            ("Aquarium Fish Tank 20L", 1299m, 1799m), ("Fish Food Flakes 100g", 149m, 229m),
            ("Rabbit Hutch Indoor", 2499m, 3499m), ("Hamster Wheel Silent", 399m, 599m),
            ("Reptile Heat Lamp Bulb", 399m, 599m), ("Turtle Tank Starter Kit", 1299m, 1799m),
            ("Dog Nail Clipper Safety", 199m, 299m), ("Pet First Aid Kit", 699m, 999m),
            ("GPS Pet Tracker Collar", 2499m, 3499m), ("Automatic Feeder Pet", 1999m, 2799m),
            ("Pet Water Fountain 2L", 1299m, 1799m), ("Dog Bed Orthopedic L", 2499m, 3499m),
        };

        var productData = new (string name, decimal price, decimal comparePrice)[][]
        {
            electronicsProducts, fashionProducts, homeKitchenProducts, booksProducts,
            sportsProducts, beautyProducts, toysProducts, automotiveProducts, gardenProducts,
            healthProducts, stationeryProducts, petProducts
        };

        var catBrandMap = new Dictionary<string, string>
        {
            { "electronics", "techpro" }, { "fashion", "stylehub" }, { "home-kitchen", "homeessentials" },
            { "books", "bookworld" }, { "sports", "sportmax" }, { "beauty", "glowbeauty" },
            { "toys", "funzone" }, { "automotive", "driveright" }, { "garden", "greenthumb" },
            { "health", "vitaplus" }, { "stationery", "scriptmate" }, { "pet-supplies", "pawsnclaws" },
        };

        for (int ci = 0; ci < categories.Count; ci++)
        {
            var cat = categories[ci];
            var data = productData[ci];
            var brandSlug = catBrandMap[cat.Slug!];
            var brand = brands.First(b => b.Slug == brandSlug);
            var altBrand = brands[rng.Next(brands.Count)];

            for (int pi = 0; pi < data.Length; pi++)
            {
                var (name, price, comparePrice) = data[pi];
                var useBrand = (pi % 3 == 0) ? altBrand : brand;
                products.Add(new Product
                {
                    Name = name,
                    Slug = $"{cat.Slug}-{Slugify(name)}",
                    Sku = $"{cat.Slug!.ToUpper()[..3]}-{1000 + ci * 30 + pi}",
                    ShortDescription = $"High-quality {name} from {useBrand.Name}.",
                    Description = $"<p>{name} by {useBrand.Name}. This product offers exceptional quality and value. Ideal for everyday use with premium materials and craftsmanship.</p><ul><li>Brand: {useBrand.Name}</li><li>Category: {cat.Name}</li></ul>",
                    Price = price,
                    ComparePrice = comparePrice,
                    Stock = rng.Next(5, 150),
                    Rating = Math.Round(rng.NextDouble() * 1.5 + 3.5, 1),
                    RatingCount = rng.Next(10, 500),
                    ReviewCount = rng.Next(5, 200),
                    SoldCount = rng.Next(20, 2000),
                    CategoryId = cat.Id,
                    BrandId = useBrand.Id,
                    ImageUrl = $"https://placehold.co/400x400/e8e8e8/555555?text={Uri.EscapeDataString(name[..Math.Min(name.Length, 15)])}",
                    IsFeatured = pi < 4,
                    IsDeal = pi < 2 || pi == 15,
                    IsNew = pi >= 25,
                    CreatedOn = now.AddDays(-rng.Next(1, 365))
                });
            }
        }

        // Keep the seeded catalog useful for real browsing and infinite-scroll demos.
        var baseProductCount = products.Count;
        for (var index = baseProductCount; index < 2000; index++)
        {
            var source = products[index % baseProductCount];
            products.Add(new Product
            {
                Name = $"{source.Name} Edition {index + 1}",
                Slug = $"{source.Slug}-{index + 1}",
                Sku = $"{source.Sku}-{index + 1}",
                ShortDescription = source.ShortDescription,
                Description = source.Description,
                Price = source.Price,
                ComparePrice = source.ComparePrice,
                Stock = rng.Next(5, 150),
                Rating = source.Rating,
                RatingCount = source.RatingCount,
                ReviewCount = source.ReviewCount,
                SoldCount = rng.Next(20, 2000),
                CategoryId = source.CategoryId,
                BrandId = source.BrandId,
                ImageUrl = source.ImageUrl,
                ImageUrl2 = source.ImageUrl2,
                ImageUrl3 = source.ImageUrl3,
                Colors = source.Colors,
                Sizes = source.Sizes,
                IsFeatured = false,
                IsDeal = index % 17 == 0,
                IsNew = true,
                CreatedOn = now.AddDays(-rng.Next(1, 365))
            });
        }

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // ── Users ──────────────────────────────────────────────────────────────
        var hasher = new PasswordHasher();
        var adminSalt = hasher.GenerateSalt();
        var custSalt = hasher.GenerateSalt();

        var users = new List<User>
        {
            new()
            {
                FullName = "Admin User",
                Email = "admin@novakart.com",
                PasswordHash = hasher.HashPassword("Admin123!", adminSalt),
                PasswordSalt = adminSalt,
                Role = "Admin",
                Phone = "9876543210",
                CreatedOn = now
            },
            new()
            {
                FullName = "John Doe",
                Email = "john@example.com",
                PasswordHash = hasher.HashPassword("Customer123!", custSalt),
                PasswordSalt = custSalt,
                Role = "Customer",
                Phone = "9876543211",
                CreatedOn = now
            }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // ── Coupons ────────────────────────────────────────────────────────────
        var coupons = new List<Coupon>
        {
            new()
            {
                Code = "SAVE10",
                Description = "10% off on orders above ₹500 (max ₹100 discount)",
                MinOrderValue = 500,
                FixedAmount = null,
                PercentDiscount = 10,
                MaxDiscountAmount = 100,
                FreeShipping = false,
                IsActive = true,
                TotalUses = 0,
                StartsOn = now.AddDays(-30),
                EndsOn = now.AddDays(335),
                CreatedOn = now
            },
            new()
            {
                Code = "FLAT50",
                Description = "Flat ₹50 off on orders above ₹200",
                MinOrderValue = 200,
                FixedAmount = 50,
                PercentDiscount = null,
                MaxDiscountAmount = 0,
                FreeShipping = false,
                IsActive = true,
                TotalUses = 0,
                StartsOn = now.AddDays(-30),
                EndsOn = now.AddDays(335),
                CreatedOn = now
            },
            new()
            {
                Code = "FREESHIP",
                Description = "Free shipping on all orders",
                MinOrderValue = 0,
                FixedAmount = null,
                PercentDiscount = null,
                MaxDiscountAmount = 0,
                FreeShipping = true,
                IsActive = true,
                TotalUses = 0,
                StartsOn = now.AddDays(-30),
                EndsOn = now.AddDays(335),
                CreatedOn = now
            },
            new()
            {
                Code = "WELCOME20",
                Description = "20% off for new customers (max ₹200)",
                MinOrderValue = 300,
                FixedAmount = null,
                PercentDiscount = 20,
                MaxDiscountAmount = 200,
                FreeShipping = false,
                IsActive = true,
                TotalUses = 0,
                StartsOn = now.AddDays(-30),
                EndsOn = now.AddDays(335),
                CreatedOn = now
            },
            new()
            {
                Code = "FLAT200",
                Description = "Flat ₹200 off on orders above ₹1500",
                MinOrderValue = 1500,
                FixedAmount = 200,
                PercentDiscount = null,
                MaxDiscountAmount = 0,
                FreeShipping = true,
                IsActive = true,
                TotalUses = 0,
                StartsOn = now.AddDays(-30),
                EndsOn = now.AddDays(335),
                CreatedOn = now
            }
        };

        context.Coupons.AddRange(coupons);
        await context.SaveChangesAsync();
    }

    private static string Slugify(string text)
    {
        return System.Text.RegularExpressions.Regex.Replace(
            text.ToLowerInvariant().Replace("'", "").Replace("\"", ""),
            @"[^a-z0-9]+", "-").Trim('-');
    }
}
